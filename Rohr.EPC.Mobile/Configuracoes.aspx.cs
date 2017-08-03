using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Web;

namespace Rohr.EPC.Mobile
{
    public partial class Configuracoes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MenuRodape.Menu = 3;
                if (Request.QueryString["p"] != null)
                {
                    SetarPerfilUsuario(Convert.ToInt32(Request.QueryString["p"]));
                    Response.Redirect("Lista.aspx",false);
                }

                ObterPerfisUsuario(new Util().GetSessaoUsuario());
            }
            catch (Exception ex)
            {
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                Response.Redirect(ResolveUrl("~/Login.aspx?error=true"), false);
            }
        }

        private void ObterPerfisUsuario(Usuario usuario)
        {
            List<Perfil> listPerfil = new List<Perfil>();

            foreach (Perfil perfil in usuario.Perfis)
            { 
                //Incluso no dia 23/05 o Supervisor PTA, conforme solicitação do Reginaldo Rocha
                if (perfil.IdPerfil == 5 || perfil.IdPerfil == 8 || perfil.IdPerfil == 9 || perfil.IdPerfil == 10 || perfil.IdPerfil == 7)
                    listPerfil.Add(perfil);
            }

            Repeater1.DataSource = listPerfil;
            Repeater1.DataBind();
        }
        private void SetarPerfilUsuario(Int32 idPerfil)
        {
            Session["perfil"] = null;
            Session["documento"] = null;
            Session["listWorkflow"] = null;

            Session["perfil"] = idPerfil;
        }
    }
}