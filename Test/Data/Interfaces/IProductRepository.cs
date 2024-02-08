using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Interfaces
{
    public interface IProductRepository
    {
        Task<IList<Product>> GetAll();
    }
}