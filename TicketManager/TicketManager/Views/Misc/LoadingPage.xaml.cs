using TicketManager.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            Database.RegisterSyncfusionLicense();
            InitializeComponent();
        }

        public double ProgressValue
        {
            get => pgBar.Progress;
            set => pgBar.Progress = value;
        }
    }
}