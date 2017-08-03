using EO.Pdf;
using EO.Pdf.Acm;
using EO.Pdf.Contents;
using EO.Pdf.Drawing;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Rohr.EPC.Business
{
    public class PdfBusiness
    {


        Boolean _carimboAprovacaoJuridico;
        Boolean _marcaDAguaMinuta;
        Boolean _marcaDAguaCancelado;
        Int32 _idDocumento;
        Int32 _idUsuario;

        public PdfBusiness()
        {
            Runtime.AddLicense(
                "J/oO5Kfq6doPvUaBpLHLn3Xj7fQQ7azc6c/nrqXg5/YZ8p7cwp61n1mXpM0M" +
                "66Xm+8+4iVmXpLHLn1mXwPIP41nr/QEQvFu807/745+ZpAcQ8azg8//ooW+l" +
                "tLPLrneEjrHLn1mzs/IX66juwp61n1mXpM0a8Z3c9toZ5aiX6PIf5HaZvMDg" +
                "rmuntcXNn6/c9gQU7qe0psbNn2i1kZvLn1mXwAQU5qfY+AYd5Hfk2tkVt2is" +
                "9PMh6a6i7tofqmnf3eIivHazswQU5qfY+AYd5HeEjs3a66La6f8e5HeEjnXj" +
                "7fQQ7azcwp61n1mXpM0X6Jzc8gQQyJ21usTksHCuucbdtnWm8A==");


            HtmlToPdf.Options.PageSize = new SizeF(PdfPageSizes.A4.Width, PdfPageSizes.A4.Height);

            HtmlToPdf.Options.GeneratePageImages = true;
            HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.None;
            HtmlToPdf.Options.OutputArea = new RectangleF(0.5f, 1f, 7.2f, 10f);
            HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.None;
            HtmlToPdf.Options.AutoFitY = HtmlToPdfAutoFitMode.None;
        }


        public PdfDocument GerarPdf(Documento documento, Int32 idPerfil, Int32 idUsuario)
        {
            _idDocumento = documento.IdDocumento;
            _idUsuario = idUsuario;




            PdfDocument documentoPdf = new PdfDocument();
            PdfDocument documentoPdfFinal = new PdfDocument();
            StringBuilder html = new StringBuilder(MontarHtmlDocumento(documento));
            HtmlToPdfResult result = HtmlToPdf.ConvertHtml(html.ToString(), documentoPdf);
            if (result.LastPosition > 4 ||
                (result.LastPosition > 1.5 && documento.Modelo.Segmento.IdSegmento == 3))
            {

                html.Append(MontarHtmlEspacoVazio(result.LastPosition));
                html.Append(MontarHtmlParteAssinatura(documento, true));

                FormatarTabela(html);

                result = HtmlToPdf.ConvertHtml(html.ToString(), documentoPdf);

                if (result.LastPosition <= 9.6)


                    html.Append(MontarHtmlEspacoVazio(result.LastPosition));

            }
            else
            {

                html.Append(MontarHtmlParteAssinatura(documento, false));

                FormatarTabela(html);

                result = HtmlToPdf.ConvertHtml(html.ToString(), documentoPdf);


                html.Append(MontarHtmlEspacoVazio(result.LastPosition));


            }

            html.Append(MontarHtmlCondicoesGerais(documento));
            if (documento.EProposta && documento.PortfolioObras == true)
            {
                html.Append(MontarHtmlPortfolioObras(documento));
            }

            if (documento.EProposta && documento.ExibirHistoria == true)
            {

                html.Append(MontarHtmlHistoriaRohr(documento));
            }



            HtmlToPdf.Options.BeforeRenderPage = BeforeRenderPage;
            GerarCarimboAprovacaoJuridico(idPerfil, documento);
            GerarMarcaDAgua(idPerfil, documento);
            GerarCabecalho(documento);
            GerarRodape(documento, idUsuario);


            HtmlToPdf.ConvertHtml(html.ToString(), documentoPdfFinal);

            return documentoPdfFinal;
        }

        public PdfDocument GerarPdfModeloSemFormacao(Int32 idModelo)
        {


            PdfDocument documentoPdf = new PdfDocument();
            Modelo modelo = new ModeloBusiness().ObterModeloPorId(idModelo);
            StringBuilder html = new StringBuilder();

            html.Append("<!doctype html><body style=\"font-family:'Helvetica';font-size:13px; \"><meta charset=\"utf-8\">");
            html.Append(modelo.Titulo);
            html.Append("<br>");


            foreach (Parte parte in modelo.ListParte)
            {

                html.Append("<div style=\"text-align: justify; font-family:'Helvetica';font-size:13px;\">");

                html.Append(parte.TextoParte);


                html.Append("</div>");
                html.Append("<br />");
            }

            FormatarTabela(html);

            HtmlToPdf.ConvertHtml(html.ToString(), documentoPdf);
            HtmlToPdf.Options.BeforeRenderPage = BeforeRenderPage;

            return documentoPdf;
        }

        public PdfDocument GerarPdfPortfolioObras(Int32 idModeloPortfolioObras)
        {
            PdfDocument documentoPdf = new PdfDocument();
            ModeloPortfolioObras modelo = new ModeloPortfolioObrasBusiness().ObterPorId(idModeloPortfolioObras);
            StringBuilder html = new StringBuilder();

            html.Append("<!doctype html><body style=\"font-family:'MontSerrat', sans-serif; font-size:12px;\"><meta charset=\"utf-8\">");
            html.Append(modelo.Texto);
            html.Append("<br/>");


            HtmlToPdf.ConvertHtml(html.ToString(), documentoPdf);
            HtmlToPdf.Options.BeforeRenderPage = BeforeRenderPage;

            return documentoPdf;

        }

        public PdfDocument GerarPdfHistoriaRohr(Int32 idModeloHistoriaRohr)
        {
            PdfDocument documentoPdf = new PdfDocument();
            ModeloHistoriaRohr modelo = new ModeloHistoriaRohrBusiness().ObterPorId(idModeloHistoriaRohr);
            StringBuilder html = new StringBuilder();

            html.Append("<!doctype html><body style=\"font-family:'MontSerrat', sans-serif; font-size:12px;\"><meta charset=\"utf-8\">");
            html.Append(modelo.Texto);
            html.Append("<br/>");


            HtmlToPdf.ConvertHtml(html.ToString(), documentoPdf);
            HtmlToPdf.Options.BeforeRenderPage = BeforeRenderPage;

            return documentoPdf;
        }

        public PdfDocument GerarPdfModeloCondicoesGeraisSemFormacao(Int32 idModeloCondicoesGerais)
        {
            PdfDocument documentoPdf = new PdfDocument();
            ModeloCondicoesGerais modelo = new ModeloCondicoesGeraisBusiness().ObterPorId(idModeloCondicoesGerais);
            StringBuilder html = new StringBuilder();

            html.Append("<!doctype html><body style=\"font-family:'Helvetica', sans-serif;font-size:13px;\"><meta charset=\"utf-8\"><div style=\"page-break-before: always;\"></div>");
            html.Append(modelo.Texto);
            html.Append("<br />");

            FormatarTabela(html);

            HtmlToPdf.ConvertHtml(html.ToString(), documentoPdf);
            HtmlToPdf.Options.BeforeRenderPage = BeforeRenderPage;

            return documentoPdf;
        }

        void GerarCarimboAprovacaoJuridico(Int32 idPerfil, Documento documento)
        {
            if ((idPerfil == 3 || idPerfil == 4) && !documento.EProposta)
                _carimboAprovacaoJuridico = true;
            //if ((_idUsuario == 106) && !documento.EProposta)
            //    _carimboAprovacaoJuridico = false;    
        }

        void GerarMarcaDAgua(Int32 idPerfil, Documento documento)
        {
            if ((documento.Eminuta && idPerfil != 3 && idPerfil != 4))// && idPerfil != 6) Retirar marca d´agua para COMERCIAL PTA
                _marcaDAguaMinuta = true;

            if (documento.EProposta && new DocumentoBusiness().VerificarPropostaCancelada(documento.NumeroDocumento))
                _marcaDAguaCancelado = true;
        }

        void GerarCabecalho(Documento documento)
        {
            String caminhoLogo = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\logo-topo.png";


            StringBuilder htmlCabecalho = new StringBuilder();
            HtmlToPdf.Options.HeaderHtmlPosition = 0.15f;

            string dateTime = "24/05/2017";
            DateTime dtAtual = Convert.ToDateTime(dateTime);

            if (documento.DataParaExibicao >= dtAtual && documento.Modelo.IdModelo >= 119)
            {


                //NOVO LAYOUT - RAFAEL VALE
                htmlCabecalho.Append("<div style=\"font-family: 'Montserrat', sans-serif;font-size:10px;height:75px;\"> ");
                htmlCabecalho.Append("<div style=\"float:right;padding:5px;\"><img src='" + caminhoLogo + "' style=\"height:70px;width:73px\"/></div>");


            }
            else
            {
                //MODELO ANTIGO
                htmlCabecalho.Append("<div style=\"font-family:'Segoe UI';font-size:14px;background-color:#222222;color:#ffffff;height:75px;\"> ");
                htmlCabecalho.Append("<div style=\"float:left;padding:8px;\"><img src='" + caminhoLogo + "' style=\"height:62px;width:65px\"/></div>");
                //htmlCabecalho.Append("<div style=\"float:left;padding:10px;\"><img src='" + caminhoLogo + "'/></div>");

                htmlCabecalho.Append(documento.Modelo.Segmento.IdSegmento != 3
                ? String.Format("<div style=\"float:left;padding-top:25px;padding-left:25px;\">{0}&nbsp;-&nbsp;</div>",
                ObterTipoDocumentoParaExibicao(documento))
                : String.Format("<div style=\"float:left;padding-top:25px;padding-left:15px;\">{0}&nbsp;-&nbsp;</div>",
                ObterTipoDocumentoParaExibicao(documento)));

                htmlCabecalho.Append(String.Format("<div style=\"float:left;color:#FFC000;padding-top:25px;\">{0}</div>", ObterSegmentoParaExibicao(documento.Modelo.Segmento.Descricao)));

                htmlCabecalho.Append(documento.Modelo.Segmento.IdSegmento != 3
                ? String.Format("<div style=\"float:right;text-align:right;font-size:11px;padding-top:15px;padding-right:10px;width:260px;\">{0}<br>{1}<br>",
                ObterDataParaExibicao(documento),
                ObterNumeroDocumentoParaExibicao(documento))
                : String.Format("<div style=\"float:right;text-align:right;font-size:11px;padding-top:15px;padding-right:10px;width:220px;\">{0}<br>{1}<br>",
                ObterDataParaExibicao(documento),
                ObterNumeroDocumentoParaExibicao(documento)));


                htmlCabecalho.Append(String.Format("Obra: {0}</div></div>", ObterObraParaExibicao(documento.DocumentoObra.Nome)));
                htmlCabecalho.Append("<div style=\"background-color:#FFC000;height:4px;\"></div>");

            }

            HtmlToPdf.Options.HeaderHtmlFormat = htmlCabecalho.ToString();
        }

        void GerarRodape(Documento documento, Int32 idUsuario)
        {


            String caminhoRodape = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\bg-paginacao.png";

            string dateTime = "24/05/2017";
            DateTime dtAtual = Convert.ToDateTime(dateTime);
            HtmlToPdf.Options.FooterHtmlPosition = 11.01f;

            Company oCompany = new Company().ObterFilial(
                        new CompanyAddressesContact().ObterCompanyAddressesContactsById(
                        new Specifications().ObterSpecificationById(documento.DocumentoComercial.CodigoSistemaOrigem).ListItemId).CompanyId);


            if (documento.DataParaExibicao >= dtAtual && documento.Modelo.IdModelo >= 119)
            {


                //NOVO LAYOUT - RAFAEL VALE
                HtmlToPdf.Options.FooterHtmlFormat = "<div style=\"font-family: 'Helvetica', sans-serif;font-size:9px;margin-bottom: 5px;border-bottom-width: 0px; text-align:center;\">"
                    + String.Format("A Rohr apoia a Lei Federal nº 12.846 de 01/08/2013 e está implantando o Programa de Integridade(Compliance).Vamos adicionar mais este quesito a nossa parceria.<br><br></div>")

                    + String.Format("<div style=\"font-family: 'Helvetica', sans-serif;float:left;padding-bottom: 5px;width:90%; font-size:10px;text-align:left;\">{0}<br><strong>{1}/{2} - {3}</strong><div style=\"float: right;\"></strong>WWW.<strong>ROHR</strong>.COM.BR</div></div>",
                    ObterDataParaExibicao(documento),
                    oCompany.Sigla,
                    ObterSegmentoParaExibicao(documento.Modelo.Segmento.Descricao),
                    ObterNumeroDocumentoParaExibicao(documento))
                    + "<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700' rel=\"stylesheet\" type=\"text/css\"><div style=\"font-family: Montserrat, sans-serif;width: 6%;height:10%;padding-left:25px;padding-bottom:7px; float:right;margin-top: 5px;line-height:50px; \"><img src='" + caminhoRodape + "' style=\"font-family: Montserrat, sans-serif;padding-right:10px;padding-bottom:27px;z-index:-1;position: absolute;right: 9px; width:32px;\"/>{page_number}</div>";

            }
            else
            {
                //MODELO ANTIGO 
                HtmlToPdf.Options.FooterHtmlFormat = "<div style=\"font-family:'Helvetica';font-size:10px;text-align:center;background-color:#FFC000;padding:5px;line-height:16px;\">"
                + String.Format("Rohr S/A Estruturas Tubulares - Unidade {0} - {1}<br>A Rohr apoia a Lei Federal nº 12.846 de 01/08/2013 e está implantando o Programa de Integridade(Compliance).<br>Vamos adicionar mais este quesito a nossa parceria.",
                oCompany.CompanyName,
                new CompanyAddress().ObterCompanyAddresses(oCompany.Id).Phone)

                + String.Format("<div style=\"width:100%;\"><div style=\"width:2%; float:left; font-size:9px;text-align:left;\">{0}</div><div style=\"width:1%; display:inline-block;\">www.rohr.com.br</div>", GerarDataHoraEmissao(idUsuario))
                + "<div style=\"width: 33.33 %; float:right; text - align:right;\">Página {page_number} de {total_pages}</div></div>";
            }

        }

        String ObterDataParaExibicao(Documento documento)
        {
            String cidade;
            if (documento.DocumentoFilial != null)
                cidade = documento.DocumentoFilial.Cidade;
            else
            {
                cidade = new CompanyAddress().ObterCompanyAddresses(
                            new CompanyAddressesContact().ObterCompanyAddressesContactsById(
                                new Specifications().ObterSpecificationById(documento.DocumentoComercial.CodigoSistemaOrigem)
                                    .ListItemId
                                   ).CompanyId
                                  ).City.Description;
            }

            if (cidade.Length > 22)
                cidade = cidade.Substring(0, 22);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR", false);

            if (documento.DataParaExibicao.CompareTo(new DateTime()) == 0)
                return cidade + " - MINUTA - " + DateTime.Now.ToString("dd/MM/yyyy");

            int dia = documento.DataParaExibicao.Day;
            string mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(documento.DataParaExibicao.Month);
            int ano = documento.DataParaExibicao.Year;

            //return cidade + ", " + dia + " de " + mes + " de " + ano;

            return dia + " de " + mes + " de " + ano;
        }
        String ObterNumeroDocumentoParaExibicao(Documento documento)
        {
            if (documento.NumeroDocumento == 0)
                throw new MyException("Não foi possível recuperar o número do documento");

            String descricaoDocumento = "Proposta";

            if (!documento.EProposta)
                descricaoDocumento = "Contrato";

            if (documento.RevisaoCliente > 0)
                return String.Format("{2} {0:N0}-{1}", documento.NumeroDocumento, documento.RevisaoCliente, descricaoDocumento);

            return String.Format("{0} {1:N0}", descricaoDocumento, documento.NumeroDocumento);
        }
        String ObterSegmentoParaExibicao(String segmento)
        {
            if (String.IsNullOrEmpty(segmento))
                throw new MyException("Não foi possível recuperar o segmento");

            return segmento.ToUpper();
        }
        String ObterObraParaExibicao(String obra)
        {
            if (String.IsNullOrEmpty(obra))
                throw new MyException("Não foi possível recuperar a obra");

            return obra;
        }
        String ObterTipoDocumentoParaExibicao(Documento documento)
        {
            return documento.EProposta ? "PROPOSTA COMERCIAL" : "CONTRATO COMERCIAL";
        }


        String MontarHtmlParteAssinatura(Documento documento, Boolean quebraPagina)
        {
            StringBuilder html = new StringBuilder();

            html.Append(quebraPagina ? "<div style=\"page-break-before: always; text-align: justify; font-family:'Helvetica', sans-serif;font-size:13px;\">" : "<div style=\"text-align: justify;font-size: 12px;\">");
            var parte = documento.Modelo.ListParte.SingleOrDefault(x => x.IdParte == 11 || x.IdParte == 42 || x.IdParte == 64 || x.IdParte == 151);
            html.Append(TratarVariaveis(documento, parte));
            html.Append("</div>");

            return html.ToString();
        }


        String MontarHtmlDocumento(Documento documento)
        {
            StringBuilder html = new StringBuilder();

            foreach (Parte parte in documento.Modelo.ListParte.Where(x => x.IdParte != 11 && x.IdParte != 42 && x.IdParte != 64 && x.IdParte != 151))
            {

                html.Append("<div style=\"text-align: justify; font-family:'Helvetica', sans-serif;font-size:13px;\">");
                html.Append(TratarVariaveis(documento, parte));

                html.Append("</div>");
                html.Append("<br />");

                //incluir imagem no documento
                if (parte.ListChave != null && parte.ListChave.Any(L => L.ChaveDescricao.Equals("{rohr.documento.objeto}")))
                {
                    var DocuImagens = new DocumentoImagensBusiness().GetDocumentoImagens(documento.IdDocumento);

                    if (DocuImagens != null)
                    {
                        html.Append("<div style=\"text-align: justify; font-family:'Helvetica', sans-serif;font-size:13px; page-break-before: always; page-break-after: always; text-align: justify;\">");
                        html.Append("<br/><br/>");
                        html.Append("<div style=\"height:150px;\">");

                        html.Append(documento.DocumentoDescricaoGeralFoto.DescricaoGeral);
                        html.Append("</div>");
                        //html.Append("<div style=\" text-align: justify;\">");

                        html.Append("<table>");

                        html.Append("<tr>");


                        Int32 Total = 0;

                        foreach (DocumentoImagens DocImg in DocuImagens)
                        {

                            if (Total % 4 == 0 && Total > 0)
                            {

                                html.Append("<tr>");
                                html.Append("<td>");
                                html.Append("&nbsp;<div style=\"page-break-before: always;\"></div>");
                                html.Append("</td>");
                                html.Append("</tr>");


                            }

                            String Url = DocImg.Url.Replace("~", "");
                            Url = Url.Replace("/", @"\");

                            String url = AppDomain.CurrentDomain.BaseDirectory + Url;
                            //html.Append("<div style=\"text-align: justify;\">");

                            html.Append("<td><br/><br/>");
                            html.Append("<div style=\"font-family:'Helvetica', sans-serif;text-align:center;font-size:13px;padding: 5px 5px 35px 5px; height: 280px;width: 310px; background-color: #dfdcda;\">");
                            html.Append("<img src='" + url + "' style=\"height: 270px; width: 310px; padding: 0 0 10px 0; text-align: center;\"/>");
                            html.Append(DocImg.Descricao);
                            html.Append("</div>");
                            html.Append("</td>");


                            Total++;

                            if (Total % 2 == 0)
                            {
                                html.Append("<tr></tr>");
                            }

                        }

                        html.Append("</tr>");
                        html.Append("</table>");
                        html.Append("</div>");
                    }

                }

            }



            return html.ToString();
        }

        static String MontarHtmlEspacoVazio(Double lastPosition)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<div style=\"text-align: center; font-size:16;\">");

            const Double totalPaginaCompleta = 9.7;
            Double preenchido = lastPosition;

            while (preenchido < totalPaginaCompleta)
            {
                preenchido += 0.21;
                html.Append(".<br />");
            }

            html.Append("</div><div style=\"page-break-after: always;\"></div>");

            return html.ToString();
        }

        string MontarDescricaoGeralParte1(Documento documento)
        {
            return new DocumentoDescricaoGeralPortfolioBusiness().ObterDescricaoGeralFormatado(documento);
        }

        string MontarDescricaoGeralParte2(Documento documento)
        {
            return new DocumentoDescricaoGeralPortfolioBusiness().ObterDescricaoGeralFormatadoParte2(documento);
        }


        String MontarHtmlPortfolioObras(Documento documento)
        {

            StringBuilder html = new StringBuilder();
            String htmlPortfolio = String.Empty;
            if (documento.EProposta && documento.PortfolioObras)
            {
                htmlPortfolio = new ModeloPortfolioObrasBusiness().ObterPorModeloDocumento(documento).Texto;
                var DocuPortfolio = new DocumentoPortfolioBusiness().GetDocumentoImagens(documento.IdDocumento);

                if (DocuPortfolio != null)
                {



                    html.Append(htmlPortfolio);
                    html.Append("<h1  style=\"font-family: Montserrat, sans-serif;\">PORTFÓLIO DE OBRAS</h1>");
                    html.Append("<div style=\"font-family: Montserrat, sans-serif;font-stretch: ultra-condensed;letter-spacing: 1px;font-size:15px;text-align:justify; height:120px\">");
                    html.Append(documento.DocumentoDescricaoGeralPortfolio.Descricao);
                    html.Append("</div>");
                    html.Append("<table>");
                    html.Append("<tr>");


                    Int32 Total = 0;

                    foreach (DocumentoPortfolioObras DocImg in DocuPortfolio)
                    {

                        if (Total % 4 == 0 && Total > 0)
                        {

                            html.Append("<tr>");
                            html.Append("<td>");
                            html.Append("&nbsp;<div style=\"page-break-before: always;\"></div>");
                            html.Append("</td>");
                            html.Append("</tr>");


                        }

                        String Url = DocImg.Url.Replace("~", "");
                        Url = Url.Replace("/", @"\");

                        String url = AppDomain.CurrentDomain.BaseDirectory + Url;

                        html.Append("<td><br/><br/>");
                        html.Append("<div style=\"font-family:'MontSerrat', sans-serif;margin-right: 15px;text-align:center;font-size:13px;padding: 5px 10px 35px 5px; height: 280px;width: 310px; background-color: #dfdcda;\">");
                        html.Append("<img src='" + url + "' style=\"height: 270px; width: 310px; padding: 0 0 10px 0; text-align: center;\"/>");
                        html.Append(DocImg.DescricaoImagem);
                        html.Append("</div></td>");


                        Total++;

                        if (Total % 2 == 0)
                        {
                            html.Append("<tr></tr>");
                        }


                    }
                    html.Append("</tr>");
                    html.Append("</table>");
                    html.Append("<div  style=\"page-break-after: always;\"> </div>");



                }

                var docuPortfolioParte2 = new DocumentoPortfolioBusiness().GetDocumentoImagensParte2(documento.IdDocumento);


                if (docuPortfolioParte2 != null)
                {
                    html.Append("<div style=\"font-family: Montserrat, sans-serif;font-stretch: ultra-condensed;letter-spacing: 1px;font-size:15px;text-align:justify; height:120px\">");
                    html.Append(documento.DocumentoDescricaoGeralPortfolioParte2.Descricao);
                    html.Append("</div>");
                    html.Append("<table>");
                    html.Append("<tr>");


                    Int32 Total = 0;

                    foreach (DocumentoPortfolioObras DocImg in docuPortfolioParte2)
                    {

                        if (Total % 4 == 0 && Total > 0)
                        {

                            html.Append("<tr>");
                            html.Append("<td>");
                            html.Append("&nbsp;<div style=\"page-break-before: always;\"></div>");
                            html.Append("</tr>");


                        }

                        String Url = DocImg.Url.Replace("~", "");
                        Url = Url.Replace("/", @"\");

                        String url = AppDomain.CurrentDomain.BaseDirectory + Url;


                        html.Append("<td><br/><br/>");
                        html.Append("<div style=\"font-family:'MontSerrat', sans-serif;margin-right: 15px;text-align:center;font-size:13px;padding: 5px 5px 35px 5px; height: 280px;width: 310px; background-color: #dfdcda;\">");
                        html.Append("<img src='" + url + "' style=\"height: 270px; width: 310px; padding: 0 0 10px 0; text-align: center;\"/>");
                        html.Append(DocImg.DescricaoImagem);
                        html.Append("</div></td>");


                        Total++;

                        if (Total % 2 == 0)
                        {
                            html.Append("<tr></tr>");
                        }

                    }
                    html.Append("</tr>");
                    html.Append("</table>");
                    html.Append("<div  style=\"page-break-after: always;\"> </div>");
                }


            }
            return html.ToString();

        }


        String MontarHtmlHistoriaRohr(Documento documento)
        {
            String FotoSaida1 = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\timeline-new.png";
            String FotoSaida2 = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\timeline-2.png";
            String FotoSaida3 = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\ROHR_CasaBranca_305.jpg";
            String FotoSaida4 = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\ROHR_CasaBranca_554.jpg";
            String FotoSaida5 = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\MAPA_Proposta.png";
            String Divisao = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\divisao-timeline.png";
            StringBuilder html = new StringBuilder();
            String htmlHistoriaRohr = String.Empty;
            if (documento.EProposta && documento.ExibirHistoria)
            {

                htmlHistoriaRohr = new ModeloHistoriaRohrBusiness().ObterPorModeloDocumento(documento).Texto;

                String Diviasao = String.Format("{0}", "<img src='" + Divisao + "' style=\"height: 20px;\" ><br/>");

                htmlHistoriaRohr = htmlHistoriaRohr.Replace("{rohr.historia.divisao}", Diviasao);

                String Saida1 = String.Format("{0}", "<img src='" + FotoSaida1 + "' style=\"font-family:'MontSerrat', sans-serif;height: 290px; width: 269px; padding: 0 0 10px 0; text-align: center;\"> Obra da Basílica de Aparecida (SP)");

                htmlHistoriaRohr = htmlHistoriaRohr.Replace("{rohr.historia.timeline1}", Saida1);

                String Saida2 = String.Format("{0}", "<img src='" + FotoSaida2 + "' style=\"font-family:'MontSerrat', sans-serif;height: 290px; width: 269px; padding: 0 0 10px 0; text-align: center;\">Manutenção do Cristo Redentor (RJ) ");

                htmlHistoriaRohr = htmlHistoriaRohr.Replace("{rohr.historia.timeline2}", Saida2);

                String Saida3 = String.Format("{0}", "<img src='" + FotoSaida3 + "'' style=\"font-family:'MontSerrat', sans-serif;height: 270px; width: 310px; padding: 0 0 10px 0; text-align: center;\">Fábrica da Rohr em Casa Branca/SP");

                htmlHistoriaRohr = htmlHistoriaRohr.Replace("{rohr.historia.casabranca305}", Saida3);

                String Saida4 = String.Format("{0}", "<img src='" + FotoSaida4 + "' style=\"font-family:'MontSerrat', sans-serif;height: 270px; width: 310px; padding: 0 0 10px 0; text-align: center;\">Depósito da Rohr em Casa Branca/SP");

                htmlHistoriaRohr = htmlHistoriaRohr.Replace("{rohr.historia.casabranca554}", Saida4);

                String Saida5 = String.Format("{0}", "<img src='" + FotoSaida5 + "' style=\"height: 200px; width:200px;\">");

                htmlHistoriaRohr = htmlHistoriaRohr.Replace("{rohr.historia.mapa}", Saida5);
            }
            html.Append(htmlHistoriaRohr);
            html.Append("</body>");
            return html.ToString();
        }
        String MontarHtmlCondicoesGerais(Documento documento)
        {
            StringBuilder html = new StringBuilder();
            String htmlCondicoesGerais = String.Empty;
            if (!documento.Eminuta && documento.Modelo.CondicoesGeraisObrigatorio && ((documento.EProposta && !new DocumentoBusiness().VerificarPropostaCancelada(documento.NumeroDocumento)) || !documento.EProposta))
            {
                htmlCondicoesGerais = new ModeloCondicoesGeraisBusiness().ObterPorModeloDocumento(documento).Texto;
                String saida = String.Format("{0}, {1} de {2} de {3}",

                    new Company().ObterFilial(
                        new CompanyAddressesContact().ObterCompanyAddressesContactsById(
                        new Specifications().ObterSpecificationById(documento.DocumentoComercial.CodigoSistemaOrigem).ListItemId).CompanyId).CompanyName

                    , documento.DataParaExibicao.ToString("dd")
                    , documento.DataParaExibicao.ToString("MMMM")
                    , documento.DataParaExibicao.ToString("yyyy"));
                htmlCondicoesGerais = htmlCondicoesGerais.Replace("{rohr.documento.data}", saida);
            }
            else if (!documento.EProposta && (!_carimboAprovacaoJuridico) && documento.Modelo.CondicoesGeraisObrigatorio)
            {
                htmlCondicoesGerais = new ModeloCondicoesGeraisBusiness().ObterPorModeloDocumento(documento).Texto;
                String saida = String.Format("{0}, {1} de {2} de {3}",

                    new Company().ObterFilial(
                        new CompanyAddressesContact().ObterCompanyAddressesContactsById(
                        new Specifications().ObterSpecificationById(documento.DocumentoComercial.CodigoSistemaOrigem).ListItemId).CompanyId).CompanyName

                    , documento.DataParaExibicao.ToString("dd")
                    , documento.DataParaExibicao.ToString("MMMM")
                    , documento.DataParaExibicao.ToString("yyyy"));
                htmlCondicoesGerais = htmlCondicoesGerais.Replace("{rohr.documento.data}", saida);
            }

            html.Append(htmlCondicoesGerais);
            html.Append("</body");
            return html.ToString();
        }

        String MontarHtmlObjeto(Documento documento)
        {
            return new DocumentoObjetoBusiness().ObterObjetoFormatado(documento);
        }

        String MontarHtmlDescricaoGeralFotos(Documento documento)
        {
            return new DocumentoDescricaoGeralImagemBusiness().ObterDescricaoGeralFormatado(documento);
        }


        String MontarHtmlResumo(Documento documento)
        {
            return new DocumentoResumoPropostaBusiness().ObterResumoFormatado(documento);
        }

        String MontarHtmlPrecos(Documento documento)
        {
            return new DocumentoObjetoBusiness().ObterPrecoFormatado(documento);
        }
        String GerarDataHoraEmissao(Int32 idUsuario)
        {
            return String.Format("{0}-{1}", Convert.ToInt64(DateTime.Now.Ticks), idUsuario);
        }

        String SubstituirChaves(PartePreenchida partePreenchida, Documento documento)
        {
            StringBuilder htmlFormatado = new StringBuilder();




            htmlFormatado.Append(partePreenchida.Texto);

            var res = documento.Modelo.ListParte.Where(x => x.IdParte == partePreenchida.IdParte);
            if (!res.Any())
                return htmlFormatado.ToString();
            if (res.Single().ListChave == null)
                return htmlFormatado.ToString();
            foreach (Chave chave in documento.Modelo.ListParte.Single(x => x.IdParte == partePreenchida.IdParte).ListChave)
            {
                if (documento.ListChavePreenchida == null)
                    continue;
                Chave chave1 = chave;
                var chavePreenchida = documento.ListChavePreenchida.Where(x => chave1 != null && x.IdChave == chave1.IdChave);

                if (chavePreenchida.Any())
                    htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, chavePreenchida.Single().Texto);

                else switch (chave.IdChave)
                    {
                        case 23:
                            {
                                String objetoDocumento = MontarHtmlObjeto(documento);
                                if (!String.IsNullOrWhiteSpace(objetoDocumento))
                                    htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, objetoDocumento);
                            }
                            break;
                        case 70:
                            htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, String.Format("{0:N2} ({1})", documento.ValorFaturamentoMensal, UtilNumeroExtenso.ObterValorPorExtenso(documento.ValorFaturamentoMensal)));
                            break;
                        case 47:
                            {
                                String precoDocumento = MontarHtmlPrecos(documento);
                                if (!String.IsNullOrWhiteSpace(precoDocumento))
                                    htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, precoDocumento);
                            }
                            break;
                        case 80:
                            {
                                String resumoProposta = MontarHtmlResumo(documento);
                                if (!String.IsNullOrWhiteSpace(resumoProposta))
                                    htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, resumoProposta);

                            }
                            break;
                        case 82:
                            {
                                String FotoComercial = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\FotoComercial\" + documento.DocumentoComercial.PrimeiroNome + "_" + documento.DocumentoComercial.Sobrenome + ".png";

                                htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, String.Format("{0}", "<img src='" + FotoComercial + "' style=\"width: 90px; height: 100px; padding: 5px 5px 5px 5px;\">"));

                            }
                            break;
                        case 78:
                            {
                                Company oCompany = new Company().ObterFilial(
                                    new CompanyAddressesContact().ObterCompanyAddressesContactsById(
                                        new Specifications().ObterSpecificationById(documento.DocumentoComercial.CodigoSistemaOrigem).ListItemId).CompanyId);

                                htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, String.Format("{0}/{1} - {2}", oCompany.Sigla,
                                   ObterSegmentoParaExibicao(documento.Modelo.Segmento.Descricao),
                                   ObterNumeroDocumentoParaExibicao(documento)));

                            }
                            break;

                    }
            }
            return htmlFormatado.ToString();
        }
        String SubstituirChaves(Parte parte, Documento documento)
        {
            StringBuilder htmlFormatado = new StringBuilder();

            if (parte == null) return "";

            htmlFormatado.Append(parte.TextoParte);


            if (parte.ListChave == null) return htmlFormatado.ToString();

            foreach (Chave chave in parte.ListChave)
            {
                if (documento.ListChavePreenchida == null) continue;
                Chave chave1 = chave;
                var chavePreenchida = documento.ListChavePreenchida.Where(x => chave1 != null && x.IdChave == chave1.IdChave);

                if (chavePreenchida.Any())
                    htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, chavePreenchida.Single().Texto);
                else switch (chave.IdChave)
                    {

                        case 23:
                            String objetoDocumento = MontarHtmlObjeto(documento);
                            if (!String.IsNullOrWhiteSpace(objetoDocumento))
                                htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, objetoDocumento);
                            break;
                        case 47:
                            String precoDocumento = MontarHtmlPrecos(documento);
                            if (!String.IsNullOrWhiteSpace(precoDocumento))
                                htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, precoDocumento);
                            break;
                        case 70:
                            htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, String.Format("{0:N2} ({1})", documento.ValorFaturamentoMensal, UtilNumeroExtenso.ObterValorPorExtenso(documento.ValorFaturamentoMensal)));
                            break;
                        case 74:
                            htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, String.Format("{0:N2} ({1})", documento.ValorNegocio, UtilNumeroExtenso.ObterValorPorExtenso(documento.ValorNegocio)));
                            break;
                        case 80:
                            String resumoProposta = MontarHtmlResumo(documento);
                            if (!String.IsNullOrWhiteSpace(resumoProposta))
                                htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, resumoProposta);
                            break;
                        case 82:
                            String FotoComercial = AppDomain.CurrentDomain.BaseDirectory + @"Content\Image\FotoComercial\" + documento.DocumentoComercial.PrimeiroNome + "_" + documento.DocumentoComercial.Sobrenome + ".png";

                            htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, String.Format("{0}", "<img src='" + FotoComercial + "' style=\"width: 90px; height: 100px; padding: 5px 5px 5px 5px;\">"));

                            break;
                        case 78:
                            Company oCompany = new Company().ObterFilial(
                                new CompanyAddressesContact().ObterCompanyAddressesContactsById(
                                    new Specifications().ObterSpecificationById(documento.DocumentoComercial.CodigoSistemaOrigem).ListItemId).CompanyId);

                            htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, String.Format("{0}/{1} - {2}", oCompany.Sigla,
                                ObterSegmentoParaExibicao(documento.Modelo.Segmento.Descricao),
                                ObterNumeroDocumentoParaExibicao(documento)));

                            break;

                        default:
                            htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, String.Empty);
                            break;
                    }
            }

            return htmlFormatado.ToString();
        }
        String TratarVariaveis(Documento documento, Parte parte)
        {
            if (parte == null) return String.Empty;


            if (documento.ListPartePreenchida == null || documento.ListChavePreenchida.Count <= 0)
                return SubstituirChaves(parte, documento);
            var res = documento.ListPartePreenchida.Where(x => x.IdParte == parte.IdParte);
            return res.Any() ? SubstituirChaves(res.Single(), documento) : SubstituirChaves(parte, documento);
        }

        static void FormatarTabela(StringBuilder html)
        {
            html.Replace("<table style=\"width:100%; border=0; cellspacing=0; cellpadding=0;background-color: #f5ad29;color: #FFF;\">", "<table style=\"width:100%;  border=0; cellspacing=0;background-color: #f5ad29; cellpadding=0;font-size:13px;\">");
            html.Replace("<table>", "<table style=\"font-size:13px; border=0; cellspacing=0; cellpadding=0; width:100%\">");
        }

        void BeforeRenderPage(object sender, PdfPageEventArgs e)
        {

            if (_marcaDAguaMinuta)
            {
                PdfTextLayer pdfTextLayer = new PdfTextLayer
                {
                    Font = new PdfFont("Helvetica", 60),
                    NonStrokingColor = Color.FromArgb(168, 168, 168)
                };
                pdfTextLayer.GfxMatrix.Rotate(55);

                PdfTextContent textContent = new PdfTextContent("SEM VALIDADE COMERCIAL")
                {
                    PositionMode = PdfTextPositionMode.Offset,
                    Offset = new PdfPoint(120, 0)
                };

                pdfTextLayer.Contents.Add(textContent);

                e.Page.Contents.Add(pdfTextLayer);
            }

            if (_marcaDAguaCancelado)
            {
                PdfTextLayer pdfTextLayer = new PdfTextLayer
                {
                    Font = new PdfFont("Helvetica", 80),
                    NonStrokingColor = Color.FromArgb(168, 168, 168)
                };
                pdfTextLayer.GfxMatrix.Rotate(45);

                PdfTextContent textContent = new PdfTextContent("C A N C E L A D O")
                {
                    PositionMode = PdfTextPositionMode.Offset,
                    Offset = new PdfPoint(240, 100)
                };

                pdfTextLayer.Contents.Add(textContent);

                e.Page.Contents.Add(pdfTextLayer);
            }



            if (!_carimboAprovacaoJuridico) return;
            string pathImage = AppDomain.CurrentDomain.BaseDirectory + WorkflowBusiness.ObterObterCaminhoAssinatura(_idDocumento, _idUsuario);

            HtmlToPdf.ConvertHtml("<center><img height='110px' width='100px' style='position:absolute; top:87%; left:86%; ' src='" + pathImage + "'></center>", e.Page);
        }
    }
}
