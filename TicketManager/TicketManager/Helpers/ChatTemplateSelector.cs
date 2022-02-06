using TicketManager.Data;
using Xamarin.Forms;

namespace TicketManager.Helpers
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IncomingDataTemplate { get; set; }
        public DataTemplate OutgoingDataTemplate { get; set; }
        public DataTemplate InfoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as TicketChat;
            if (messageVm == null)
                return null;

            if (messageVm.Message.StartsWith("Ticket Information"))
            {
                return InfoDataTemplate;
            }

            if (TicketChat.FBUserLocalId == messageVm.SenderUID)
            {
                return OutgoingDataTemplate;
            }

            return IncomingDataTemplate;
        }
    }
}
