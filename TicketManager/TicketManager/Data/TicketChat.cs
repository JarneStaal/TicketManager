using Firebase.Database.Query;
using System.Threading.Tasks;
using TicketManager.Models;

namespace TicketManager.Data
{
    public class TicketChat
    {
        public static string FBUserLocalId { get; set; }
        public string SenderEmail { get; set; }
        public string SenderUID { get; set; }
        public string Message { get; set; }
        public string DateTime { get; set; }
        public string MessageInfo => $"{SenderEmail} {DateTime}";

        public static async Task SendMessage(Ticket ticket, string message)
        {
            var user = await Authentication.GetUser();
            var localidSender = user.User.LocalId;
            string email = user.User.Email;
            await Database.firebaseClient.Child("TicketMessages").Child(ticket.CreatorUID).Child(ticket.TicketId).PostAsync(new TicketChat
            {
                SenderEmail = email,
                SenderUID = localidSender,
                Message = message,
                DateTime = System.DateTime.Now.ToString("t")
            });
        }
    }
}