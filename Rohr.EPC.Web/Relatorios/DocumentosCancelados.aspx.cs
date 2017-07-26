using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web.Relatorios
{
    public partial class DocumentosCancelados : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = new DocumentoBusiness().ObterTodosDocumentosCancelados(new Util().GetSessaoUsuario().Filiais);
                Repeater1.DataSource = dataTable;
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            TratarNomeCliente(((DataRowView)(e.Item.DataItem)).Row.ItemArray[8].ToString(), e);
            TratarNomeObra(((DataRowView)(e.Item.DataItem)).Row.ItemArray[10].ToString(), e);
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
    }
}