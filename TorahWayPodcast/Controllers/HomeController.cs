using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Net;
using System.IO;
using HtmlAgilityPack;

using System.Globalization; // for date parsindg

using TorahWayPodcast.Models;

namespace TorahWayPodcast.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Rss2()
        {
            Response.ContentType = "application/xml";
            return View (Shiurim.Instance); 
        }

        public ActionResult ParseHtml()
        {
            String log = "";

            Uri baseURI = new Uri("http://www.torahway.org.uk/)");
            string requestUrl = "http://www.torahway.org.uk/";
            string rawHtml = String.Empty;

            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(sr);
                Shiurim.Instance.Clear();
                //doc.Load("c:\\temp\\torahway_home.html");
                var n = doc.DocumentNode.SelectNodes("/html/body/center/font/font/b/td/table/tr/td/table");
                foreach (var nn in n.Take(10))
                {
                    Uri link = new Uri(baseURI, nn.SelectSingleNode("./tr/td/a").Attributes["href"].Value);
                    string strDate = nn.SelectSingleNode("./tr/td[1]").InnerText.Trim();
                    string strRavSubject = nn.SelectSingleNode("./tr/td[2]").InnerText;
                    strRavSubject = strRavSubject.Replace("(MP3 to follow, Click for WMA Format)", "");
                    string[] tab = strRavSubject.Split('-');

                    Shiur shiur = new Shiur();
                    shiur.Rav = tab[0].Trim(new char[] { '\r' }).Trim();
                    shiur.Subject = tab[1].Trim(new char[] { '\r' }).Trim();
                    shiur.Url = link.AbsoluteUri;

                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-GB");
                    log += "parsing date " + strDate + "\n";
                    bool success = true;
                    try
                    {
                        shiur.DatePublished = DateTime.Parse(strDate, ci);
                    }
                    catch
                    {
                        // eat it 
                        success = false;
                    }
                    log += success ? "...success !" : "...FAIL";

                    Shiurim.Instance.Add(shiur);
                }
            }
            return Content(log);
        }

    }
}
