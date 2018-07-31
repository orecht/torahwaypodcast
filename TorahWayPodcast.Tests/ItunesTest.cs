using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using NUnit.Framework;

using TorahWayPodcast.Models;
using TorahWayPodcast.Storage;
using System.IO;

namespace TorahWayPodcast.Tests
{
    [TestFixture]
    public class ItunesTest
    {
        Shiurim Shiurim;

        [SetUp]
        public void ReadShiurimFromFile()
        {
            IStorage<Shiurim> Storage = new FileStorage(Path.Combine(TestContext.CurrentContext.TestDirectory, @"test_data\shiurim.dat"));
            Assert.IsNotNull(Storage);

            Shiurim = Storage.Read();

            Assert.IsNotNull(Shiurim);
            Assert.Greater(Shiurim.Count, 0);
        }

        [Test]
        public void AllUrlsAreValid()
        {
            int i = 1;
            foreach (Shiur shiur in Shiurim.OrderBy(x => Guid.NewGuid())) // queryURLs in random order
            {
                try
                {
                    Log(string.Format("Testing URL {0} ({1}/{2})", shiur.Url, i++, Shiurim.Count));
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(shiur.Url);
                    request.Method = "HEAD";
                    request.Timeout = 5000; // 5s
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        //Log(String.Join("\n", response.Headers));

                        Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, String.Format("{0} has return code of {1}", shiur.Url, response.StatusCode));
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.Timeout)
                    {
                        Assert.Fail(shiur.Url + " timed out");
                    }
                }
            }

        }

        void Log(string s)
        {
            Console.WriteLine(s);
        }
    }
}

