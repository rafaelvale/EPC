using Rohr.EPC.Business;
using System;
using System.Web;
using System.Web.UI;

namespace Rohr.EPC.Web
{
    public partial class Erro : Page
    {
        public String erro;
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception ex = HttpContext.Current.Server.GetLastError();
            NLog.Log().Error(ex);
            ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            erro = ex.Message;
        }
    }
}