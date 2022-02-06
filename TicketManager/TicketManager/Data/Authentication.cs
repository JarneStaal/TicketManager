using Firebase.Auth;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TicketManager.Views.Account;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TicketManager.Data
{
    public class Authentication
    {
        public static string WebAPIKey = "AIzaSyBohsbGODQ69vD-ID4kZfWppI3qz-nzY8s";
        public static FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
        public static string AdminEmail = "jarne.staal9@gmail.com";
        public static async Task<FirebaseAuth> GetUser()
        {
            var savedFirebaseAuth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
            var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
            Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
            return savedFirebaseAuth;
        }

        public static async Task SignInUser(string email, string password)
        {
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            var content = await auth.GetFreshAuthAsync();
            var serializedContent = JsonConvert.SerializeObject(content);
            Preferences.Set("MyFirebaseRefreshToken", serializedContent);
        }

        public static async Task<bool> CanLogin(string email, string password)
        {
            try
            {
                await SignInUser(email, password);
                return true;
            }
            catch (FirebaseAuthException)
            {
                return false;
            }
        }

        public static async Task RegisterUser(string email, string password)
        {
            var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
            await authProvider.SendEmailVerificationAsync(auth.FirebaseToken);
        }

        public static async Task UpdateUserPassword(string password)
        {
            var savedFirebaseAuth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
            var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
            await authProvider.ChangeUserPassword(refreshedContent.FirebaseToken, password);
        }

        public static async Task RemoveUser()
        {
            var savedFirebaseAuth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
            var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
            await authProvider.DeleteUserAsync(refreshedContent.FirebaseToken);
            Database.DeleteUserData();
            Preferences.Remove("MyFirebaseRefreshToken");
            Application.Current.MainPage = new LoginRegisterPage();
            await Application.Current.MainPage.DisplayAlert("Alert", "Account has been succesfully deleted", "OK");
        }

        public static async Task<bool> IsAdmin()
        {
            var user = await GetUser();
            if (user.User.Email.Equals(AdminEmail))
                return true;

            return false;
        }

        public static async Task AddSeedUsers()
        {
            try
            {
                await RegisterUser($"admin@gmail.com", "Admin456!");
                await RegisterUser($"user@gmail.com", "User123!");
            }
            catch (FirebaseAuthException)
            {
                Debug.WriteLine("USERS ALREADY EXIST");
            }
        }


        public static string GetTwoLetterLanguageName()
        {
            var country = RegionInfo.CurrentRegion.DisplayName;
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            var englishRegion = regions.FirstOrDefault(region => region.EnglishName.Contains(country));
            var countryAbbrev = englishRegion.TwoLetterISORegionName;
            var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            if (countryAbbrev.ToLower().Equals("be"))
            {
                countryAbbrev = "nl";
            }

            var langname = allCultures.FirstOrDefault(c => c.TwoLetterISOLanguageName == countryAbbrev.ToLower());
            if (langname == null)
            {
                countryAbbrev = "en";
            }
            else
            {
                countryAbbrev = langname.TwoLetterISOLanguageName;
            }
            return countryAbbrev;
        }
    }
}
