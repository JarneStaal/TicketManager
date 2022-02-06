using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TicketManager.Models;

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

                    Database.TicketCollection = null;
                    var listTickets = JsonConvert.DeserializeObject<List<Ticket>>(json);
                    if (!string.IsNullOrEmpty(json))
                    {
                        foreach (var feedback in listTickets)
                        {
                            rsCollection.Add(feedback);
                        }
                    }
                }
                Database.TicketCollection = rsCollection;
            }
        }
    }
}
