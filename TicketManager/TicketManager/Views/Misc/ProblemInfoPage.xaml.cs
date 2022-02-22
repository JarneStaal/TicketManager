using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManager.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager.Views.Misc
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProblemInfoPage : ContentPage
    {
        private Problem _problem;
        public ProblemInfoPage(Problem problem)
        {
            InitializeComponent();
            _problem = problem;
            this.BindingContext = _problem;
        }
    }
}