using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto.Entities
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public List<SaleItensDto> SaleItens { get; set; } = new();
    }
}
