using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public DateTime date_added { get; set; }
        public bool is_main { get; set; }
        public string public_photo_id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}