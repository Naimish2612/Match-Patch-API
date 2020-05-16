using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTO.Message
{
    public class MessageCreateDTO
    {
        public MessageCreateDTO()
        {
            this.message_sent_date = DateTime.Now;
        }

        public int senderId { get; set; }
        public int recipientId { get; set; }
        public DateTime message_sent_date { get; set; }
        public string content { get; set; }
    }
}
