using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;

using TorahWayPodcast.Models;

namespace TorahWayPodcast.Controllers
{
    public class HomeController : Controller
    {
        List<Shiur> f_Shiurim = new List<Shiur>();

        public ActionResult Index()
        {
            PopulateFeed();

            SyndicationFeed feed =
                new SyndicationFeed("The Torah Way Podcast",
                                    "Get the Torah Way in your Podcast reader",
                                    new Uri("http://"+Request.Url.Host),
                                    "TorahWayPodcastID",
                                    DateTime.Now);


            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach (Shiur s in f_Shiurim)
            {
                SyndicationItem item = new SyndicationItem(s.Author, 
                                                            s.Subject, 
                                                            new Uri(s.Url), 
                                                            s.Author+"_"+s.Date.ToString(), 
                                                            DateTime.Now);
                items.Add(item);
            }
            
            feed.Items = items;

            return new RssActionResult() { Feed = feed };
        }

        private void PopulateFeed()
        {
            f_Shiurim.Add(new Shiur() { Author = "Rav Bern", AuthorPosition = "Magid shiur, Machon Yaacov", Subject = "Why another shiur", Date = DateTime.Now - new TimeSpan(1, 0, 0, 0), Url = "http://torahway.org.uk/y1/s1.mp3" });
            f_Shiurim.Add(new Shiur() { Author = "Rav Bern2", AuthorPosition = "Magid shiur, Machon Yaacov", Subject = "Awson shiur", Date = DateTime.Now - new TimeSpan(2, 0, 0, 0), Url = "http://torahway.org.uk/y1/s2.mp3" });
        }

    }
}
