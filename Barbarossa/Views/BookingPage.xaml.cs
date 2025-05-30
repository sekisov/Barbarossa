using Barbarossa.ViewModels;

namespace Barbarossa.Views
{
    public partial class BookingPage : ContentPage
    {
        public BookingPage(BookingViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}