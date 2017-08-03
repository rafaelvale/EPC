using System.Text;
using EO.Pdf;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Threading;
using System.Web;
using System.Web.SessionState;

namespace Rohr.EPC.Web
{
    public class BaixarDocumento : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                Documento documento = null;
                Int32 idPerfilAtivo;
                Int32 idUsuario;

                String parametro;
                if (context.Request.QueryString["p"] != null)
                    parametro = context.Request.QueryString["p"];
                else
                    throw new Exception();

                if (context.Session["usuario"] != null)
                {
                    idPerfilAtivo = new Util().GetSessaoPerfilAtivo();
                    idUsuario = new Util().GetSessaoUsuario().IdUsuario;

                    if (context.Session["documento"] != null && context.Request.QueryString["v"] == null && context.Session["documentoBaixar"] == null)
                        documento = ((Documento)context.Session["documento"]);
                    else if (context.Session["documentoBaixar"] != null)
                        documento = ((Documento)context.Session["documentoBaixar"]);

                    context.Session["documentoBaixar"] = null;
                }
                else
                    throw new Exception();

                if (String.Compare(parametro, "a1a1", StringComparison.OrdinalIgnoreCase) == 0)
                    GerarPdf(documento, idPerfilAtivo, context, idUsuario);
                else if (String.Compare(parametro, "a2b2", StringComparison.OrdinalIgnoreCase) == 0)
                    GerarPlanilhaOrcamentaria(documento, context);
                else
                    throw new Exception();
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                context.Response.Write("Não foi possível gerar o documento. ");
            }
        }
        void GerarPdf(Documento documento, Int32 idPerfilAtivo, HttpContext context, Int32 idUsuario)
        {
            if (Util.VerificarModeloCliente(documento.Modelo.ModeloTipo))
            {
                Arquivo oArquivo = new ArquivoBusiness().ObterArquivoPorIdDocumento(documento.IdDocumento);
                new AuditoriaLogBusiness().AdicionarLogPdf(documento, context.Request.Browser);

                context.Response.Clear();
                context.Response.ClearContent();
                context.Response.ContentType = oArquivo.Tipo;
                context.Response.AddHeader("content-disposition", "inline=" + documento.Modelo.Titulo + "-" + documento.NumeroDocumento + ".pdf");
                context.Response.BinaryWrite(oArquivo.Conteudo);
                context.Response.End();
            }
            else
            {
                PdfDocument doc = new PdfBusiness().GerarPdf(documento, idPerfilAtivo, idUsuario);
                new AuditoriaLogBusiness().AdicionarLogPdf(documento, context.Request.Browser);


                context.Response.Clear();
                context.Response.ClearContent();
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("content-disposition", "filename=" + documento.Modelo.Titulo + "-" + documento.NumeroDocumento + ".pdf");
                doc.Save(context.Response.OutputStream);
                context.Response.End();
            }
        }
        void GerarPlanilhaOrcamentaria(Documento documento, HttpContext context)
        {
            if (documento.ListDocumentoObjeto == null)
                context.Response.Write(String.Format("O documento {0}, não tem objeto preenchido.", documento.NumeroDocumento));
            else
            {
                context.Response.Clear();
                context.Response.AppendHeader("content-disposition", "attachment; filename=Planilha Orcamentaria.xls");
                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.Charset = "utf-8";
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.Write(new PlanilhaOrcamentariaBusiness().GerarPlanilhaOrcamentaria(documento));
                new AuditoriaLogBusiness().AdicionarLogPlanilhaOrcamentaria(documento, context.Request.Browser);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}