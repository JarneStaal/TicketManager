using EmailValidation;
using Xamarin.Forms;

namespace TicketManager.Behaviors
{
    public class EmailValidationBehaviour : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            OSAppTheme currentTheme = Application.Current.RequestedTheme;

            ((Entry)sender).Text = args.NewTextValue.Replace(" ", "");
            bool emailIsValid = EmailValidator.Validate(args.NewTextValue);
            if (emailIsValid)
            {
                ((Entry)sender).TextColor = Color.Black;
                if (currentTheme == OSAppTheme.Dark)
                {
                    ((Entry)sender).TextColor = Color.White;

                }
            }
            else
            {
                ((Entry)sender).TextColor = Color.Red;
            }
        }
    }
}
