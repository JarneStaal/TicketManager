using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManager.Data;
using TicketManager.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager.Views.Misc
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelfHelpPage : ContentPage
	{
		public SelfHelpPage ()
		{
			InitializeComponent ();
            if (searchResultsLv.ItemsSource == null)
            {
                searchResultsLv.ItemsSource = Database.QACollection;
            }
        }

        private void GoBackToHome_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new UserControlPage();
        }

        private void searchResultsLv_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Problem problem = (Problem) e.SelectedItem;
                var result = Database.QACollection.FirstOrDefault(x => x.Issue == problem.Issue);
                this.Navigation.PushAsync(new ProblemInfoPage(result));
            }
            searchResultsLv.SelectedItem = null;
        }
    }
}