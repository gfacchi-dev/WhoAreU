using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhoAreU.Models
{
    public class ProfileViewModel
    {
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Propic { get; set; }
        public bool? IsFollowed { get; set; }
        public IEnumerable<ApplicationUser> Followers { get; set; }
        public IEnumerable<ApplicationUser> Followed { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
