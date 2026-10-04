using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Features.Product.Commands.CreateProduct
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
