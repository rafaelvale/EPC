using Rohr.EPC.Business;
using System;
using System.Web;

namespace Rohr.EPC.Web.MasterPage
{
    public partial class MasterAuxiliar : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
    }
}