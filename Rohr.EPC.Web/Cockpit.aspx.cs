using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class Cockpit : BasePage
    {
        public int PaginaAtual
        {
            get
            {
                object o = ViewState["_PaginaAtual"];
                if (o == null)
                    return 0;
                return (int)o;
            }

            set
            {
                if (value < 0)
                    ViewState["_PaginaAtual"] = 0;
                else
                    ViewState["_PaginaAtual"] = value;
            }
        }

        static void VerificarPendencia(Int32 idDocumento, RepeaterItemEventArgs e)
        {
            Workflow oWorkflow = new WorkflowBusiness().ObterUltimaAcaoPorIdDocumento(idDocumento);

            TimeSpan diferenca = DateTime.Now.Subtract(oWorkflow.DataCadastro);
            Decimal metaEmMinuto = (((oWorkflow.Meta * 24) * 60));

            Decimal prazoLimite = metaEmMinuto / 3;
            prazoLimite = prazoLimite * 2;

            Image image = ((Image)e.Item.FindControl("imgPendente"));

            if (prazoLimite == 0)
                image.Visible = false;
            else if (diferenca.TotalMinutes <= Convert.ToDouble(prazoLimite))
                image.ImageUrl = "Content/Image/flag_blue.png";
            else if (diferenca.TotalMinutes > Convert.ToDouble(prazoLimite) && diferenca.TotalMinutes <= Convert.ToDouble(metaEmMinuto))
                image.ImageUrl = "Content/Image/flag_yellow.png";
            else
                image.ImageUrl = "Content/Image/flag_red.png";

            ((Label)e.Item.FindControl("lblPendente")).Text = " " + new Util().TratarTempo(oWorkflow.DataCadastro);
            ((Label)e.Item.FindControl("lblPendente")).ToolTip = "Meta: " + new Util().TratarTempoFracao(oWorkflow.Meta);
        }
        static void TratarNomeCliente(String nomeCliente, RepeaterItemEventArgs e)
        {
            Label oLabel = ((Label)e.Item.FindControl("lblNomeCliente"));
            if (nomeCliente.Length > 23)
                oLabel.Text = nomeCliente.Substring(0, 23) + "...";
            else
                oLabel.Text = nomeCliente;

            oLabel.ToolTip = nomeCliente;

        }
        static void TratarNomeObra(String nomeObra, RepeaterItemEventArgs e)
        {
            Label oLabel = ((Label)e.Item.FindControl("lblNomeObra"));

            if (nomeObra.Length > 19)
                oLabel.Text = nomeObra.Substring(0, 19) + "...";
            else
                oLabel.Text = nomeObra;

            oLabel.ToolTip = nomeObra;
        }
        static void TratarBloqueio(String numeroDocumento, String idUsuarioBloqueio, RepeaterItemEventArgs e)
        {
            /// Verifica se o documento está bloqueado temporariamente para análise.


            Label oLabel = ((Label)e.Item.FindControl("lblNumeroDocumento"));

            if (!String.IsNullOrEmpty(idUsuarioBloqueio))
            {
                oLabel.Text = "<i class=\"icon-tag\"></i> ";
                oLabel.ToolTip = "Documento bloqueado";
            }
        }

        void ExibirModal()
        {
            if (new Util().GetSessaoUsuario().ExibirModal)
            {
                UsuarioBusiness.AtualizarExibicaoModal(new Util().GetSessaoUsuario());
                Page.ClientScript.RegisterStartupScript(GetType(), "script1", "<script>$(document).ready(function () {$('#modal-content').modal();})</script>");
            }
        }

        void CarregarDocumentos(Boolean todos, Int32 idWorkflowAcao)
        {
            List<Filial> filial = new Util().GetSessaoUsuario().Filiais.ToList();

            Int32 idPerfil = new Util().GetSessaoPerfilAtivo();

            //HASH Usuário Julio, apesar de ter o perfil de gerencia não aprova como gerente os documento de Porto Alegre e Curitiba.
            if (new Util().GetSessaoUsuario().IdUsuario == 20 && idPerfil == 8)
            {
                filial.RemoveAll(f => f.IdFilial == 2);
                filial.RemoveAll(f => f.IdFilial == 7);
            }
            //HASH

            DataTable dataTable = new DocumentoBusiness().ObterTodos(filial, idPerfil, idWorkflowAcao);

            String filtroPrincipal = String.Empty;
            String filtroSecundario = String.Empty;

            switch (idPerfil)
            {
                case 1:
                    filtroPrincipal = "idWorkflowEtapa = 10 or idWorkflowEtapa = 1";
                    filtroSecundario = "idWorkflowEtapa <> 10 AND idWorkflowEtapa <> 1";
                    break;
                case 3:
                    filtroPrincipal = "idWorkflowEtapa = 3";
                    filtroSecundario = "idWorkflowEtapa  <> 3";
                    break;
                case 4:
                    filtroPrincipal = "idWorkflowEtapa = 4";
                    filtroSecundario = "idWorkflowEtapa  <> 4";
                    break;
                case 5:
                    filtroPrincipal = "idWorkflowEtapa = 6";
                    filtroSecundario = "idWorkflowEtapa  <> 6";
                    break;
                case 6:
                    filtroPrincipal = "idWorkflowEtapa = 10 or idWorkflowEtapa = 7 or idWorkflowEtapa = 1";
                    filtroSecundario = "idWorkflowEtapa <> 10 AND idWorkflowEtapa <> 7 AND idWorkflowEtapa <> 1";
                    break;
                case 7:
                    filtroPrincipal = "idWorkflowEtapa = 8";
                    filtroSecundario = "idWorkflowEtapa <> 8";
                    break;
                case 8:
                    filtroPrincipal = "idWorkflowEtapa = 9";
                    filtroSecundario = "idWorkflowEtapa  <> 9";
                    break;
                case 9:
                    filtroPrincipal = "idWorkflowEtapa = 11";
                    filtroSecundario = "idWorkflowEtapa  <> 11";
                    break;
                case 10:
                    filtroPrincipal = "idWorkflowEtapa = 12";
                    filtroSecundario = "idWorkflowEtapa  <> 12";
                    break;
                default:
                    filtroPrincipal = "idWorkflowEtapa <> 0";
                    filtroSecundario = "idWorkflowEtapa  <> 0";
                    break;
            }

            DataView dv = dataTable.DefaultView;
            dv.Sort = "dataCadastroWorkflow Desc";
            dv.RowFilter = filtroPrincipal;
            DataTable dtAux = dv.ToTable();

            if (idPerfil != 2 && todos)
            {
                DataView dv1 = dataTable.DefaultView;
                dv1.Sort = "dataCadastroWorkflow Desc";
                dv1.RowFilter = filtroSecundario;
                DataTable dtAux1 = dv.ToTable();
                dtAux.Merge(dtAux1);
            }

            if (idPerfil == 2)
                btnExibicao.Visible = false;

            PagedDataSource oPagedDataSource = new PagedDataSource { DataSource = dtAux.DefaultView };

            ConfigurarPaginacao(oPagedDataSource);

            Repeater1.DataSource = oPagedDataSource;
            Repeater1.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Util.ObterServidorDados(lblServidor);
                ConfigurarBotaoExibirDocumentos();
                ConfigurarFiltroProximaAcao();
                

                if (!Page.IsPostBack)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "script", "<script>$(document).ready(function () {$('.container-fluid').delay(300).fadeIn();})</script>");
                    ExibirModal();
                }
                else
                    Page.ClientScript.RegisterStartupScript(GetType(), "script", "<script>$(document).ready(function () {$('.container-fluid').delay(10).fadeIn();})</script>");
                
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "')</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);

                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
                VerificarPendencia(Int32.Parse(((DataRowView)(e.Item.DataItem)).Row.ItemArray[0].ToString()), e);
                TratarNomeCliente(((DataRowView)(e.Item.DataItem)).Row.ItemArray[9].ToString(), e);
                TratarNomeObra(((DataRowView)(e.Item.DataItem)).Row.ItemArray[14].ToString(), e);
                TratarBloqueio(((DataRowView)(e.Item.DataItem)).Row.ItemArray[3].ToString(),
                               ((DataRowView)(e.Item.DataItem)).Row.ItemArray[21].ToString(),
                               e);

                ((CheckBox)e.Item.FindControl("ckDocumento")).Attributes.Add("idDocumento", ((DataRowView)(e.Item.DataItem)).Row.ItemArray[0].ToString());
                ((HiddenField)e.Item.FindControl("hdIdDocumento")).Value = ((DataRowView)(e.Item.DataItem)).Row.ItemArray[0].ToString();
          
                
        }
        protected void btnExibicao_Click(object sender, EventArgs e)
        {
            try
            {
                new Util().GetSessaoUsuario().Perfis.Find(p => p.IdPerfil == new Util().GetSessaoPerfilAtivo()).ExibirTodosDocumento = new PerfilBusiness().AtualizarExibirTodosDocumento(new Util().GetSessaoUsuario().IdUsuario, new Util().GetSessaoPerfilAtivo());
                ViewState["exibirTodos"] = !Convert.ToBoolean(ViewState["exibirTodos"]);
                ConfigurarBotaoExibirDocumentos();
                new AuditoriaLogBusiness().AdicionarLogModeloExibicao(Request.Browser);
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "')</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);

                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnProximo_Click(object sender, EventArgs e)
        {
            try
            {
                PaginaAtual += 1;
                CarregarDocumentos(Convert.ToBoolean(ViewState["exibirTodos"]), Convert.ToInt32(ViewState["filtroProximaAcao"]));
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "')</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);

                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                PaginaAtual -= 1;
                CarregarDocumentos(Convert.ToBoolean(ViewState["exibirTodos"]), Convert.ToInt32(ViewState["filtroProximaAcao"]));
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "')</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);

                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void lbMarcarBloqueio_Click(object sender, EventArgs e)
        {
            try
            {
                List<DocumentoBloqueado> listDocumentoBloqueado = new List<DocumentoBloqueado>();
                foreach (RepeaterItem item in ((Repeater)Page.Form.Controls[1].Controls[22]).Items)
                {
                    if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem) continue;
                    CheckBox checkbox = (CheckBox)item.FindControl("ckDocumento");

                    if (!checkbox.Checked) continue;

                    listDocumentoBloqueado.Add(
                        new DocumentoBloqueado
                        {
                            IdDocumento = Int32.Parse(checkbox.Attributes["idDocumento"]),
                            IdUsuario = new Util().GetSessaoUsuario().IdUsuario,
                            IdPerfil = new Util().GetSessaoPerfilAtivo()
                        });

                    checkbox.Checked = false;

                }

                if (listDocumentoBloqueado.Count <= 0)
                    throw new MyException("Nenhum documento selecionado.");

                new DocumentoBloqueadoBusiness().AdicionarDocumentoBloqueado(listDocumentoBloqueado);
                CarregarDocumentos(Convert.ToBoolean(ViewState["exibirTodos"]), Convert.ToInt32(ViewState["filtroProximaAcao"]));
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
            }
        }
        protected void lblRemoverBloqueio_Click(object sender, EventArgs e)
        {
            try
            {
                List<Int32> listDocumentoBloqueado = new List<Int32>();
                foreach (RepeaterItem item in ((Repeater)Page.Form.Controls[1].Controls[22]).Items)
                {
                    if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem) continue;
                    CheckBox checkbox = (CheckBox)item.FindControl("ckDocumento");

                    if (!checkbox.Checked) continue;
                    listDocumentoBloqueado.Add(Int32.Parse(checkbox.Attributes["idDocumento"]));

                    checkbox.Checked = false;
                }

                if (listDocumentoBloqueado.Count <= 0)
                    throw new MyException("Nenhum documento selecionado.");

                new DocumentoBloqueadoBusiness().RemoverBloqueio(listDocumentoBloqueado);
                CarregarDocumentos(Convert.ToBoolean(ViewState["exibirTodos"]), Convert.ToInt32(ViewState["filtroProximaAcao"]));
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
            }
        }
        protected void linkButtonPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format("Relatorios/Pesquisa.aspx?search={0}", txtPesquisar.Text));
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
            }
        }

        void ConfigurarBotaoExibirDocumentos()
        {
            Usuario oUsuario = new Util().GetSessaoUsuario();
            ViewState["exibirTodos"] = oUsuario.Perfis.Find(p => p.IdPerfil == new Util().GetSessaoPerfilAtivo()).ExibirTodosDocumento;

            btnExibicao.Text = Convert.ToBoolean(ViewState["exibirTodos"]) ? "Exibir apenas os meus" : "Exibir todos";

            CarregarDocumentos(Convert.ToBoolean(ViewState["exibirTodos"]), Convert.ToInt32(ViewState["filtroProximaAcao"]));
        }
        void ConfigurarFiltroProximaAcao()
        {
            if (ViewState["filtroProximaAcao"] == null)
                ViewState["filtroProximaAcao"] = 0;
        }
        void ConfigurarPaginacao(PagedDataSource oPagedDataSource)
        {
            oPagedDataSource.AllowPaging = true;
            oPagedDataSource.PageSize = 30;

            if (PaginaAtual >= oPagedDataSource.PageCount)
                PaginaAtual = oPagedDataSource.PageCount - 1;

            oPagedDataSource.CurrentPageIndex = PaginaAtual;
            btnProximo.Enabled = !oPagedDataSource.IsLastPage;
            btnAnterior.Enabled = !oPagedDataSource.IsFirstPage;

            if (oPagedDataSource.DataSourceCount > 0)
            {
                if (oPagedDataSource.CurrentPageIndex == 0 && oPagedDataSource.DataSourceCount > 1)
                    lblTotalDocumentos.Text = "1 - " + oPagedDataSource.PageSize + " de " + oPagedDataSource.DataSourceCount;
                if (oPagedDataSource.CurrentPageIndex == 0 && oPagedDataSource.DataSourceCount == 1)
                    lblTotalDocumentos.Text = "1 - 1 de 1";
                else if (oPagedDataSource.IsLastPage)
                    lblTotalDocumentos.Text = (((oPagedDataSource.CurrentPageIndex + 1) * oPagedDataSource.PageSize) - oPagedDataSource.PageSize + 1) + " - " + oPagedDataSource.DataSourceCount + " de " + oPagedDataSource.DataSourceCount;
                else
                    lblTotalDocumentos.Text = (((oPagedDataSource.CurrentPageIndex + 1) * oPagedDataSource.PageSize) - oPagedDataSource.PageSize + 1) + " - " + ((oPagedDataSource.CurrentPageIndex + 1) * oPagedDataSource.PageSize) + " de " + oPagedDataSource.DataSourceCount;
            }
            else
                lblTotalDocumentos.Text = "&nbsp;";
        }
        protected void lkb_Click(Object sender, EventArgs e)
        {
            Int32 idWorkflowAcao = 0;

            LinkButton lb = (LinkButton)(sender);

            idWorkflowAcao = Int32.Parse(lb.CommandArgument);
            
            if (lb.Text == "Todas")
                btnAcaoSerExecutada.InnerHtml = String.Format("{0} <span class=\"caret\"></span>", "Ação a ser executada");
            else
                btnAcaoSerExecutada.InnerHtml = String.Format("{0} <span class=\"caret\"></span>", lb.Text);

            ViewState["filtroProximaAcao"] = idWorkflowAcao.ToString();


            PaginaAtual = 0;
            CarregarDocumentos(Convert.ToBoolean(ViewState["exibirTodos"]), idWorkflowAcao);
        }
    }
}