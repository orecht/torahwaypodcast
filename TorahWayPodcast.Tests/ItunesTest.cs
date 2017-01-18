using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using NUnit.Framework;

using TorahWayPodcast.Models;
using TorahWayPodcast.Storage;


namespace TorahWayPodcast.Tests
{
    [TestFixture]
    public class ItunesTest
    {
        Shiurim Shiurim;

        [TestFixtureSetUp]
        public void ReadShiurimFromFile()
        {
            IStorage<Shiurim> Storage = new FileStorage(@"E:\GitHub\torahwaypodcast\TorahWayPodcast.Tests\test_data\shiurim.dat");
            Assert.IsNotNull(Storage);

            Shiurim = Storage.Read();

            Assert.IsNotNull(Shiurim);

            Log("Shiurim");
            Log("Shiurim has count of " + Shiurim.Count);
            Assert.IsTrue(Shiurim.Count > 0);
        }

        [Test]
        public void AllUrAreValid()
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

