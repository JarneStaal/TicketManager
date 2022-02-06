using Android.App;
using Android.Content;
using System.Threading.Tasks;

namespace TicketManager.Droid.Resources.drawable
{
    [Activity(Label = "TicketManager", Icon = "@drawable/logo_splash_v3_cropped", MainLauncher = true, Theme = "@style/MyTheme.Splash", NoHistory = true)]
    internal class SplashActivity : Activity
    {
        protected override async void OnResume()
        {
            base.OnResume();
            await Startup();
        }

        async Task Startup()
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}