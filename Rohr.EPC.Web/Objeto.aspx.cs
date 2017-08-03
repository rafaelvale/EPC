using System.Text;
using CKEditor.NET;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class Objeto : BasePage
    {
        Documento _documento;

        protected void Page_Load(object sender, EventArgs e)
        {
            
                try
                {
                    CKEditorObservacoesObjeto.config.toolbar = new object[] { new object[] { "Maximize", "-", "Undo", "Redo", "-", "Bold", "Italic", "RemoveFormat" } };
                    CKEditorObservacoesObjeto.config.enterMode = EnterMode.BR;
                    CKEditorObservacoesObjeto.config.removePlugins = "elementspath";
                    CKEditorObservacoesObjeto.config.pasteFromWordPromptCleanup = true;
                    CKEditorObservacoesObjeto.config.forcePasteAsPlainText = true;
                    CKEditorObservacoesObjeto.config.fillEmptyBlocks = false;
                    CKEditorObservacoesObjeto.config.ignoreEmptyParagraph = false;


                    _documento = new Util().GetSessaoDocumento();
                    new DocumentoBusiness().VerificarPropostaFechadaPMWeb(_documento);
                    CarregarPainelDetalhes();
                    CarregarObjeto();
                    VerificarModeloCliente(_documento);

                    if (IsPostBack) return;

                    if (_documento.ListDocumentoObjeto == null || _documento.ListDocumentoObjeto.Count <= 0)
                        ObterItensTela();
                    else
                    {
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c02")).Checked = _documento.ListDocumentoObjeto[0].ExibirDescricaoResumida;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c04")).Checked = _documento.ListDocumentoObjeto[0].ExibirQuantidade;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c05")).Checked = _documento.ListDocumentoObjeto[0].ExibirUnidade;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c06")).Checked = _documento.ListDocumentoObjeto[0].ExibirPeso;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c07")).Checked = _documento.ListDocumentoObjeto[0].ExibirValorTabelaLocacao;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c08")).Checked = _documento.ListDocumentoObjeto[0].ExibirValorPraticadoLocacao;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c09")).Checked = _documento.ListDocumentoObjeto[0].ExibirValorTabelaIndenizacao;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c10")).Checked = _documento.ListDocumentoObjeto[0].ExibirValorPraticadoIndenizacao;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c11")).Checked = _documento.ListDocumentoObjeto[0].ExibirDesconto;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c12")).Checked = _documento.ListDocumentoObjeto[0].ExibirValorTotalItem;
                        ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c13")).Checked = _documento.ListDocumentoObjeto[0].ExibirPesoTotalItem;

                        if (_documento.Modelo.Segmento.IdSegmento == 2)
                        {
                            ckSubTotalPeso.Checked = false;
                            ckSubTotalPeso.Enabled = false;
                        }

                        ckSubtotalFaturamentoMensal.Checked = _documento.ListDocumentoObjeto[0].ExibirFaturamentoMensalObjeto;
                        ckValorTotalFaturamentoMensal.Checked = _documento.ListDocumentoObjeto[0].ExibirFaturamentoMensal;
                        ckValorTotalNegocio.Checked = _documento.ListDocumentoObjeto[0].ExibirValorNegocio;
                        ckSubtotalNegocio.Checked = _documento.ListDocumentoObjeto[0].ExibirValorNegocioObjeto;
                        ckPrevisaoUtilizacao.Checked = _documento.ListDocumentoObjeto[0].ExibirPrevisaoUtilizacao;
                        ckSubTotalPeso.Checked = _documento.ListDocumentoObjeto[0].ExibirSubTotalPeso;

                        if (_documento.Modelo.Segmento.IdSegmento == 3)
                        {
                            ckValorTotalNegocio.Checked = false;
                            ckValorTotalNegocio.Enabled = false;

                            ckSubtotalNegocio.Checked = false;
                            ckSubtotalNegocio.Enabled = false;
                        }

                        ObterItensTela();
                    }

                    if (!_documento.EProposta)
                    {
                        ckValorTotalFaturamentoMensal.Enabled = true;
                        ckValorTotalFaturamentoMensal.Checked = false;

                        ckValorTotalNegocio.Enabled = false;
                        ckValorTotalNegocio.Checked = true;
                    }

                    CarregarObservacaoObjeto(_documento);

                    ckValorTotalFaturamentoMensal.Text = String.Format("Valor total do Faturamento Mensal (R$ {0:N2})", _documento.ValorFaturamentoMensal);
                    ckValorTotalNegocio.Text = String.Format("Valor total do Negócio (R$ {0:N2})", _documento.ValorNegocio);
                }
                catch (Exception ex)
                {
                    Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                    btnContinuar.Enabled = false;
                    btnContinuar.Attributes.Add("disabled", "disabled");
                    tableColunas.Visible = false;
                    demaisOpcoes.Visible = false;
                    lkPlanilhaOrcamentaria.Visible = false;
                    NLog.Log().Error(ex);
                    ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                }
            
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {

            try
            {
                ObterItensTela();

                Session["documento"] = null;
                Session["documento"] = _documento;
                new DocumentoObjetoBusiness().AdicionarDocumentoObjeto(_documento);

                new AuditoriaLogBusiness().AdicionarLogDocumentoObjeto(_documento, Request.Browser);

                if (_documento.Modelo.Segmento.IdSegmento == 3 && _documento.Modelo.ModeloTipo.IdModeloTipo != 3)
                    Response.Redirect("Partes.aspx", false);
                else if (_documento.Modelo.ModeloTipo.IdModeloTipo == 1)
                    Response.Redirect("Resumo.aspx", false);
                else
                    Response.Redirect("Precos.aspx", false);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnDesistir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Redirecionar.aspx?p=" + CriptografiaBusiness.Criptografar("99"), false);
            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void lkPlanilhaOrcamentaria_Click(object sender, EventArgs e)
        {
            try
            {
                if (_documento.ListDocumentoObjeto == null)
                    throw new MyException("Não foi possível identificar o orçamento");
                Response.Clear();
                Response.AppendHeader("content-disposition", "attachment; filename=Planilha Orcamentaria.xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "utf-8";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(new PlanilhaOrcamentariaBusiness().GerarPlanilhaOrcamentaria(_documento));
                new AuditoriaLogBusiness().AdicionarLogPlanilhaOrcamentaria(_documento, Request.Browser);
                Response.End();
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">$(document).ready(function() {alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');});</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        void CarregarObjeto()
        {
            Int32 idEstimatePmweb;
            Estimates oEstimates;

            if (_documento.EProposta)
            {
                DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(_documento.NumeroDocumento);
                idEstimatePmweb = new Estimates().ObterEstimatePorDocumentoId(oDocumentTodoList.IdDocumentoTodoList).Id;
                oEstimates = new Estimates().ObterEstimate(idEstimatePmweb);
            }
            else
            {
                CostManagementCommitments oCostManagementCommitments = new CostManagementCommitments().ObterContrato(_documento.CodigoSistemaOrigem);
                idEstimatePmweb = oCostManagementCommitments.EstimatedId;
                oEstimates = new Estimates().ObterEstimateContrato(idEstimatePmweb);
            }

            _documento.PercentualLimpeza = Convert.ToDecimal(new Specifications().ObterPercentualLimpezaProposta(oEstimates.Id).Measure);

            lblRevisao.Text = oEstimates.RevisionNumber.ToString();
            lblDataRevisao.Text = oEstimates.RevisionDate.ToString();
            lblOrcamento.Text = oEstimates.RevisionId.ToString();
            lblIdOrcamento.Text = oEstimates.Id.ToString();

            List<ObjetoOrcamento> listObjetoOrcamento = new ObjetoOrcamento().ObterObjetoOrcamento(oEstimates.Id);

            if (listObjetoOrcamento.Count <= 1)
            {
                ckSubtotalFaturamentoMensal.Checked = false;
                ckSubtotalFaturamentoMensal.Enabled = false;
                ckSubtotalNegocio.Checked = false;
                ckSubtotalNegocio.Enabled = false;
            }

            if (_documento.Modelo.Segmento.IdSegmento == 3)
            {
                ckSubtotalFaturamentoMensal.Checked = false;
                ckSubtotalFaturamentoMensal.Enabled = false;

                if (listObjetoOrcamento.Any(objetoOrcamento => objetoOrcamento.ItemObjetoOrcamento.Count > 2))
                    throw new MyException("Neste modelo só é permitido até 2 itens por objeto.");
            }

            if (_documento.Modelo.Segmento.IdSegmento == 2)
            {
                ckSubTotalPeso.Checked = false;
                ckSubTotalPeso.Enabled = false;
            }

            foreach (ObjetoOrcamento objetoOrcamento in listObjetoOrcamento)
            {
                Label label = new Label
                {
                    Text = objetoOrcamento.Objeto,
                    ID = "lblObjeto" + objetoOrcamento.Id,
                    CssClass = "descriptionObject"
                };

                HiddenField hiddenFieldArea = new HiddenField
                {
                    Value = objetoOrcamento.Area.ToString(),
                    ID = "hiddenArea" + objetoOrcamento.Id
                };

                HiddenField hiddenFieldVolume = new HiddenField
                {
                    Value = objetoOrcamento.Volume.ToString(),
                    ID = "hiddenVolume" + objetoOrcamento.Id
                };

                HiddenField hiddenFieldPrevisaoInicio = new HiddenField
                {
                    Value = objetoOrcamento.PrevisaoInicio.ToString(),
                    ID = "hiddenPrevisaoInicio" + objetoOrcamento.Id
                };

                HiddenField hiddenFieldPrevisaoTermino = new HiddenField
                {
                    Value = objetoOrcamento.PrevisaoTermino.ToString(),
                    ID = "hiddenPrevisaoTermino" + objetoOrcamento.Id
                };

                panelObjeto.Controls.Add(label);
                panelObjeto.Controls.Add(hiddenFieldArea);
                panelObjeto.Controls.Add(hiddenFieldVolume);
                panelObjeto.Controls.Add(hiddenFieldPrevisaoInicio);
                panelObjeto.Controls.Add(hiddenFieldPrevisaoTermino);


                if (_documento.ListDocumentoObjeto != null)
                {
                    foreach (DocumentoObjeto documentoObjeto in _documento.ListDocumentoObjeto.Where(documentoObjeto => documentoObjeto.CodigoSistemaOrigem == objetoOrcamento.Id))
                    {
                        foreach (ItemObjetoOrcamento oItemObjetoOrcamento in objetoOrcamento.ItemObjetoOrcamento)
                        {
                            DocumentoObjetoItem oDocumentoObjetoItem = documentoObjeto.DocumentoObjetoItem.FirstOrDefault(documentoObjetoItem => oItemObjetoOrcamento.Id == documentoObjetoItem.CodigoSistemaOrigem);

                            if (oDocumentoObjetoItem == null)
                                oItemObjetoOrcamento.Exibir = true;
                            else
                                oItemObjetoOrcamento.Exibir = oDocumentoObjetoItem.Exibir;
                        }
                    }
                }

                Repeater repeater = new Repeater
                {
                    HeaderTemplate = new TemplateRepeaterObjeto(ListItemType.Header),
                    ItemTemplate = new TemplateRepeaterObjeto(ListItemType.Item),
                    AlternatingItemTemplate = new TemplateRepeaterObjeto(ListItemType.AlternatingItem),
                    FooterTemplate = new TemplateRepeaterObjeto(ListItemType.Footer),
                    DataSource = objetoOrcamento.ItemObjetoOrcamento
                };
                repeater.DataBind();
                repeater.ID = objetoOrcamento.Id.ToString();

                panelObjeto.Controls.Add(label);
                panelObjeto.Controls.Add(repeater);
            }
        }
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

        void ObterItensTela()
        {
            Int32 ordemApresentacaoObjeto = 1;
            Int32 ordemApresentacaoItem = 1;
            DocumentoObjeto oDocumentoObjeto = null;
            List<DocumentoObjeto> listDocumentoObjeto = new List<DocumentoObjeto>();

            if (_documento.ListDocumentoObjeto == null)
                _documento.ListDocumentoObjeto = new List<DocumentoObjeto>();

            _documento.ListDocumentoObjeto.Clear();

            Decimal area = 0, volume = 0;
            DateTime previsaoInicio = new DateTime(), previsaoTermino = new DateTime();

            foreach (Control item in panelObjeto.Controls)
            {
                Label label = item as Label;
                if (label != null)
                {
                    oDocumentoObjeto = new DocumentoObjeto();
                    Label labelObjeto = label;
                    oDocumentoObjeto.DescricaoObjeto = labelObjeto.Text;
                    oDocumentoObjeto.CodigoSistemaOrigem = Int32.Parse(labelObjeto.ID.Substring(9));
                    oDocumentoObjeto.IdOrcamentoSistemaOrigem = Int32.Parse(lblIdOrcamento.Text);
                    oDocumentoObjeto.RevisaoOrcamentoSistemaOrigem = Int32.Parse(lblRevisao.Text);
                    oDocumentoObjeto.ExibirDescricaoResumida = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c02")).Checked;
                    oDocumentoObjeto.ExibirDescricaoCliente = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c03")).Checked;
                    oDocumentoObjeto.ExibirQuantidade = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c04")).Checked;
                    oDocumentoObjeto.ExibirUnidade = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c05")).Checked;
                    oDocumentoObjeto.ExibirPeso = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c06")).Checked;
                    oDocumentoObjeto.ExibirValorTabelaLocacao = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c07")).Checked;
                    oDocumentoObjeto.ExibirValorPraticadoLocacao = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c08")).Checked;
                    oDocumentoObjeto.ExibirValorTabelaIndenizacao = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c09")).Checked;
                    oDocumentoObjeto.ExibirValorPraticadoIndenizacao = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c10")).Checked;
                    oDocumentoObjeto.ExibirDesconto = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c11")).Checked;
                    oDocumentoObjeto.ExibirValorTotalItem = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c12")).Checked;
                    oDocumentoObjeto.ExibirPesoTotalItem = ((HtmlInputCheckBox)tabelaColunas.Rows[0].FindControl("c13")).Checked;
                    oDocumentoObjeto.ExibirSubTotalPeso = ckSubTotalPeso.Checked;
                    oDocumentoObjeto.ExibirFaturamentoMensalObjeto = ckSubtotalFaturamentoMensal.Checked;
                    oDocumentoObjeto.ExibirFaturamentoMensal = ckValorTotalFaturamentoMensal.Checked;
                    oDocumentoObjeto.ExibirValorNegocio = ckValorTotalNegocio.Checked;
                    oDocumentoObjeto.ExibirValorNegocioObjeto = ckSubtotalNegocio.Checked;
                    oDocumentoObjeto.ExibirPrevisaoUtilizacao = ckPrevisaoUtilizacao.Checked;

                    oDocumentoObjeto.DataCadastro = DateTime.Now;
                    oDocumentoObjeto.OrdemApresentacao = ordemApresentacaoObjeto++;

                    oDocumentoObjeto.Area = area;
                    oDocumentoObjeto.Volume = volume;

                    oDocumentoObjeto.PrevisaoInicio = previsaoInicio;
                    oDocumentoObjeto.PrevisaoTermino = previsaoTermino;

                    oDocumentoObjeto.IdDocumento = _documento.IdDocumento;
                }
                else if (item is HiddenField)
                {
                    HiddenField hiddenField = (HiddenField)item;

                    if (String.Compare(item.ID.Substring(0, 10), "hiddenArea", StringComparison.OrdinalIgnoreCase) == 0)
                        area = Decimal.Parse(hiddenField.Value);
                    else if (String.Compare(item.ID.Substring(0, 12), "hiddenVolume", StringComparison.OrdinalIgnoreCase) == 0)
                        volume = Decimal.Parse(hiddenField.Value);
                    else if (String.Compare(item.ID.Substring(0, 20), "hiddenPrevisaoInicio", StringComparison.OrdinalIgnoreCase) == 0)
                        previsaoInicio = DateTime.Parse(hiddenField.Value);
                    else if (String.Compare(item.ID.Substring(0, 21), "hiddenPrevisaoTermino", StringComparison.OrdinalIgnoreCase) == 0)
                        previsaoTermino = DateTime.Parse(hiddenField.Value);
                }
                else if (item is Repeater)
                {
                    foreach (RepeaterItem repeaterItem in item.Controls)
                    {
                        DocumentoObjetoItem oDocumentoObjetoItem = new DocumentoObjetoItem();
                        if (repeaterItem.ItemType != ListItemType.Item && repeaterItem.ItemType != ListItemType.AlternatingItem) continue;
                        oDocumentoObjetoItem.DescricaoResumida = ((Label)repeaterItem.Controls[0].FindControl("resumida")).Text;
                        oDocumentoObjetoItem.DescricaoCliente = ((Label)repeaterItem.Controls[0].FindControl("descricao")).Text;
                        oDocumentoObjetoItem.Unidade = ((Label)repeaterItem.Controls[0].FindControl("UnidadeMedida")).Text;
                        oDocumentoObjetoItem.Peso = Decimal.Parse(((Label)repeaterItem.Controls[0].FindControl("Peso")).Text);
                        oDocumentoObjetoItem.Quantidade = Decimal.Parse(((Label)repeaterItem.Controls[0].FindControl("Quantidade")).Text);
                        oDocumentoObjetoItem.ValorTabelaLocacao = Decimal.Parse(((Label)repeaterItem.Controls[0].FindControl("ValorTabelaLocacao")).Text);
                        oDocumentoObjetoItem.ValorPraticadoLocacao = Decimal.Parse(((Label)repeaterItem.Controls[0].FindControl("ValorPraticadoLocacao")).Text);
                        oDocumentoObjetoItem.ValorTabelaIndenizacao = Decimal.Parse(((Label)repeaterItem.Controls[0].FindControl("ValorTabelaIndenizacao")).Text);
                        oDocumentoObjetoItem.ValorPraticadoIndenizacao = Decimal.Parse(((Label)repeaterItem.Controls[0].FindControl("ValorPraticadoIndenizacao")).Text);
                        oDocumentoObjetoItem.Desconto = Decimal.Parse(((Label)repeaterItem.Controls[0].FindControl("Desconto")).Text);
                        oDocumentoObjetoItem.Exibir = ((CheckBox)repeaterItem.Controls[0].FindControl("ckExibir")).Checked;
                        oDocumentoObjetoItem.CodigoSistemaOrigem = Int32.Parse(((CheckBox)repeaterItem.Controls[0].FindControl("ckExibir")).Attributes["idItemObjetoOrcamento"]);
                        oDocumentoObjetoItem.CodigoItemSistemaOrigem = Int32.Parse(((CheckBox)repeaterItem.Controls[0].FindControl("ckExibir")).Attributes["codigoItem"]);
                        oDocumentoObjetoItem.CodigoTabelaPrecoSistemaOrigem = Int32.Parse(((CheckBox)repeaterItem.Controls[0].FindControl("ckExibir")).Attributes["idTabelaPreco"]);
                        oDocumentoObjetoItem.CodigoGrupoSistemaOrigem = Int32.Parse(((CheckBox)repeaterItem.Controls[0].FindControl("ckExibir")).Attributes["codigoSubGrupoItem"]);
                        if (oDocumentoObjeto != null)
                            oDocumentoObjeto.DocumentoObjetoItem.Add(oDocumentoObjetoItem);

                        oDocumentoObjetoItem.OrdemApresentacao = ordemApresentacaoItem;
                        ordemApresentacaoItem++;

                        if (_documento.Modelo.ModeloTipo.IdModeloTipo == 3)
                            ((CheckBox)repeaterItem.Controls[0].FindControl("ckExibir")).Enabled = false;
                    }
                    listDocumentoObjeto.Add(oDocumentoObjeto);

                    _documento.ListDocumentoObjeto.Add(oDocumentoObjeto);
                }
            }
            new DocumentoObjetoBusiness().ObterDescontoMedio(_documento);
            new DocumentoObjetoBusiness().CalcularValorNegocioTotal(_documento);
            new DocumentoObjetoBusiness().CalcularFaturamentoMensalTotal(_documento);

            _documento.DocumentoObjetoObservacao.Observacao = CKEditorObservacoesObjeto.Text;
            _documento.DocumentoObjetoObservacao.IdDocumento = _documento.IdDocumento;
        }
        void CarregarObservacaoObjeto(Documento documento)
        {
            CKEditorObservacoesObjeto.Text = new DocumentoObjetoObservacaoBusiness().ObterUltimaObservacao(documento.IdDocumento);
        }
        void VerificarModeloCliente(Documento documento)
        {
            if (documento.Modelo.ModeloTipo.IdModeloTipo == 3)
            {
                demaisOpcoes.Visible = false;
                tabelaColunas.Visible = false;
                lblDescricao.Text = "Contrato do cliente. Não é necessário formatar o objeto.";
                lblDescricao.CssClass = "label label-info";
            }
            else
            {
                demaisOpcoes.Visible = true;
                tabelaColunas.Visible = true;
                lblDescricao.Text = "Selecione as colunas a serem exibidas";
            }
        }
    }
}