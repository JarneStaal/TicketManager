using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using TicketManager.Models;
using TicketManager.Records;
using TicketManager.Views.Misc;
using Xamarin.Forms;

namespace TicketManager.Data
{
    public class TicketAction
    {
        public static FirebaseClient firebaseClient =
            new FirebaseClient("https://ticketmanager-16cfb-default-rtdb.europe-west1.firebasedatabase.app/");
        public static async Task GatherAllTickets()
        {
            var userTicket = await firebaseClient
                .Child("Ticket")
                .Child("UserTicket")
                .OnceAsync<object>();

            if (userTicket.Count > 0)
            {
                ObservableCollection<Ticket> rsCollection = new ObservableCollection<Ticket>();
                foreach (var item in userTicket)
                {
                    var json = item.Object.ToString();
                    if (json.Equals("[]"))
                        json = null;

                    
                    if (!string.IsNullOrEmpty(json))
                    {
                        var listTickets = JsonConvert.DeserializeObject<List<Ticket>>(json);
                        foreach (var feedback in listTickets)
                        {
                            rsCollection.Add(feedback);
                        }
                    }
                }
                Database.TicketCollection = rsCollection;
            }
            else
            {
                Database.TicketCollection = new ObservableCollection<Ticket>();
            }
        }

        public static async Task RemoveTicket(Ticket ticket)
        {
            var tempCollection = new ObservableCollection<Ticket>(Database.TicketCollection.Where(x => x.TicketId != ticket.TicketId));
            await firebaseClient.Child("Ticket").Child("UserTicket").DeleteAsync();
            await firebaseClient.Child("TicketMessages").Child(TicketChat.FBUserLocalId).Child(ticket.TicketId).DeleteAsync();
            await firebaseClient.Child("Ticket").Child("UserTicket").PutAsync(new TicketRecord
            {
                TicketInfoJSON = JsonConvert.SerializeObject(tempCollection)

            });
            Database.TicketCollection = tempCollection;     
            await Database.RetrieveCurrentUserTickets();
            await Application.Current.MainPage.DisplayAlert("Alert", "Chat beëindigd", "OK");
            Application.Current.MainPage = new UserControlPage();
        }
    }
}
