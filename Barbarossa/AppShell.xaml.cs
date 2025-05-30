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
    }
}
