using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int senderId { get; set; }
        public User sender { get; set; }
        public int recipientId { get; set; }
        public User recipient { get; set; }
        public string content { get; set; }
        public bool is_read { get; set; }
        public DateTime? read_date { get; set; }
        public DateTime message_sent_date { get; set; }
        public bool sender_deleted { get; set; }
        public bool recipient_deleted { get; set; }

    }
}
