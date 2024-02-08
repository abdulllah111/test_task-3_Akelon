using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Interfaces
{
    public interface IClientRepository
    {
        Task<IList<Client>> GetAll();
        Task<Client> Get(int id);
        Task<Client> Update(Client client);

    }
}