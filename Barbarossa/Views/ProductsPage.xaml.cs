using Barbarossa.ViewModels;

namespace Barbarossa.Views
{
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage(ProductsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Дополнительная загрузка при появлении страницы
            if (BindingContext is ProductsViewModel vm)
            {
                vm.LoadProductsCommand.Execute(null);
            }
        }
    }
}