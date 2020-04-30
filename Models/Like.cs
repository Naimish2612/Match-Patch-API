using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Models
{
    public partial class Like
    {
        public int liker_id { get; set; }
        public int likee_id { get; set; }
        public User liker { get; set; }
        public User likee { get; set; }
    }
}
