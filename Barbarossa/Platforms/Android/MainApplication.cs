using Android.App;
using Android.Runtime;
using System.Net;

namespace Barbarossa.Platforms.Android
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            // Для обхода проблем с SSL/TLS на Android
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}