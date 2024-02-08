using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Data.FromExcel
{
    public interface IExcelContext
    {
        Task<List<Product>> Products {get; set;}
        Task<List<Client>> Clients {get; set;}
        Task<List<Order>> Orders {get; set;}

        void Update<T>(List<T> data);
        void SaveChanges();
    }
}