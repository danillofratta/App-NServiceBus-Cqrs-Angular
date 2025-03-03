using MassTransit;
using Microsoft.Extensions.Logging;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Contracts.Event;

namespace Sale.Core.Domain.SagaMassTransit
{
    public class SaleSagaStateMachine : MassTransitStateMachine<SaleSagaState>
    {
        private readonly ILogger<SaleSagaStateMachine> _logger;

        public State StockReserving { get; private set; }
        public State PaymentProcessing { get; private set; }
        public State Completed { get; private set; }
        public State Failed { get; private set; }

        public Event<SaleCreatedEvent> SaleCreated { get; private set; }
        public Event<StockConfirmedEvent> StockConfirmed { get; private set; }
        public Event<StockInsufficientEvent> StockInsufficient { get; private set; }
        public Event<PaymentConfirmedEvent> PaymentConfirmed { get; private set; }
        public Event<PaymentFailEvent> PaymentFailed { get; private set; }

        public SaleSagaStateMachine(ILogger<SaleSagaStateMachine> logger)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState);
            
            Event(() => SaleCreated, x =>
                x.CorrelateById(s => s.CorrelationId, context => context.Message.CorrelationId)
                .SelectId(context => context.Message.CorrelationId));  

            Event(() => StockConfirmed, x => x.CorrelateById(s => s.SaleId, context => context.Message.SaleId));
            Event(() => StockInsufficient, x => x.CorrelateById(s => s.SaleId, context => context.Message.SaleId));
            Event(() => PaymentConfirmed, x => x.CorrelateById(s => s.SaleId, context => context.Message.SaleId));
            Event(() => PaymentFailed, x => x.CorrelateById(s => s.SaleId, context => context.Message.SaleId));

            Initially(
                When(SaleCreated)
                    .Then(context =>
                    {
                        _logger.LogInformation("Iniciando saga para venda {SaleId}. Verificando estoque...", context.Message.SaleId);
                        context.Saga.SaleId = context.Message.SaleId;
                        if (context.Saga.ProcessStarted.HasValue) return;
                        context.Saga.ProcessStarted = DateTime.UtcNow;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Send(new Uri("queue:StockSagaEndpoint"), new ReserveStockCommand
                        {
                            SaleId = context.Saga.SaleId,
                            SaleItens = context.Message.SaleItens
                        });
                    })
                    .TransitionTo(StockReserving));

            During(StockReserving,
                When(StockConfirmed)
                    .Then(context =>
                    {
                        _logger.LogInformation("Estoque confirmado para venda {SaleId}. Processando pagamento...", context.Message.SaleId);
                        if (context.Saga.StockConfirmed.HasValue)
                        {
                            _logger.LogWarning("Stock already confirmed for sale {SaleId}", context.Message.SaleId);
                            return;
                        }
                        if (context.Saga.PaymentRequested.HasValue)
                        {
                            _logger.LogWarning("Payment already requested for sale {SaleId}", context.Message.SaleId);
                            return;
                        }
                        context.Saga.StockConfirmed = DateTime.UtcNow;
                        context.Saga.PaymentRequested = DateTime.UtcNow;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Send(new Uri("queue:SaleSagaEndpoint"), new SaleStockConfirmedCommand
                        {
                            SaleId = context.Saga.SaleId
                        });
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Send(new Uri("queue:PaymentSagaEndpoint"), new ProcessPaymentCommand
                        {
                            SaleId = context.Saga.SaleId,
                            Valor = context.Message.Total
                        });
                    })
                    .TransitionTo(PaymentProcessing),

                When(StockInsufficient)
                    .Then(context =>
                    {
                        _logger.LogWarning("Estoque insuficiente para venda {SaleId}. Cancelando venda...", context.Message.SaleId);
                        if (context.Saga.StockInsufficient.HasValue) return;
                        context.Saga.StockInsufficient = DateTime.UtcNow;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Send(new Uri("queue:SaleSagaEndpoint"), new SaleStockInsufficienCommand
                        {
                            SaleId = context.Saga.SaleId
                        });
                    })
                    .TransitionTo(Failed)
                    .Finalize());

            During(PaymentProcessing,
                When(PaymentConfirmed)
                    .Then(context =>
                    {
                        _logger.LogInformation("Pagamento confirmado para venda {SaleId}. Finalizando venda...", context.Message.SaleId);
                        if (context.Saga.PaymentConfirmed.HasValue) return;
                        context.Saga.PaymentConfirmed = DateTime.UtcNow;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Send(new Uri("queue:SaleSagaEndpoint"), new SalePaymentConfirmedCommand
                        {
                            SaleId = context.Saga.SaleId
                        });
                    })
                    .TransitionTo(Completed)
                    .Finalize(),
                When(PaymentFailed)
                    .Then(context =>
                    {
                        _logger.LogWarning("Falha no pagamento para venda {SaleId}. Cancelando venda...", context.Message.SaleId);
                        if (context.Saga.PaymentCancelled.HasValue) return;
                        context.Saga.PaymentCancelled = DateTime.UtcNow;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Send(new Uri("queue:SaleSagaEndpoint"), new SalePaymentCancelledCommand
                        {
                            SaleId = context.Saga.SaleId
                        });
                    })
                    .TransitionTo(Failed)
                    .Finalize());
        }
    }

}