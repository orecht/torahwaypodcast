using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TorahWayPodcast.Models
{
    [Serializable]
    public class Shiur
    {
        public string Rav { get; set; }
        public string RavPosition { get; set; }
        public string Subject { get; set; }
        public string Url { get; set; }
        public long FileSize { get; set; } // in bytes
        public long Duration { get; set; } // in seconds
        public DateTime DatePublished { get; set; }
    }
}
