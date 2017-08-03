using Rohr.EPC.Business;
using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class Finalizar : BasePage
    {
        Documento _documento;
        Workflow CriarWorkflow(Documento documento, Int32 idUsuario, Int32 idPerfil)
        {
            Workflow oWorkflow = new Workflow
            {
                DataCadastro = DateTime.Now,
                IdUsuario = idUsuario,
                IdPerfil = idPerfil,
                IdDocumento = documento.IdDocumento
            };

            if (documento.EProposta)
            {
                if (documento.Modelo.Segmento.IdSegmento != 3)
                {
                    if (new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.Gerente ||
                        new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.Superintendencia ||
                        new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                        new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.VicePresidencia)
                    {
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(1);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(9);
                    }
                    else
                    {
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(3);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                    }
                }
                else
                {
                    oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(49);
                    oWorkflow.WorkflowEtapa = new WorkflowEtapa(8);
                }

            }
            else
            {
                if (documento.Modelo.Segmento.IdSegmento != 3)
                {
                    oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(1);
                    oWorkflow.WorkflowEtapa = new WorkflowEtapa(9);
                }
                else
                {
                    oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(49);
                    oWorkflow.WorkflowEtapa = new WorkflowEtapa(8);
                }
            }

            return oWorkflow;
        }
        WorkflowAcaoExecutada CriarWorkflowAcaoExecutada(Documento documento, Int32 idUsuario, Int32 idPerfil, String justificativa = "")
        {
            WorkflowAcaoExecutada oWorkflowAcaoExecutada = new WorkflowAcaoExecutada
            {
                DataCadastro = DateTime.Now,
                IdUsuario = idUsuario,
                IdPerfil = idPerfil,
                IdDocumento = documento.IdDocumento,
                WorkflowAcao = documento.EProposta ? WorkflowAcaoBusiness.ObterPorId(19) : WorkflowAcaoBusiness.ObterPorId(26),
                NumeroDocumento = documento.NumeroDocumento,
                Justificativa = justificativa
            };

            return oWorkflowAcaoExecutada;
        }
        void CarregarDataParaExibicao()
        {
            txtDataExibicao.Text = _documento.DataParaExibicao.ToString("dd/MM/yyyy");
        }
        void CarregarMensagemEmailComercial()
        {
            CKEditorMensagemEmail.config.toolbar = new object[] { new object[] { "Bold" } };
            CKEditorMensagemEmail.config.removePlugins = "elementspath";

            StringBuilder html = new StringBuilder();
            html.Append(String.Format("<strong>Olá {0},</strong><br /><br />", _documento.DocumentoComercial.PrimeiroNome));

            if (_documento.EProposta)
            {
                if (new WorkflowAlcadaBusiness().VerificarAlcada(_documento) != WorkflowAlcada.Alcada.Comercial)
                {
                    if (_documento.Edicao)
                        html.Append(String.Format("A proposta N&ordm;: {0:N0}, foi revisada e está fora da sua alçada. A proposta será encaminhada para análise.<br /><br />", _documento.NumeroDocumento.ToString("N0")));
                    else
                        html.Append(String.Format("A proposta N&ordm;: {0:N0}, foi elaborada e está fora da sua alçada. A proposta será encaminhada para análise.<br /><br />", _documento.NumeroDocumento.ToString("N0")));
                }
                else
                {
                    if (_documento.Edicao)
                        html.Append(String.Format("A proposta N&ordm;: {0:N0}, foi revisada e está disponível para envio ao cliente.<br /><br />", _documento.NumeroDocumento));
                    else
                        html.Append(String.Format("A proposta N&ordm;: {0:N0}, foi elaborada e está disponível para envio ao cliente.<br /><br />", _documento.NumeroDocumento));
                }
            }
            else
            {
                if (new WorkflowAlcadaBusiness().VerificarAlcada(_documento) != WorkflowAlcada.Alcada.Comercial)
                    html.Append(String.Format("O contrato N&ordm;: {0:N0}, foi elaborado e encaminhada para aprovação. Será necessário aguardar a aprovação para envio ao cliente.<br /><br />", _documento.NumeroDocumento));
            }


            html.Append(String.Format("Cliente: {0}<br />", _documento.DocumentoCliente.Nome));
            html.Append(String.Format("Obra: {0}<br />", _documento.DocumentoObra.Nome));
            html.Append(String.Format("Modelo: {0}<br />", _documento.Modelo.Titulo));
            html.Append(String.Format("Valor do Faturamento Mensal: R$ {0:N2}<br />", _documento.ValorFaturamentoMensal));
            html.Append(String.Format("Valor do Negócio: R$ {0:N2}<br />", _documento.ValorNegocio));
            html.Append(String.Format("Desc.(-) / Acrésc.(+) médio concedido: {0:N2}%<br />", _documento.PercentualDesconto));
            html.Append(String.Format("Maior/Menor desconto concedido (item): {0:N2}% / {1:N2}%<br /><br />",
                                                                        new DocumentoObjetoBusiness().ObterMaiorDesconto(_documento).ToString("N2"),
                                                                        new DocumentoObjetoBusiness().ObterMenorDesconto(_documento).ToString("N2")));
            html.Append(String.Format("Criado por: {0}<br /><br />", _documento.Usuario.PrimeiroNome));
            html.Append(String.Format("Abs<br />{0}", _documento.Usuario.PrimeiroNome));

            lblEmailComercial.Text = _documento.DocumentoComercial.Email;

#if DEBUG
            rbEmailSim1.Checked = false;
            rbEmailNao.Checked = true;
            lblEmailComercial.Text = "";
#endif
            Label lbl = new Label();
            Util.ObterServidorDados(lbl);
            if(lbl.Text.Contains("HOMOLOGAÇÃO"))
            {
                rbEmailSim1.Checked = false;
                rbEmailNao.Checked = true;
                lblEmailComercial.Text = "";
            }

            CKEditorMensagemEmail.Text = html.ToString();
        }
        void CarregarMensagemAlcada()
        {
            WorkflowAlcada.Alcada alcada = new WorkflowAlcadaBusiness().VerificarAlcada(_documento);
            if (_documento.EProposta)
            {
                if (alcada == WorkflowAlcada.Alcada.Gerente ||
                    alcada == WorkflowAlcada.Alcada.Superintendencia ||
                    alcada == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                    alcada == WorkflowAlcada.Alcada.VicePresidencia)
                {
                    lblMensagemAlcada.Text = "O Documento está fora da sua alçada e será encaminhado para análise do gerente da filial.";
                    lblMensagemAlcada.CssClass = "alert alert-error";
                }
            }
            else
            {
                if (alcada == WorkflowAlcada.Alcada.Superintendencia ||
                    alcada == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                    alcada == WorkflowAlcada.Alcada.VicePresidencia)
                {
                    lblMensagemAlcada.Text = "O Documento está fora da sua alçada e será encaminhado para análise conforme definição do workflow.";
                    lblMensagemAlcada.CssClass = "alert alert-error";
                }
            }
        }

        void EnviarEmail()
        {
            String titulo = _documento.Edicao ? "EPC - Documento revisado" : "EPC - Documento criado";

            if (rbEmailSim1.Checked)
                new ModeloEmailBusiness().EnviarEmail(CKEditorMensagemEmail.Text, titulo, lblEmailComercial.Text);

            if ((!_documento.EProposta || _documento.EProposta && new WorkflowAlcadaBusiness().VerificarAlcada(_documento) != WorkflowAlcada.Alcada.Comercial) && _documento.Modelo.Segmento.IdSegmento != 3)
                new ModeloEmailBusiness().EnviarEmail(_documento, 8);
            else  if (_documento.Modelo.Segmento.IdSegmento == 3)
                new ModeloEmailBusiness().EnviarEmail(_documento, 7);

        }
        void PrepararTela()
        {
            txtDescontoConcedido.ReadOnly = true;
            txtValorFaturamentoMensal.ReadOnly = true;
            txtValorNegocio.ReadOnly = true;
            txtMaiorDesconto.ReadOnly = true;
            txtMenorDesconto.ReadOnly = true;

            btnContinuar.Text = _documento.EProposta ? "Gerar proposta" : "Gerar contrato";

            txtDescontoConcedido.Text = String.Format("{0:N2}", _documento.PercentualDesconto);
            txtValorFaturamentoMensal.Text = String.Format("{0:N2}", _documento.ValorFaturamentoMensal);
            txtValorNegocio.Text = String.Format("{0:N2}", _documento.ValorNegocio);
            txtMaiorDesconto.Text = new DocumentoObjetoBusiness().ObterMaiorDesconto(_documento).ToString("N2");
            txtMenorDesconto.Text = new DocumentoObjetoBusiness().ObterMenorDesconto(_documento).ToString("N2");
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
        DateTime ValidarData()
        {
            DateTime dataParaExibicao;

            if (!DateTime.TryParse(txtDataExibicao.Text, out dataParaExibicao))
                throw new MyException("A data informada não é valida");

            if (DateTime.Now.Subtract(dataParaExibicao).TotalDays >= 31)
                throw new MyException("A data informada não pode ser menor do que 30 dias da data atual");

            if (dataParaExibicao.Subtract(DateTime.Now).TotalDays >= 3)
                throw new MyException("A data informada não pode ser maior do que 2 dias da data atual");

            return dataParaExibicao;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _documento = new Util().GetSessaoDocumento();
                new DocumentoBusiness().VerificarPropostaFechadaPMWeb(_documento);

                if (IsPostBack) return;
                CarregarPainelDetalhes();
                PrepararTela();
                CarregarDataParaExibicao();
                CarregarMensagemEmailComercial();
                CarregarMensagemAlcada();
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, "Não foi possível recuperar o documento", Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                new DocumentoBusiness().VerificarPropostaFechadaPMWeb(_documento);
                Int32 idPerfil = new Util().GetSessaoPerfilAtivo();
                Int32 idUsuario = new Util().GetSessaoUsuario().IdUsuario;
                _documento.DataParaExibicao = ValidarData();
                new DocumentoBusiness().FinalizarDocumento(idPerfil, _documento, CriarWorkflow(_documento, idUsuario, idPerfil), CriarWorkflowAcaoExecutada(_documento, idUsuario, idPerfil, txtJustificativa.Text));
                new AuditoriaLogBusiness().AdicionarLogDocumento(_documento, Request.Browser);
                EnviarEmail();

                Response.Redirect("Fim.aspx", false);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, "Não foi possível salvar o documento. " + ex.Message, Util.TipoMensagem.Erro);
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
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            String planilha = new PlanilhaOrcamentariaBusiness().GerarPlanilhaOrcamentaria(_documento);
            new AuditoriaLogBusiness().AdicionarLogPlanilhaOrcamentaria(_documento, Request.Browser);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=Planilha Orcamentaria.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            EnableViewState = false;
            Response.Write(planilha);
            Response.End();
        }
    }
}