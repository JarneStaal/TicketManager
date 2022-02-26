using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
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

        
        private void DescriptionEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var listOfResults = new ObservableCollection<Problem>();
            searchResultsLv.ItemsSource = new ObservableCollection<Problem>();
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                var searchStringArray = e.NewTextValue.ToLower().Split(' ');

                foreach (var problem in Database.QACollection)
                {
                    foreach (var searchStr in searchStringArray)
                    {
                        if (problem.Issue.ToLower().Contains(searchStr))
                        {
                            if (!listOfResults.Any(x => x.Issue == problem.Issue))
                            {
                                listOfResults.Add(problem);
                            }
                        }
                    }
                }

                if (listOfResults.Any())
                {
                    searchResultsLv.ItemsSource = listOfResults;
                    return;
                }
                else
                {
                    searchResultsLv.ItemsSource = new ObservableCollection<Problem>();
                    return;
                }
            }
            searchResultsLv.ItemsSource = new ObservableCollection<Problem>(Database.QACollection);
        }
    }
}