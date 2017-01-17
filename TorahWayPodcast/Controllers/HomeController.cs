using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Configuration;

using System.Globalization; // for date parsindg

using TorahWayPodcast.Models;
using TorahWayPodcast.Storage;

// for debug
using System.Security.Principal;
using System.Security.AccessControl;

namespace TorahWayPodcast.Controllers
{
    public class HomeController : Controller
    {
        private IStorage<Shiurim> Storage = new FileAndCacheStorage();

        public ActionResult Rss2()
        {
            Response.ContentType = "application/xml";
            return View(Storage.Read().OrderByDescending(s => s.DatePublished)); 
        }

        /*
        public ActionResult Rss2Feed()
        {
            SyndicationFeed feed = new SyndicationFeed();
            feed.Title = new TextSyndicationContent("The Torah Way podcast");
            feed.Description = new TextSyndicationContent("Start your day the Torah Way");
            feed.Links.Add(new SyndicationLink(new Uri("http://www.torahway.org.uk")));
            feed.Language = "en-GB";
            feed.Generator = "Oren's little concoction";
            feed.LastUpdatedTime = DateTime.Now;
            feed.ImageUrl = new Uri("http://@Request.Url.Authority/Content/torahway_1400.png");
            feed.AttributeExtensions.Add()


            feed.Authors.Add(new SyndicationPerson("someone@microsoft.com"));
            feed.Categories.Add(new SyndicationCategory("How To Sample Code"));
            

            SyndicationItem item1 = new SyndicationItem(
                "Item One",
                "This is the content for item one",
                new Uri("http://localhost/Content/One"),
                "ItemOneID",
                DateTime.Now);

        }*/

        public ActionResult ParseHtml()
        {
            string log = "";  

            try
            {
                Uri baseURI = new Uri("http://www.torahway.org.uk/)");
                string requestUrl = "http://www.torahway.org.uk/";
                string rawHtml = String.Empty;

                Shiurim shiurim = new Shiurim();

                HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                HttpWebResponse response = (HttpWebResponse)http.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(sr);

                    HtmlNodeCollection nodelistShiur = doc.DocumentNode.SelectNodes("//table/tr/td/table/tr/td/a/../..");
                    foreach (HtmlNode nn in nodelistShiur)
                    {
                        Uri link = new Uri(baseURI, nn.SelectSingleNode("./td/a").Attributes["href"].Value);
                        string strDate = nn.SelectSingleNode("./td[1]").InnerText.Trim();
                        string strRavSubject = nn.SelectSingleNode("./td[2]").InnerText;
                        strRavSubject = strRavSubject.Replace("(MP3 to follow, Click for WMA Format)", "");
                        string[] tab = strRavSubject.Split('-');

                        Shiur shiur = new Shiur();
                        shiur.Rav = CleanupText(tab[0]);
                        shiur.Subject = tab[1].Trim().Trim(new char[] { '"' });
                        shiur.Url = link.AbsoluteUri;

                        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-GB");
                        log += "parsing date " + strDate;
                        bool success = true;
                        // Can't use TryParse. there is no TryParse with CultureInfo
                        try
                        {
                            shiur.DatePublished = DateTime.Parse(strDate, ci);
                        }
                        catch
                        {
                            // eat it 
                            success = false;
                        }
                        log += (success ? "...success !" : "...FAIL") + "\n";

                        // TODO: get the real duration. Need to download the file and read mp3/wma header
                        shiur.Duration = new TimeSpan(0, 30, 0).Ticks;  // ~ 30 min

                        // TODO: get real size
                        shiur.FileSize = 10000000; // ~ 10MB

                        shiurim.Add(shiur);
                    }
                }

                Storage.Write(shiurim);
            }
            catch (Exception e)
            {
                log += "Caught exception:" + e.Message;
                if (e != null && e.InnerException != null)
                    log += e.InnerException.Message;
            }

            return Content(log, "text/plain");
        }

        private string CleanupText(string t)
        {
            return t.Trim(new char[] { '\r', '"' }).Trim();
        }
    }
}
