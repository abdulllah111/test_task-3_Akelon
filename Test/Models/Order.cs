using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public int OrderNumber { get; set; }
        public int Quantity { get; set; }
        public DateTime PostingDate { get; set; }

    }
}