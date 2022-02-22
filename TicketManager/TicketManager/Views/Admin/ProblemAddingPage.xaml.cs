using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManager.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProblemAddingPage : ContentPage
    {
        public ProblemAddingPage()
        {
            InitializeComponent();
        }

        private async void AddQA_Clicked(object sender, EventArgs e)
        {
            await ProblemAction.AddProblem(problemEntry.Text, solutionEntry.Text);
        }
    }
}