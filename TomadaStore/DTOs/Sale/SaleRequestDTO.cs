using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomadaStore.Models.DTOs.SaleRequestDTO
{
    public class SaleRequestDTO
    {
        public List<string> ProductsId { get; init; } = new();
    }
}
