using Xamarin.Forms;

namespace TicketManager.Behaviors
{
    public class MaxLengthValidationBehaviour : Behavior<Entry>
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
            bool lengthIsValid = LengthValidate(args.NewTextValue.Length);
            if (lengthIsValid)
            {
                ((Entry)sender).TextColor = Color.Black;
                if (currentTheme == OSAppTheme.Dark)
                {
                    ((Entry)sender).TextColor = Color.White;
                }
            }
            else
            {
                string entryTextWithoutLastChar = args.NewTextValue.Remove(args.NewTextValue.Length - 1, 1);
                ((Entry)sender).TextColor = Color.Red;
                ((Entry)sender).Text = entryTextWithoutLastChar;
            }
        }

        private bool LengthValidate(int entryText)
        {
            if (entryText <= 150)
                return true;

            return false;
        }
    }
}
