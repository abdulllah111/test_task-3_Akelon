using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Interfaces;
using Test.Models;

namespace Test.Data.FromExcel.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private IExcelContext _excelContext;

        public ClientRepository(ExcelContext excelContext)
        {
            _excelContext = excelContext;
        }


        public async Task<Client> Get(int id)
        {
            var clients = await _excelContext.Clients;
            return clients.FirstOrDefault(client => client.Id == id)!;
        }

        public async Task<IList<Client>> GetAll()
        {
            return await _excelContext.Clients;
        }

        public async Task<Client> Update(Client client)
        {
            var clients = await _excelContext.Clients;

            // Находим клиента по идентификатору
            var existingClient = clients.FirstOrDefault(c => c.Id == client.Id);
            if (existingClient == null)
            {
                throw new Exception("Клиент с указанным идентификатором не найден.");
            }

            // Обновляем данные клиента
            existingClient.CompanyName = client.CompanyName;
            existingClient.Contact = client.Contact;
            existingClient.Address = client.Address;

            // Сохраняем изменения в базе данных(Excel) и возвращаем обновленный клиент.
            _excelContext.Update(clients);
            _excelContext.SaveChanges();
            return existingClient;
        }
    }
}