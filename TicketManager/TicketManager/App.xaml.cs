using TicketManager.Data;
using TicketManager.Views.Account;
using TicketManager.Views.Misc;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TicketManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TicketPage();
        }

        protected override async void OnStart()
        {
            var token = Preferences.Get("MyFirebaseRefreshToken", "");
            if (!string.IsNullOrEmpty(token))
            {
                var user = await Authentication.GetUser();

                //Assigning this so we don't need to request the LocalId for every feedback message
                TicketChat.FBUserLocalId = user.User.LocalId;
                await TicketAction.GatherAllTickets();
                await Database.RetrieveCurrentUserTickets();

                MainPage = new UserControlPage();
            }
            else
            {
                MainPage = new LoginRegisterPage();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
