using Barbarossa.ViewModels;
using Microsoft.Maui.Controls;

namespace Barbarossa.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(UserViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is UserViewModel vm)
            {
                vm.UpdateUserData();
            }
        }
    }
}