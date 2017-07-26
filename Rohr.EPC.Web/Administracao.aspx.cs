using Rohr.EPC.Business;
using System;

namespace Rohr.EPC.Web
{
    public partial class Administracao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            CarregarUsuarios();
        }

        void CarregarUsuarios()
        {
            repeaterUsuarios.DataSource = UsuarioBusiness.ObterTodos();
            repeaterUsuarios.DataBind();
        }
    }
}