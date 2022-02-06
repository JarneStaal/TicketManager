using Firebase.Database.Query;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TicketManager.Data;
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
    }
}