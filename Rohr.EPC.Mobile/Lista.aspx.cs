using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Mobile
{
    public partial class Lista : System.Web.UI.Page
    {
        private Int32 _totalOportunidades;
        private Int32 _totalContratos;

        void CarregarDocumentos(Boolean eProposta)
        {
            List<Filial> filial = new Util().GetSessaoUsuario().Filiais;

            Int32 idPerfil = new Util().GetSessaoPerfilAtivo();
            DataTable dataTable = new DocumentoBusiness().ObterTodos(filial, idPerfil, 0);

            String filtroPrincipal = String.Empty;
            switch (idPerfil)
            {
                case 1:
                    filtroPrincipal = "(idWorkflowEtapa = 10 or idWorkflowEtapa = 1)";
                    break;
                case 5:
                    filtroPrincipal = "idWorkflowEtapa = 6";
                    break;
                case 4:
                    filtroPrincipal = "idWorkflowEtapa = 4";
                    break;
                case 3:
                    filtroPrincipal = "idWorkflowEtapa = " + idPerfil;
                    break;
                case 6:
                    filtroPrincipal = "(idWorkflowEtapa = 10 or idWorkflowEtapa = 7 or idWorkflowEtapa = 1)";
                    break;
                case 7:
                    filtroPrincipal = "idWorkflowEtapa = 8";
                    break;
                case 8:
                    filtroPrincipal = "idWorkflowEtapa = 9";
                    break;
                case 9:
                    filtroPrincipal = "idWorkflowEtapa = 11";
                    break;
                case 10:
                    filtroPrincipal = "idWorkflowEtapa = 12";
                    break;
            }

            DataView dv = dataTable.DefaultView;
            dv.Sort = "dataCadastroWorkflow Desc";
            dv.RowFilter = filtroPrincipal;
            DataTable dtAux = dv.ToTable();

            _totalOportunidades = dtAux.Select("EProposta = True").Length;
            _totalContratos = dtAux.Select("EProposta = False").Length;

            Session["totalOportunidade"] = _totalOportunidades.ToString();
            Session["totalContrato"] = _totalContratos.ToString();

            DataView dvAux = dtAux.DefaultView;
            dvAux.RowFilter = String.Format("EProposta = {0} ", eProposta);

            Repeater1.DataSource = dvAux.ToTable();
            Repeater1.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(Request.QueryString["t"]) || Request.QueryString["t"] == "1")
                {
                    CarregarDocumentos(true);
                    MenuRodape.Menu = 1;
                }
                else
                {
                    CarregarDocumentos(false);
                    MenuRodape.Menu = 2;
                }
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "')</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
    }
}