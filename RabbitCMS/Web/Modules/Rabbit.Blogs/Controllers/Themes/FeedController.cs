using Rabbit.Blogs.Services.Themes;
using Rabbit.Contents.Models;
using Rabbit.Contents.Services;
using Rabbit.Web.Mvc.Themes;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;

namespace Rabbit.Blogs.Controllers.Themes
{
    [Themed(false)]
    public class FeedController : Controller
    {
        private readonly IThemePostService _postService;
        private readonly SiteSettingsRecord _siteSettings;

        public FeedController(IThemePostService postService, ISiteSettingsService siteSettingsService)
        {
            _postService = postService;
            _siteSettings = siteSettingsService.Get();
        }

        public void Index()
        {
            var list = _postService.GetHomeList();

            var tenantName = _siteSettings.Name;
            var description = _siteSettings.Seo.Description;
            var url = Request.Url;
            var link = $"{url.Scheme}://{url.Authority}";

            var document = new XDocument(new XDeclaration("1.0", "utf-8", "false"));

            var rss = new XElement("rss");
            rss.SetAttributeValue("version", "2.0");

            document.Add(rss);

            var change = new XElement("channel",
                new XElement("title", tenantName),
                new XElement("link", link + Request.ApplicationPath),
                new XElement("description", description),
                new XElement("language", CultureInfo.CurrentCulture.EnglishName),
                //                new XElement("pubDate", "aaaaaa"),
                new XElement("lastBuildDate", DateTime.Now));
            rss.Add(change);

            foreach (var post in list)
            {
                var itemElement = new XElement("item",
                    new XElement("title", post.Title),
                    new XElement("description", post.Summary),
                    new XElement("author", post.User.Name),
                    new XElement("pubDate", post.CreateTime));

                var category = post.Categorys.FirstOrDefault();
                if (category != null)
                {
                    var categoryElement = new XElement("category", "asasd");
                    //            categoryElement.SetAttributeValue("domain", "http://www.fool.com/cusip");
                    itemElement.Add(categoryElement);
                }

                var routeValues = new RouteValueDictionary(new { area = "Rabbit.Blogs", post.Seo.RoutePath });
                routeValues.Add("CategoryRoutePath", category != null ? category.Seo.RoutePath : "unclassified");
                var postUrl = link + Url.Action("Detailed", "Post", routeValues);

                itemElement.Add(new XElement("link", postUrl));
                itemElement.Add(new XElement("comments", postUrl + "#comments"));
                itemElement.Add(new XElement("guid", postUrl));

                var sourceElement = new XElement("source", tenantName);
                sourceElement.SetAttributeValue("url", link);
                itemElement.Add(sourceElement);

                change.Add(itemElement);
            }

            document.Save(Response.OutputStream);

            Response.ContentType = "text/xml";
        }
    }
}