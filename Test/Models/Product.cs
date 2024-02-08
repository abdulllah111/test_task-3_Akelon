using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; } 
        public required string Unit { get; set; }
        public decimal UnitPrice { get; set; }

    }
}