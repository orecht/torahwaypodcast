using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using TorahWayPodcast.BO;
using TorahWayPodcast.Models;

namespace TorahWayPodcast.BO2.Test
{
    public class MockLogger : ILogger
    {
        public void Log(string message)
        {
            // do nothing
        }
    }

    [TestFixture]
    public class PodcastManagerTest
    {
        [Test]
        public void GenerateRssDoesNotFail()
        {
            var shiurim = new List<Shiur>()
            {
                new Shiur { DatePublished = DateTime.Now, Rav = "Rav Oren", Duration = new TimeSpan(0, 30, 0), RavPosition = "The boss", Subject = "My view on everything", Url = "http://orenrecht.co.uk/shiurim/1" },
                new Shiur { DatePublished = DateTime.Now - new TimeSpan(-1, 0, 0, 0), Rav = "Rav Oren", Duration = new TimeSpan(0, 30, 0), RavPosition = "The boss", Subject = "My view on everything 2 ", Url = "http://orenrecht.co.uk/shiurim/2" },
            };

            Assert.IsNotEmpty(shiurim);

            var manager = new PodcastManager(new MemoryStorage(),
                new MockLogger());

            Assert.IsNotNull(manager);

            var dir = TestContext.CurrentContext.TestDirectory;
            var viewFile = "rss2_test.cshtml";

            Assert.IsTrue(Directory.Exists(dir));
            Assert.IsTrue(File.Exists(Path.Combine(dir, viewFile)));

            var resultTask = manager.GenerateRssFeedAsync(shiurim, dir , viewFile);

            var result = resultTask.Result;

            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}
