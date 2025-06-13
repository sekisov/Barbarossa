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
        builder.Services.AddTransient<BookingPage>();


        // Регистрация конвертеров
        builder.Services.AddSingleton<NullToBoolConverter>();
        builder.Services.AddSingleton<GreaterThanZeroConverter>();
        builder.Services.AddSingleton<TimeSlotBackgroundConverter>();
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<UserViewModel>();
        builder.Services.AddTransient<ProfilePage>();

        var app = builder.Build();
        Services = app.Services;
        return app;
    }
}