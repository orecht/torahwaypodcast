using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TorahWayPodcast.Models
{
    /// <summary>
    /// List of shiurim. This is the container for the entire data model. 
    /// </summary>
    [Serializable]
    public class Shiurim : List<Shiur>
    {
        public Shiurim()
            : base()
        { }

        public Shiurim(HashSet<Shiur> hashset)
            : base (hashset)
        { }

        public Shiurim(IEnumerable<Shiur> list)
            : base(list)
        { }


    }
}