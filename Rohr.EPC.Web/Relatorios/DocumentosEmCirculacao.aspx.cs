using Rohr.EPC.Business;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web.Relatorios
{
    public partial class DocumentosEmCirculacao : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = new DocumentoBusiness().ObterTodosDocumentosEmCirculacao(new Util().GetSessaoUsuario().Filiais);

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
            VerificarPeriodoCriado(DateTime.Parse(((DataRowView)(e.Item.DataItem)).Row.ItemArray[0].ToString()), e);
            TratarNomeCliente(((DataRowView)(e.Item.DataItem)).Row.ItemArray[6].ToString(), e);
            TratarNomeObra(((DataRowView)(e.Item.DataItem)).Row.ItemArray[7].ToString(), e);
            TratarNomeFilial(Int32.Parse(((DataRowView)(e.Item.DataItem)).Row.ItemArray[10].ToString()), e);
        }

        static void TratarNomeCliente(String nomeCliente, RepeaterItemEventArgs e)
        {
            Label oLabel = ((Label)e.Item.FindControl("lblNomeCliente"));
            if (nomeCliente.Length > 27)
                oLabel.Text = nomeCliente.Substring(0, 27) + "...";
            else
                oLabel.Text = nomeCliente;

            oLabel.ToolTip = nomeCliente;

        }
        static void TratarNomeObra(String nomeObra, RepeaterItemEventArgs e)
        {
            Label oLabel = ((Label)e.Item.FindControl("lblNomeObra"));

            if (nomeObra.Length > 22)
                oLabel.Text = nomeObra.Substring(0, 22) + "...";
            else
                oLabel.Text = nomeObra;

            oLabel.ToolTip = nomeObra;
        }
        static void TratarNomeFilial(Int32 idFilial, RepeaterItemEventArgs e)
        {
            Label oLabel = ((Label)e.Item.FindControl("lblFilial"));
            oLabel.Text = Util.GetNomeFilial(idFilial);
        }
        static void VerificarPeriodoCriado(DateTime dataCadastro, RepeaterItemEventArgs e)
        {
            ((Label)e.Item.FindControl("lblCriado")).Text = new Util().TratarTempo(dataCadastro);
        }
    }
}