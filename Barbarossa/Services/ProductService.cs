using Barbarossa.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace Barbarossa.Services
{
    public class ProductService : IProductService
    {
        public async Task<ObservableCollection<Product>> GetProductsAsync()
        {
            try
            {
                // Имитация загрузки данных
                await Task.Delay(500);

                // Возвращаем ObservableCollection напрямую
                return new ObservableCollection<Product>
                {
                    new Product
                    {
                        Id = "1", // Явно указываем string
                        Name = "Набор для ухода за бородой Premium",
                        Description = "Полный набор для профессионального ухода",
                        Price = 2990,
                        ImageUrl = "beard_kit_premium.jpg"
                    },
                    new Product
                    {
                        Id = "2", // Явно указываем string
                        Name = "Масло для бороды Дубовый лес",
                        Description = "Натуральное масло с древесным ароматом",
                        Price = 990,
                        ImageUrl = "beard_oil_oak.jpg"
                    },
                    new Product
                    {
                        Id = "3",
                        Name = "Расческа из кабана",
                        Description = "Натуральная щетина для идеальной укладки",
                        Price = 1590,
                        ImageUrl = "boar_brush.jpg"
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в ProductService: {ex.Message}");
                return new ObservableCollection<Product>();
            }
        }
    }
}