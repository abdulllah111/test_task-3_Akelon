using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Test.Models;

namespace Test.Data.FromExcel
{
    public class ExcelContext : IExcelContext, IDisposable
    {
        private XLWorkbook _workbook;
        private readonly Dictionary<Type, int> _sheetIndexes = new();


        public Task<List<Product>> Products { get; set; }
        public Task<List<Client>> Clients { get; set; }
        public Task<List<Order>> Orders { get; set; }


        public ExcelContext(string filePath)
        {
            _workbook = new XLWorkbook(filePath);
            AddSheetIndex<Product>(1);
            AddSheetIndex<Client>(2);
            AddSheetIndex<Order>(3);

            Products = Read<Product>();
            Clients = Read<Client>();
            Orders = Read<Order>();
        }




        private void AddSheetIndex<T>(int sheetIndex)
        {
            _sheetIndexes[typeof(T)] = sheetIndex;
        }

        private async Task<List<T>> Read<T>()
        {
            List<T> data = new List<T>();

            if (!_sheetIndexes.ContainsKey(typeof(T)))
            {
                throw new ArgumentException($"Лист с данными типа {typeof(T).Name} не был добавлен в контекст.");
            }

            var worksheet = _workbook.Worksheet(_sheetIndexes[typeof(T)]);

            // Получаем свойства типа T
            var properties = typeof(T).GetProperties();

            // Пропускаем первую строку с заголовками
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                T item = Activator.CreateInstance<T>();

                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    var cell = row.Cell(i + 1);

                    // Получаем значение ячейки в виде строки и преобразуем его к типу свойства объекта
                    var cellValue = cell.Value.ToString();
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var convertedValue = Convert.ChangeType(cellValue, propertyType);

                    // Присваиваем значение свойству объекта
                    property.SetValue(item, convertedValue);
                }

                data.Add(item);
            }

            return data;
        }
        public void Update<T>(List<T> data)
        {
            if (!_sheetIndexes.ContainsKey(typeof(T)))
            {
                throw new ArgumentException($"Лист с данными типа {typeof(T).Name} не был добавлен в контекст.");
            }

            var worksheet = _workbook.Worksheet(_sheetIndexes[typeof(T)]);

            // Получаем свойства типа T
            var properties = typeof(T).GetProperties();

            // Определяем, на каком столбце находится свойство Id
            int idColumnIndex = -1;
            for (int i = 1; i <= properties.Length; i++)
            {
                if (properties[i - 1].Name == "Id")
                {
                    idColumnIndex = i;
                    break;
                }
            }

            if (idColumnIndex == -1)
            {
                throw new ArgumentException("Тип T не содержит свойства Id.");
            }

            // Проходим по списку объектов и обновляем данные в Excel
            foreach (var item in data)
            {
                int rowIndex = 2; // Начинаем считывание данных со второй строки (первая строка - заголовки)

                // Находим строку с данными, соответствующими текущему объекту
                while (!worksheet.Cell(rowIndex, idColumnIndex).IsEmpty() &&
                       worksheet.Cell(rowIndex, idColumnIndex).GetValue<int>() != (int)item!.GetType().GetProperty("Id")!.GetValue(item)!)
                {
                    rowIndex++;
                }

                // Обновляем данные в Excel
                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    var value = property.GetValue(item)?.ToString(); // Получаем значение свойства как строку
                    worksheet.Cell(rowIndex, i + 1).SetValue(value);
                }
            }
        }

        public void SaveChanges()
        {
            _workbook.Save();
        }

        public void Dispose()
        {
            _workbook.Dispose();
        }
    }
}