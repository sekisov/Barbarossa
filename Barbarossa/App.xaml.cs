using Barbarossa.ViewModels;
using Barbarossa.Views;

namespace Barbarossa;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Получаем ViewModel через DI
        var viewModel = MauiProgram.Services.GetRequiredService<BookingViewModel>();

        // Создаем страницу, передавая ViewModel
        MainPage = new NavigationPage(new BookingPage(viewModel));
    }
}