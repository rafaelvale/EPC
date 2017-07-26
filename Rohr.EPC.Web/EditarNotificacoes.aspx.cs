using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq;

namespace Rohr.EPC.Web
{
    public partial class EditarNotificacoes : BasePage
    {
        void CarregarPerfilNotificacoesEmail(Usuario usuario)
        {
            NotificacaoUsuario oNotificacaoUsuario = new NotificacaoUsuarioBusiness().ObterPorIdUsuario(usuario.IdUsuario);

            var idPerfil = from p in usuario.Perfis
                           where p.IdPerfil == 8
                           select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Análise do Gerente da filial", "8", true));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseGerente;
            }

            idPerfil = from p in usuario.Perfis
                       where p.IdPerfil == 5
                       select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Análise da Superintendência", "5", true));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseSuperintendencia;
            }

            idPerfil = from p in usuario.Perfis
                       where p.IdPerfil == 9
                       select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Análise da Diretoria Operacional", "9", true));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseDiretoriaOperacional;
            }

            idPerfil = from p in usuario.Perfis
                       where p.IdPerfil == 10
                       select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Análise da Vice-Presidência", "10", true));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseVicePresidencia;
            }

            idPerfil = from p in usuario.Perfis
                       where p.IdPerfil == 3
                       select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Análise do Jurídico", "3", true));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseJuridico;
            }
        }
        NotificacaoUsuario RecuperarNotificacoesUsuarioEmail()
        {
            NotificacaoUsuario oNotificacaoUsuario = new NotificacaoUsuario();

            if (ckListNotificacoes.Items.FindByValue("3") != null)
                oNotificacaoUsuario.AnaliseJuridico = ckListNotificacoes.Items.FindByValue("3").Selected;

            if (ckListNotificacoes.Items.FindByValue("4") != null)
                oNotificacaoUsuario.AnaliseDiretoria = ckListNotificacoes.Items.FindByValue("4").Selected;

            if (ckListNotificacoes.Items.FindByValue("5") != null)
                oNotificacaoUsuario.AnaliseSuperintendencia =  ckListNotificacoes.Items.FindByValue("5").Selected;

            if (ckListNotificacoes.Items.FindByValue("8") != null)
                oNotificacaoUsuario.AnaliseGerente =  ckListNotificacoes.Items.FindByValue("8").Selected;

            if (ckListNotificacoes.Items.FindByValue("9") != null)
                oNotificacaoUsuario.AnaliseDiretoriaOperacional = ckListNotificacoes.Items.FindByValue("9").Selected;

            if (ckListNotificacoes.Items.FindByValue("10") != null)
                oNotificacaoUsuario.AnaliseVicePresidencia = ckListNotificacoes.Items.FindByValue("10").Selected;

            return oNotificacaoUsuario;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack) return;
                Usuario oUsuario = UsuarioBusiness.ObterPorId(new Util().GetSessaoUsuario().IdUsuario);
                txtEmail.Text = Util.LimparDominioEmail(oUsuario.Email);
                CarregarPerfilNotificacoesEmail(oUsuario);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario oUsuario = new Util().GetSessaoUsuario();
                new NotificacaoUsuarioBusiness().Atualizar(oUsuario, RecuperarNotificacoesUsuarioEmail());
                UsuarioBusiness.AtualizarEmail(txtEmail.Text, oUsuario);
                new AuditoriaLogBusiness().AdicionarLogAlteracaoNotificacao(Request.Browser);
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