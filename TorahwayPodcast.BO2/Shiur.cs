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
        public TimeSpan Duration { get; set; }
        public DateTime DatePublished { get; set; }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            // RSS feed can't have 2 itmes with the same URL. Cf RSS spec. 
            return Url;
        }

        public override bool Equals(object obj)
        {
            if (obj is Shiur)
            {
                var sh = obj as Shiur;
                return sh.GetHashCode() == this.GetHashCode();
            }
            else
            {
                return false;
            }
        }
    }
}
