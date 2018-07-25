using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using HtmlAgilityPack;

using System.Globalization; // for date parsindg

using TorahWayPodcast.Models;
using TorahWayPodcast.Storage;

// for debug
using System.Security.Principal;
using System.Security.AccessControl;
using System.ServiceModel.Syndication;

namespace TorahWayPodcast.BO
{
    public class PodcastManager
    {
        private IStorage<Shiurim> Storage;
        private ILogger Logger;

        public PodcastManager(IStorage<Shiurim> storage, ILogger logger)
        {
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Shiur> Rss2()
        {
            return Storage.Read()
            .OrderByDescending(s => s.DatePublished);
        }

        public IEnumerable<Shiur> Mp3Feed()
        {
            return Storage.Read()
                .Where(s => s.Url.Contains(".mp3"))
                .OrderByDescending(s => s.DatePublished);
        }

        public bool ParseHtml()
        {
            // Use HashSet to RSS spec says there can't be doublons in an RSS feed. Equality criteria for 2 items is URL as per RSS spec
            var shiurim = new HashSet<Shiur>();

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
                        Logger.Log($"parsing date {strDate}");
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
                        Logger.Log((success ? "...success !" : "...FAIL") + "\n");

                        // TODO: get the real duration. Need to download the file and read mp3/wma header
                        shiur.Duration = new TimeSpan(0, 30, 0);  // ~ 30 min

                        // TODO: get real size
                        shiur.FileSize = 10000000; // ~ 10MB

                        shiurim.Add(shiur);
                    }
                }

                var uniqueShiurim = new Shiurim(shiurim);
                Storage.Write(uniqueShiurim);

                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Caught exception:" + e.Message);
                if (e != null && e.InnerException != null)
                    Logger.Log(e.InnerException.Message);

                // Return as many shurim as you can.
                var uniqueShiurim = new Shiurim(shiurim);
                Storage.Write(uniqueShiurim);

                return false;
            }
        }

        private string CleanupText(string t)
        {
            return t.Trim(new char[] { '\r', '"' }).Trim();
        }

        public string GenerateRssFeed(IEnumerable<Shiur> shiurim)
        {
            ControllerContext c;

            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent("The Torah Way podcast"),
                //Links = new System.Collections.ObjectModel.Collection<SyndicationLink>
                //{
                //    new SyndicationLink(new Uri("http://www.torahway.org.uk<"))
                //},
                Description = new TextSyndicationContent("Start your day the Torah Way"),
                Language = "en-GB",
                Generator = "My little concoction",
                LastUpdatedTime = DateTime.Now,
                ImageUrl = new Uri($"~/Content/torahway_1400.png")


            };
        }

        public virtual string RenderViewToString(
    ControllerContext controllerContext,
    string viewPath,
    string masterPath,
    ViewDataDictionary viewData,
    TempDataDictionary tempData)
        {
            Stream filter = null;
            ViewPage viewPage = new ViewPage();

            //Right, create our view
            viewPage.ViewContext = new ViewContext(controllerContext, new WebFormView(viewPath, masterPath), viewData, tempData);

            //Get the response context, flush it and get the response filter.
            var response = viewPage.ViewContext.HttpContext.Response;
            response.Flush();
            var oldFilter = response.Filter;

            try
            {
                //Put a new filter into the response
                filter = new MemoryStream();
                response.Filter = filter;

                //Now render the view into the memorystream and flush the response
                viewPage.ViewContext.View.Render(viewPage.ViewContext, viewPage.ViewContext.HttpContext.Response.Output);
                response.Flush();

                //Now read the rendered view.
                filter.Position = 0;
                var reader = new StreamReader(filter, response.ContentEncoding);
                return reader.ReadToEnd();
            }
            finally
            {
                //Clean up.
                if (filter != null)
                {
                    filter.Dispose();
                }

                //Now replace the response filter
                response.Filter = oldFilter;
            }
        }
    }
}
