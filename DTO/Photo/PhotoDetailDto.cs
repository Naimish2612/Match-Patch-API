using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTO.Photo
{
    public class PhotoDetailDto
    {
        public int Id { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public DateTime date_added { get; set; }
        public bool is_main { get; set; }
    }
}
