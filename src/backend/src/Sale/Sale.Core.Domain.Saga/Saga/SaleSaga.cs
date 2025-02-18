using Sale.Core.Domain.Saga.Events;
using Sale.Core.Domain.Saga.Commands;

namespace Sale.Core.Domain.Saga
{
    public class SaleSaga : Saga<SaleSagaData>,
    IAmStartedByMessages<SaleCreatedEvent>,
    IHandleMessages<StockConfirmedEvent>,
    IHandleMessages<StockInsufficientEvent>,
    IHandleMessages<PaymentConfirmedEvent>,
    IHandleMessages<PaymentFailEvent>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SaleSagaData> mapper)
        {
            mapper.ConfigureMapping<SaleCreatedEvent>(msg => msg.SaleId).ToSaga(saga => saga.SaleId);
            mapper.ConfigureMapping<StockConfirmedEvent>(msg => msg.SaleId).ToSaga(saga => saga.SaleId);
            mapper.ConfigureMapping<StockInsufficientEvent>(msg => msg.SaleId).ToSaga(saga => saga.SaleId);
            mapper.ConfigureMapping<PaymentConfirmedEvent>(msg => msg.SaleId).ToSaga(saga => saga.SaleId);
            mapper.ConfigureMapping<PaymentFailEvent>(msg => msg.SaleId).ToSaga(saga => saga.SaleId);
        }

        public async Task Handle(SaleCreatedEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Saga started to be sold {message.SaleId}. Checking Stock...");

            await context.Send(new ReserveStockCommand
            {
                SaleId = message.SaleId,
                SaleItens = message.SaleItens
            });
        }

        public async Task Handle(StockConfirmedEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Inventory confirmed for Sale {message.SaleId}. Processing payment...");

            await context.Send(new Sale.Core.Domain.Saga.Commands.ProcessPaymentCommand
            {
                SaleId = message.SaleId,
                Valor = message.Total
            });
        }

        public async Task Handle(StockInsufficientEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Inventory sotck for Sale {message.SaleId}. Canceling sale...");

            MarkAsComplete();
            //await context.Publish(new SaleCancelledEvent { SaleId = message.SaleId });
        }

        public async Task Handle(PaymentConfirmedEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Payment confirmed for Sale {message.SaleId}. Finalizing sale ..");

            MarkAsComplete();
            //await context.Publish(new SaleFinishedEvent { SaleId = message.SaleId });
        }

        public async Task Handle(PaymentFailEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Payment failed for Sale {message.SaleId}. Canceling sale...");

            MarkAsComplete();
            //await context.Publish(new SaleCancelledEvent { SaleId = message.SaleId });
        }
    }
}
