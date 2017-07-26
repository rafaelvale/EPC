using System.Web.UI;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class Detalhe : Page
    {
        Documento _documento;
        void CarregarPainelDetalhes()
        {
            Documento documentoProposta = null;
            if (!_documento.EProposta)
            {
                DocumentoBusiness oDocumentoBusiness = new DocumentoBusiness();
                Int32 idDocumentoProposta = oDocumentoBusiness.ObterIdDocumentoProposta(_documento.IdDocumento);
                documentoProposta = oDocumentoBusiness.ObterPorId(idDocumentoProposta);
                lblContrato.Visible = true;
                divContrato.Visible = true;
            }
            List<Label> listControles = new List<Label> { lblOportunidade, lblCliente, lblObra, lblModelo, lblComercial, lblContrato };
            new Util().CarregarPainelDetalhes(listControles, _documento, documentoProposta);
        }
        void HabilitarAbaJuridico()
        {
            if (new Util().GetSessaoPerfilAtivo() != 3 || _documento.EProposta || _documento.Modelo.IdModelo == 3)
                return;
            Workflow oWorkflow = new WorkflowBusiness().ObterUltimaAcaoPorIdDocumento(_documento.IdDocumento);

            liAcaoJuridico.Visible = true;

            if (oWorkflow.WorkflowAcao.IdWorkflowAcao == 7 || oWorkflow.WorkflowAcao.IdWorkflowAcao == 8
                || oWorkflow.WorkflowAcao.IdWorkflowAcao == 9 || oWorkflow.WorkflowAcao.IdWorkflowAcao == 10 || oWorkflow.WorkflowAcao.IdWorkflowAcao == 11
                || oWorkflow.WorkflowAcao.IdWorkflowAcao == 12 || oWorkflow.WorkflowAcao.IdWorkflowAcao == 13 || oWorkflow.WorkflowAcao.IdWorkflowAcao == 14)
                btnLiberarDocumento.Enabled = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _documento = new Util().GetSessaoDocumento();
                CarregarPainelDetalhes();
                HabilitarAbaJuridico();

                Repeater1.DataSource = new DocumentoBusiness().ObterTodasVersoes(_documento.NumeroDocumento);
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void lkVisualizar_Click(object sender, EventArgs e)
        {
            LinkButton oLinkButtton = (LinkButton)sender;
            Int32 idDocumento = 0;

            if (oLinkButtton.CommandName == "idDocumento")
                idDocumento = Int32.Parse(oLinkButtton.CommandArgument);

            Session["documentoBaixar"] = null;
            Session["documentoBaixar"] = new DocumentoBusiness().RecuperarDocumento(idDocumento);

            Page.ClientScript.RegisterStartupScript(GetType(), "newOpen", Util.AbrirPopup(ResolveUrl("~/BaixarDocumento.ashx?p=a1a1&v=t"), 830, 650), false);
        }
        protected void lkOrcamento_Click(object sender, EventArgs e)
        {
            LinkButton oLinkButtton = (LinkButton)sender;
            Int32 idDocumento = 0;

            if (oLinkButtton.CommandName == "idDocumento")
                idDocumento = Int32.Parse(oLinkButtton.CommandArgument);

            Session["documentoBaixar"] = null;
            Session["documentoBaixar"] = new DocumentoBusiness().RecuperarDocumento(idDocumento);

            Page.ClientScript.RegisterStartupScript(GetType(), "newOpen", Util.AbrirPopup(ResolveUrl("~/BaixarDocumento.ashx?p=a2b2"), 830, 250), false);
        }
        protected void btnLiberarDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                Workflow workflow = new Workflow
                {
                    IdUsuario = new Util().GetSessaoUsuario().IdUsuario,
                    IdPerfil = new Util().GetSessaoPerfilAtivo(),
                    WorkflowAcao = new WorkflowAcao(54),
                    IdDocumento = _documento.IdDocumento,
                    Justificativa = txtObservacao.Text
                };
                WorkflowBusiness oWorkflowBusiness = new WorkflowBusiness();
                oWorkflowBusiness.ExecutarAcao(workflow, WorkflowBusiness.Acao.Reprovar, _documento);
                Response.Redirect(String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("99")), false);
            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
            }
        }
    }
}