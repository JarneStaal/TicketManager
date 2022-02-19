using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Firebase.Database.Query;
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
            var collection = new List<string>();
            collection.Add("All");
            foreach (var item in Database.ProblemList)
            {
                collection.Add(item);
            }

            //sfPicker.ItemsSource = collection;

            CheckForNewTickets();
        }

        private async void CheckForNewTickets()
        {
            while (true)
            {
                await TicketAction.GatherAllTickets();
                if (Database.TicketCollection.Any())
                {
                    ticketListView.ItemsSource = new ObservableCollection<Ticket>(Database.TicketCollection);
                }
                await Task.Delay(2000);
            }
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

        //private async void picker_OnSelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    string selectedSubject = sfPicker.SelectedItem.ToString();
        //    await TicketAction.GatherAllTickets();
        //    if (selectedSubject.Equals("All"))
        //    {
               
        //        var result = Database.TicketCollection.OrderByDescending(x => x.Problem);
        //        if (result.Any())
        //        {
        //            ticketListView.ItemsSource = new ObservableCollection<Ticket>(result);
        //            return;
        //        }
        //    }
        //    var result2 = Database.TicketCollection.Where(x => x.Problem == selectedSubject);
        //    if (result2.Any())
        //    {
        //        ticketListView.ItemsSource = new ObservableCollection<Ticket>(result2);
        //    }

        //    ticketListView.ItemsSource = new ObservableCollection<Ticket>();
        //}
    }
}