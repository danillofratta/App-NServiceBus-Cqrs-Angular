using Microsoft.Extensions.Logging;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Contracts.Event;

namespace Sale.Core.Domain.SagaNServiceBus
{
    public class SaleSaga : Saga<SaleSagaData>,
        IAmStartedByMessages<SaleCreatedEvent>,
        IHandleMessages<StockConfirmedEvent>,
        IHandleMessages<StockInsufficientEvent>,
        IHandleMessages<PaymentConfirmedEvent>,
        IHandleMessages<PaymentFailEvent>
    {
        private readonly ILogger<SaleSaga> _logger;

        public SaleSaga( ILogger<SaleSaga> logger)
        {
            _logger = logger;
            _logger.LogInformation("Nova instância de SaleSaga criada com ID de instância {InstanceId}", Guid.NewGuid());
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SaleSagaData> mapper)
        {
            //mapper.MapSaga(sagaData => sagaData.SaleId)
            //    .ToMessage<SaleCreatedEvent>(message => message.SaleId)
            //    .ToMessage<StockConfirmedEvent>(message => message.SaleId)
            //    .ToMessage<StockInsufficientEvent>(message => message.SaleId)
            //    .ToMessage<PaymentConfirmedEvent>(message => message.SaleId)
            //    .ToMessage<PaymentFailEvent>(message => message.SaleId);
            mapper.ConfigureMapping<SaleCreatedEvent>(message => message.SaleId)
            .ToSaga(sagaData => sagaData.SaleId);
            mapper.ConfigureMapping<StockConfirmedEvent>(message => message.SaleId)
                .ToSaga(sagaData => sagaData.SaleId);
            mapper.ConfigureMapping<StockInsufficientEvent>(message => message.SaleId)
                .ToSaga(sagaData => sagaData.SaleId);
            mapper.ConfigureMapping<PaymentConfirmedEvent>(message => message.SaleId)
                .ToSaga(sagaData => sagaData.SaleId);
            mapper.ConfigureMapping<PaymentFailEvent>(message => message.SaleId)
                .ToSaga(sagaData => sagaData.SaleId);
        }

        public async Task Handle(SaleCreatedEvent message, IMessageHandlerContext context)
        {
            _logger.LogInformation("Iniciando saga para venda {SaleId}. Verificando estoque...", message.SaleId);

            try
            {
                if (Data.ProcessStarted.HasValue)
                    return;

                Data.ProcessStarted = DateTime.UtcNow;

                await context.Send("StockSagaEndpoint", new ReserveStockCommand
                {
                    SaleId = message.SaleId,
                    SaleItens = message.SaleItens
                });
                _logger.LogInformation("Comando ReserveStockCommand enviado para venda {SaleId}", message.SaleId);

                _logger.LogDebug("Comando ReserveStockCommand enviado para venda {SaleId}", message.SaleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar SaleCreatedEvent para venda {SaleId}", message.SaleId);
                throw;
            }
        }

        public async Task Handle(StockConfirmedEvent message, IMessageHandlerContext context)
        {
            _logger.LogInformation("Estoque confirmado para venda {SaleId}. Processando pagamento...", message.SaleId);

            try
            {
                if (Data.StockConfirmed.HasValue)
                {
                    _logger.LogWarning("Stock already confirmed for sale {SaleId}", message.SaleId);
                    return;
                }

                if (Data.PaymentRequested.HasValue)
                {
                    _logger.LogWarning("Payment already requested for sale {SaleId}", message.SaleId);
                    return;
                }

                Data.StockConfirmed = DateTime.UtcNow;

                await context.Send("SaleSagaEndpoint", new SaleStockConfirmedCommand
                {   
                    SaleId = message.SaleId
                });

                Data.PaymentRequested = DateTime.UtcNow;

                await context.Send("PaymentSagaEndpoint", new ProcessPaymentCommand
                {
                    SaleId = message.SaleId,
                    Valor = message.Total
                });


                _logger.LogDebug("Comando ProcessPaymentCommand enviado para venda {SaleId}", message.SaleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar StockConfirmedEvent para venda {SaleId}", message.SaleId);
                throw;
            }
        }

        public async Task Handle(StockInsufficientEvent message, IMessageHandlerContext context)
        {
            _logger.LogWarning("Estoque insuficiente para venda {SaleId}. Cancelando venda...", message.SaleId);

            try
            {
                if (Data.StockInsufficient.HasValue)
                    return;

                Data.StockInsufficient = DateTime.UtcNow;

                await context.Send("SaleSagaEndpoint", new SaleStockInsufficienCommand
                {
                    SaleId = message.SaleId
                });
                MarkAsComplete();

                _logger.LogInformation("Venda {SaleId} cancelada por falta de estoque", message.SaleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar StockInsufficientEvent para venda {SaleId}", message.SaleId);
                throw;
            }

            await Task.CompletedTask;
        }

        public async Task Handle(PaymentConfirmedEvent message, IMessageHandlerContext context)
        {
            _logger.LogInformation("Pagamento confirmado para venda {SaleId}. Finalizando venda...", message.SaleId);

            try
            {
                if (Data.PaymentConfirmed.HasValue)
                    return;

                Data.PaymentConfirmed = DateTime.UtcNow;

                await context.Send("SaleSagaEndpoint", new SalePaymentConfirmedCommand
                {
                    SaleId = message.SaleId
                });
                MarkAsComplete();

                _logger.LogInformation("Venda {SaleId} finalizada com sucesso", message.SaleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar PaymentConfirmedEvent para venda {SaleId}", message.SaleId);
                throw;
            }

            await Task.CompletedTask;
        }

        public async Task Handle(PaymentFailEvent message, IMessageHandlerContext context)
        {
            _logger.LogWarning("Falha no pagamento para venda {SaleId}. Cancelando venda...", message.SaleId);

            try
            {
                if (Data.PaymentCancelled.HasValue)
                    return;

                await context.Send("SaleSagaEndpoint", new SalePaymentCancelledCommand
                {
                    SaleId = message.SaleId
                });
                MarkAsComplete();

                _logger.LogInformation("Venda {SaleId} cancelada por falha no pagamento", message.SaleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar PaymentFailEvent para venda {SaleId}", message.SaleId);
                throw;
            }

            await Task.CompletedTask;
        }
    }
}