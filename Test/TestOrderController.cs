using Test.Controllers;
using Test.Data.FromExcel;
using Test.Data.FromExcel.Repositories;

namespace Test
{
    public static class TestOrderController
    {
        public static async void Test()
        {
            var path = "data.xlsx";

            var productRpository = new ProductRepository(new ExcelContext(path));

            var products = await productRpository.GetAll();

            foreach (var product in products)
            {
                System.Console.WriteLine(product.Name);
            }

            var context = new ExcelContext(path);

            var orderRepository = new OrderRepository(context);
            var productRepository = new ProductRepository(context);
            var clientRepository = new ClientRepository(context);

            var orderController = new OrderController(orderRepository, productRepository, clientRepository);

            var OrdersInfoByProductName = await orderController.GetOrdersInfoByProductName("Молоко");

            System.Console.WriteLine(OrdersInfoByProductName[0].ClientName);


            var updateClient = await orderController.UpdateClientContact("ООО Надежда", "Хамдамов");

            System.Console.WriteLine(updateClient.Contact);

            var goldenClient = await orderController.GetGoldenClient(2023, 6);

            System.Console.WriteLine(goldenClient.Contact);
        }
    }
}