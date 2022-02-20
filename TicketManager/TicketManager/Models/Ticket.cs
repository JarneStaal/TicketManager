using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManager.Models
{
    public class Ticket
    {
        public string TicketId { get; set; }
        public string CreatorUID { get; set; }
        public string SenderUID { get; set; }
        public string Problem { get; set; }
        public string Description { get; set; }
        public string DateTime { get; set; }
    }
}
