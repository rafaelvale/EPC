using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq;

namespace Rohr.EPC.Web
{
    public partial class ConfiguracoesConta : BasePage
    {
        void ExibirAlerta(Usuario usuario)
        {
            if (usuario.PrimeiroAcesso)
            {
                lblAlerta.Text = "Este é o seu primeiro acesso. Para continuar altere a sua senha.";
                lblAlerta.Visible = true;

                ckListNotificacoes.Enabled = true;
                panelSenha.Visible = true;
                linkEditarSenha.Visible = false;
                linkEditarNotificacoes.Visible = false;
                panelAcao.Visible = true;
                panelSessaoSenha.Visible = false;
                panelAtividadeConta.Visible = false;
            }
            else
            {
                lblAlerta.Text = String.Empty;
                lblAlerta.Visible = false;
                panelAtividadeConta.Visible = true;
            }
        }
        void CarregarDadosUsuario(Usuario usuario)
        {
            lblNome.Text = String.Format("{0} {1}", usuario.PrimeiroNome, usuario.Sobrenome);
            if (usuario.DataUltimaAlteracaoSenha.HasValue)
                lblDataAlteracaoSenha.Text = String.Format("{0} às {1}", usuario.DataUltimaAlteracaoSenha.Value.ToString("dd/MM/yyyy"), usuario.DataUltimaAlteracaoSenha.Value.ToString("HH:mm:ss"));
            lblCadastroUsuario.Text = usuario.DataCadastro.ToString("dd/MM/yyyy");
            lblMatricula.Text = usuario.Matricula.ToString();


            lblEmail.Text = usuario.Email ?? "Não foi informado";
        }
        void CarregarFilialUsuario(Usuario usuario)
        {
            lblFilial.Text = String.Empty;

            lblFilial.Text = usuario.Filiais.Count == 1 ? "Filial: " : "Filiais: ";

            for (int i = 0; i < usuario.Filiais.Count; i++)
            {
                Company oCompany = new FilialBusiness().ObterFilialPmweb(usuario.Filiais[i].CodigoOrigem);
                lblFilial.Text += oCompany.CompanyName.ToLower();
                if (i != usuario.Filiais.Count - 1)
                    lblFilial.Text += " | ";
            }
        }
        void CarregarPerfilUsuario(Usuario usuario)
        {
            lblPerfil.Text = String.Empty;

            lblPerfil.Text = usuario.Perfis.Count == 1 ? "Perfil: " : "Perfis: ";

            for (int i = 0; i < usuario.Perfis.Count; i++)
            {
                lblPerfil.Text += usuario.Perfis[i].Descricao;
                if (i != usuario.Perfis.Count - 1)
                    lblPerfil.Text += " | ";
            }
        }
        void CarregarPerfilNotificacoesEmail(Usuario usuario)
        {
            if (usuario.PrimeiroAcesso) return;

            NotificacaoUsuario oNotificacaoUsuario = new NotificacaoUsuarioBusiness().ObterPorIdUsuario(usuario.IdUsuario);

            var idPerfil = from p in usuario.Perfis
                           where p.IdPerfil == 8
                           select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Análise do Gerente da filial", "", false));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseGerente;
            }

            idPerfil = from p in usuario.Perfis
                       where p.IdPerfil == 5
                       select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Análise da Superintendência", "", false));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseSuperintendencia;
            }

            idPerfil = from p in usuario.Perfis
                       where p.IdPerfil == 9
                       select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Diretoria Operacional", "", false));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseDiretoriaOperacional;
            }

            idPerfil = from p in usuario.Perfis
                       where p.IdPerfil == 10
                       select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Vice-Presidência", "", false));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseVicePresidencia;
            }

            idPerfil = from p in usuario.Perfis
                       where p.IdPerfil == 3
                       select p.IdPerfil;

            if (idPerfil.Any())
            {
                ckListNotificacoes.Items.Add(new ListItem("Análise do Jurídico", "", false));
                ckListNotificacoes.Items[ckListNotificacoes.Items.Count - 1].Selected = oNotificacaoUsuario.AnaliseJuridico;
            }
        }
        NotificacaoUsuario RecuperarNotificacoesUsuarioEmail()
        {
            NotificacaoUsuario oNotificacaoUsuario = new NotificacaoUsuario
            {
                AnaliseGerente = ckListNotificacoes.Items[0].Selected,
                AnaliseSuperintendencia = ckListNotificacoes.Items[1].Selected,
                AnaliseJuridico = ckListNotificacoes.Items[2].Selected,
                ContratoAssinadoCliente = ckListNotificacoes.Items[3].Selected,
                ContratoArquivado = ckListNotificacoes.Items[4].Selected,
                ResumoDiario = ckListNotificacoes.Items[5].Selected,
                ResumoSemanal = ckListNotificacoes.Items[6].Selected,
                ResumoMensal = ckListNotificacoes.Items[7].Selected
            };

            return oNotificacaoUsuario;
        }
        void AtualizarUsuario()
        {
            Usuario oUsuario = new Util().GetSessaoUsuario();
            UsuarioBusiness.AlterarSenha(txtSenhaAtual.Text, txtNovaSenha.Text, txtConfirmarSenha.Text, oUsuario);
            new AuditoriaLogBusiness().AdicionarLogAlteracaoSenha(Request.Browser);
            new NotificacaoUsuarioBusiness().Atualizar(oUsuario, RecuperarNotificacoesUsuarioEmail());
            new AuditoriaLogBusiness().AdicionarLogAlteracaoNotificacao(Request.Browser);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack) return;

                Usuario oUsuario = UsuarioBusiness.ObterPorId(new Util().GetSessaoUsuario().IdUsuario);
                ExibirAlerta(oUsuario);
                CarregarDadosUsuario(oUsuario);
                CarregarFilialUsuario(oUsuario);
                CarregarPerfilUsuario(oUsuario);
                CarregarPerfilNotificacoesEmail(oUsuario);
            }
            catch (Exception ex)
            {
                lblAlerta.Text = ex.Message;
                lblAlerta.Visible = true;
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                AtualizarUsuario();
                Usuario oUsuario = new Util().GetSessaoUsuario();
                oUsuario.PrimeiroAcesso = false;
                Session["usuario"] = oUsuario;

                Response.Redirect(String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("99")), false);
            }
            catch (Exception ex)
            {
                lblAlerta.Text = ex.Message;
                lblAlerta.Visible = true;
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnDesistir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(
                    new Util().GetSessaoUsuario().PrimeiroAcesso
                        ? String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("0"))
                        : String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("99")), false);
            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
    }
}