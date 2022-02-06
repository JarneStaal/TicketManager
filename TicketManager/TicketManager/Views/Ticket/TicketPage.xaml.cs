using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketManager.Data;
using TicketManager.Views.Ticket;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager.Views.Misc
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketPage : ContentPage
    {
        public TicketPage()
        {
            InitializeComponent();

            if (Database.CurrentUserTicketsCollection.Count == 0)
            {
                TicketAction.GatherAllTickets();
            }

            SfPicker.ItemsSource = new List<string> { "TV", "CHROMECAST", "GSM", "PC/LAPTOP", "LADER", "WIFI", "ETHERNET", "PRINTER", "OVEN" };
            if (userFeedbackLv.ItemsSource == null)
            {
                userFeedbackLv.ItemsSource = Database.CurrentUserTicketsCollection;
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
                    await DisplayAlert("Alert", "Ticket submitted succesfully!", "OK");
                    descriptionEntry.Text = string.Empty;
                    userFeedbackLv.ItemsSource = Database.CurrentUserTicketsCollection;
                }
                else
                {
                    await DisplayAlert("Alert", "Make sure reason is picked and short message is not empty.", "OK");
                }
            }
            catch (FirebaseException)
            {
                await DisplayAlert("Alert", "Ticket failed to submit.", "OK");
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