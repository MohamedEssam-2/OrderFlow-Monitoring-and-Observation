using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Features.Customer.Queries.GetAllCustomer
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
