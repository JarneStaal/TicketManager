using Firebase.Auth;
using System;
using System.Threading.Tasks;
using TicketManager.Data;
using TicketManager.Views.Misc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginRegisterPage : ContentPage
    {
        public LoginRegisterPage()
        {
            Database.RegisterSyncfusionLicense();
            InitializeComponent();
        }

        private async void Login_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(emailEntry.Text) && !string.IsNullOrEmpty(passwordEntry.Text))
                {
                    await StartLoadingVisual();
                    await Authentication.SignInUser(emailEntry.Text, passwordEntry.Text);
                    var user = await Authentication.GetUser();
                    if (user != null)
                    {
                        TicketChat.FBUserLocalId = user.User.LocalId;
                        await Database.RetrieveCurrentUserTickets();
                        if (await Authentication.IsAdmin())
                        {
                            await TicketAction.GatherAllTickets();
                        }

                        Application.Current.MainPage = new UserControlPage();
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Please fill in both fields", "OK");
                    logDiv.IsVisible = true;
                }
            }
            catch (Exception)
            {
                await StartLoadingVisual();

                if (await Application.Current.MainPage
                        .DisplayAlert("Alert", "Invalid email or password", "RESET PASSWORD?", "CANCEL"))
                {
                    try
                    {
                        await Authentication.authProvider.SendPasswordResetEmailAsync(emailEntry.Text, Authentication.GetTwoLetterLanguageName());
                        await DisplayAlert("Alert", "Please check your email.", "OK");
                    }
                    catch (Exception)
                    {
                        await StartLoadingVisual();
                        await Application.Current.MainPage.DisplayAlert("Error", $"Email is not registered.", "OK");
                        StopLoadingVisual();
                    }
                }
                StopLoadingVisual();
            }
        }



        private async void Register_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                await Authentication.RegisterUser(emailEntry.Text, passwordEntry.Text);
                await StartLoadingVisual();
                await DisplayAlert("Alert", "Login with the account you just created", "OK");
                StopLoadingVisual();
            }
            catch (FirebaseAuthException ex)
            {
                logDiv.IsVisible = false;
                loadingDiv.IsVisible = true;
                await Application.Current.MainPage.DisplayAlert("Error", $"{GetExceptionReason(ex)}", "OK");
                StopLoadingVisual();
            }
        }

        private string GetExceptionReason(FirebaseAuthException ex)
        {
            var reason = ex.Reason;
            string fullreason = string.Empty;
            switch (reason)
            {
                case AuthErrorReason.InvalidEmailAddress:
                    fullreason = "Your email is invalid. \nPlease provide a valid email.";
                    emailEntry.Text = string.Empty;
                    break;
                case AuthErrorReason.WeakPassword:
                    fullreason = "Your password is too weak. \nYou need atleast 6 characters.";
                    passwordEntry.Text = string.Empty;
                    passwordEntry.Focus();
                    break;
                case AuthErrorReason.MissingPassword:
                    fullreason = "Please fill in the password field.";
                    passwordEntry.Focus();
                    break;
                case AuthErrorReason.MissingEmail:
                    fullreason = "Please fill in the email field with a valid email.";
                    emailEntry.Focus();
                    break;
                case AuthErrorReason.EmailExists:
                    fullreason = "This email is already registered. \nPlease login.";
                    passwordEntry.Text = string.Empty;
                    passwordEntry.Focus();
                    break;
            }

            return fullreason;
        }

        private async Task StartLoadingVisual()
        {
            logDiv.IsVisible = false;
            loadingDiv.IsVisible = true;
        }

        private void StopLoadingVisual()
        {
            logDiv.IsVisible = true;
            loadingDiv.IsVisible = false;
        }
    }
}