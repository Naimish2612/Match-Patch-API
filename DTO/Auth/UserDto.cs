using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTO.Auth
{
    public class UserDto
    {

        public UserDto()
        {
            this.create_at = DateTime.Now;
            this.last_active = DateTime.Now;
        }

        [Required]
        public string user_name { get; set; }
        [Required]
        [StringLength(10,MinimumLength =3,ErrorMessage ="You must specify password between 3 and 10 characters.")]
        public string password { get; set; }
        [Required]
        public string gender { get; set; }
        [Required]
        public DateTime date_of_birth { get; set; }
        [Required]
        public string known_as { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string country { get; set; }
        public DateTime last_active { get; set; }
        public DateTime create_at { get; set; }
    }
}
