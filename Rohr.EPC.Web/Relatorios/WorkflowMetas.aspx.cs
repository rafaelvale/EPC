using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EO.Pdf;
using Rohr.EPC.Entity;
using Rohr.EPC.Business;
using System.Threading;

namespace Rohr.EPC.Web.Relatorios
{
    public partial class WorkflowMetas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Repeater1.DataSource = new ModeloBusiness().ObterTodosModelosRelatorio();
            Repeater1.DataBind();

            Repeater2.DataSource = new ModeloCondicoesGeraisBusiness().ObterLista();
            Repeater2.DataBind();
        }
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((Label)e.Item.FindControl("lblMeta")).Text = new Util().TratarTempoFracao(((Modelo)e.Item.DataItem).ModeloMeta.Meta);
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton oLinkButton = (LinkButton)sender;
                PdfDocument doc = new PdfBusiness().GerarPdfModeloSemFormacao(Convert.ToInt32(oLinkButton.CommandArgument));

                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearHeaders();
                response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=ModeloRohr.pdf");
                doc.Save(response.OutputStream);
                response.End();
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">$(document).ready(function() {alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');});</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void LinkButtonModeloCondicoesGerais_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton oLinkButton = (LinkButton)sender;
                PdfDocument doc = new PdfBusiness().GerarPdfModeloCondicoesGeraisSemFormacao(Convert.ToInt32(oLinkButton.CommandArgument));

                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearHeaders();
                response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=ModeloRohr.pdf");
                doc.Save(response.OutputStream);
                response.End();
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">$(document).ready(function() {alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');});</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        protected void LinkButtonModeloHistoriaRohr_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton oLinkButton = (LinkButton)sender;
                PdfDocument doc = new PdfBusiness().GerarPdfHistoriaRohr(Convert.ToInt32(oLinkButton.CommandArgument));

                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearHeaders();
                response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=ModeloRohr.pdf");
                doc.Save(response.OutputStream);
                response.End();
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">$(document).ready(function() {alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');});</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        protected void LinkButtonModeloPortfolioObras_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton oLinkButton = (LinkButton)sender;
                PdfDocument doc = new PdfBusiness().GerarPdfPortfolioObras(Convert.ToInt32(oLinkButton.CommandArgument));

                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearHeaders();
                response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=ModeloRohr.pdf");
                doc.Save(response.OutputStream);
                response.End();
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">$(document).ready(function() {alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');});</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
    }
}