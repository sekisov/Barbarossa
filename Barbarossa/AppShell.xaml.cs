using Barbarossa.Services;
using Barbarossa.Views;

namespace Barbarossa
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(BookingPage), typeof(BookingPage));
        }
        private async void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            var userService = Handler.MauiContext.Services.GetService<IUserService>();
            await userService.InitializeAsync();

            var authRoutes = new[] { "ProfilePage", "BookingPage" }; // Защищенные страницы

            if (!userService.IsAuthenticated && authRoutes.Any(r => e.Target.Location.OriginalString.Contains(r)))
            {
                e.Cancel();
                await Shell.Current.GoToAsync($"//LoginPage?ReturnUrl={e.Target.Location.OriginalString}");
            }
        }
    }
}
