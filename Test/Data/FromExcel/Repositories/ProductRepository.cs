using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Interfaces;
using Test.Models;

namespace Test.Data.FromExcel.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private IExcelContext _excelContext;

        public ProductRepository(ExcelContext excelContext)
        {
            _excelContext = excelContext;            
        }


        public async Task<IList<Product>> GetAll()
        {
            return await _excelContext.Products;
        }
    }
}