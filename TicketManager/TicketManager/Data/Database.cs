using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TicketManager.Models;
using TicketManager.Records;
using Xamarin.Essentials;

namespace TicketManager.Data
{
    public class Database
    {
        public static FirebaseClient firebaseClient =
            new FirebaseClient("https://ticketmanager-16cfb-default-rtdb.europe-west1.firebasedatabase.app/");
        public static ObservableCollection<Ticket> CurrentUserTicketsCollection = new ObservableCollection<Ticket>();
        public static ObservableCollection<Ticket> TicketCollection = new ObservableCollection<Ticket>();
        public static ObservableCollection<Problem> QACollection = new ObservableCollection<Problem>();
        public static List<string> ProblemList = new List<string>
            {"TV", "CHROMECAST", "GSM","PC/LAPTOP","LADER", "WIFI", "ETHERNET", "PRINTER", "OVEN"};
        public static async void DeleteUserData()
        {
            var fbAuth = await Authentication.GetUser();
            var localId = fbAuth.User.LocalId;
            await firebaseClient.Child("Data").Child(localId).DeleteAsync();
        }

        public static void DeleteAllLocalData()
        {
            var localDataFiles = Directory.GetFiles(FileSystem.AppDataDirectory + "/");
            foreach (var file in localDataFiles)
            {
                File.Delete(file);
            }

            Preferences.Remove("MyFirebaseRefreshToken");
        }

        public static async Task RetrieveCurrentUserTickets()
        {
            await TicketAction.GatherAllTickets();
            var tempColl = new ObservableCollection<Ticket>();
            var results = TicketCollection.Where(x => x.SenderUID == TicketChat.FBUserLocalId);
            foreach (var fb in results)
            {
                tempColl.Add(fb);
            }

            CurrentUserTicketsCollection = tempColl;
        }

        public static async Task AddUserTicket(string problem, string description)
        {
            var user = await Authentication.GetUser();
            var id = user.User.LocalId;


            var random = new Random();
            int fbid = random.Next(1, 999999999);

            Ticket ticket = new Ticket
            {
                CreatorUID = id,
                SenderUID = id,
                TicketId = fbid.ToString(),
                Problem = problem,
                Description = description,
                DateTime = DateTime.Now.ToString("f")
            };
            TicketCollection.Add(ticket);

            await firebaseClient.Child("Ticket").Child("UserTicket").PutAsync(new TicketRecord
            {
                TicketInfoJSON = JsonConvert.SerializeObject(TicketCollection)

            });
            await RetrieveCurrentUserTickets();
            await TicketChat.SendMessage(ticket, $"Ticket Information\n--------------------------------------" +
                                                 $"\nProblem: {problem}" +
                                                 $"\nDescription: {description}");
        }
        public static void RegisterSyncfusionLicense()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTc3NDgyQDMxMzkyZTM0MmUzMFB5Q2sxMWtraHh6WjZLT2k0amR6RkNZNkZJVDZtZmNIUmJPRnVadWo0emM9");
        }
    }
}
