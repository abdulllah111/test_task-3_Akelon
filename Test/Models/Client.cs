using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Client
    {
        public int Id {get; set;}
        public required string CompanyName {get; set;}
        public required string Address {get;set;}
        public required string Contact {get;set;}
    }
}