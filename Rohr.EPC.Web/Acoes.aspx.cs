using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Rohr.EPC.Web
{
    public partial class Acoes : BasePage
    {
        Int32 _idWorkflowAcao;
        Int32 _totalDocumentos;

        void MontarTela()
        {
            Util.TratarTextoBotaoAcao(btnAcaoPrimaria, btnAcaoSecundaria, _idWorkflowAcao);
            lblAcao.Text = btnAcaoPrimaria.Text;
            lblTotal.Text = _totalDocumentos.ToString();
        }
        void ValidarJustificativa()
        {
            List<String> listPalavrasBloqueadas = new List<String>
            {
                "revisão",
                "alterar objeto",
                "-",
                "valor",
                "Editar objetos",
                "editar  valor",
                "editar valor",
                "editar",
                "refazer",
                "preço",
                ".",
                "ajustes",
                "digitação",
                "revisar",
                "vamos revisar",
                "objeto",
                "reprovar",
                "Revisão.",
               "correção",
               "fazer alterações"
            };

            if (String.IsNullOrWhiteSpace(txtObservacao.Text.Trim()))
                throw new MyException("Informe uma observação :(");

            if (txtObservacao.Text.Trim().Length <= 14)
                throw new MyException("Observação muito curta. Detalhe mais o motivo da reprovação :(");

            if (txtObservacao.Text.Trim().Length > 500)
                throw new MyException("Observação muito longa. Resuma o motivo da reprovação :(");

            if (listPalavrasBloqueadas.Any(palavraBloqueada => txtObservacao.Text.Trim().ToLower() == palavraBloqueada))
                throw new MyException("Observação inválida. Detalhe mais o motivo da reprovação :(");

        }

        static DataTable CriarDataTable()
        {
            DataTable dt = new DataTable("documentos");
            dt.Columns.Add(new DataColumn("numeroDocumento", typeof(Int32)));
            dt.Columns.Add(new DataColumn("nomeCliente", typeof(String)));
            dt.Columns.Add(new DataColumn("nomeObra", typeof(String)));
            dt.Columns.Add(new DataColumn("descontoMedio", typeof(Double)));
            dt.Columns.Add(new DataColumn("valorNegocio", typeof(Double)));
            dt.Columns.Add(new DataColumn("alcada", typeof(String)));
            dt.Columns.Add(new DataColumn("maiorMenorDesconto", typeof(String)));
            dt.Columns.Add(new DataColumn("observacaoUltimaReprovacao", typeof(String)));
            dt.Columns.Add(new DataColumn("nomeUsuarioUltimaReprovacao", typeof(String)));

            return dt;
        }
        static DataRow MontarLinhaDataTable(DataTable dt, Int32 idDocumento)
        {
            DataRow dr = dt.NewRow();
            Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(idDocumento);
            dr["numeroDocumento"] = oDocumento.NumeroDocumento;
            dr["nomeCliente"] = oDocumento.DocumentoCliente.Nome;
            dr["nomeObra"] = oDocumento.DocumentoObra.Nome;
            dr["descontoMedio"] = oDocumento.PercentualDesconto;
            dr["valorNegocio"] = oDocumento.ValorNegocio;
            dr["maiorMenorDesconto"] = String.Format("{0:N2}% / {1:N2}%", new DocumentoObjetoBusiness().ObterMaiorDesconto(oDocumento), new DocumentoObjetoBusiness().ObterMenorDesconto(oDocumento));
            dr["alcada"] = new WorkflowAlcadaBusiness().ObterDescricaoAlcada(oDocumento);

            WorkflowAcaoExecutada oWorkflowAcaoExecutada = new WorkflowAcaoExecutadaBusiness().ObterUltimaReprovacaoGerenciaPorIdDocumento(oDocumento.IdDocumento);

            if (ReferenceEquals(null, oWorkflowAcaoExecutada)) return dr;
            dr["observacaoUltimaReprovacao"] = oWorkflowAcaoExecutada.Justificativa;
            Usuario oUsuario = UsuarioBusiness.ObterPorId(oWorkflowAcaoExecutada.IdUsuario);
            dr["nomeUsuarioUltimaReprovacao"] = oUsuario.PrimeiroNome;

            return dr;
        }
        void VerificarBloqueio()
        {
            List<Workflow> listWorkflow = (List<Workflow>)(Session["listWorkflow"]);

            foreach (Workflow workflow in listWorkflow)
            {
                DocumentoBloqueado oDocumentoBloqueado = new DocumentoBloqueadoBusiness().ObterBloqueioAtivo(workflow.IdDocumento);
                if (oDocumentoBloqueado == null) continue;
                Int32 idUsuarioBloqueio = oDocumentoBloqueado.IdUsuario;
                if (idUsuarioBloqueio != 0 && idUsuarioBloqueio != new Util().GetSessaoUsuario().IdUsuario)
                    throw new MyException(String.Format("O documento nº: {0}, está bloqueado. Não é possível executar a ação :(",
                        new DocumentoBusiness().RecuperarDocumento(workflow.IdDocumento).NumeroDocumento));
                if (idUsuarioBloqueio != 0 && idUsuarioBloqueio == new Util().GetSessaoUsuario().IdUsuario)
                    new DocumentoBloqueadoBusiness().RemoverBloqueio(workflow.IdDocumento);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["listWorkflow"] == null)
                    throw new MyException("Não foi possível recuperar o documento");

                List<Workflow> listWorkflow = (List<Workflow>)(Session["listWorkflow"]);

                if (listWorkflow.Count != 0)
                    _idWorkflowAcao = listWorkflow[0].WorkflowAcao.IdWorkflowAcao;

                _totalDocumentos = listWorkflow.Count;

                DataTable dt = CriarDataTable();
                foreach (Workflow workflow in listWorkflow)
                {
                    dt.Rows.Add(MontarLinhaDataTable(dt, workflow.IdDocumento));
                }
                Repeater1.DataSource = dt;
                Repeater1.DataBind();

                MontarTela();
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                btnAcaoPrimaria.Enabled = false;
                btnAcaoSecundaria.Enabled = false;
            }
        }
        protected void btnAcaoPrimaria_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["listWorkflow"] == null)
                    throw new MyException("Não foi possível recuperar o documento");

                VerificarBloqueio();

                List<Workflow> listWorkflow = (List<Workflow>)(Session["listWorkflow"]);

                foreach (Workflow workflow in listWorkflow)
                {
                    workflow.Justificativa = txtObservacao.Text.Trim();
                    workflow.IdUsuario = new Util().GetSessaoUsuario().IdUsuario;
                    workflow.IdPerfil = new Util().GetSessaoPerfilAtivo();
                    Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(workflow.IdDocumento);
                    WorkflowBusiness owWorkflowBusiness = new WorkflowBusiness();
                    owWorkflowBusiness.ExecutarAcao(workflow, WorkflowBusiness.Acao.Aprovar, oDocumento);

                    new AuditoriaLogBusiness().AdicionarLogDocumentoAcaoPrimaria(oDocumento, Request.Browser);
                }
                Response.Redirect(String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("99")), false);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnAcaoSecundaria_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["listWorkflow"] == null)
                    throw new MyException("Não foi possível recuperar o documento");

                VerificarBloqueio();

                if ((_idWorkflowAcao >= 1 && _idWorkflowAcao <= 7) || _idWorkflowAcao == 49 || _idWorkflowAcao == 55 || _idWorkflowAcao == 56)
                    ValidarJustificativa();

                List<Workflow> listWorkflow = (List<Workflow>)(Session["listWorkflow"]);

                foreach (Workflow workflow in listWorkflow)
                {
                    workflow.Justificativa = txtObservacao.Text;
                    workflow.IdUsuario = new Util().GetSessaoUsuario().IdUsuario;
                    workflow.IdPerfil = new Util().GetSessaoPerfilAtivo();
                    Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(workflow.IdDocumento);
                    WorkflowBusiness owWorkflowBusiness = new WorkflowBusiness();
                    owWorkflowBusiness.ExecutarAcao(workflow, WorkflowBusiness.Acao.Reprovar, oDocumento);

                    new AuditoriaLogBusiness().AdicionarLogDocumentoAcaoSecundaria(oDocumento, Request.Browser);
                }

                Response.Redirect(String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("99")), false);
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