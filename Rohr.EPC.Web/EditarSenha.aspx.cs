using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Web;

namespace Rohr.EPC.Web
{
    public partial class EditarSenha : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario oUsuario = new Util().GetSessaoUsuario();
                UsuarioBusiness.AlterarSenha(txtSenhaAtual.Text, txtNovaSenha.Text, txtConfirmarSenha.Text, oUsuario);
                new AuditoriaLogBusiness().AdicionarLogAlteracaoSenha(Request.Browser);
                Response.Redirect("ConfiguracoesConta.aspx", false);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
    }
}