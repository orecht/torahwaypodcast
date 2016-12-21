using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

using TorahWayPodcast.Models;

namespace TorahWayPodcast.Storage
{
    public class CacheStorage : IStorage<Shiurim>
    {
        public Shiurim Read()
        {
            var cache = HttpContext.Current.Cache;

            if (cache["shiurim"] != null)
                return cache["shiurim"] as Shiurim;
            else
                return new Shiurim();
        }

        public void Write(Shiurim shiurim)
        {
            var cache = HttpContext.Current.Cache;

            cache.Add("shiurim",
                shiurim,
                null,
                DateTime.MaxValue,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                null);
        }
    }
}