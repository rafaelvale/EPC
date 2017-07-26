using Rohr.EPC.Business;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web.Relatorios
{
    public partial class Pesquisa : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack) return;
                if (Request.QueryString["Search"] == null) return;
                txtPesquisar.Text = Request.QueryString["Search"];

                DataTable dataTable = new DocumentoBusiness().ObterTodosDocumento(txtPesquisar.Text, new Util().GetSessaoPerfilAtivo(), new Util().GetSessaoUsuario());

                Repeater1.DataSource = dataTable;
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">$(document).ready(function() {alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');});</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        protected void linkButtonPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = new DocumentoBusiness().ObterTodosDocumento(txtPesquisar.Text, new Util().GetSessaoPerfilAtivo(), new Util().GetSessaoUsuario());

                Repeater1.DataSource = dataTable;
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">$(document).ready(function() {alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');});</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            TratarNomeCliente(((DataRowView)(e.Item.DataItem)).Row.ItemArray[9].ToString(), e);
            TratarNomeObra(((DataRowView)(e.Item.DataItem)).Row.ItemArray[13].ToString(), e);
            TratarNomeFilial(Int32.Parse(((DataRowView)(e.Item.DataItem)).Row.ItemArray[8].ToString()), e);
            TratarProximaAcao(((DataRowView)(e.Item.DataItem)).Row.ItemArray[14].ToString(), ((DataRowView)(e.Item.DataItem)).Row.ItemArray[12].ToString(), e);

            ((CheckBox)e.Item.FindControl("ckDocumento")).Attributes.Add("idDocumento", ((DataRowView)(e.Item.DataItem)).Row.ItemArray[0].ToString());
        }

        static void TratarNomeCliente(String nomeCliente, RepeaterItemEventArgs e)
        {
            Label oLabel = ((Label)e.Item.FindControl("lblNomeCliente"));

            if (nomeCliente.Length > 32)
                oLabel.Text = nomeCliente.Substring(0, 32) + "...";
            else
                oLabel.Text = nomeCliente;

            oLabel.ToolTip = nomeCliente;

        }
        static void TratarNomeObra(String nomeObra, RepeaterItemEventArgs e)
        {
            Label oLabel = ((Label)e.Item.FindControl("lblNomeObra"));

            if (nomeObra.Length > 30)
                oLabel.Text = nomeObra.Substring(0, 30) + "...";
            else
                oLabel.Text = nomeObra;

            oLabel.ToolTip = nomeObra;
        }
        static void TratarNomeFilial(Int32 idFilial, RepeaterItemEventArgs e)
        {
            Label oLabel = ((Label)e.Item.FindControl("lblFilial"));
            oLabel.Text = Util.GetNomeFilial(idFilial);
        }
        static void TratarProximaAcao(String idDocumentoStatus, String proximaAcao, RepeaterItemEventArgs e)
        {
            Label oLabel = ((Label)e.Item.FindControl("lblDescricaoAcao"));

            if (String.Compare(idDocumentoStatus, "5", StringComparison.Ordinal) == 0)
                oLabel.Text += "<span class=\"label label-warning\">Contrato Gerado</span>";
            else if (String.Compare(idDocumentoStatus, "2", StringComparison.Ordinal) == 0)
                oLabel.Text += "<span class=\"label label-success\">Concluído</span>";
            else if (String.Compare(idDocumentoStatus, "3", StringComparison.Ordinal) == 0)
                oLabel.Text += "<span class=\"label label-info\">Cancelado</span>";
            else if (String.Compare(idDocumentoStatus, "7", StringComparison.Ordinal) == 0)
                oLabel.Text += "<span class=\"label label-important\">Sem Negócio</span>";
            else
                oLabel.Text = proximaAcao;
        }
    }
}