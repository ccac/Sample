using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

using SilverlightDemoLiveSearch_Web.LiveSearchService;
using System.Collections.Generic;

namespace SilverlightDemoLiveSearch_Web
{
    /// <summary>
    /// Summary description for LiveSearchWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LiveSearchWebService : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        [WebMethod]
        public SearchResultItem[] DoSearch(string query)
        {
            MSNSearchPortTypeClient s = new MSNSearchPortTypeClient();
            SearchRequest searchRequest = new SearchRequest();
            int arraySize = 1;
            SourceRequest[] sr = new SourceRequest[arraySize];

            sr[0] = new SourceRequest();
            sr[0].Source = SourceType.Web;

            searchRequest.Query = query;
            searchRequest.Requests = sr;

            searchRequest.AppID = "C0680205851CCC0E38946DB8FF74156C1C826A86";
            searchRequest.CultureInfo = "zh-CN";
            SearchResponse searchResponse;

            searchResponse = s.Search(searchRequest);

            List<SearchResultItem> lists = new List<SearchResultItem>();
            foreach (SourceResponse sourceResponse in searchResponse.Responses)
            {
                Result[] sourceResults = sourceResponse.Results;
                foreach (Result sourceResult in sourceResults)
                {
                    SearchResultItem item = new SearchResultItem();
                    if ((sourceResult.Title != null) && (sourceResult.Title != String.Empty))
                        item.Title = sourceResult.Title;

                    if ((sourceResult.Description != null) && (sourceResult.Description != String.Empty))
                        item.Description = sourceResult.Description;

                    if ((sourceResult.Url != null) && (sourceResult.Url != String.Empty))
                        item.Url = sourceResult.Url;

                    lists.Add(item);
                }
            }
            return lists.ToArray();
        }
    }
}
