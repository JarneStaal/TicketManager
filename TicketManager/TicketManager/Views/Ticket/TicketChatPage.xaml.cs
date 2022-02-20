using Firebase.Database.Query;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TicketManager.Data;
using TicketManager.Views.Misc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager.Views.Ticket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketChatPage : ContentPage
    {
        private Models.Ticket _ticket;
        public ObservableCollection<TicketChat> ChatItems { get; set; } =
            new ObservableCollection<TicketChat>();
        public TicketChatPage(Models.Ticket ticket)
        {
            Database.RegisterSyncfusionLicense();
            InitializeComponent();
            _ticket = ticket;

            BindingContext = this;
            ScrollToBottom();

            Database.firebaseClient
                .Child("TicketMessages").Child(_ticket.CreatorUID).Child(_ticket.TicketId)
                .AsObservable<TicketChat>()
                .Subscribe(async dbevent =>
                {
                    if (dbevent.Object != null)
                    {
                        ChatItems.Add(dbevent.Object);
                        await ScrollToBottom();
                    }
                });
            CheckIfTicketClosed();
        }

        private async void CheckIfTicketClosed()
        {
            while (true)
            {
                await TicketAction.GatherAllTickets();
                if (Database.TicketCollection.FirstOrDefault(x => x.Description == _ticket.Description) == null)
                {
                    await DisplayAlert("Alert", "Ticket is gesloten", "OK");
                    Application.Current.MainPage = new UserControlPage();
                    break;
                }
                await Task.Delay(2000);
            }
        }

        private async Task ScrollToBottom()
        {
            if (ChatItems.Count > 0)
            {
                int index = ChatItems.IndexOf(ChatItems.Last());
                ChatItemscollView.ScrollTo(index);
            }
        }

        private async void SendMessage_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(messageEditor.Text))
            {
                await TicketChat.SendMessage(_ticket, messageEditor.Text);
                messageEditor.Text = string.Empty;
            }
        }

        private async void EndChat_OnClick(object sender, EventArgs e)
        {
            await TicketAction.RemoveTicket(_ticket);
        }
    }
}