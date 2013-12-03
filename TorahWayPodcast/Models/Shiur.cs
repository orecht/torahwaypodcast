using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TorahWayPodcast.Models
{
    public class Shiur
    {
        public string Author { get; set; }
        public string AuthorPosition { get; set; }
        public string Subject { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
    }
}