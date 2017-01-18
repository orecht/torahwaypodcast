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
        [Test]
        public void AreAllUrlsValid()
        {
            IStorage<Shiurim> Storage = new FileStorage(@"E:\GitHub\torahwaypodcast\TorahWayPodcast.Tests\test_data\shiurim.dat");
            Assert.IsNotNull(Storage);

            var shiurim = Storage.Read();

            Assert.IsNotNull(shiurim);

            Log("Shiurim");
            Log("Shiurim has count of "  + shiurim.Count);
            Assert.IsTrue(shiurim.Count > 0);

            int i = 1;
            foreach (Shiur shiur in shiurim)
            {
                Log(string.Format("Testing URL {0} ({1}/{2})", shiur.Url, i++, shiurim.Count));
                HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(shiur.Url);
                http.Method = "HEAD";
                http.Timeout = 3000; // 5s 
                HttpWebResponse response = (HttpWebResponse)http.GetResponse();

                Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, String.Format("{0} has return code of {1}", shiur.Url, response.StatusCode));
            }

        }

        void Log(string s)
        {
            Console.WriteLine(s);
        }
    }
}

