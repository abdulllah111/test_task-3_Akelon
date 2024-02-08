using Test.Controllers;
using Test.Data.FromExcel;
using Test.Data.FromExcel.Repositories;


public class Program
{

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Введите путь к файлу с данными:");
        string? filePath = Console.ReadLine();

        // Проверяем, что введенный путь не пустой
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("Некорректный путь к файлу.");
            return;
        }

        var context = new ExcelContext(filePath);

        var orderRepository = new OrderRepository(context);
        var productRepository = new ProductRepository(context);
        var clientRepository = new ClientRepository(context);

        var orderController = new OrderController(orderRepository, productRepository, clientRepository);

        bool Work = true;

        while (Work == true)
        {
            System.Console.WriteLine($"\n\nМеню: Введите 1 - информации о клиентах, заказавших товар\n" +
            "Введите 2 - изменение контактного лица клиента\nВведите 3 - клиента с наибольшим количеством заказов\nВедите 4 для выхода\n\n");

            int selectedOption = int.Parse(Console.ReadLine()!);

            switch (selectedOption)
            {
                case 1:
                    System.Console.WriteLine("Введите название товара:\n");
                    string? productName = Console.ReadLine();
                    var OrdersInfoByProductName = await orderController.GetOrdersInfoByProductName(productName!);
                    foreach (var order in OrdersInfoByProductName)
                    {
                        System.Console.WriteLine($"{order.ClientName} {order.Quantity} {order.Price} {order.OrderDate}\n");
                    }
                    break;
                case 2:
                    System.Console.WriteLine("Введите наименование организации:\n");
                    string? companyName = Console.ReadLine();
                    System.Console.WriteLine("Введите ФИО нового контактного лица:\n");
                    string? newContactName = Console.ReadLine();
                    var updateClient = await orderController.UpdateClientContact(companyName!, newContactName!);
                    System.Console.WriteLine($"Ok:  {updateClient.Contact}");
                    break;
                case 3:
                    System.Console.WriteLine("Введите год:\n");
                    int year = int.Parse(Console.ReadLine()!);
                    System.Console.WriteLine("Введите месяц:\n");
                    int month = int.Parse(Console.ReadLine()!);
                    var goldenClient = await orderController.GetGoldenClient(year, month);
                    System.Console.WriteLine(goldenClient.CompanyName);
                    break;
                case 4:
                    Work = false;
                    break;
                default:
                    Console.WriteLine("Некорректный ввод");
                    break;
            }
        }
    }
}


