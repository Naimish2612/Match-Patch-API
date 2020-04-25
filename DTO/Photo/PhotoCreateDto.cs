using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTO.Photo
{
    public class PhotoCreateDto
    {
        public PhotoCreateDto()
        {
            this.date_added = DateTime.Now;
        }

        public string url { get; set; }
        public IFormFile file{ get; set; }
        public string description { get; set; }
        public DateTime date_added { get; set; }
        public string public_photo_id { get; set; }
        
    }
}
