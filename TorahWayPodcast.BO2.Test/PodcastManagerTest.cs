using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using TorahWayPodcast.BO;
using TorahWayPodcast.Models;

namespace TorahWayPodcast.BO2.Test
{
    [TestFixture]
    public class PodcastManagerTest
    {
        [Test]
        public async void GenerateRssDoesNotFail()
        {
            var shiurim = new List<Shiur>()
            {
                new Shiur { DatePublished = DateTime.Now, Rav = "Rav Oren", Duration = new TimeSpan(0, 30, 0), RavPosition = "The boss", Subject = "My view on everything", Url = "http://orenrecht.co.uk/shiurim/1" },
                new Shiur { DatePublished = DateTime.Now - new TimeSpan(-1, 0, 0, 0), Rav = "Rav Oren", Duration = new TimeSpan(0, 30, 0), RavPosition = "The boss", Subject = "My view on everything 2 ", Url = "http://orenrecht.co.uk/shiurim/2" },
            };

            var manager = new PodcastManager(new MemoryStorage(),
                null);

            var result = await manager.GenerateRssFeedAsync(shiurim);

            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}
