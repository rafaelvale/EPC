using Rohr.PMWeb;
using System;
using System.Threading;
using System.Web;

namespace Rohr.EPC.Web
{
    public class PMWebDownloaderDB : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.QueryString["Rand"] != null)
                {
                    File file = new File().GetFile(context.Request.QueryString["Rand"]);

                    if (file == null)
                        throw new ApplicationException("Não foi possível baixar o arquivo.");

                    context.Response.Clear();
                    context.Response.ClearContent();
                    context.Response.ContentType = file.ContentType;
                    context.Response.AddHeader("content-disposition", "inline=" + file.FileName + file.Extension);
                    context.Response.BinaryWrite(file.FileContent);
                    context.Response.End();
                }
                else
                    throw new ApplicationException("Não foi possível baixar o arquivo.");
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.ClearContent();
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.Message);
            }
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