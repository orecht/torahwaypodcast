<Query Kind="Program">
  <Reference Relative="..\bin\HtmlAgilityPack.dll">E:\GitHub\torahwaypodcast\TorahWayPodcast\bin\HtmlAgilityPack.dll</Reference>
  <Reference Relative="..\bin\TorahWayPodcast.dll">E:\GitHub\torahwaypodcast\TorahWayPodcast\bin\TorahWayPodcast.dll</Reference>
  <Namespace>HtmlAgilityPack</Namespace>
  <Namespace>TorahWayPodcast.Models</Namespace>
</Query>

void Main()
{
	string file = @"E:\GitHub\torahwaypodcast\TorahWayPodcast\tests\torahway.org.uk_20161223_index.thml";
	Uri baseURI = new Uri("http://www.torahway.org.uk/)");
	
	string log = "";
	var shiurim = new Shiurim();
	
	                    HtmlDocument doc = new HtmlDocument();
	                    doc.Load(file);
						
						/*
						string xpathTest = "//table/tr/td/table/tr/td/a/../..";
						doc.DocumentNode.SelectNodes(xpathTest).Select(p => p.InnerHtml).Dump();
						*/
						
						string xpath = "//table/tr/td/table/tr/td/a/../..";
	                    HtmlNodeCollection nodelistShiur = doc.DocumentNode.SelectNodes(xpath);
	                    foreach (HtmlNode nn in nodelistShiur)
	                    {
	                            Uri link = new Uri(baseURI, nn.SelectSingleNode("./td/a").Attributes["href"].Value);
	                            string strDate = nn.SelectSingleNode("./td[1]").InnerText.Trim();
	                            string strRavSubject = nn.SelectSingleNode("./td[2]").InnerText;
	                            strRavSubject = strRavSubject.Replace("(MP3 to follow, Click for WMA Format)", "");
	                            string[] tab = strRavSubject.Split('-');
	
	                            Shiur shiur = new Shiur();
	                            shiur.Rav = tab[0].Trim(new char[] { '\r', '"' }).Trim();
	                            shiur.Subject = tab[1].Trim().Trim(new char[] { '"' });
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
	
	                            shiurim.Add(shiur);
						}
						
						shiurim.Dump();
}

// Define other methods and classes here

