using System;
using System.Collections.Generic;

namespace WhoAreU.Models
{
    public partial class Post
    {
        public int Pkpost { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
        public string Fkuser { get; set; }
    }
}
