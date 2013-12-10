using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TorahWayPodcast.Models
{
    public class Shiurim : List<Shiur>
    {
        static private Shiurim f_instance = new Shiurim();
        static public Shiurim Instance
        {
            get
            {
                return f_instance;
            }
        }

        private Shiurim()
            : base()
        {
        }
    }
}