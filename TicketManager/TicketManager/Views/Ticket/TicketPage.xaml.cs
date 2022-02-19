using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database.Query;
using TicketManager.Data;
using TicketManager.Views.Ticket;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager.Views.Misc
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketPage : ContentPage
    {
        public ObservableCollection<Models.Ticket> Tickets { get; set; } =
            new ObservableCollection<Models.Ticket>();
        public TicketPage()
        {
            InitializeComponent();

            if (Database.CurrentUserTicketsCollection.Count == 0)
            {
                TicketAction.GatherAllTickets();
            }

            SfPicker.ItemsSource = new List<string> { "TV", "CHROMECAST", "GSM", "PC/LAPTOP", "LADER", "WIFI", "INTERNET", "PRINTER", "OVEN" };
            CheckForNewTickets();
        }

        private async void CheckForNewTickets()
        {
            while (true)
            {
                Database.CurrentUserTicketsCollection = new ObservableCollection<Models.Ticket>
                    (Database.CurrentUserTicketsCollection.Where(x => x.Problem == string.Empty));
                Database.TicketCollection = new ObservableCollection<Models.Ticket>
                    (Database.TicketCollection.Where(x => x.Problem == string.Empty));
                await TicketAction.GatherAllTickets();
                await Database.RetrieveCurrentUserTickets();
                userFeedbackLv.ItemsSource = new ObservableCollection<Models.Ticket>(Database.CurrentUserTicketsCollection);
            }
        }

        private async void SubmitTicket_Clicked(object sender, EventArgs e)
        {
            try
            {
                string problem = SfPicker.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(descriptionEntry.Text) && !string.IsNullOrEmpty(problem))
                {
                    await Database.AddUserTicket(problem, descriptionEntry.Text);
                    await DisplayAlert("Alert", "Ticket succesvol aangemaakt!", "OK");
                    descriptionEntry.Text = string.Empty;
                    userFeedbackLv.ItemsSource = Database.CurrentUserTicketsCollection;
                }
                else
                {
                    await DisplayAlert("Alert", "Beschrijf het probleem.", "OK");
                }
            }
            catch (FirebaseException)
            {
                await DisplayAlert("Alert", "Ticket kan niet worden aangemaakt.", "OK");
            }
        }

        private void UserFeedbackLv_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Models.Ticket feedback = (Models.Ticket)e.SelectedItem;
                var result = Database.CurrentUserTicketsCollection.FirstOrDefault(x => x.TicketId == feedback.TicketId);
                this.Navigation.PushAsync(new TicketChatPage(result));
            }
            userFeedbackLv.SelectedItem = null;
        }
    }
}