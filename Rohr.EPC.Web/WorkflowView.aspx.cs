using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class WorkflowView : BasePage
    {
        public string semNegocio;
        public String Workflow;
        Documento _documento;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _documento = new Util().GetSessaoDocumento();

                CarregarPainelDetalhes();
                CarregarWorkflow();
                CarregarBloqueios();
                CarregarAnexos();
                VerificarOportunidadeSemNegocio();
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        void CarregarWorkflow()
        {
            DataTable historicoAcoes = new WorkflowBusiness().ObterHistoricoAcoes(_documento);

            historicoAcoes.Columns.Add("realizado");
            historicoAcoes.DefaultView.Sort = "dataCadastro desc";

            DateTime proximaDataCadastro = new DateTime();
            if (historicoAcoes.Rows.Count != 0)
                proximaDataCadastro = DateTime.Parse(historicoAcoes.Rows[0]["dataCadastro"].ToString());

            for (int i = 1; i < historicoAcoes.Rows.Count; i++)
            {
                TimeSpan diferenca = proximaDataCadastro.Subtract(DateTime.Parse(historicoAcoes.Rows[i]["dataCadastro"].ToString()));
                historicoAcoes.Rows[i - 1]["realizado"] = new Util().TratarTempoMinuto(Convert.ToDecimal(diferenca.TotalMinutes));
                proximaDataCadastro = DateTime.Parse(historicoAcoes.Rows[i]["dataCadastro"].ToString());


            }         



            Repeater1.DataSource = historicoAcoes;
            Repeater1.DataBind();

            Workflow oWorkflow = new WorkflowBusiness().ObterUltimaAcaoPorIdDocumento(_documento.IdDocumento);
            _documento.Modelo.ModeloMeta = new ModeloMetaBusiness().ObterPorId(_documento.Modelo.ModeloMeta.IdModeloMeta);


            lblMetaDocumento.Text = new Util().TratarTempoFracao(_documento.Modelo.ModeloMeta.Meta);
            lblDocumentoCriado.Text = new Util().TratarTempo(_documento.DataCadastro);

            if (_documento.DocumentoStatus.IdDocumentoStatus == 3)
            {
                lblProximoPasso.Text = "<span class=\"label label-info\">Cancelado</span>";
                lblMetaProximoPasso.Text = String.Empty;
                lblMetaDocumento.Text = String.Empty;
                lblLabelDocumentoCriado.Text = String.Empty;
                lblDocumentoCriado.Text = "Cancelado";
            }
            else if (_documento.DocumentoStatus.IdDocumentoStatus != 3 && oWorkflow.WorkflowAcao.IdWorkflowAcao == 5 && new DocumentoBusiness().PropostaTemContrato(_documento.NumeroDocumento) || oWorkflow.WorkflowAcao.IdWorkflowAcao == 40)
            {
                lblProximoPasso.Text = "<span class=\"label label-warning\">Concluído</span>";
                lblMetaProximoPasso.Text = String.Empty;
                lblLabelDocumentoCriado.Text = "Criado em";
                lblDocumentoCriado.Text = _documento.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss");
            }
            else
            {
                lblMetaProximoPasso.Text = new Util().TratarTempoFracao(oWorkflow.WorkflowAcao.Meta);
                lblProximoPasso.Text = oWorkflow.WorkflowAcao.Descricao;
            }

            Workflow = _documento.EProposta ? ObterWorkflowAcoesProposta(oWorkflow) : ObterWorkflowAcoesContrato(oWorkflow);
        }
        void CarregarBloqueios()
        {
            DataTable dtBloqueios = new DocumentoBloqueadoBusiness().ObterListaBloqueios(_documento);
            dtBloqueios.Columns.Add("tempo");

            for (int i = 0; i < dtBloqueios.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(dtBloqueios.Rows[i]["DataDesbloqueio"].ToString()))
                {
                    DateTime dataCadastro = DateTime.Parse(dtBloqueios.Rows[i]["DataCadastro"].ToString());
                    DateTime dataDesbloqueio = DateTime.Parse(dtBloqueios.Rows[i]["DataDesbloqueio"].ToString());

                    TimeSpan diferenca = dataDesbloqueio.Subtract(dataCadastro);

                    dtBloqueios.Rows[i]["tempo"] = diferenca.ToString();
                }
            }

            RepeaterBloqueios.DataSource = dtBloqueios;
            RepeaterBloqueios.DataBind();
        }
        void CarregarAnexos()
        {
            int idPropostaPMWeb;
            DocumentoBusiness oDocumentoBusiness = new DocumentoBusiness();

            if (_documento.EProposta)
                idPropostaPMWeb = _documento.CodigoSistemaOrigem;
            else
                idPropostaPMWeb = oDocumentoBusiness.ObterPropostaPorIdDocumentoContrato(_documento.IdDocumento).CodigoSistemaOrigem;

            RepeaterAnexos.DataSource = new PMWeb.DocumentAttachment().GetAllFiles(idPropostaPMWeb);
            RepeaterAnexos.DataBind();
        }

        String ObterLinhaDoTempoContrato(Int32 linhaAtiva)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Dictionary<int, String> dictionary = new Dictionary<int, string>();

            if (_documento.Modelo.Segmento.IdSegmento == 3)
            {
                dictionary.Add(0, "<li class='{status}' title='Responsável: Filial'>Finalizar minuta contratual</li><!--");
                dictionary.Add(1, "--><li class='{status}' title='Responsável: Supervisor PTA'>Análise do Supervisor PTA</li><!--");
                dictionary.Add(2, "--><li class='{status}' title='Responsável: Filial'>Enviar contrato para assinatura do cliente</li><!--");
                dictionary.Add(3, "--><li class='{status}' title='Responsável: Filial'>Receber contrato assinado do cliente</li><!--");
                dictionary.Add(4, "--><li class='{status}' title='Responsável: Filial'>Enviar contrato assinado para o jurídico</li><!--");
                dictionary.Add(5, "--><li class='{status}' title='Responsável: Jurídico'>Receber contrato assinado da filial</li><!--");
                dictionary.Add(6, "--><li class='{status}' title='Responsável: Jurídico'>Arquivar contrato</li><!--");
                dictionary.Add(7, "--><li class='{status}'>Concluído</li>");
            }
            else
            {
                if (new WorkflowAlcadaBusiness().VerificarAlcada(_documento) == WorkflowAlcada.Alcada.Superintendencia)
                {
                    if (linhaAtiva > 2)
                        linhaAtiva += 1;

                    dictionary.Add(0, "<li class='{status}' title='Responsável: Filial'>Finalizar minuta contratual</li><!--");
                    dictionary.Add(1, "--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--");
                    dictionary.Add(2, "--><li class='{status}' title='Responsável: Diretoria Operacional / Superint.'>Análise da Diretoria Operacional / Superint.</li><!--");
                    dictionary.Add(3, "--><li class='{status}' title='Responsável: Jurídico'>Análise do Jurídico</li><!--");
                    dictionary.Add(4, "--><li class='{status}' title='Responsável: Diretoria'>Análise da Diretoria</li><!--");
                    dictionary.Add(5, "--><li class='{status}' title='Responsável: Jurídico'>Enviar contrato assinado para a filial</li><!--");
                    dictionary.Add(6, "--><li class='{status}' title='Responsável: Filial'>Receber contrato assinado do jurídico</li><!--");
                    dictionary.Add(7, "--><li class='{status}' title='Responsável: Filial'>Enviar contrato para assinatura do cliente</li><!--");
                    dictionary.Add(8, "--><li class='{status}' title='Responsável: Filial'>Receber contrato assinado do cliente</li><!--");
                    dictionary.Add(9, "--><li class='{status}' title='Responsável: Filial'>Enviar contrato assinado para o jurídico</li><!--");
                    dictionary.Add(10, "--><li class='{status}' title='Responsável: Jurídico'>Receber contrato assinado da filial</li><!--");
                    dictionary.Add(11, "--><li class='{status}' title='Responsável: Jurídico'>Arquivar contrato</li><!--");
                    dictionary.Add(12, "--><li class='{status}'>Concluído</li>");
                }
                else
                {
                    dictionary.Add(0, "<li class='{status}' title='Responsável: Filial'>Finalizar minuta contratual</li><!--");
                    dictionary.Add(1, "--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--");
                    dictionary.Add(2, "--><li class='{status}' title='Responsável: Jurídico'>Análise do Jurídico</li><!--");
                    dictionary.Add(3, "--><li class='{status}' title='Responsável: Diretoria'>Análise da Diretoria</li><!--");
                    dictionary.Add(4, "--><li class='{status}' title='Responsável: Jurídico'>Enviar contrato assinado para a filial</li><!--");
                    dictionary.Add(5, "--><li class='{status}' title='Responsável: Filial'>Receber contrato assinado do jurídico</li><!--");
                    dictionary.Add(6, "--><li class='{status}' title='Responsável: Filial'>Enviar contrato para assinatura do cliente</li><!--");
                    dictionary.Add(7, "--><li class='{status}' title='Responsável: Filial'>Receber contrato assinado do cliente</li><!--");
                    dictionary.Add(8, "--><li class='{status}' title='Responsável: Filial'>Enviar contrato assinado para o jurídico</li><!--");
                    dictionary.Add(9, "--><li class='{status}' title='Responsável: Jurídico'>Receber contrato assinado da filial</li><!--");
                    dictionary.Add(10, "--><li class='{status}' title='Responsável: Jurídico'>Arquivar contrato</li><!--");
                    dictionary.Add(11, "--><li class='{status}'>Concluído</li>");
                }
            }
            for (Int32 i = 0; i < linhaAtiva; i++)
                stringBuilder.Append(dictionary[i].Replace("{status}", "progtrckr-done"));

            stringBuilder.Append(dictionary[linhaAtiva].Replace("{status}", "progtrckr-do"));

            for (int i = linhaAtiva + 1; i < dictionary.Count; i++)
                stringBuilder.Append(dictionary[i].Replace("{status}", "progtrckr-todo"));

            return stringBuilder.ToString();
        }
        String ObterLinhaDoTempoProposta(Int32 linhaAtiva, Int32 idWorkflow)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Dictionary<int, String> dictionary = new Dictionary<int, string>();

            if (_documento.Modelo.Segmento.IdSegmento == 3)
            {
                dictionary.Add(0, "<li class='{status}' title='Responsável: Filial'>Finalizar minuta proposta</li><!--");
                dictionary.Add(1, "--><li class='{status}' title='Responsável: Supervisor PTA'>Análise do Supervisor PTA</li><!--");
                dictionary.Add(2, "--><li class='{status}' title='Responsável: Filial'>Enviar para análise / assinatura do cliente</li><!--");
                dictionary.Add(3, "--><li class='{status}' title='Responsável: Filial'>Receber proposta do cliente</li><!--");
                dictionary.Add(4, "--><li class='{status}' title='Responsável: Filial'>Elaborar contrato</li><!--");
                dictionary.Add(5, "--><li class='{status}'>Concluído</li>");

                if (new DocumentoBusiness().PropostaTemContrato(_documento.NumeroDocumento))
                    linhaAtiva += 1;
            }
            else
            {
                switch (new WorkflowAlcadaBusiness().VerificarAlcada(_documento))
                {
                    case WorkflowAlcada.Alcada.Superintendencia:
                        switch (idWorkflow)
                        {
                            case 1:
                                linhaAtiva += 1;
                                break;
                            case 2:
                                linhaAtiva += 1;
                                break;
                            default:
                                if (idWorkflow != 15 && idWorkflow != 17)
                                    linhaAtiva += 2;
                                break;
                        }
                        dictionary.Add(0, "<li class='{status}' title='Responsável: Filial'>Finalizar minuta proposta</li><!--");
                        dictionary.Add(1, "--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--");
                        dictionary.Add(2, "--><li class='{status}' title='Responsável: Diretoria Operacional / Superint.'>Análise da Diretoria Operacional / Superint.</li><!--");
                        dictionary.Add(3, "--><li class='{status}' title='Responsável: Filial'>Enviar para análise / assinatura do cliente</li><!--");
                        dictionary.Add(4, "--><li class='{status}' title='Responsável: Filial'>Receber proposta do cliente</li><!--");
                        dictionary.Add(5, "--><li class='{status}' title='Responsável: Filial'>Elaborar contrato</li><!--");
                        dictionary.Add(6, "--><li class='{status}'>Concluído</li>");
                        break;
                    case WorkflowAlcada.Alcada.Gerente:
                        if (idWorkflow == 15 || idWorkflow == 17)
                            linhaAtiva = 0;
                        else
                            linhaAtiva += 1;
                        dictionary.Add(0, "<li class='{status}' title='Responsável: Filial'>Finalizar minuta proposta</li><!--");
                        dictionary.Add(1, "--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--");
                        dictionary.Add(2, "--><li class='{status}' title='Responsável: Filial'>Enviar para análise / assinatura do cliente</li><!--");
                        dictionary.Add(3, "--><li class='{status}' title='Responsável: Filial'>Receber proposta do cliente</li><!--");
                        dictionary.Add(4, "--><li class='{status}' title='Responsável: Filial'>Elaborar contrato</li><!--");
                        dictionary.Add(5, "--><li class='{status}'>Concluído</li>");
                        break;
                    default:
                        dictionary.Add(0, "<li class='{status}' title='Responsável: Filial'>Finalizar minuta proposta</li><!--");
                        dictionary.Add(1, "--><li class='{status}' title='Responsável: Filial'>Enviar para análise / assinatura do cliente</li><!--");
                        dictionary.Add(2, "--><li class='{status}' title='Responsável: Filial'>Receber proposta do cliente</li><!--");
                        dictionary.Add(3, "--><li class='{status}' title='Responsável: Filial'>Elaborar contrato</li><!--");
                        dictionary.Add(4, "--><li class='{status}'>Concluído</li>");
                        break;
                }
            }

            for (Int32 i = 0; i < linhaAtiva; i++)
                stringBuilder.Append(dictionary[i].Replace("{status}", "progtrckr-done"));

            stringBuilder.Append(dictionary[linhaAtiva].Replace("{status}", "progtrckr-do"));

            for (int i = linhaAtiva + 1; i < dictionary.Count; i++)
                stringBuilder.Append(dictionary[i].Replace("{status}", "progtrckr-todo"));

            return stringBuilder.ToString();
        }

        String ObterLinhaDoTempoPropostaFluxoNovo(WorkflowAcao workflowAcao)
        {
            String status = "progtrckr-do";
            StringBuilder stringBuilder = new StringBuilder();

            if (workflowAcao.IdWorkflowAcao == 15 || workflowAcao.IdWorkflowAcao == 17)
            {
                stringBuilder.Append("<li class='{status}' title='Responsável: Comercial'>Finalizar minuta proposta</li><!--".Replace("{status}", status));
                status = "progtrckr-todo";
            }
            else
            {
                status = "progtrckr-done";
                stringBuilder.Append("<li class='{status}' title='Responsável: Comercial'>Finalizar minuta proposta</li><!--".Replace("{status}", status));
                status = "progtrckr-do";
            }

            WorkflowAlcada.Alcada oWorkflowAlcada = new WorkflowAlcadaBusiness().VerificarAlcada(_documento);
            if (oWorkflowAlcada == WorkflowAlcada.Alcada.Gerente ||
                oWorkflowAlcada == WorkflowAlcada.Alcada.Superintendencia ||
                oWorkflowAlcada == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                oWorkflowAlcada == WorkflowAlcada.Alcada.VicePresidencia)
            {
                if (status == "progtrckr-todo")
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--".Replace("{status}", status));
                else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 1)
                {
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--".Replace("{status}", status));
                    status = "progtrckr-todo";
                }
                else
                {
                    status = "progtrckr-done";
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--".Replace("{status}", status));
                    status = "progtrckr-do";
                }
            }

            if (oWorkflowAlcada == WorkflowAlcada.Alcada.Superintendencia ||
                oWorkflowAlcada == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                oWorkflowAlcada == WorkflowAlcada.Alcada.VicePresidencia)
            {
                if (status == "progtrckr-todo")
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Superintendência'>Análise da Superintendência</li><!--".Replace("{status}", status));
                else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 2)
                {
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Superintendência'>Análise da Superintendência</li><!--".Replace("{status}", status));
                    status = "progtrckr-todo";
                }
                else
                {
                    status = "progtrckr-done";
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Superintendência'>Análise da Superintendência</li><!--".Replace("{status}", status));
                    status = "progtrckr-do";
                }
            }

            if (oWorkflowAlcada == WorkflowAlcada.Alcada.DiretoriaOperacional || oWorkflowAlcada == WorkflowAlcada.Alcada.VicePresidencia)
            {
                if (status == "progtrckr-todo")
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Diretoria Operacional'>Análise da Diretoria Operacional</li><!--".Replace("{status}", status));
                else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 55)
                {
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Diretoria Operacional'>Análise da Diretoria Operacional</li><!--".Replace("{status}", status));
                    status = "progtrckr-todo";
                }
                else
                {
                    status = "progtrckr-done";
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Diretoria Operacional'>Análise da Diretoria Operacional</li><!--".Replace("{status}", status));
                    status = "progtrckr-do";
                }
            }

            if (oWorkflowAlcada == WorkflowAlcada.Alcada.VicePresidencia)
            {
                if (status == "progtrckr-todo")
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Vice-Presidência'>Análise da Vice-Presidência</li><!--".Replace("{status}", status));
                else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 56)
                {
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Vice-Presidência'>Análise da Vice-Presidência</li><!--".Replace("{status}", status));
                    status = "progtrckr-todo";
                }
                else
                {
                    status = "progtrckr-done";
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Vice-Presidência'>Análise da Vice-Presidência</li><!--".Replace("{status}", status));
                    status = "progtrckr-do";
                }
            }


            if (status == "progtrckr-todo")
                stringBuilder.Append("--><li class='{status}' title='Responsável: Comercial'>Enviar para análise / assinatura do cliente</li><!--".Replace("{status}", status));
            else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 3)
            {
                stringBuilder.Append("--><li class='{status}' title='Responsável: Comercial'>Enviar para análise / assinatura do cliente</li><!--".Replace("{status}", status));
                status = "progtrckr-todo";
            }
            else
            {
                status = "progtrckr-done";
                stringBuilder.Append("--><li class='{status}' title='Responsável: Comercial'>Enviar para análise / assinatura do cliente</li><!--".Replace("{status}", status));
                status = "progtrckr-do";
            }

            if (status == "progtrckr-todo")
                stringBuilder.Append("--><li class='{status}' title='Responsável: Comercial'>Receber proposta do cliente</li><!--".Replace("{status}", status));
            else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 4)
            {
                stringBuilder.Append("--><li class='{status}' title='Responsável: Comercial'>Receber proposta do cliente</li><!--".Replace("{status}", status));
                status = "progtrckr-todo";
            }
            else
            {
                status = "progtrckr-done";
                stringBuilder.Append("--><li class='{status}' title='Responsável: Comercial'>Receber proposta do cliente</li><!--".Replace("{status}", status));
                status = "progtrckr-do";
            }

            if (status == "progtrckr-todo")
                stringBuilder.Append("--><li class='{status}' title='Responsável: Comercial'>Elaborar contrato</li><!--".Replace("{status}", status));
            else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 5 && _documento.DocumentoStatus.IdDocumentoStatus != 5)
            {
                stringBuilder.Append("--><li class='{status}' title='Responsável: Comercial'>Elaborar contrato</li><!--".Replace("{status}", status));
                status = "progtrckr-todo";
            }
            else
            {
                status = "progtrckr-done";
                stringBuilder.Append("--><li class='{status}' title='Responsável: Comercial'>Elaborar contrato</li><!--".Replace("{status}", status));
                status = "progtrckr-done";
            }


            stringBuilder.Append("--><li class='{status}'>Concluído</li>".Replace("{status}", status));

            return stringBuilder.ToString();
        }
        String ObterLinhaDoTempoContratoFluxoNovo(WorkflowAcao workflowAcao)
        {
            String status = "progtrckr-do";
            StringBuilder stringBuilder = new StringBuilder();

            if (workflowAcao.IdWorkflowAcao == 16 || workflowAcao.IdWorkflowAcao == 17)
            {
                stringBuilder.Append("<li class='{status}' title='Responsável: Comercial'>Finalizar minuta contratual</li><!--".Replace("{status}", status));
                status = "progtrckr-todo";
            }
            else
            {
                status = "progtrckr-done";
                stringBuilder.Append("<li class='{status}' title='Responsável: Comercial'>Finalizar minuta contratual</li><!--".Replace("{status}", status));
                status = "progtrckr-do";
            }

            WorkflowAlcada.Alcada oWorkflowAlcada;

            if (workflowAcao.IdWorkflowAcao == 16 || workflowAcao.IdWorkflowAcao == 17)
                oWorkflowAlcada = WorkflowAlcada.Alcada.VicePresidencia;
            else
                oWorkflowAlcada = new WorkflowAlcadaBusiness().VerificarAlcada(_documento);

            if (status == "progtrckr-todo")
                stringBuilder.Append("--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--".Replace("{status}", status));
            else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 1)
            {
                stringBuilder.Append("--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--".Replace("{status}", status));
                status = "progtrckr-todo";
            }
            else
            {
                status = "progtrckr-done";
                stringBuilder.Append("--><li class='{status}' title='Responsável: Gerente da filial'>Análise do gerente da filial</li><!--".Replace("{status}", status));
                status = "progtrckr-do";
            }

            if (oWorkflowAlcada == WorkflowAlcada.Alcada.Superintendencia ||
                oWorkflowAlcada == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                oWorkflowAlcada == WorkflowAlcada.Alcada.VicePresidencia)
            {
                if (status == "progtrckr-todo")
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Superintendência'>Análise da Superintêndencia</li><!--".Replace("{status}", status));
                else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 2)
                {
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Superintendência'>Análise da Superintêndencia</li><!--".Replace("{status}", status));
                    status = "progtrckr-todo";
                }
                else
                {
                    status = "progtrckr-done";
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Superintendência'>Análise da Superintêndencia</li><!--".Replace("{status}", status));
                    status = "progtrckr-do";
                }
            }

            if (oWorkflowAlcada == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                oWorkflowAlcada == WorkflowAlcada.Alcada.VicePresidencia)
            {
                if (status == "progtrckr-todo")
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Diretoria Operacional'>Análise da Diretoria Operacional</li><!--".Replace("{status}", status));
                else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 55)
                {
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Diretoria Operacional'>Análise da Diretoria Operacional</li><!--".Replace("{status}", status));
                    status = "progtrckr-todo";
                }
                else
                {
                    status = "progtrckr-done";
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Diretoria Operacional'>Análise da Diretoria Operacional</li><!--".Replace("{status}", status));
                    status = "progtrckr-do";
                }
            }

            if (oWorkflowAlcada == WorkflowAlcada.Alcada.VicePresidencia)
            {
                if (status == "progtrckr-todo")
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Vice-Presidência'>Análise da Vice-Presidência</li><!--".Replace("{status}", status));
                else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == 56)
                {
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Vice-Presidência'>Análise da Vice-Presidência</li><!--".Replace("{status}", status));
                    status = "progtrckr-todo";
                }
                else
                {
                    status = "progtrckr-done";
                    stringBuilder.Append("--><li class='{status}' title='Responsável: Vice-Presidência'>Análise da Vice-Presidência</li><!--".Replace("{status}", status));
                    status = "progtrckr-do";
                }
            }


            String[,,] etapas = new String[,,]
            {
                { { "Jurídico", "Análise do Jurídico", "6" }},
                { { "Jurídico", "Enviar contrato para a filial", "65" }},
                { { "Comercial", "Receber contrato do Jurídico", "66" }},
                { { "Comercial", "Enviar contrato para assinatura do cliente", "10" }},
                { { "Comercial", "Receber contrato assinado do cliente", "11" }},
                { { "Comercial", "Enviar contrato assinado para o Jurídico", "68" }},
                { { "Jurídico", "Receber contrato assinado da filial", "12" }},
                { { "Jurídico", "Legitimar contrato recebido da filial", "70" }},
                { { "Diretoria", "Assinatura da Diretoria", "74" }},
                { { "Jurídico", "Arquivar contrato (via Rohr)", "76" }},
                { { "Jurídico", "Enviar contrato para a filial (via Cliente)", "77" }},
                { { "Comercial", "Receber contrato do Jurídico (via Cliente)", "78" }},
                { { "Comercial", "Enviar contrato para cliente (via cliente)", "79" }},
            };

            for (int i = 0; i < etapas.Length / 3; i++)
            {
                if (status == "progtrckr-todo")
                    stringBuilder.Append(String.Format("--><li class='{0}' title='Responsável: {1}'>{2}</li><!--", status, etapas[i, 0, 0], etapas[i, 0, 1]));
                else if (status == "progtrckr-do" && workflowAcao.IdWorkflowAcao == Int32.Parse(etapas[i, 0, 2]))
                {
                    stringBuilder.Append(String.Format("--><li class='{0}' title='Responsável: {1}'>{2}</li><!--", status, etapas[i, 0, 0], etapas[i, 0, 1]));
                    status = "progtrckr-todo";
                }
                else
                {
                    stringBuilder.Append(String.Format("--><li class='{0}' title='Responsável: {1}'>{2}</li><!--", "progtrckr-done", etapas[i, 0, 0], etapas[i, 0, 1]));
                    status = "progtrckr-do";
                }
            }

            if (workflowAcao.IdWorkflowAcao == 40)
                status = "progtrckr-done";

            stringBuilder.Append("--><li class='{status}'>Concluído</li>".Replace("{status}", status));

            return stringBuilder.ToString();
        }
        String ObterWorkflowAcoesProposta(Workflow workflow)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (_documento.DataCadastro >= new DateTime(2016, 01, 26) || workflow.DataCadastro >= new DateTime(2016, 01, 26))
            {
                stringBuilder.Append("<ol class='progtrckr progtrckr-proposta-n'>");
                stringBuilder.Append(ObterLinhaDoTempoPropostaFluxoNovo(workflow.WorkflowAcao));
            }
            else
            {
                stringBuilder.Append("<ol class='progtrckr progtrckr-proposta'>");
                if (workflow.WorkflowAcao.IdWorkflowAcao == 15)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(0, 15));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 17)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(0, 17));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 3 && _documento.Modelo.Segmento.IdSegmento != 3)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(1, 3));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 4 && _documento.Modelo.Segmento.IdSegmento != 3)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(2, 4));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 5 && _documento.Modelo.Segmento.IdSegmento != 3)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(4, 5));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 1)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(0, 1));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 2)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(1, 2));

                //TV
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 49)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(1, 49));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 3 && _documento.Modelo.Segmento.IdSegmento == 3)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(2, 3));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 4 && _documento.Modelo.Segmento.IdSegmento == 3)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(3, 4));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 5 && _documento.Modelo.Segmento.IdSegmento == 3)
                    stringBuilder.Append(ObterLinhaDoTempoProposta(4, 5));
            }

            stringBuilder.Append("</ol>");

            return stringBuilder.ToString();
        }
        String ObterWorkflowAcoesContrato(Workflow workflow)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (_documento.DataCadastro >= new DateTime(2016, 01, 26) || workflow.DataCadastro >= new DateTime(2016, 01, 26))
            {
                stringBuilder.Append("<ol class='progtrckr progtrckr-contrato-n'>");
                stringBuilder.Append(ObterLinhaDoTempoContratoFluxoNovo(workflow.WorkflowAcao));
            }
            else
            {
                stringBuilder.Append("<ol class='progtrckr progtrckr-contrato'>");

                if (workflow.WorkflowAcao.IdWorkflowAcao == 16 || workflow.WorkflowAcao.IdWorkflowAcao == 17)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(0));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 1)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(1));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 2)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(2));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 6)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(2));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 7)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(3));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 8)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(4));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 9)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(5));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 40 && _documento.Modelo.Segmento.IdSegmento != 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(11));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 10 && _documento.Modelo.Segmento.IdSegmento != 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(6));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 11 && _documento.Modelo.Segmento.IdSegmento != 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(7));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 14 && _documento.Modelo.Segmento.IdSegmento != 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(8));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 12 && _documento.Modelo.Segmento.IdSegmento != 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(9));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 13 && _documento.Modelo.Segmento.IdSegmento != 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(10));

                //TV
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 49)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(1));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 10 && _documento.Modelo.Segmento.IdSegmento == 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(2));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 11 && _documento.Modelo.Segmento.IdSegmento == 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(3));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 14 && _documento.Modelo.Segmento.IdSegmento == 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(4));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 12 && _documento.Modelo.Segmento.IdSegmento == 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(5));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 13 && _documento.Modelo.Segmento.IdSegmento == 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(6));
                else if (workflow.WorkflowAcao.IdWorkflowAcao == 40 && _documento.Modelo.Segmento.IdSegmento == 3)
                    stringBuilder.Append(ObterLinhaDoTempoContrato(7));
            }

            stringBuilder.Append("</ol>");

            return stringBuilder.ToString();
        }

        void VerificarOportunidadeSemNegocio()
        {
            if (_documento.EProposta)
                if (_documento.DocumentoStatus.IdDocumentoStatus == 7)
                {
                    DocumentoSemNegocio oDocumentoSemNegocio = new DocumentoSemNegocioBusiness().ObterPorId(_documento.IdDocumento);
                    semNegocio = "style = \"opacity: 0.65;\"";
                    lblSemNegocioData.Text = string.Format("Oportunidade perdidade em: {0}.", oDocumentoSemNegocio.DataCadastro.ToString("dd/MM/yyyy"));
                    if (string.IsNullOrEmpty(oDocumentoSemNegocio.Observacao))
                        lblSemNegocioMotivo.Text = string.Format("{0}", oDocumentoSemNegocio.Motivo);
                    else
                        lblSemNegocioMotivo.Text = string.Format("{0} - {1}", oDocumentoSemNegocio.Motivo, oDocumentoSemNegocio.Observacao);
                    divSemNegocio.Visible = true;
                }
                else
                    semNegocio = string.Empty;
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            if (ViewState["idPerfil"] != null)
            {
                if (Int32.Parse(ViewState["idPerfil"].ToString()) != Int32.Parse(DataBinder.Eval(e.Item.DataItem, "idPerfil").ToString()))
                    if (String.Compare(ViewState["corFundo"].ToString(), "#ffc", StringComparison.OrdinalIgnoreCase) == 0)
                        ViewState["corFundo"] = String.Empty;
                    else
                        ViewState["corFundo"] = "#ffc";
            }
            else
                ViewState["corFundo"] = String.Empty;

            ViewState["idPerfil"] = Int32.Parse(DataBinder.Eval(e.Item.DataItem, "idPerfil").ToString());
            DataBinder.Eval(e.Item.DataItem, "DescricaoWorkflowAcao");
            HtmlTableRow tableRow = (HtmlTableRow)e.Item.FindControl("row");
            tableRow.Attributes.CssStyle.Add("background-color", ViewState["corFundo"].ToString());

            Decimal metaEmMinuto = Decimal.Parse(DataBinder.Eval(e.Item.DataItem, "Meta").ToString());

            tableRow.Cells[4].InnerText = metaEmMinuto != 0 ? new Util().TratarTempoFracao(metaEmMinuto) : "-";

            tableRow.Cells[4].Attributes.Add("class", "text-nowrap");
            tableRow.Cells[5].Attributes.Add("class", "text-nowrap");
        }

        protected void lkVisualizar_Click(object sender, EventArgs e)
        {
            LinkButton oLinkButtton = (LinkButton)sender;

            Page.ClientScript.RegisterStartupScript(
                GetType(),
            "newOpen",
                Util.AbrirPopup(
                    ResolveUrl(string.Format("~/PMWebDownloaderDB.ashx?Rand={0}", oLinkButtton.CommandArgument)),
                    830,
                    650),
                false);
        }
    }
}