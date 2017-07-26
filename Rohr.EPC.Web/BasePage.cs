using System;
using System.Web.UI;

namespace Rohr.EPC.Web
{
    public class BasePage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (Session["usuario"] == null)
                Response.Redirect("Redirecionar.aspx");
        }
    }
}