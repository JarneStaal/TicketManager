using System;
using TicketManager.Data;
using TicketManager.Views.Account;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager.Views.Misc
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserControlPage : Shell
    {
        public UserControlPage()
        {
            InitializeComponent();
            GetProfileInformationAndRefreshToken();
            CheckIfAdmin();
        }

        private async void CheckIfAdmin()
        {
            if (await Authentication.IsAdmin())
            {
                statisticsControl.IsVisible = true;
                feedbackControl.IsVisible = false;
            }
        }

        private async void GetProfileInformationAndRefreshToken()
        {
            var savedfirebaseauth = await Authentication.GetUser();
            MyUserName.Text = $"Welcome {savedfirebaseauth.User.Email} !";
        }

        private void Logout_OnClicked(object sender, EventArgs e)
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            Application.Current.MainPage = new LoginRegisterPage();
        }
    }
}