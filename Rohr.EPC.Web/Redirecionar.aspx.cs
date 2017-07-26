using System.Web.UI;
using Rohr.EPC.Business;
using System;
using System.Web;
using System.Web.Security;

namespace Rohr.EPC.Web
{
    public partial class Redirecionar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["p"] != null)
                {
                    String destino = Request.QueryString["p"];
                    Redirect(CriptografiaBusiness.Descriptografar(destino));
                }
                else
                {
                    NLog.Log().Error("Não foi possível recuperar a sessão do usuário");
                    RemoverSessao();
                    Redirect("0");
                }
            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        private void Redirect(String destino)
        {
            try
            {
                switch (destino)
                {
                    case "0":
                        new AuditoriaLogBusiness().AdicionarLogLogout(Request.Browser);
                        RemoverSessao();
                        FormsAuthentication.SignOut();
                        Response.Redirect("Signout.aspx", false);
                        break;
                    case "10":
                    case "9":
                    case "8":
                    case "7":
                    case "6":
                    case "5":
                    case "4":
                    case "3":
                    case "2":
                    case "1":
                    {
                        Int32 idPerfil = Int32.Parse(destino);
                        SetarPerfilUsuario(idPerfil);
                        Response.Redirect(ResolveUrl("~/Cockpit.aspx"), false);
                    }
                        break;
                    case "50":
                        Response.Redirect(ResolveUrl("~/ConfiguracoesConta.aspx"), false);
                        break;
                    case "101":
                        Session["documento"] = null;
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/PropostaDocumento.aspx"), false);
                        break;
                    case "102": //Geração de contrato (1 versão)
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/ContratoDocumento.aspx"), false);
                        break;
                    case "400": //Edição de contrato.
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/Variaveis.aspx"), false);
                        break;
                    case "455":
                        Session["listWorkflow"] = null;
                        Response.Redirect("./WorkflowView.aspx", false);
                        break;
                    case "500":
                        Session["documento"] = null;
                        Response.Redirect(ResolveUrl("~/Acoes.aspx"), false);
                        break;
                    case "103":
                        Response.Redirect(ResolveUrl("~/AnaliseJuridico.aspx"), false);
                        break;
                    case "99":
                        Session["documento"] = null;
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/Cockpit.aspx"), false);
                        break;
                    //case "201":
                    //    Session["documento"] = null;
                    //    Session["listWorkflow"] = null;
                    //    Response.Redirect(ResolveUrl("~/Relatorios/RelatorioSLA.aspx"), false);
                    //    break;
                    case "202":
                        Session["documento"] = null;
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/Relatorios/DocumentosEmCirculacao.aspx"), false);
                        break;
                    case "203":
                        Session["documento"] = null;
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/Relatorios/DocumentosCancelados.aspx"), false);
                        break;
                    //case "204":
                    //    Session["documento"] = null;
                    //    Session["listWorkflow"] = null;
                    //    Response.Redirect(ResolveUrl("~/Relatorios/EfetividadeComercial.aspx"), false);
                    //    break;
                    //case "205":
                    //    Session["documento"] = null;
                    //    Session["listWorkflow"] = null;
                    //    Response.Redirect(ResolveUrl("~/Relatorios/Performance.aspx"), false);
                    //    break;
                    case "206":
                        Session["documento"] = null;
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/Relatorios/WorkflowMetas.aspx"), false);
                        break;
                    case "207":
                        Session["documento"] = null;
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/Relatorios/Pesquisa.aspx"), false);
                        break;
                    case "208":
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/Detalhe.aspx"), false);
                        break;
                    case "308":
                        Response.Redirect(ResolveUrl("~/Admin/UploadFotos.aspx"), false);
                        break;
                    default:
                        NLog.Log().Error("Não foi possível recuparar o destino.");
                        Session["documento"] = null;
                        Session["listWorkflow"] = null;
                        Response.Redirect(ResolveUrl("~/Login.aspx?error=true"), false);
                        break;
                }
            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                RemoverSessao();
                Response.Redirect(ResolveUrl("~/Login.aspx?error=true"), false);
            }
        }
        private void RemoverSessao()
        {
            Session["usuario"] = null;
            Session["perfil"] = null;
            Session["documento"] = null;
            Session["listWorkflow"] = null;

            Session.RemoveAll();
            Session.Abandon();
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