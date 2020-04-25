using DatingApp.API.DTO.Photo;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTO.Users
{
    public partial class UserDetailDto
    {
        public int Id { get; set; }
        public string user_name { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public string known_as { get; set; }
        public DateTime last_active { get; set; }
        public string introduction { get; set; }
        public string looking_for { get; set; }
        public string interests { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string photo_url { get; set; }
        public ICollection<PhotoDetailDto> Photos { get; set; }
        public DateTime create_at { get; set; }
    }

    public partial class UserListDto
    {
        public int Id { get; set; }
        public string user_name { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public string known_as { get; set; }
        public DateTime last_active { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string photo_url { get; set; }
        public DateTime create_at { get; set; }
    }
}
