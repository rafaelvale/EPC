using System;
using System.Web;
using Rohr.PMWeb;

namespace Rohr.EPC.Web
{
    public class GetImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["entityId"] == null) return;
            Int32 entityId = Int32.Parse(context.Request.QueryString["entityId"]);

            File file = new File().GetImage(entityId);


            if (file.FileContent == null) return;
            context.Response.ContentType = "image/png";
            context.Response.BinaryWrite(file.FileContent);



        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}