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
            string log = "";  

            try
            {
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
                    HtmlNodeCollection nodelistShiur = doc.DocumentNode.SelectNodes("/html/body/center/font/font/b/td/table/tr/td/table/tr");
                    foreach (HtmlNode nn in nodelistShiur)
                    {
                        if (Shiurim.Instance.Count > 10)
                            break; 

                        if (nn.SelectSingleNode("./td/a") != null)
                        {
                            Uri link = new Uri(baseURI, nn.SelectSingleNode("./td/a").Attributes["href"].Value);
                            string strDate = nn.SelectSingleNode("./td[1]").InnerText.Trim();
                            string strRavSubject = nn.SelectSingleNode("./td[2]").InnerText;
                            strRavSubject = strRavSubject.Replace("(MP3 to follow, Click for WMA Format)", "");
                            string[] tab = strRavSubject.Split('-');

                            Shiur shiur = new Shiur();
                            shiur.Rav = tab[0].Trim(new char[] { '\r' }).Trim();
                            shiur.Subject = tab[1].Trim(new char[] { '\r' }).Trim();
                            shiur.Url = link.AbsoluteUri;

                            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-GB");
                            log += "parsing date " + strDate;
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
                            log += (success ? "...success !" : "...FAIL") + "\n";

                            Shiurim.Instance.Add(shiur);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log += "Caught exception:" + e.Message;
                if (e != null && e.InnerException != null)
                    log += e.InnerException.Message;
            }

            return Content(log, "text/plain");
        }

    }
}
