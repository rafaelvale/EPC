using System.Web;
using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rohr.EPC.Business
{
    public class DocumentoObjetoBusiness
    {
        public void AdicionarDocumentoObjeto(Documento documento)
        {
            new DocumentoObjetoDAO().AdicionarDocumentoObjeto(documento);
        }

        public List<DocumentoObjeto> ObterPorIdDocumento(Int32 idDocumento)
        {
            return new DocumentoObjetoDAO().ObterPorIdDocumento(idDocumento);
        }

        public String ObterObjetoFormatado(Documento documento)
        {
            return documento.Modelo.Segmento.IdSegmento == 3 ? ObterObjetoFormatadoPlataforma(documento) : ObterObjetoFormatadoEquipamentos(documento);
        }

        String ObterObjetoFormatadoEquipamentos(Documento documento)
        {
            List<DocumentoObjeto> listDocumentoObjeto = new DocumentoObjetoDAO().ObterPorIdDocumento(documento.IdDocumento);

            StringBuilder htmlObjeto = new StringBuilder();

            if (listDocumentoObjeto == null) return htmlObjeto.ToString();

            htmlObjeto.Append("<style>");
            htmlObjeto.Append(".table {font-family: 'MontSerrat', Sans-serif;");
            htmlObjeto.Append("  font-size: 10px;width: 100%;border-spacing: 0;border-collapse: separate;border-left: 0;border-bottom: 1px solid #f1efee;border-right: 1px solid #f1efee;}");
            htmlObjeto.Append(".table th, .table td {line-height: 17px; text-align: left; vertical-align: top; border-top: 1px solid #f1efee; border-left: 1px solid #f1efee; vertical-align:middle;}");
            htmlObjeto.Append(".r {text-align: right !important;}");
            htmlObjeto.Append(".table th {font-weight: bold; background-color:#f5ad29;}</style>");

            for (int c = 0; c < listDocumentoObjeto.Count; c++)
            {
                var res = listDocumentoObjeto[c].DocumentoObjetoItem.Where(x => x.Exibir);

                Decimal pesoTotalObjeto = 0;

                htmlObjeto.Append(String.Format("<div style='font-family: Helvetica, Geneva, sans-serif;'>{0}</div>", listDocumentoObjeto[c].DescricaoObjeto));

                if (listDocumentoObjeto[c].ExibirPrevisaoUtilizacao)
                    htmlObjeto.Append(String.Format("<div style='font-family: Helvetica, Geneva, sans-serif; padding-top: 10px;'>Previsão de utilização: {0} dias</div>",
                        CalcularPrevisaoUtilizacaoObjeto(listDocumentoObjeto[c].PrevisaoInicio, listDocumentoObjeto[c].PrevisaoTermino)));

                if (res.Any())
                {
                    htmlObjeto.Append("<table class='table'><thead><tr>");
                    htmlObjeto.Append(ObterCabecalhoObjeto(listDocumentoObjeto[0], documento));
                    htmlObjeto.Append("</thead></tr>");
                }

                Boolean materialConsumo = false;
                foreach (DocumentoObjetoItem d in listDocumentoObjeto[c].DocumentoObjetoItem)
                {
                    pesoTotalObjeto += d.Peso * d.Quantidade;

                    if (d.Exibir)
                        htmlObjeto.Append(ObterItensObjeto(listDocumentoObjeto[c], d));

                    if (!materialConsumo && (d.CodigoGrupoSistemaOrigem == 3406 || d.CodigoGrupoSistemaOrigem == 3407 || d.CodigoGrupoSistemaOrigem == 3415))//consumo
                        materialConsumo = true;
                }

                if (res.Any())
                    htmlObjeto.Append("</table>");

                if (documento.ListDocumentoObjeto.Count > 0 && documento.ListDocumentoObjeto[0].ExibirFaturamentoMensalObjeto)
                {
                    if (CalcularFaturamentoMensalObjeto(documento.ListDocumentoObjeto[c]) > 0)
                    {
                        if (documento.Modelo.Segmento.IdSegmento != 3 && documento.Modelo.Segmento.IdSegmento != 6 && documento.Modelo.Segmento.IdSegmento != 2)
                            htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'>Subtotal Locação Mensal: R$ {0:N2}</div>", CalcularFaturamentoMensalObjeto(documento.ListDocumentoObjeto[c])));
                        else if (documento.Modelo.Segmento.IdSegmento == 6 && !materialConsumo)
                        {
                            if (listDocumentoObjeto[c].PrevisaoInicio.Year == 1 && listDocumentoObjeto[c].PrevisaoTermino.Year == 1)
                                htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'>Subtotal Locação Mensal: R$ {0:N2}</div>", CalcularFaturamentoMensalObjeto(documento.ListDocumentoObjeto[c])));
                            else
                            {
                                htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'>Subtotal Locação Diária: R$ {0:N2}</div>", CalcularFaturamentoMensalObjeto(documento.ListDocumentoObjeto[c]) / 30));
                                htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'>Subtotal Locação Mensal: R$ {0:N2}</div>", CalcularFaturamentoMensalObjeto(documento.ListDocumentoObjeto[c])));
                            }
                        }
                        else
                            htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'>Subtotal: R$ {0:N2}</div>", CalcularFaturamentoMensalObjeto(documento.ListDocumentoObjeto[c])));
                    }
                }

                if (documento.ListDocumentoObjeto.Count > 0 && listDocumentoObjeto[c].ExibirValorNegocioObjeto)
                {
                    Decimal valorNegocio = new DocumentoObjetoBusiness().CalcularValorNegocioObjeto(documento.ListDocumentoObjeto[c]);

                    if (valorNegocio > 0)
                        htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'>Subtotal da Locação para o período de utilização: R$ {0:N2}</div>", valorNegocio));
                }

                if (listDocumentoObjeto[c].ExibirSubTotalPeso && CalcularFaturamentoMensalObjeto(documento.ListDocumentoObjeto[c]) > 0)
                    if (documento.Modelo.Segmento.IdSegmento != 2)
                        htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;font-size:9px;'>Subtotal Peso: {0:N4} Kg</div>", pesoTotalObjeto));

                if (c != listDocumentoObjeto.Count)
                    htmlObjeto.Append("<br />");
            }

            String tipoDocumento = documento.Modelo.Segmento.IdSegmento == 2 ? "mão de obra" : "locação";


            //CONFORME CONVERSADO COM TELMA NO DIA 07/07/2017 O VALOR GLOBAL NÃO DEVE APARECER NO TIPO EMPREITADA
            //if (documento.Modelo.Segmento.IdSegmento == 7)
            //{
            //    htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:left;'>Valor Global: R$ {0:N2}</div>", documento.ValorNegocio));
            //    htmlObjeto.Append("<br />");
            //}
            //else
            //{
                if (documento.ListDocumentoObjeto.Count > 0 && documento.ListDocumentoObjeto[0].ExibirFaturamentoMensal)
                    htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'>Valor total da {1} mensal: R$ {0:N2}</div>", documento.ValorFaturamentoMensal, tipoDocumento));

                if (documento.ListDocumentoObjeto.Count > 0 && documento.ListDocumentoObjeto[0].ExibirValorNegocio)
                {
                    htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'>Valor total estimado da {1} para o período: R$ {0:N2}</div>", documento.ValorNegocio, tipoDocumento));
                    htmlObjeto.Append("<br />");

                    if (documento.EProposta)
                        htmlObjeto.Append(String.Format("<div style=\"text-align: justify;\">{0} {1:N2} ({2}), {3}</div>",
                                                        "Para todos os efeitos legais, o valor total estimado da {4} para o período é de R$",
                                                        documento.ValorNegocio,
                                                        UtilNumeroExtenso.ObterValorPorExtenso(documento.ValorNegocio),
                                                        "podendo variar para mais ou para menos em função da(s) quantidade(s) efetivamente executada(s), não representando, portanto, nenhuma garantia de faturamento ou eventual direito ao recebimento integral do PREÇO, mas sim mera estimativa.",
                                                        tipoDocumento));

                    htmlObjeto.Append("<br />");
                }
            //}

            htmlObjeto.Append(String.Format("<div style=\"text-align: justify;\">{0}</div>", new DocumentoObjetoObservacaoBusiness().ObterPorIdDocumento(documento.IdDocumento).Observacao));
            return htmlObjeto.ToString();
        }
        String ObterObjetoFormatadoPlataforma(Documento documento)
        {
            List<DocumentoObjeto> listDocumentoObjeto = new DocumentoObjetoDAO().ObterPorIdDocumento(documento.IdDocumento);

            StringBuilder htmlObjeto = new StringBuilder();
            StringBuilder htmlObjetoEspeficicacaoTecnica = new StringBuilder();

            if (listDocumentoObjeto == null) return htmlObjeto.ToString();

            htmlObjeto.Append("<style>");
            htmlObjeto.Append(".table {font-family: Helvetica;");
            htmlObjeto.Append("  font-size: 10px;width: 100%;border-spacing: 0;border-collapse: separate;border-left: 0;border-bottom: 1px solid #ccc;border-right: 1px solid #ccc;}");
            htmlObjeto.Append(".table th, .table td {line-height: 17px; text-align: left; vertical-align: top; border-top: 1px solid #ccc; border-left: 1px solid #ccc; vertical-align:middle;}");
            htmlObjeto.Append(".r {text-align: right !important;}");
            htmlObjeto.Append(".table th {font-weight: bold; background-color:#f3f3f3; white-space:nowrap;}</style>");

            Decimal valorTotalDocumento = 0;
            for (int c = 0; c < listDocumentoObjeto.Count; c++)
            {
                var res = listDocumentoObjeto[c].DocumentoObjetoItem.Where(x => x.Exibir);
                Decimal pesoTotalObjeto = 0, valorFaturamentoMensal = 0;

                htmlObjeto.Append(String.Format("<div style='font-family: Helvetica, Geneva, sans-serif;'>{0}</div>", listDocumentoObjeto[c].DescricaoObjeto));

                if (res.Any())
                {
                    htmlObjeto.Append("<table class='table'><thead><tr>");
                    htmlObjeto.Append(ObterCabecalhoObjeto(listDocumentoObjeto[0], documento));
                    htmlObjeto.Append("</tr></thead>");

                    htmlObjetoEspeficicacaoTecnica.Append("<br /><table class='table'>");
                }

                foreach (DocumentoObjetoItem d in listDocumentoObjeto[c].DocumentoObjetoItem)
                {
                    pesoTotalObjeto += d.Peso * d.Quantidade;
                    valorFaturamentoMensal += d.ValorPraticadoLocacao * d.Quantidade;

                    if (!d.Exibir) continue;

                    htmlObjeto.Append(ObterItensObjeto(listDocumentoObjeto[c], d));
                    htmlObjetoEspeficicacaoTecnica.Append(ObterItensObjetoPlataforma(d));
                }

                if (!res.Any()) continue;
                htmlObjeto.Append("</table>");
                htmlObjetoEspeficicacaoTecnica.Append("</table>");

                htmlObjeto.Append(htmlObjetoEspeficicacaoTecnica);
                htmlObjetoEspeficicacaoTecnica.Clear();

                if (listDocumentoObjeto[c].ExibirFaturamentoMensalObjeto)
                    htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'>Subtotal Locação Mensal: R$ {0:N2}</div>", valorFaturamentoMensal));
                if (listDocumentoObjeto[c].ExibirSubTotalPeso)
                    if (documento.Modelo.IdModelo != 2)
                        htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;font-size:9px;'>Subtotal Peso: {0:N4} Kg</div>", pesoTotalObjeto));

                if (c != listDocumentoObjeto.Count)
                    htmlObjeto.Append("</br>");

                valorTotalDocumento += valorFaturamentoMensal;
            }

            if (documento.ListDocumentoObjeto.Count > 0 && documento.ListDocumentoObjeto[0].ExibirFaturamentoMensal)
                htmlObjeto.Append(String.Format("<div style='font-family: Helvetica;text-align:right;'><p>Valor Total Locação: R$ {0:N2}</p></div>", valorTotalDocumento));

            htmlObjeto.Append(String.Format("<br /><div style=\"text-align: justify;\">{0}</div>", new DocumentoObjetoObservacaoBusiness().ObterPorIdDocumento(documento.IdDocumento).Observacao));
            return htmlObjeto.ToString();
        }
        public String ObterPrecoFormatado(Documento documento)
        {
            List<DocumentoObjetoDetalheContrato> listDocumentoObjetoDetalheContrato = new DocumentoObjetoDetalheContratoBusiness().Obter(documento.IdDocumento);

            StringBuilder htmlObjeto = new StringBuilder();

            if (listDocumentoObjetoDetalheContrato == null) return htmlObjeto.ToString();
            htmlObjeto.Append("<style>");
            htmlObjeto.Append(".table {font-family: Helvetica;");
            htmlObjeto.Append("  font-size: 10px;width: 100%;border-spacing: 0;border-collapse: separate;border-left: 0;border-bottom: 1px solid #ccc;border-right: 1px solid #ccc;}");
            htmlObjeto.Append(".table th, .table td {line-height: 17px; text-align: left; vertical-align: top; border-top: 1px solid #ccc; border-left: 1px solid #ccc; vertical-align:middle;}");
            htmlObjeto.Append(".r {text-align: right !important;}");
            htmlObjeto.Append(".table th {font-weight: bold; background-color:#f3f3f3; white-space:nowrap;}");
            htmlObjeto.Append(".titulo {font-weight: bold; background-color:#f3f3f3; white-space:nowrap;}</style>");
            htmlObjeto.Append("<table class='table'><thead><tr>");
            htmlObjeto.Append("<th>Descrição</th>");

            if (documento.Modelo.Segmento.IdSegmento != 2)
            {
                htmlObjeto.Append("<th style=\"width: 80px;\">V.U.L.(R$)</th>");
                htmlObjeto.Append("<th  style=\"width: 80px;\">V.U.I.(R$)</th>");
                htmlObjeto.Append("<th  style=\"width: 50px;\">Unidade</th>");
                htmlObjeto.Append("</thead></tr>");
            }
            else
            {
                htmlObjeto.Append("<th  style=\"width: 80px;\">V.U.L. Hora Homem / Normal(R$)</th>");
                //htmlObjeto.Append("<th  style=\"width: 80px;\">V.U.I.(R$)</th>");
                htmlObjeto.Append("<th  style=\"width: 50px;\">Unidade</th>");
                htmlObjeto.Append("</thead></tr>");
            }

            htmlObjeto.Append(ObterSubGrupoPreco(listDocumentoObjetoDetalheContrato, documento));

            htmlObjeto.Append("</table>");
            return htmlObjeto.ToString();
        }

        String ObterSubGrupoPreco(List<DocumentoObjetoDetalheContrato> listDocumentoObjetoDetalheContrato, Documento documento)
        {
            StringBuilder retorno = new StringBuilder();
            foreach (DocumentoObjetoDetalheContrato d in (from c in listDocumentoObjetoDetalheContrato
                                                          where String.Compare(c.Tipo, "subgrupo", StringComparison.OrdinalIgnoreCase) == 0
                                                          && c.Totalizador
                                                          select c))
            {
                retorno.Append("<tr>");
                retorno.Append(String.Format("<td class='titulo' colspan=\"4\">{0}</td>", d.DescricaoResumida));
                retorno.Append("</tr>");
                retorno.Append(ObterItensPreco(d.CodigoItemSistemaOrigem, listDocumentoObjetoDetalheContrato, documento));
            }

            return retorno.ToString();
        }
        String ObterItensPreco(Int32 idGrupo, IEnumerable<DocumentoObjetoDetalheContrato> listDocumentoObjetoDetalheContrato, Documento documento)
        {
            StringBuilder retorno = new StringBuilder();
            foreach (DocumentoObjetoDetalheContrato item in (from c in listDocumentoObjetoDetalheContrato
                                                             where c.Totalizador == false
                                                                   && c.CodigoGrupoItemSistemaOrigem == idGrupo
                                                             select c))
            {
                String descricao = item.Descricao;

                if (item.DescricaoResumida.Length < item.Descricao.Length && item.DescricaoResumida == item.Descricao.Substring(0, item.DescricaoResumida.Length) && !String.IsNullOrWhiteSpace(item.DescricaoResumida) && String.CompareOrdinal(item.DescricaoResumida, "MO") != 0)
                    descricao = item.Descricao.Substring(item.DescricaoResumida.Length + 2, (item.Descricao.Length - item.DescricaoResumida.Length) - 2).Trim();

                retorno.Append("<tr>");
                retorno.Append(String.Format("<td style='white-space: nowrap'>{0}</td>", descricao));

                retorno.Append(item.ExibirVUL
                    ? String.Format("<td class='r'>{0:N2}</td>", item.ValorLocacao)
                    : "<td class='r'>---</td>");

                if (documento.Modelo.Segmento.IdSegmento != 2)
                {
                    retorno.Append(item.ExibirVUI
                        ? String.Format("<td class='r'>{0:N2}</td>", item.ValorIndenizacao)
                        : "<td class='r'></td>");
                }

                retorno.Append(String.Format("<td class='r'>{0}</td>", item.Unidade));
                retorno.Append("</tr>");
            }

            return retorno.ToString();
        }
        String ObterCabecalhoObjeto(DocumentoObjeto documentoObjeto, Documento documento)
        {
            StringBuilder cabecalhoObjeto = new StringBuilder();

            if (documentoObjeto.ExibirDescricaoResumida)
                cabecalhoObjeto.Append("<th style=\"width: 80px;\">Nome</th>");
            if (documentoObjeto.ExibirDescricaoCliente)
                cabecalhoObjeto.Append("<th style=\"width: 200px;\">Descrição</th>");
            if (documentoObjeto.ExibirQuantidade)
                cabecalhoObjeto.Append("<th style=\"width: 100px;\">Quantidade</th>");
            if (documentoObjeto.ExibirUnidade)
                cabecalhoObjeto.Append("<th style=\"width: 20px;\">UM</th>");
            if (documentoObjeto.ExibirPeso)
                cabecalhoObjeto.Append("<th style=\"width: 100px;\">Peso Unit.(Kg)</th>");
            if (documentoObjeto.ExibirValorTabelaLocacao)
                cabecalhoObjeto.Append(documento.Modelo.IdModelo != 2
                    ? "<th style=\"width: 100px;\">V.U.L. (R$)</th>"
                    : "<th style=\"width: 100px;\">V.U.L. Hora Homem / Normal(R$)</th>");
            if (documentoObjeto.ExibirValorTabelaIndenizacao)
                cabecalhoObjeto.Append("<th style=\"width: 100px;\">V.U.I. (R$)</th>");
            if (documentoObjeto.ExibirValorPraticadoLocacao)
                cabecalhoObjeto.Append(documento.Modelo.Segmento.IdSegmento != 2
                    ? "<th style=\"width: 100px; \">V.U.L. Prat. (R$)</th>"
                    : "<th style=\"width: 100px;\">V.U.L. Prat. Hora Homem / Normal(R$)</th>");
            if (documentoObjeto.ExibirValorPraticadoIndenizacao)
                cabecalhoObjeto.Append("<th style=\"width: 100px;\">V.U.I. Prat. (R$)</th>");
            if (documentoObjeto.ExibirDesconto)
                cabecalhoObjeto.Append("<th style=\"width: 60px;\">Desconto(%)</th>");
            if (documentoObjeto.ExibirValorTotalItem)
                cabecalhoObjeto.Append("<th style=\"width: 120px;\">Total(R$)</th>");
            if (documentoObjeto.ExibirPesoTotalItem)
                cabecalhoObjeto.Append("<th style=\"width: 120px;\">Peso Total(kg)</th>");

            return cabecalhoObjeto.ToString();
        }
        String ObterItensObjeto(DocumentoObjeto documentoObjeto, DocumentoObjetoItem documentoObjetoItem)
        {
            StringBuilder itensObjeto = new StringBuilder();

            Decimal valorTotal = documentoObjetoItem.Quantidade * documentoObjetoItem.ValorPraticadoLocacao;
            Decimal pesoTotal = documentoObjetoItem.Quantidade * documentoObjetoItem.Peso;

            itensObjeto.Append("<tr>");
            if (documentoObjeto.ExibirDescricaoResumida)
                itensObjeto.Append(String.Format("<td style='white-space: nowrap'>{0}</td>", documentoObjetoItem.DescricaoResumida));
            if (documentoObjeto.ExibirDescricaoCliente)
            {
                //Trata descrição dos itens.
                StringBuilder sb = new StringBuilder();
                foreach (char c in documentoObjetoItem.DescricaoCliente.Trim())
                {
                    if (!char.IsWhiteSpace(c))
                        sb.Append(c);
                    else
                        sb.Append(" ");
                }

                if (String.Compare(sb.ToString().Substring(0, 2), "$$", StringComparison.Ordinal) == 0)
                {
                    Int32 res;
                    itensObjeto.Append(Int32.TryParse(sb.ToString().Substring(2, 4), out res)
                        ? String.Format("<td>{0}</td>", sb.ToString().Substring(8, sb.Length - 8))
                        : String.Format("<td>{0}</td>", sb.ToString().Substring(2, sb.Length - 2)));
                }
                else if (String.CompareOrdinal(documentoObjetoItem.DescricaoResumida, "MO") == 0)
                    itensObjeto.Append(String.Format("<td>{0}</td>", sb));
                else
                {
                    Int32 tamanhoResumido = documentoObjetoItem.DescricaoResumida.Length;

                    if (sb.ToString().Length > tamanhoResumido)
                    {
                        itensObjeto.Append(
                            String.CompareOrdinal(sb.ToString().Substring(0, tamanhoResumido),
                                documentoObjetoItem.DescricaoResumida) == 0
                                ? String.Format("<td>{0}</td>",
                                    sb.ToString()
                                        .Substring(tamanhoResumido + 3, sb.ToString().Length - (tamanhoResumido + 3)))
                                : String.Format("<td>{0}</td>", sb));
                    }
                    else
                        itensObjeto.Append(String.Format("<td>{0}</td>", sb));
                }

            }
            if (documentoObjeto.ExibirQuantidade)
            {
                itensObjeto.Append(documentoObjetoItem.Unidade == "PC"
                    ? String.Format("<td class='r'>{0:N0}</td>", documentoObjetoItem.Quantidade)
                    : String.Format("<td class='r'>{0:N2}</td>", documentoObjetoItem.Quantidade));
            }
            if (documentoObjeto.ExibirUnidade)
                itensObjeto.Append(String.Format("<td>{0}</td>", documentoObjetoItem.Unidade));
            if (documentoObjeto.ExibirPeso)
                itensObjeto.Append(String.Format("<td class='r'>{0:N4}</td>", documentoObjetoItem.Peso));
            if (documentoObjeto.ExibirValorTabelaLocacao)
                itensObjeto.Append(String.Format("<td class='r'>{0:N2}</td>", documentoObjetoItem.ValorTabelaLocacao));
            if (documentoObjeto.ExibirValorTabelaIndenizacao)
                itensObjeto.Append(String.Format("<td class='r'>{0:N2}</td>", documentoObjetoItem.ValorTabelaIndenizacao));
            if (documentoObjeto.ExibirValorPraticadoLocacao)
                itensObjeto.Append(String.Format("<td class='r'>{0:N2}</td>", documentoObjetoItem.ValorPraticadoLocacao));
            if (documentoObjeto.ExibirValorPraticadoIndenizacao)
                itensObjeto.Append(String.Format("<td class='r'>{0:N2}</td>", documentoObjetoItem.ValorPraticadoIndenizacao));
            if (documentoObjeto.ExibirDesconto)
                itensObjeto.Append(String.Format("<td class='r'>{0:N2}</td>", documentoObjetoItem.Desconto));
            if (documentoObjeto.ExibirValorTotalItem)
                itensObjeto.Append(String.Format("<td class='r'>{0:N2}</td>", valorTotal));
            if (documentoObjeto.ExibirPesoTotalItem)
                itensObjeto.Append(String.Format("<td class='r'>{0:N4}</td>", pesoTotal));

            itensObjeto.Append("</tr>");

            return itensObjeto.ToString();
        }
        String ObterItensObjetoPlataforma(DocumentoObjetoItem documentoObjetoItem)
        {
            if (documentoObjetoItem.CodigoItemSistemaOrigem == 56542) return String.Empty;

            Boolean existeImagem = new File().ExisteImagem(documentoObjetoItem.CodigoItemSistemaOrigem);

            StringBuilder itensObjeto = new StringBuilder();
            itensObjeto.Append("<tr>");

            itensObjeto.Append(existeImagem
                ? String.Format("<th colspan='3'>{0}</th>", documentoObjetoItem.DescricaoCliente)
                : String.Format("<th colspan='2'>{0}</th>", documentoObjetoItem.DescricaoCliente));
            itensObjeto.Append("</tr>");

            Int16 i = 0;

            List<Specifications> listSpecifications = new Specifications().ObterDetalhesPlataformas(documentoObjetoItem.CodigoItemSistemaOrigem).Where(
                specifications => (!String.IsNullOrWhiteSpace(specifications.Measure)
                    && specifications.Measure != "0"
                    && specifications.SpecificationTemplateId != 234)
                 || specifications.SpecificationTemplateId == 234).ToList();
            Int32 totalLinhas = listSpecifications.Count;


            foreach (Specifications specifications in listSpecifications)
            {
                if (specifications.SpecificationTemplateId != 234 &&
                    !String.IsNullOrWhiteSpace(specifications.Measure) && specifications.Measure != "0")
                {
                    itensObjeto.Append("<tr>");
                    itensObjeto.Append(String.Format("<td style='white-space: nowrap'>{0}</td>",
                        specifications.NomeTemplate));
                    itensObjeto.Append(String.Format("<td style='white-space: nowrap'>{0}</td>",
                        specifications.Measure));

                    if (i == 0 && existeImagem)
                        itensObjeto.Append(
                            "<td style=\"text-align:center;\" rowspan=\" " + (totalLinhas - 1) +
                            " \" width=\"255px\"><img src='http://" +
                            HttpContext.Current.Request.Url.Authority + "/getimage.ashx?entityId=" +
                            documentoObjetoItem.CodigoItemSistemaOrigem +
                            "' height=\"265px;\" width=\"250px\"></td>");

                    itensObjeto.Append("</tr>");
                    i++;
                }
                else if (specifications.SpecificationTemplateId == 234 && specifications.Notes != String.Empty)
                {
                    itensObjeto.Append("<tr>");
                    itensObjeto.Append(existeImagem
                        ? String.Format("<td colspan='3'>{0}</td>", specifications.Notes)
                        : String.Format("<td colspan='2'>{0}</td>", specifications.Notes));
                    itensObjeto.Append("</tr>");
                }
            }

            return itensObjeto.ToString();
        }

        public Decimal CalcularValorNegocioObjeto(DocumentoObjeto documentoObjeto)
        {
            if (documentoObjeto.PrevisaoInicio.Year == 1 && documentoObjeto.PrevisaoTermino.Year == 1)
                return 0;

            return documentoObjeto.DocumentoObjetoItem.Sum(documentoObjetoItem =>
                ((documentoObjetoItem.ValorPraticadoLocacao * documentoObjetoItem.Quantidade) / 30) *
                (documentoObjeto.PrevisaoTermino.Subtract(documentoObjeto.PrevisaoInicio).Days + 1));
        }
        public void CalcularValorNegocioTotal(Documento documento)
        {
            documento.ValorNegocio = documento.ListDocumentoObjeto.Sum(documentoObjeto => CalcularValorNegocioObjeto(documentoObjeto));
        }

        public void CalcularFaturamentoMensalTotal(Documento documento)
        {
            documento.ValorFaturamentoMensal = documento.ListDocumentoObjeto == null ? 0 : documento.ListDocumentoObjeto.Sum(t => t.DocumentoObjetoItem.Sum(d => d.ValorPraticadoLocacao * d.Quantidade));
        }
        public Decimal CalcularFaturamentoMensalObjeto(DocumentoObjeto documentoObjeto)
        {
            return documentoObjeto.DocumentoObjetoItem.Sum(documentoObjetoItem => documentoObjetoItem.ValorPraticadoLocacao * documentoObjetoItem.Quantidade);
        }

        public void ObterDescontoMedio(Documento documento)
        {
            Decimal valorTabela = 0;
            Decimal valorPraticado = 0;

            if (documento.ListDocumentoObjeto != null && documento.ListDocumentoObjeto.Count > 0)
            {
                foreach (DocumentoObjeto documentoObjeto in documento.ListDocumentoObjeto)
                {
                    valorTabela += documentoObjeto.DocumentoObjetoItem.Sum(x => x.Quantidade * x.ValorTabelaLocacao);
                    valorPraticado += documentoObjeto.DocumentoObjetoItem.Sum(x => x.Quantidade * x.ValorPraticadoLocacao);
                }
            }

            if (valorTabela != 0)
                documento.PercentualDesconto = Math.Round(((valorPraticado / valorTabela) - 1) * 100, 2);
            else
                documento.PercentualDesconto = (decimal)0.00;
        }
        public Decimal ObterMaiorDesconto(Documento documento)
        {
            Decimal maiorDesconto = 0;

            foreach (DocumentoObjeto documentoObjeto in documento.ListDocumentoObjeto)
            {
                foreach (DocumentoObjetoItem documentoObjetoItem in documentoObjeto.DocumentoObjetoItem)
                {
                    maiorDesconto = documentoObjetoItem.Desconto > maiorDesconto ? documentoObjetoItem.Desconto : maiorDesconto;
                }
            }

            return maiorDesconto * -1;
        }
        public Decimal ObterMenorDesconto(Documento documento)
        {
            Decimal menorDesconto = 100;

            foreach (DocumentoObjeto documentoObjeto in documento.ListDocumentoObjeto)
            {
                foreach (DocumentoObjetoItem documentoObjetoItem in documentoObjeto.DocumentoObjetoItem)
                {
                    menorDesconto = documentoObjetoItem.Desconto < menorDesconto && documentoObjetoItem.Desconto > 0 ? documentoObjetoItem.Desconto : menorDesconto;
                }
            }

            if (menorDesconto == 100)
                menorDesconto = 0;

            return menorDesconto * -1;
        }
        static Int32 CalcularPrevisaoUtilizacaoObjeto(DateTime previsaoInicio, DateTime previsaoTermino)
        {
            if (previsaoInicio.Year != 1 && previsaoTermino.Year != 1)
                return previsaoTermino.Subtract(previsaoInicio).Days + 1;

            return 0;
        }
        public static Int32 CalcularPrevisaoUtilizacaoContrato(Documento documento)
        {
            DateTime menorPrevisaoInicio = DateTime.MaxValue, maiorPrevisaoTermino = DateTime.MinValue;

            if (documento.ListDocumentoObjeto == null)
                return 0;

            foreach (DocumentoObjeto oDocumentoObjeto in documento.ListDocumentoObjeto)
            {
                if (oDocumentoObjeto.PrevisaoInicio < menorPrevisaoInicio)
                    menorPrevisaoInicio = oDocumentoObjeto.PrevisaoInicio;

                if (oDocumentoObjeto.PrevisaoTermino > maiorPrevisaoTermino)
                    maiorPrevisaoTermino = oDocumentoObjeto.PrevisaoTermino;

            }
            return menorPrevisaoInicio.Year != 1 ? maiorPrevisaoTermino.Subtract(menorPrevisaoInicio).Days + 1 : 0;
        }
    }
}