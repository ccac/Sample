<%@ WebHandler Language="C#" Class="Mashup" %>

using System;
using System.Web;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

public class Mashup : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string[] urls = {
                  "http://blogs.msdn.com/webnext/rss.xml",
                  "http://blogs.msdn.com/tims/rss.xml",
                  "http://feeds.feedburner.com/JesseLiberty-SilverlightGeek"
                };
        List<SyndicationFeed> feeds = new List<SyndicationFeed>();
        foreach (string url in urls)
        {
            using (XmlReader feedReader = XmlReader.Create(url))
            {
                feeds.Add(SyndicationFeed.Load(feedReader));
            }
        }
        DateTime minDate = DateTime.Now.AddDays(-30);
        var mashedItems =
            from feed in feeds
            from item in feed.Items
            where item.PublishDate > minDate
            orderby item.PublishDate descending
            select item;

        int maxResults = 15;
        SyndicationFeed mashedFeed = new SyndicationFeed(mashedItems.Take(maxResults));
        using (XmlWriter writer = XmlWriter.Create(context.Response.OutputStream))
        {
            context.Response.ContentType = "text/xml";
            mashedFeed.SaveAsRss20(writer);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}