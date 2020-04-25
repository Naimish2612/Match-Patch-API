using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTO.Users
{
    public class UserUpdateDto
    {
        public string introduction { get; set; }
        public string looking_for { get; set; }
        public string interests { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }
}
