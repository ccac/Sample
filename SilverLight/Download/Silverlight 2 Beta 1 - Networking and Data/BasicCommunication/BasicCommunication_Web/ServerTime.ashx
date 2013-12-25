<%@ WebHandler Language="C#" Class="ServerTime" %>

using System;
using System.Web;

public class ServerTime : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        //context.Response.Write(DateTime.Now.ToLongTimeString());
        DateTime result = DateTime.Now;
        if (context.Request.HttpMethod == "POST")
        {
            result = DateTime.Parse(context.Request.Form["NewTime"]);
        }
        context.Response.Write(result.ToLongTimeString());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}