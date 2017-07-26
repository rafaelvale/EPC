using Rohr.EPC.Business;
using System;

namespace Rohr.EPC.Web
{
    public partial class Fim : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnNovaProposta_Click(object sender, EventArgs e)
        {
            Response.Redirect("Redirecionar.aspx?p=" + CriptografiaBusiness.Criptografar("101"));
        }
        protected void btnPaginaPrincipal_Click(object sender, EventArgs e)
        {
            Response.Redirect("Redirecionar.aspx?p=" + CriptografiaBusiness.Criptografar("99"));
        }
    }
}