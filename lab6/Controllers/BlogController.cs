using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lab6.common;
using lab6.Models;
using System.ServiceModel.Syndication;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace lab6.Controllers
{
    
    public class BlogController : Controller
    {
        // GET: Blog
        TMDT6 db = new TMDT6();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostFeed(string type)
        {
            Category category = db.Categories.Where(s => s.Alias.Contains(type)).FirstOrDefault();
            if(category==null)
            {
                return HttpNotFound();
            }
            IEnumerable<Article> posts = (from s in db.Articles where s.Category.Alias.Contains(type) select s).ToList();
            var feed = new SyndicationFeed(category.Name, "RSS Feed", new Uri("https://vnexpress.net/RSS"), Guid.NewGuid().ToString(),
                DateTime.Now);
            var items = new List<SyndicationItem>();
            foreach(Article art in posts)
            {
                string postUrl = string.Format("https://vnexpress.net/" + art.Alias + "-{0}", art.Id);
                var item = new SyndicationItem(Helper.RemoveIllegalCharacters(art.Tittle),
                    Helper.RemoveIllegalCharacters(art.Description),
                    new Uri(postUrl), art.Id.ToString(), art.DatePulished.Value);
                items.Add(item);
            }

            return new RSSActionResult { Feed = feed };
        }
        public ActionResult ReadRSS()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReadRSS(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            WebClient w = new WebClient();
            w.Encoding = ASCIIEncoding.UTF8;
            string RSSData = w.DownloadString(url);
            XDocument xml = XDocument.Parse(RSSData, LoadOptions.PreserveWhitespace);
            var RSSFeedData = (from x in xml.Descendants("item")
                               select new RSSFeed
                               {
                                   Title = ((string)x.Element("title")),
                                   Link = ((string)x.Element("link")),
                                   Description = ((string)x.Element("description")),
                                   PubDate = ((string)x.Element("pubDate")),

                               });
            ViewBag.RSSFeed = RSSFeedData;
            ViewBag.URL = url;
            return View();
        }

    }
}