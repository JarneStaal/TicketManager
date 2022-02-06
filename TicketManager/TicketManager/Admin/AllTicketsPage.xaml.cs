using System;
using System.Collections.Generic;
using System.Linq;
using TicketManager.Data;
using TicketManager.Models;
using TicketManager.Views.Ticket;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllTicketsPage : ContentPage
    {
        public AllTicketsPage()
        {
            InitializeComponent();
            //TODO: FIX THIS
            //((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            //((NavigationPage)Application.Current.MainPage).BarTextColor = Color.OrangeRed;

            var collection = new List<string>();
            collection.Add("All");
            foreach (var item in Database.ProblemList)
            {
                collection.Add(item);
            }

            sfPicker.ItemsSource = collection;
            if (Database.TicketCollection.Count != 0)
                if (ticketListView.ItemsSource == null)
                    ticketListView.ItemsSource = Database.TicketCollection;
                else
                    nofb.IsVisible = true;

        }

        private async void ShowAllTickets_Clicked(object sender, EventArgs e)
        {
            await TicketAction.GatherAllTickets();
            ticketListView.ItemsSource = Database.TicketCollection.OrderByDescending(x => x.Problem);
        }

        private void ticketLv_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Ticket ticket = (Ticket)e.SelectedItem;
                var result = Database.TicketCollection.FirstOrDefault(x => x.TicketId == ticket.TicketId);
                this.Navigation.PushAsync(new TicketChatPage(result));
            }
            ticketListView.SelectedItem = null;
        }

        private async void picker_OnSelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            string selectedSubject = sfPicker.SelectedItem.ToString();
            if (selectedSubject.Equals("All"))
            {
                await TicketAction.GatherAllTickets();
                ticketListView.ItemsSource = Database.TicketCollection.OrderByDescending(x => x.Problem);
                return;
            }
            await TicketAction.GatherAllTickets();
            ticketListView.ItemsSource = Database.TicketCollection.Where(x => x.Problem == selectedSubject);
        }
    }
}