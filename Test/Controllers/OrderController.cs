using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Interfaces;
using Test.Models;

namespace Test.Controllers
{
    public class OrderController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;

        public OrderController(IOrderRepository orderRepository, IProductRepository productRepository, IClientRepository clientRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
        }

        public async Task<List<OrderInfoVM>> GetOrdersInfoByProductName(string productName)
        {
            var orders = await _orderRepository.GetAll();
            var products = await _productRepository.GetAll();
            var clients = await _clientRepository.GetAll();

            var ordersInfo = new List<OrderInfoVM>();

            // Получаем информацию о продукте
            var product = products.FirstOrDefault(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));

            // Находим все заявки, в которых присутствует товар с заданным наименованием
            var relevantOrders = orders.Where(o => o.ProductId == product!.Id);

            // Для каждого такой такой заявки добавляем информацию в список
            foreach (var order in relevantOrders)
            {
                if (product != null)
                {
                    // Создаем объект OrderInfoVM и добавляем его в список
                    ordersInfo.Add(new OrderInfoVM
                    {
                        ClientName = clients.FirstOrDefault(c => c.Id == order.ClientId)!.CompanyName,
                        Quantity = order.Quantity,
                        Price = order.Quantity * product.UnitPrice, // Полная цена заявки
                        OrderDate = order.PostingDate
                    });
                }
            }

            return ordersInfo;
        }

        public async Task<Client> UpdateClientContact(string companyName, string newContactName)
        {
            var clients = await _clientRepository.GetAll();
            var client = clients.FirstOrDefault(c => c.CompanyName.Equals(companyName, StringComparison.OrdinalIgnoreCase));
            if (client != null)
            {
                client.Contact = newContactName;
                await _clientRepository.Update(client);
            }
            return client!;
        }

        public async Task<Client> GetGoldenClient(int year, int month)
        {
            var orders = await _orderRepository.GetAll();
            var selectedOrders = orders.Where(o => o.PostingDate.Year == year && o.PostingDate.Month == month).ToList();

            var clientOrdersCount = new Dictionary<int, int>();

            foreach (var order in selectedOrders)
            {
                if (clientOrdersCount.ContainsKey(order.ClientId))
                {
                    clientOrdersCount[order.ClientId]++;
                }
                else
                {
                    clientOrdersCount[order.ClientId] = 1;
                }
            }

            if (clientOrdersCount.Count == 0)
            {
                throw new Exception($"Нет заказов за {month}/{year}");
            }

            var goldenClientId = clientOrdersCount.OrderByDescending(kv => kv.Value).First().Key;

            var goldenClient = await _clientRepository.Get(goldenClientId);

            return goldenClient;
        }
    }
}