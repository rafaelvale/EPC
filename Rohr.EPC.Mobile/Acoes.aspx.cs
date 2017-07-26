using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Mobile
{
    public partial class Acoes : System.Web.UI.Page
    {
        Int32 _idWorkflowAcao;

        void MontarTela()
        {
            Util.TratarTextoBotaoAcao(btnAcaoPrimaria, btnAcaoSecundaria, _idWorkflowAcao);
        }
        void ValidarJustificativa()
        {
            if (String.IsNullOrWhiteSpace(txtObservacao.Text))
                throw new MyException("Informe uma observação :(");
        }

        static DataTable CriarDataTable()
        {
            DataTable dt = new DataTable("documentos");
            dt.Columns.Add(new DataColumn("numeroDocumento", typeof(Int32)));
            dt.Columns.Add(new DataColumn("nomeCliente", typeof(String)));
            dt.Columns.Add(new DataColumn("nomeObra", typeof(String)));
            dt.Columns.Add(new DataColumn("percentualDesconto", typeof(Double)));
            dt.Columns.Add(new DataColumn("valorNegocio", typeof(Double)));
            dt.Columns.Add(new DataColumn("alcada", typeof(String)));
            dt.Columns.Add(new DataColumn("idDocumento", typeof(Int32)));

            return dt;
        }
        static DataRow MontarLinhaDataTable(DataTable dt, Int32 idDocumento)
        {
            DataRow dr = dt.NewRow();
            Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(idDocumento);
            dr["numeroDocumento"] = oDocumento.NumeroDocumento;
            dr["nomeCliente"] = oDocumento.DocumentoCliente.Nome;
            dr["nomeObra"] = oDocumento.DocumentoObra.Nome;
            dr["percentualDesconto"] = oDocumento.PercentualDesconto;
            dr["valorNegocio"] = oDocumento.ValorNegocio;
            dr["idDocumento"] = oDocumento.IdDocumento;
            dr["alcada"] = new WorkflowAlcadaBusiness().ObterDescricaoAlcada(oDocumento);

            return dr;
        }

        static void VerificarBloqueio(int idDocumento)
        {
            DocumentoBloqueado oDocumentoBloqueado = new DocumentoBloqueadoBusiness().ObterBloqueioAtivo(idDocumento);
            if (oDocumentoBloqueado == null) return;
            Int32 idUsuarioBloqueio = oDocumentoBloqueado.IdUsuario;
            if (idUsuarioBloqueio != 0 && idUsuarioBloqueio != new Util().GetSessaoUsuario().IdUsuario)
                throw new MyException(String.Format("O documento nº: {0}, está bloqueado. Não é possível executar a ação :(",
                    new DocumentoBusiness().RecuperarDocumento(idDocumento).NumeroDocumento));
            if (idUsuarioBloqueio != 0 && idUsuarioBloqueio == new Util().GetSessaoUsuario().IdUsuario)
                new DocumentoBloqueadoBusiness().RemoverBloqueio(idDocumento);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MenuRodape.Menu = Convert.ToInt16(Request.QueryString["i"]);
                Int32 idDocumento = Convert.ToInt32(Request.QueryString["i"]);

                DataTable dt = CriarDataTable();
                dt.Rows.Add(MontarLinhaDataTable(dt, idDocumento));

                Repeater1.DataSource = dt;
                Repeater1.DataBind();

                _idWorkflowAcao = new WorkflowBusiness().ObterUltimaAcaoPorIdDocumento(idDocumento).WorkflowAcao.IdWorkflowAcao;

                MontarTela();
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                btnAcaoPrimaria.Enabled = false;
                btnAcaoSecundaria.Enabled = false;

            }
        }
        protected void btnAcaoPrimaria_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 idDocumento = Convert.ToInt32(Request.QueryString["i"]);
                VerificarBloqueio(Convert.ToInt32(idDocumento));

                Workflow oWorkflow = new WorkflowBusiness().ObterUltimaAcaoPorIdDocumento(idDocumento);
                oWorkflow.Justificativa = txtObservacao.Text;
                oWorkflow.IdUsuario = new Util().GetSessaoUsuario().IdUsuario;
                oWorkflow.IdPerfil = new Util().GetSessaoPerfilAtivo();
                oWorkflow.IdDocumento = idDocumento;

                Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(oWorkflow.IdDocumento);
                WorkflowBusiness oWorkflowBusiness = new WorkflowBusiness();
                oWorkflowBusiness.ExecutarAcao(oWorkflow, WorkflowBusiness.Acao.Aprovar, oDocumento);

                new AuditoriaLogBusiness().AdicionarLogDocumentoAcaoPrimaria(oDocumento, Request.Browser);

                Session["totalOportunidade"] = null;
                Session["totalContrato"] = null;
                Response.Redirect(String.Format("Lista.aspx?t={0}", Request.QueryString["t"]), false);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnAcaoSecundaria_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 idDocumento = Convert.ToInt32(Request.QueryString["i"]);
                VerificarBloqueio(Convert.ToInt32(idDocumento));

                if (_idWorkflowAcao < 8)
                    ValidarJustificativa();

                Workflow oWorkflow = new WorkflowBusiness().ObterUltimaAcaoPorIdDocumento(idDocumento);
                oWorkflow.Justificativa = txtObservacao.Text;
                oWorkflow.IdUsuario = new Util().GetSessaoUsuario().IdUsuario;
                oWorkflow.IdPerfil = new Util().GetSessaoPerfilAtivo();
                oWorkflow.IdDocumento = idDocumento;

                Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(oWorkflow.IdDocumento);
                WorkflowBusiness oWorkflowBusiness = new WorkflowBusiness();
                oWorkflowBusiness.ExecutarAcao(oWorkflow, WorkflowBusiness.Acao.Reprovar, oDocumento);
                new AuditoriaLogBusiness().AdicionarLogDocumentoAcaoSecundaria(oDocumento, Request.Browser);

                Session["totalOportunidade"] = null;
                Session["totalContrato"] = null;
                Response.Redirect(String.Format("Lista.aspx?t={0}", Request.QueryString["t"]), false);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            {
                DocumentoComercial oDocumentoComercial = new DocumentoComercialBusiness().ObterPorIdDocumento(Int32.Parse(((DataRowView)(e.Item.DataItem)).Row.ItemArray[6].ToString()));

                HtmlAnchor oHtmlAnchorCelular = (HtmlAnchor)e.Item.FindControl("lblCelular");
                oHtmlAnchorCelular.HRef = String.Format("tel:+{0}", oDocumentoComercial.Telefone);
                oHtmlAnchorCelular.InnerText = String.Format("Celular: {0}", oDocumentoComercial.Telefone);

                HtmlAnchor oHtmlAnchorEmail = (HtmlAnchor)e.Item.FindControl("lblEmail");
                oHtmlAnchorEmail.HRef = String.Format("mailto:{0}", oDocumentoComercial.Email);
                oHtmlAnchorEmail.InnerText = String.Format("Email: {0}", oDocumentoComercial.Email);

                Label oLabel = (Label)e.Item.FindControl("lblComercial");
                oLabel.Text = String.Format("{0} {1}", oDocumentoComercial.PrimeiroNome, oDocumentoComercial.Sobrenome);
            }
        }
    }
}