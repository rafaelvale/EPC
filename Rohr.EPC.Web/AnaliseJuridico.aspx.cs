using System.Drawing;
using CKEditor.NET;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class AnaliseJuridico : BasePage
    {
        Documento _documento;
        protected String PartesDocumento;
        void CarregarPainelDetalhes()
        {
            Documento documentoProposta = null;
            if (!_documento.EProposta)
            {
                DocumentoBusiness oDocumentoBusiness = new DocumentoBusiness();
                Int32 idDocumentoProposta = oDocumentoBusiness.ObterIdDocumentoProposta(_documento.IdDocumento);
                documentoProposta = oDocumentoBusiness.ObterPorId(idDocumentoProposta);

                divContrato.Visible = true;
                lblContrato.Visible = true;
            }
            List<Label> listControles = new List<Label> { lblOportunidade, lblCliente, lblObra, lblModelo, lblComercial, lblContrato };
            new Util().CarregarPainelDetalhes(listControles, _documento, documentoProposta);
        }
        void MontarCkEditor(Documento documento)
        {
            foreach (Parte parte in documento.Modelo.ListParte)
            {
                if (!parte.Exibir) continue;
                CKEditorControl ck = new CKEditorControl
                {
                    EnterMode = EnterMode.BR,
                    config =
                    {
                        pasteFromWordPromptCleanup = true,
                        forcePasteAsPlainText = true,
                        removeDialogTabs = "image:Link",
                        toolbar =
                            new object[]
                            {
                                new object[]
                                {
                                    "Maximize", "-", "Table", "-", "Cut", "Copy", "-", "Undo", "Redo", "-", "Bold",
                                    "Italic", "Underline", "-", "RemoveFormat", "Link"
                                }
                            }
                    },
                    IgnoreEmptyParagraph = false,
                    ReadOnly = true,
                    UIColor = Color.White
                };


                if (documento.ListPartePreenchida != null && (documento.ListPartePreenchida != null || documento.ListPartePreenchida.Count != 0))
                {
                    Parte parte1 = parte;
                    var resultPartePreenchida = documento.ListPartePreenchida.Where(x => x.IdParte == parte1.IdParte);

                    var partePreenchidas = resultPartePreenchida as IList<PartePreenchida> ?? resultPartePreenchida.ToList();
                    if (partePreenchidas.Any())
                    {
                        if (!partePreenchidas.First().Texto.Equals(Server.HtmlDecode(parte.TextoParte)))
                            ck.UIColor = Color.Red;


                        ck.Text = new Util().SubstituirChaves(partePreenchidas.First().Texto, partePreenchidas.First().IdParte, documento, true);
                        ck.ID = parte.IdParte.ToString();
                    }
                    else
                    {
                        ck.ID = parte.IdParte.ToString();
                        ck.Text = parte.TextoParte;
                    }
                }
                panelPartes.Controls.Add(ck);
                PartesDocumento += String.Format("<li><a href=\"#cke_contentBody_{0}\">{1}</a>", ck.ID, parte.Nome);
            }
        }

        void MontarCkEditorObservacaoObjeto(Documento documento)
        {
            if (String.IsNullOrWhiteSpace(documento.DocumentoObjetoObservacao.Observacao)) return;
            CKEditorControl ck = new CKEditorControl
            {
                EnterMode = EnterMode.BR,
                config =
                {
                    pasteFromWordPromptCleanup = true,
                    forcePasteAsPlainText = true,
                    removeDialogTabs = "image:Link",
                    toolbar =
                        new object[]
                        {
                            new object[]
                            {
                                "Maximize", "-", "Table", "-", "Cut", "Copy", "-", "Undo", "Redo", "-", "Bold",
                                "Italic", "Underline", "-", "RemoveFormat", "Link"
                            }
                        }
                },
                IgnoreEmptyParagraph = false,
                ReadOnly = true,
                UIColor = Color.White
            };


            ck.UIColor = Color.Red;
            ck.Text = documento.DocumentoObjetoObservacao.Observacao;
            ck.ID = "ckEditorObservacoesObjeto";

            panelPartes.Controls.Add(ck);
            PartesDocumento += String.Format("<li><a href=\"#cke_contentBody_{0}\">{1}</a>", ck.ID,
                "Observações Objeto");
        }
        void ValidarJustificativa()
        {
            if (String.IsNullOrWhiteSpace(txtObservacao.Text))
                throw new MyException("Informe uma observação.");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _documento = new Util().GetSessaoDocumento();

                if (Util.VerificarModeloCliente(_documento.Modelo.ModeloTipo))
                {
                    lblDescricao.Text = "Contrato do Cliente. Baixe o PDF.";
                    lblDescricao.CssClass = "label label-info";
                    lblInstrucao.Visible = false;
                    PartesDocumento += "<li><strong>Contrato do Cliente</strong></li>";
                }
                else
                {
                    PartesDocumento += "<li><strong>Contrato</strong></li>";
                    MontarCkEditorObservacaoObjeto(_documento);
                    MontarCkEditor(_documento);
                }
                CarregarPainelDetalhes();
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["documento"] == null)
                    throw new MyException("Não foi possível recuperar o documento.");

                List<Workflow> listWorkflow = (List<Workflow>)(Session["listWorkflow"]);

                if (listWorkflow.Count == 1)
                {
                    Workflow workflow = listWorkflow[0];
                    workflow.Justificativa = txtObservacao.Text;
                    workflow.IdUsuario = new Util().GetSessaoUsuario().IdUsuario;
                    workflow.IdPerfil = new Util().GetSessaoPerfilAtivo();
                    Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento);
                    WorkflowBusiness oWorkflowBusiness = new WorkflowBusiness();
                    oWorkflowBusiness.ExecutarAcao(workflow, WorkflowBusiness.Acao.Aprovar, oDocumento);
                    new AuditoriaLogBusiness().AdicionarLogDocumentoAcaoPrimaria(oDocumento, Request.Browser);

                    Response.Redirect(String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("99")), false);
                }
                else
                    throw new MyException("Não foi possível identificar o evento.");
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnDesistir_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarJustificativa();

                if (Session["documento"] == null)
                    throw new MyException("Não foi possível recuperar o documento.");

                List<Workflow> listWorkflow = (List<Workflow>)(Session["listWorkflow"]);

                if (listWorkflow.Count == 1)
                {
                    Workflow workflow = listWorkflow[0];
                    workflow.Justificativa = txtObservacao.Text;
                    workflow.IdUsuario = new Util().GetSessaoUsuario().IdUsuario;
                    workflow.IdPerfil = new Util().GetSessaoPerfilAtivo();
                    Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento);
                    WorkflowBusiness oWorkflowBusiness = new WorkflowBusiness();
                    oWorkflowBusiness.ExecutarAcao(workflow, WorkflowBusiness.Acao.Reprovar, oDocumento);
                    new AuditoriaLogBusiness().AdicionarLogDocumentoAcaoSecundaria(oDocumento, Request.Browser);

                    Response.Redirect(String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("99")), false);
                }
                else
                    throw new MyException("Não foi possível identificar o evento.");
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
    }
}