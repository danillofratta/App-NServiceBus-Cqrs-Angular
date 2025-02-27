using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Sagas;
using Sale.Core.Domain.Contracts.Command;
using Sale.Core.Domain.Contracts.Event;

namespace Sale.Core.Domain.SagaRebus
{
    public class SaleSaga : Rebus.Sagas.Saga<SaleSagaData>,
        IAmInitiatedBy<SaleCreatedEvent>,
        Rebus.Handlers.IHandleMessages<StockConfirmedEvent>,
        Rebus.Handlers.IHandleMessages<StockInsufficientEvent>,
        Rebus.Handlers.IHandleMessages<PaymentConfirmedEvent>,
        Rebus.Handlers.IHandleMessages<PaymentFailEvent>
    {
        private readonly ILogger<SaleSaga> _logger;
        private readonly IBus _bus;

        public SaleSaga(ILogger<SaleSaga> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<SaleSagaData> config)
        {
            config.Correlate<SaleCreatedEvent>(msg => msg.SaleId, saga => saga.SaleId);
            config.Correlate<StockConfirmedEvent>(msg => msg.SaleId, saga => saga.SaleId);            
            config.Correlate<StockInsufficientEvent>(msg => msg.SaleId, saga => saga.SaleId);
            config.Correlate<PaymentConfirmedEvent>(msg => msg.SaleId, saga => saga.SaleId);
            config.Correlate<PaymentFailEvent>(msg => msg.SaleId, saga => saga.SaleId);
        }

        public async Task Handle(SaleCreatedEvent message)
        {
            _logger.LogInformation("Iniciando saga para venda {SaleId}. Verificando estoque...", message.SaleId);

            try
            {
                if (Data.ProcessStarted.HasValue)
                    return;

                Data.ProcessStarted = DateTime.UtcNow;
                
                await _bus.Send(new ReserveStockCommand
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

        public async Task Handle(StockConfirmedEvent message)
        {
            _logger.LogInformation("Estoque confirmado para venda {SaleId}. Processando pagamento...", message.SaleId);

            try
            {
                if (Data.StockConfirmed.HasValue)
                {
                    _logger.LogWarning("Stock already confirmed for sale {SaleId}", message.SaleId);
                    return;
                }

                if (Data.PaymentRequested.HasValue || Data.PaymentRequestId.HasValue)
                {
                    _logger.LogWarning("Payment already requested for sale {SaleId}", message.SaleId);
                    return;
                }

                Data.StockConfirmed = DateTime.UtcNow;

                await _bus.Send(new SaleStockConfirmedCommand
                {   
                    SaleId = message.SaleId
                });

                Data.PaymentRequestId = Guid.NewGuid();
                Data.PaymentRequested = DateTime.UtcNow;

                await _bus.Send(new ProcessPaymentCommand
                {
                    SaleId = message.SaleId,
                    PaymentRequestId = Data.PaymentRequestId.Value,
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

        public async Task Handle(StockInsufficientEvent message)
        {
            _logger.LogWarning("Estoque insuficiente para venda {SaleId}. Cancelando venda...", message.SaleId);

            try
            {
                if (Data.StockInsufficient.HasValue)
                    return;

                Data.StockInsufficient = DateTime.UtcNow;

                await _bus.Send(new SaleStockInsufficienCommand
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

        public async Task Handle(PaymentConfirmedEvent message)
        {
            _logger.LogInformation("Pagamento confirmado para venda {SaleId}. Finalizando venda...", message.SaleId);

            try
            {
                if (Data.PaymentConfirmed.HasValue)
                    return;

                Data.PaymentConfirmed = DateTime.UtcNow;

                await _bus.Send(new SalePaymentConfirmedCommand
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

        public async Task Handle(PaymentFailEvent message)
        {
            _logger.LogWarning("Falha no pagamento para venda {SaleId}. Cancelando venda...", message.SaleId);

            try
            {
                if (Data.PaymentCancelled.HasValue)
                    return;

                await _bus.Send(new SalePaymentCancelledCommand
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