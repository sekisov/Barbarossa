using Barbarossa.Models;
using Barbarossa.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Barbarossa.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductService _productService;

        [ObservableProperty]
        private ObservableCollection<Product> _products = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        private bool _isLoading;

        public bool IsNotLoading => !IsLoading;

        public ProductsViewModel(IProductService productService)
        {
            _productService = productService;
            LoadProductsCommand = new AsyncRelayCommand(LoadProductsAsync);

            // Явно запускаем загрузку при создании ViewModel
            LoadProductsCommand.Execute(null);
        }

        public IAsyncRelayCommand LoadProductsCommand { get; }

        private async Task LoadProductsAsync()
        {
            IsLoading = true;
            try
            {
                var products = await _productService.GetProductsAsync();
                Products.Clear();
                foreach (var product in products)
                {
                    Debug.WriteLine($"Загружен товар: {product.Name}"); // Логируем в консоль
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task BuyProduct(Product product)
        {
            if (product == null)
                return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Подтверждение",
                $"Добавить \"{product.Name}\" за {product.Price}₽ в корзину?",
                "Да", "Нет");

            if (confirm)
            {
                // Логика добавления в корзину
                await Shell.Current.DisplayAlert("Успех", "Товар добавлен в корзину", "OK");
            }
        }
    }
}