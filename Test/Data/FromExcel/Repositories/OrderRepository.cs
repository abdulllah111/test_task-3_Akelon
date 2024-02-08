using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Interfaces;
using Test.Models;

namespace Test.Data.FromExcel.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private IExcelContext _excelContext;

        public OrderRepository(ExcelContext excelContext)
        {
            _excelContext = excelContext;
        }


        public async Task<IList<Order>> GetAll()
        {
            return await _excelContext.Orders;
        }
    }
}