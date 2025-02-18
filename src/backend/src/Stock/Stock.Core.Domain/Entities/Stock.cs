using Base.Core.Domain.Common;

//TODO PROBLEMA DE NAMESPACE NÃO ESTAVA PEGANDO CORRETAMENTE OU IDENTIFICANDO EM OUTROS PACOTES
namespace StockCoreDomainEntitties
{

    /// <summary>
    /// Todo create itens where is the log of insert products in stock
    /// </summary>
    public class Stock : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

