using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Rohr.EPC.Mobile
{
    public partial class Login : System.Web.UI.Page
    {
        private void ValidarUsuarioSenha()
        {
            try
            {
                Usuario usuario = UsuarioBusiness.ValidarUsuarioSenha(txtUsuario.Text, txtSenha.Text);

                Boolean permiteAcessoMobile = false;
                foreach (Perfil perfil in usuario.Perfis)
                {
                    if (perfil.IdPerfil == 4 || perfil.IdPerfil == 5 || perfil.IdPerfil == 7 || perfil.IdPerfil == 8 || perfil.IdPerfil == 9 || perfil.IdPerfil == 10)
                        permiteAcessoMobile = true;
                }

                if (!usuario.Ativo)
                    lblMensagem.Text = "Usuário desativo";
                else if (usuario.PrimeiroAcesso)
                    throw new Exception("O primeiro acesso não pode ser realizado através da versão mobile.");
                else if (!permiteAcessoMobile)
                    throw new Exception("No momento a versão mobile do EPC não oferece suporte para o seu perfil.");
                else
                {
                    Session["usuario"] = usuario;
                    SetarPerfil(usuario);
                    new AuditoriaLogBusiness().AdicionarLogLogin(Request.Browser);

                    FormsAuthenticationTicket o = new FormsAuthenticationTicket(usuario.IdUsuario.ToString(), false, 40);
                    FormsAuthentication.Encrypt(o);
                    FormsAuthentication.RedirectFromLoginPage(usuario.IdUsuario.ToString(), false);
                    Response.Redirect("Lista.aspx", false);
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
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        private void SetarPerfil(Usuario usuario)
        {
            if (usuario.Perfis == null || usuario.Perfis.Count == 0)
                throw new MyException("Não foi possível recuperar o perfil do usuário");

            var idPerfil = from p in usuario.Perfis
                           where p.IdPerfil == 8
                           select p.IdPerfil;

            if (idPerfil.Any())
                Session["perfil"] = 8;
            else
                Session["perfil"] = usuario.Perfis[0].IdPerfil;
        }
        private void VerificarNavegador()
        {
            HttpBrowserCapabilities navegador = Request.Browser;

            if (navegador.Browser != "IE" || navegador.MajorVersion > 9)
                lblNavegador.Visible = false;
            else
            {
                lblNavegador.Text = "O seu navegador não é compatível com o EPC. Entre em contato com o DSI para que seja feita uma atualização.";
                lblNavegador.Visible = true;
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
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["totalOportunidade"] = "";
                Session["totalContrato"] = "";

                VerificarNavegador();
                Util.ObterServidorDados(lblServidor);

                if (!IsPostBack && Request.QueryString["reason"] != null && Request.QueryString["reason"] == "@@M$")
                {
                    throw new Exception("Várias sessões com o mesmo usuário não é permitido.");
                }
            }
            catch (Exception ex)
            {
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                lblMensagem.Text = ex.Message;

                Session.Abandon();
                Session.Clear();
                FormsAuthentication.SignOut();
            }
        }
    }
}