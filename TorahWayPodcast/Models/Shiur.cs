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

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0}{1}{2}", DatePublished.ToShortTimeString(), Rav, Subject);
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
