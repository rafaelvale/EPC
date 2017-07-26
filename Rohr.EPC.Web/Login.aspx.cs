using System.Web.UI;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace Rohr.EPC.Web
{
    public partial class Login : Page
    {
        private void ValidarUsuarioSenha()
        {
            try
            {
                Usuario usuario = UsuarioBusiness.ValidarUsuarioSenha(txtUsuario.Text, txtSenha.Text);

                //ValidarMultiploAcesso(usuario.IdUsuario);

                if (!usuario.Ativo)
                    lblMensagem.Text = "Usuário desativado";
                else if (usuario.PrimeiroAcesso)
                {
                    Session["usuario"] = usuario;
                    SetarPerfil(usuario);

                    FormsAuthenticationTicket o = new FormsAuthenticationTicket(usuario.IdUsuario.ToString(), false, 40);
                    FormsAuthentication.Encrypt(o);
                    FormsAuthentication.RedirectFromLoginPage(usuario.IdUsuario.ToString(), false);
                    Response.Redirect("ConfiguracoesConta.aspx", false);
                }
                else
                {
                    Session["usuario"] = usuario;
                    SetarPerfil(usuario);
                    new AuditoriaLogBusiness().AdicionarLogLogin(Request.Browser);

                    FormsAuthenticationTicket o = new FormsAuthenticationTicket(usuario.IdUsuario.ToString(), false, 40);
                    FormsAuthentication.Encrypt(o);
                    FormsAuthentication.RedirectFromLoginPage(usuario.IdUsuario.ToString(), false);
                    Response.Redirect(String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("99")), false);
                }
            }
            catch (MyException ex)
            {
                lblMensagem.Text = ex.Message;
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        private void SetarPerfil(Usuario usuario)
        {
            if (usuario.Perfis == null || usuario.Perfis.Count == 0)
                throw new MyException("Não foi possível recuperar o perfil do usuário");

            var idPerfil = from p in usuario.Perfis
                           where p.IdPerfil == 1
                           select p.IdPerfil;

            if (idPerfil.Any())
                Session["perfil"] = 1;
            else
            {
                var idPerfilTv = from p in usuario.Perfis
                                 where p.IdPerfil == 7
                                 select p.IdPerfil;

                if (idPerfilTv.Any())
                    Session["perfil"] = 7;
                else
                    Session["perfil"] = usuario.Perfis[0].IdPerfil;
            }
        }
        private void VerificarNavegador()
        {
            HttpBrowserCapabilities navegador = Request.Browser;

            if (navegador.Browser != "IE" || navegador.MajorVersion > 9)
                lblNavegador.Visible = false;
            else
            {
                logoEpc.Visible = false;
                lblNavegador.Text = "O seu navegador não é compatível com o EPC. Entre em contato com o DSI para que seja feita uma atualização.";
                lblNavegador.Visible = true;
                btnAcessar.Enabled = false;
                NLog.Log().Fatal("O seu navegador não é compatível com o EPC. Entre em contato com o DSI para que seja feita uma atualização");
            }
        }
        void VerificarTipoAcesso()
        {
            if (Util.ObterTipoAcesso(Request.ServerVariables["HTTP_USER_AGENT"]) == Util.TipoAcesso.Celular)
            {
                Exception oException = new Exception("Acesso mobile interno");
                ExcecaoBusiness.Adicionar(oException, HttpContext.Current.Request.Url.AbsolutePath);
                Response.Redirect("http://10.11.0.55:8073", false);
            }
        }   
        private void ValidarMultiploAcesso(Int32 idUsuario)
        {
            const string msg = "<script type=\"text/javascript\">alert('Este usuário já está conectado. Será feito logout do usuário conectado.');</script>";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
            Dictionary<Int32, string> dic = ((Dictionary<Int32, string>)Application["Sessions"]);
            if (Application["Sessions"] == null)
            {
                dic = new Dictionary<Int32, string>();
                Application.Add("Sessions", dic);
                ((Dictionary<Int32, string>)Application["Sessions"]).Add(idUsuario, Session.SessionID);

            }
            else
            {
                if (dic.ContainsKey(idUsuario))
                {
                    ((Dictionary<Int32, string>)Application["Sessions"]).Remove(idUsuario);
                    ((Dictionary<Int32, string>)Application["Sessions"]).Add(idUsuario, Session.SessionID);
                }
                else
                {
                    ((Dictionary<Int32, string>)Application["Sessions"]).Add(idUsuario, Session.SessionID);
                }
            }
        }
        protected void btnAcessar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarUsuarioSenha();
            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                VerificarTipoAcesso();
                VerificarNavegador();
                Util.ObterServidorDados(lblServidor);

                if (!IsPostBack && Request.QueryString["reason"] != null && Request.QueryString["reason"] == "@@M$")
                {
                    throw new Exception("Várias sessões com o mesmo usuário não é permitido.");
                }
            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                lblMensagem.Text = ex.Message;

                Session.Abandon();
                Session.Clear();
                FormsAuthentication.SignOut();
            }
        }
    }
}