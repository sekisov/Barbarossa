using Barbarossa.Models;
using System.Collections.ObjectModel;

namespace Barbarossa.Services
{
    public class FakeProductService : IProductService
    {
        public async Task<ObservableCollection<Product>> GetProductsAsync()
        {
            await Task.Delay(500); // Имитация загрузки

            return new ObservableCollection<Product>
            {
                new Product
                {
                    Id = "1",
                    Name = "Набор для бороды Premium",
                    Description = "Масло, расческа и ножницы в подарочной упаковке",
                    Price = 2490,
                    ImageUrl = "https://barbarossa.top/wp-content/uploads/2023/06/barbarossa_cosmetics.jpg" // Реальное изображение
                },
                new Product
                {
                    Id = "2",
                    Name = "Масло для бороды",
                    Description = "Натуральное масло с ароматом кедра",
                    Price = 990,
                    ImageUrl = "https://www.letu.ru/common/img/pim/2025/01/TL_d2e1258c-a6c9-4482-8ae6-f7a981fd1d76.png"
                },
                new Product
                {
                    Id = "3",
                    Name = "Деревянная расческа",
                    Description = "Ручная работа из массива дуба",
                    Price = 1590,
                    ImageUrl = "https://pcdn.goldapple.ru/p/p/19000026922/web/696d674d61696e5f39646434396630393335313534353963613964336637663563313931663833358dcc76b839a5768.jpg"
                },
                new Product
                {
                    Id = "4",
                    Name = "Масло для усов",
                    Description = "Сильная фиксация с натуральным составом",
                    Price = 790,
                    ImageUrl = "https://cdn1.ozone.ru/s3/multimedia-1-h/7292989313.jpg"
                }
            };
        }
    }
}