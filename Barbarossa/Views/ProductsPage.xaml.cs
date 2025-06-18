using Barbarossa.ViewModels;

namespace Barbarossa.Views
{
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage()
        {
            InitializeComponent();

            // Убедитесь, что BindingContext установлен
            BindingContext = MauiProgram.Services.GetService<ProductsViewModel>();
        }
    }
}