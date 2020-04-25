using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string user_name { get; set; }
        public byte[] passowrd_hash { get; set; }
        public byte[] passowrd_salt { get; set; }
        public string gender { get; set; }
        public DateTime date_of_birth { get; set; }
        public string known_as { get; set; }
        public DateTime last_active { get; set; }
        public string introduction { get; set; }
        public string looking_for { get; set; }
        public string interests { get; set; }
        public string  city { get; set; }
        public string country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public DateTime create_at { get; set; }
    }
}
