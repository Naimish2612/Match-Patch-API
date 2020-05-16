using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTO.Message
{
    public partial class MessageToReturnDTO
    {
        public int Id { get; set; }
        public int senderId { get; set; }
        public string sender_known_as { get; set; }
        public string sender_photo_url { get; set; }
        public int recipientId { get; set; }
        public string recipient_known_as{ get; set; }
        public string recipient_photo_url { get; set; }
        public string content { get; set; }
        public bool is_read { get; set; }
        public DateTime? read_date { get; set; }
        public DateTime message_sent_date { get; set; }
    }
}
