using Barbarossa.Converters;
using Barbarossa.Services;
using Barbarossa.ViewModels;
using Barbarossa.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace Barbarossa;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
       
        // Регистрация сервисов
        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddTransient<BookingViewModel>();
        //builder.Services.AddSingleton<IProductService, ProductService>();
        builder.Services.AddSingleton<IProductService, FakeProductService>();
        builder.Services.AddSingleton<IUserService, UserService>();

        builder.Services.AddSingleton<ProductsViewModel>();
        builder.Services.AddSingleton<ProductsPage>();

        builder.Services.AddSingleton<UserViewModel>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<BookingPage>();

        // Регистрация конвертеров
        builder.Services.AddSingleton<NullToBoolConverter>();
        builder.Services.AddSingleton<GreaterThanZeroConverter>();
        builder.Services.AddSingleton<TimeSlotBackgroundConverter>();
        builder.Services.AddSingleton(SecureStorage.Default);


        builder.Services.AddSingleton<AppointmentService>();

        builder.Services.AddSingleton<BoolToTextConverter>();
        builder.Services.AddSingleton<BoolToColorConverter>();
        builder.Services.AddSingleton<BoolToCommandConverter>();

        var app = builder.Build();
        Services = app.Services;
        return app;
    }
}