using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTO.Auth
{
    public class LoginDto
    {
        public string user_name { get; set; }
        public string password { get; set; }
        public bool is_remember { get; set; }
    }
}
