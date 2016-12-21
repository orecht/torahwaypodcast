using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TorahWayPodcast.Models;

namespace TorahWayPodcast.Storage
{
    public class FileAndCacheStorage : IStorage<Shiurim>
    {
        private FileStorage  fileStorage   = new FileStorage();
        private CacheStorage cacheStorage = new CacheStorage();

        public Shiurim Read()
        {
            var data = cacheStorage.Read();
            if (!(data != null && data.Count() > 0))
            {
                // the Http Cache is empty. eg app pools have been recycled
                data = fileStorage.Read();
            }

            return data;
        }

        public void Write(Shiurim shiurim)
        {
            cacheStorage.Write(shiurim);
            fileStorage.Write(shiurim);
        }
    }
}