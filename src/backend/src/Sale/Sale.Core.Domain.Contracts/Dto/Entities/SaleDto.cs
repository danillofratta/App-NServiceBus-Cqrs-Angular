namespace Sale.Core.Domain.Contracts.Dto.Entities
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public List<SaleItensDto> SaleItens { get; set; } = new();
    }
}
