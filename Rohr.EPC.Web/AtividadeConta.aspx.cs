using Rohr.EPC.Business;
using System;
using System.Web;

namespace Rohr.EPC.Web
{
    public partial class AtividadeConta1 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Repeater1.DataSource = new AuditoriaLogBusiness().ObterLog(1, new Util().GetSessaoUsuario());
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
    }
}