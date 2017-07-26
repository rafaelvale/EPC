using System.Web;
using Rohr.EPC.Entity;
using System;
using System.Linq;
using System.Text;
using System.Xml;
using Rohr.PMWeb;

namespace Rohr.EPC.Business
{
    public class PlanilhaOrcamentariaBusiness
    {
        Decimal _pesoTotalObjeto;
        Decimal _valorTabelaLocacaoObjeto;
        Decimal _valorTotalComDescontoObjeto;
        Decimal _kgm2TotalObejto;
        Decimal _kgm3TotalObejto;
        Decimal _areaEscoradaFormaObjeto;
        Decimal _volumeObjeto;

        Decimal _pesoTotalOrcamento;
        Decimal _valorTabelaLocacaoOrcamento;
        Decimal _valorTotalComDescontoOrcamento;
        Decimal _kgm2TotalOrcamento;
        Decimal _kgm3TotalOrcamento;
        Decimal _areaEscoradaFormaOrcamento;
        Decimal _volumeOrcamento;

        public String GerarPlanilhaOrcamentaria(Documento documento)
        {
            XmlDocument xmlDocument = new XmlDocument();

            if (documento.ListDocumentoObjeto == null)
                return null;

            xmlDocument.Load(HttpContext.Current.Server.MapPath("~/Content/Modelo_PlanilhaOrcamentaria.xml"));
            string retorno = xmlDocument.InnerXml;

            retorno = retorno.Replace("%conteudo%", (GerarTabelaXmlPlanilhaOrcamentaria(documento)));

            if (documento.EProposta)
            {
                retorno = retorno.Replace("%numeroOportunidade%", documento.RevisaoCliente > 0
                    ? String.Format("{0}-{1}", documento.NumeroDocumento.ToString("N0"), documento.RevisaoCliente)
                    : documento.NumeroDocumento.ToString("N0"));
            }
            else
            {
                Documento documentoProposta = new DocumentoBusiness().ObterPorId(new DocumentoBusiness().ObterIdDocumentoProposta(documento.IdDocumento));
                retorno = retorno.Replace("%numeroOportunidade%", documentoProposta.RevisaoCliente > 0
                    ? String.Format("{0}-{1}", documentoProposta.NumeroDocumento.ToString("N0"), documentoProposta.RevisaoCliente)
                    : documentoProposta.NumeroDocumento.ToString("N0"));
            }


            retorno = retorno.Replace("%versaoOrcamento%", documento.ListDocumentoObjeto[0].RevisaoOrcamentoSistemaOrigem > 0
                ? String.Format("{0:N0}-{1}", documento.ListDocumentoObjeto[0].IdOrcamentoSistemaOrigem, documento.ListDocumentoObjeto[0].RevisaoOrcamentoSistemaOrigem)
                : documento.ListDocumentoObjeto[0].IdOrcamentoSistemaOrigem.ToString());

            retorno = documento.EProposta ? retorno.Replace("%numeroContrato%", "-----") : retorno.Replace("%numeroContrato%", documento.NumeroDocumento.ToString("N0"));
            retorno = documento.EProposta ? retorno.Replace("%tipoDocumento%", "Proposta") : retorno.Replace("%tipoDocumento%", "Contrato");

            String filial = new Company().ObterFilial(
                new CompanyAddressesContact().ObterCompanyAddressesContactsById(
                    new Specifications().ObterSpecificationById(documento.DocumentoComercial.CodigoSistemaOrigem)
                        .ListItemId).CompanyId
                ).CompanyName;
            retorno = retorno.Replace("%filial%", filial);

            retorno = retorno.Replace("%comercial%", documento.DocumentoComercial.PrimeiroNome + " " + documento.DocumentoComercial.Sobrenome);

            retorno = retorno.Replace("%endereco%", documento.DocumentoCliente.Endereco);
            retorno = retorno.Replace("%cidade%", documento.DocumentoCliente.Cidade);
            retorno = retorno.Replace("%cep%", documento.DocumentoCliente.Cep);
            retorno = retorno.Replace("%inscricaoEstadual-RG%", documento.DocumentoCliente.RgIe);
            retorno = retorno.Replace("%cnpj-cpf%", documento.DocumentoCliente.CpfCnpj);
            retorno = retorno.Replace("%nomeRohr%", documento.DocumentoCliente.Nome);
            retorno = retorno.Replace("%cliente%", documento.DocumentoCliente.Nome);

            retorno = retorno.Replace("%obra%", documento.DocumentoObra.Nome);
            retorno = retorno.Replace("%enderecoObra%", documento.DocumentoObra.Endereco);
            retorno = retorno.Replace("%bairroObra%", documento.DocumentoObra.Bairro);
            retorno = retorno.Replace("%cepObra%", documento.DocumentoObra.CEP);
            retorno = retorno.Replace("%cidadeObra%", documento.DocumentoObra.Cidade);
            retorno = retorno.Replace("%estadoObra%", documento.DocumentoObra.Estado);

            retorno = retorno.Replace("%dataHoraGeracao%", documento.DataCadastro.ToString("dd/MM/yyyy hh:mm"));

            retorno = retorno.Replace("%modeloContrato%", documento.Modelo.ModeloTipo.IdModeloTipo == 3 ? documento.Modelo.Titulo : "");

            return retorno;
        }
        String GerarTabelaXmlPlanilhaOrcamentaria(Documento documento)
        {
            StringBuilder stringBuilder = new StringBuilder();

            _pesoTotalOrcamento = 0;
            _valorTabelaLocacaoOrcamento = 0;
            _valorTotalComDescontoOrcamento = 0;
            _kgm2TotalOrcamento = 0;
            _kgm3TotalOrcamento = 0;
            _areaEscoradaFormaOrcamento = 0;
            _volumeOrcamento = 0;

            foreach (DocumentoObjeto objeto in documento.ListDocumentoObjeto)
            {
                _pesoTotalObjeto = 0;
                _valorTabelaLocacaoObjeto = 0;
                _valorTotalComDescontoObjeto = 0;
                _kgm2TotalObejto = 0;
                _kgm3TotalObejto = 0;
                _areaEscoradaFormaObjeto = 0;
                _volumeObjeto = 0;

                _pesoTotalObjeto = objeto.DocumentoObjetoItem.Sum(x => x.Quantidade * x.Peso);
                _valorTabelaLocacaoObjeto = objeto.DocumentoObjetoItem.Sum(x => x.Quantidade * x.ValorTabelaLocacao);
                _valorTotalComDescontoObjeto = objeto.DocumentoObjetoItem.Sum(x => x.Quantidade * x.ValorPraticadoLocacao);

                _areaEscoradaFormaObjeto = objeto.Area;
                _volumeObjeto = objeto.Volume;

                _kgm2TotalObejto = _areaEscoradaFormaObjeto != 0 ? (_pesoTotalObjeto / _areaEscoradaFormaObjeto) : 0;
                _kgm3TotalObejto = _volumeObjeto != 0 ? (_pesoTotalObjeto / _volumeObjeto) : 0;

                _pesoTotalOrcamento += _pesoTotalObjeto;
                _valorTabelaLocacaoOrcamento += _valorTabelaLocacaoObjeto;
                _valorTotalComDescontoOrcamento += _valorTotalComDescontoObjeto;
                _kgm2TotalOrcamento += _kgm2TotalObejto;
                _kgm3TotalOrcamento += _kgm3TotalObejto;
                _areaEscoradaFormaOrcamento += _areaEscoradaFormaObjeto;
                _volumeOrcamento += _volumeObjeto;

                StringBuilder xmlItens = new StringBuilder();

                foreach (DocumentoObjetoItem item in objeto.DocumentoObjetoItem)
                {
                    String xml;
                    Decimal valorLocacaoMensal;
                    MontarCorpoItem(item, out xml, out valorLocacaoMensal);
                    xmlItens.Append(xml);
                }

                stringBuilder.Append("<Row> ");
                stringBuilder.Append(" <Cell ss:MergeAcross=\"17\" ss:StyleID=\"s107\"><Data ss:Type=\"String\"> " + objeto.DescricaoObjeto.Replace("<", "&lt;").Replace(">", "&gt;") + "</Data></Cell> ");
                stringBuilder.Append("</Row> ");
                stringBuilder.Append(MontarLinhaPeriodoObjetoPlanilhaOrcamentaria(objeto.PrevisaoInicio, objeto.PrevisaoTermino, new DocumentoObjetoBusiness().CalcularValorNegocioObjeto(objeto)));
                stringBuilder.Append(MontarLinhaCabecalhoItemPlanilhaOrcamentaria());

                stringBuilder.Append(xmlItens);

                stringBuilder.Append(MontarTotaisObjeto());
            }

            stringBuilder.Append(MontarTotalOrcamento(documento));

            return stringBuilder.ToString();
        }
        void MontarCorpoItem(DocumentoObjetoItem item, out String saidaXml, out Decimal totalLocalMensal)
        {
            Decimal totalComDesconto = item.ValorPraticadoLocacao * item.Quantidade;
            Decimal pesoTotal = item.Peso * item.Quantidade;

            String descricaoCliente = item.DescricaoCliente.Substring(0, 2) == "$$" ? item.DescricaoCliente.Substring(2, item.DescricaoCliente.Length - 2) : item.DescricaoCliente;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<Row> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s75\"><Data ss:Type=\"String\">" + item.DescricaoResumida.Replace("<", "&lt;").Replace(">", "&gt;") + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s75\"><Data ss:Type=\"String\">" + descricaoCliente.Replace("<", "&lt;").Replace(">", "&gt;") + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round(item.Quantidade, 2).ToString().Replace(",", ".") + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s75\"><Data ss:Type=\"String\">" + item.Unidade + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s86\"><Data ss:Type=\"Number\">" + Math.Round(item.Peso, 2).ToString().Replace(",", ".") + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s86\"><Data ss:Type=\"Number\">" + Math.Round(pesoTotal, 2).ToString().Replace(",", ".") + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round(item.ValorTabelaLocacao, 2).ToString().Replace(",", ".") + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round((item.ValorTabelaLocacao * item.Quantidade), 2).ToString().Replace(",", ".") + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round(item.ValorPraticadoLocacao, 2).ToString().Replace(",", ".") + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round((item.ValorPraticadoLocacao * item.Quantidade), 2).ToString().Replace(",", ".") + "</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + (Math.Round(item.Desconto, 2) * -1).ToString().Replace(",", ".") + "</Data></Cell>");

            if (pesoTotal != 0)
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round((totalComDesconto / pesoTotal), 2).ToString().Replace(",", ".") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">0</Data></Cell>");

            if (_areaEscoradaFormaObjeto != 0)
            {
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round((item.Quantidade / _areaEscoradaFormaObjeto), 2).ToString().Replace(",", ".") + "</Data></Cell>");
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round((pesoTotal / _areaEscoradaFormaObjeto), 2).ToString().Replace(",", ".") + "</Data></Cell>");
            }
            else
            {
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">0</Data></Cell>");
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">0</Data></Cell>");
            }

            if (pesoTotal != 0)
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round(((pesoTotal / _pesoTotalObjeto) * 100), 2).ToString().Replace(",", ".") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">0</Data></Cell>");

            if (_volumeObjeto != 0)
            {
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round((item.Quantidade / _volumeObjeto), 2).ToString().Replace(",", ".") + "</Data></Cell>");
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round((pesoTotal / _volumeObjeto), 2).ToString().Replace(",", ".") + "</Data></Cell>");
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">" + Math.Round(((pesoTotal / _volumeObjeto) * 100), 2).ToString().Replace(",", ".") + "</Data></Cell>");
            }
            else
            {
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">0</Data></Cell>");
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">0</Data></Cell>");
                stringBuilder.Append(" <Cell ss:StyleID=\"s76\"><Data ss:Type=\"Number\">0</Data></Cell>");
            }

            stringBuilder.Append("</Row>");

            saidaXml = stringBuilder.ToString();
            totalLocalMensal = totalComDesconto;

            //return stringBuilder.ToString();
        }
        String MontarTotaisObjeto()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<Row> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s81\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s82\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_pesoTotalObjeto, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_valorTabelaLocacaoObjeto, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s79\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_valorTotalComDescontoObjeto, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_kgm2TotalObejto, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_kgm3TotalObejto, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append("</Row> ");

            stringBuilder.Append("<Row> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");
            stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609632\"><ss:Data ss:Type=\"String\"><B><Font html:Color=\"#000000\">Área escorada ou de forma: </Font></B><Font html:Color=\"#000000\">" + _areaEscoradaFormaObjeto.ToString("N2") + "</Font></ss:Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");

            if (_areaEscoradaFormaObjeto != 0)
                stringBuilder.Append(" <Cell ss:StyleID=\"s78\"><Data ss:Type=\"String\">R$/m2: " + (_valorTabelaLocacaoObjeto / _areaEscoradaFormaObjeto).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:StyleID=\"s78\"><Data ss:Type=\"String\">R$/m2: 0,00</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");

            if (_areaEscoradaFormaObjeto != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609612\"><Data ss:Type=\"String\">R$/m2: " + (_valorTotalComDescontoObjeto / _areaEscoradaFormaObjeto).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609612\"><Data ss:Type=\"String\">R$/m2: 0.00</Data></Cell>");
            stringBuilder.Append(InserirCelulasVazias(7));
            stringBuilder.Append("</Row> ");


            stringBuilder.Append("<Row> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");
            stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609632\"><ss:Data ss:Type=\"String\"><B><Font html:Color=\"#000000\">Volume: </Font></B><Font html:Color=\"#000000\">" + _volumeObjeto.ToString("N2") + "</Font></ss:Data></Cell> ");
            stringBuilder.Append(InserirCelulasVazias(4));

            if (_volumeObjeto != 0)
                stringBuilder.Append(" <Cell ss:StyleID=\"s78\"><Data ss:Type=\"String\">R$/m3: " + (_valorTabelaLocacaoObjeto / _volumeObjeto).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:StyleID=\"s78\"><Data ss:Type=\"String\">R$/m3: 0.00</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");

            if (_volumeObjeto != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609612\"><Data ss:Type=\"String\">R$/m3: " + (_valorTotalComDescontoObjeto / _volumeObjeto).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609612\"><Data ss:Type=\"String\">R$/m3: 0.00</Data></Cell>");

            stringBuilder.Append(InserirCelulasVazias(7));

            stringBuilder.Append("</Row> ");

            stringBuilder.Append("<Row>");
            stringBuilder.Append(InserirCelulasVazias(10));

            if (_pesoTotalObjeto != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"2\" ss:StyleID=\"m333609602\"><Data ss:Type=\"String\">R$/Kg Tab: " + (_valorTabelaLocacaoObjeto / _pesoTotalObjeto).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"2\" ss:StyleID=\"m333609602\"><Data ss:Type=\"String\">R$/Kg Tab: 0.00</Data></Cell>");

            stringBuilder.Append(InserirCelulasVazias(5));
            stringBuilder.Append("</Row>");

            stringBuilder.Append("<Row>");
            stringBuilder.Append(InserirCelulasVazias(10));

            if (_pesoTotalObjeto != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"2\" ss:StyleID=\"s100\"><Data ss:Type=\"String\">R$/Kg Desc: " + (_valorTotalComDescontoObjeto / _pesoTotalObjeto).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"2\" ss:StyleID=\"s100\"><Data ss:Type=\"String\">R$/Kg Desc: 0.00</Data></Cell>");

            stringBuilder.Append(InserirCelulasVazias(5));
            stringBuilder.Append("</Row>");

            stringBuilder.Append("<Row>");
            stringBuilder.Append(InserirCelulasVazias(10));

            if (_valorTabelaLocacaoObjeto != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"4\" ss:StyleID=\"s100\"><Data ss:Type=\"String\">Desc ( - ) / Acrésc. ( + ) Médio ( % ): " + (((_valorTotalComDescontoObjeto / _valorTabelaLocacaoObjeto) - 1) * 100).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"4\" ss:StyleID=\"s100\"><Data ss:Type=\"String\">Desc ( - ) / Acrésc. ( + ) Médio ( % ): 0.00</Data></Cell>");

            stringBuilder.Append(InserirCelulasVazias(3));
            stringBuilder.Append("</Row>");

            stringBuilder.Append("<Row>");
            stringBuilder.Append(" <Cell ss:Index=\"2\" ss:StyleID=\"s62\"/>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s62\"/>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s62\"/>");
            stringBuilder.Append("</Row>");

            return stringBuilder.ToString();
        }
        String MontarTotalOrcamento(Documento documento)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<Row>");
            stringBuilder.Append("<Cell ss:MergeAcross=\"17\" ss:StyleID=\"s106\"><Data ss:Type=\"String\">Totais</Data></Cell>");
            stringBuilder.Append("</Row>");

            stringBuilder.Append("<Row> ");
            stringBuilder.Append(InserirCelulasVazias(5));
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_pesoTotalOrcamento, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_valorTabelaLocacaoOrcamento, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s79\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_valorTotalComDescontoOrcamento, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_kgm2TotalOrcamento, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s77\"><Data ss:Type=\"Number\">" + Math.Round(_kgm3TotalOrcamento, 2).ToString().Replace(",", ".") + "</Data></Cell> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/> ");
            stringBuilder.Append("</Row> ");

            stringBuilder.Append("<Row> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");
            stringBuilder.Append(InserirCelulasVazias(6));
            stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"sNegocio\"><ss:Data ss:Type=\"String\"><B><Font html:Color=\"#FF0000\">Previsão de utilização (R$): </Font></B></ss:Data></Cell> ");

            if (documento.ValorNegocio != 0)
                
                stringBuilder.Append(" <Cell ss:StyleID=\"s86Negocio\"><Data ss:Type=\"Number\">" + Math.Round(documento.ValorNegocio, 2).ToString().Replace(",", ".") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:StyleID=\"sNegocio\"><Data ss:Type=\"Number\">0.00</Data></Cell>");

            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");
            stringBuilder.Append(InserirCelulasVazias(6));
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"><Data ss:Type=\"String\"> </Data></Cell>");
            stringBuilder.Append("</Row> ");


            stringBuilder.Append("<Row> ");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");
            stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609632\"><ss:Data ss:Type=\"String\"><B><Font html:Color=\"#000000\">Área escorada ou de forma: </Font></B><Font html:Color=\"#000000\">" + _areaEscoradaFormaOrcamento.ToString("N2") + "</Font></ss:Data></Cell> ");
            stringBuilder.Append(InserirCelulasVazias(4));

            if (_areaEscoradaFormaOrcamento != 0)
                stringBuilder.Append(" <Cell ss:StyleID=\"s78\"><Data ss:Type=\"String\">R$/m2: " + (_valorTabelaLocacaoOrcamento / _areaEscoradaFormaOrcamento).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:StyleID=\"s78\"><Data ss:Type=\"String\">R$/m2: 0.00</Data></Cell>");

            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");

            if (_areaEscoradaFormaOrcamento != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609612\"><Data ss:Type=\"String\">R$/m2: " + (_valorTotalComDescontoOrcamento / _areaEscoradaFormaOrcamento).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609612\"><Data ss:Type=\"String\">R$/m2: 0.00</Data></Cell>");

            stringBuilder.Append(InserirCelulasVazias(6));
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"><Data ss:Type=\"String\"> </Data></Cell>");
            stringBuilder.Append("</Row> ");

            stringBuilder.Append("<Row>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");
            stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"s83\"><ss:Data ss:Type=\"String\"><B><Font html:Color=\"#000000\">Volume: </Font></B><Font html:Color=\"#000000\">" + _volumeOrcamento.ToString("N2") + "</Font></ss:Data></Cell>");
            stringBuilder.Append(InserirCelulasVazias(4));

            if (_volumeOrcamento != 0)
                stringBuilder.Append(" <Cell ss:StyleID=\"s78\"><Data ss:Type=\"String\">R$/m3: " + (_valorTabelaLocacaoOrcamento / _volumeOrcamento).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:StyleID=\"s78\"><Data ss:Type=\"String\">R$/m3: 0.00</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"/>");

            if (_volumeOrcamento != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609622\"><Data ss:Type=\"String\">R$/m3: " + (_valorTotalComDescontoOrcamento / _volumeOrcamento).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"1\" ss:StyleID=\"m333609622\"><Data ss:Type=\"String\">R$/m3: 0.00</Data></Cell>");
            stringBuilder.Append(InserirCelulasVazias(6));
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"><Data ss:Type=\"String\"> </Data></Cell>");
            stringBuilder.Append("</Row>");

            stringBuilder.Append("<Row>");
            stringBuilder.Append(InserirCelulasVazias(10));
            if (_pesoTotalOrcamento != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"2\" ss:StyleID=\"m333609602\"><Data ss:Type=\"String\">R$/Kg Tab: " + (_valorTabelaLocacaoOrcamento / _pesoTotalOrcamento).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"2\" ss:StyleID=\"m333609602\"><Data ss:Type=\"String\">R$/Kg Tab: 0.00</Data></Cell>");
            stringBuilder.Append(InserirCelulasVazias(4));
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"><Data ss:Type=\"String\"> </Data></Cell>");
            stringBuilder.Append("</Row>");

            stringBuilder.Append("<Row>");
            stringBuilder.Append(InserirCelulasVazias(10));

            if (_pesoTotalOrcamento != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"2\" ss:StyleID=\"s100\"><Data ss:Type=\"String\">R$/Kg Desc: " + (_valorTotalComDescontoOrcamento / _pesoTotalOrcamento).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"2\" ss:StyleID=\"s100\"><Data ss:Type=\"String\">R$/Kg Desc: 0.00</Data></Cell>");

            stringBuilder.Append(InserirCelulasVazias(4));
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"><Data ss:Type=\"String\"> </Data></Cell>");
            stringBuilder.Append("</Row>");

            stringBuilder.Append("<Row>");
            stringBuilder.Append(InserirCelulasVazias(10));

            if (_valorTabelaLocacaoOrcamento != 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"4\" ss:StyleID=\"s100\"><Data ss:Type=\"String\">Desc ( - ) / Acrésc. ( + ) Médio ( % ): " + (((_valorTotalComDescontoOrcamento / _valorTabelaLocacaoOrcamento) - 1) * 100).ToString("N2") + "</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"4\" ss:StyleID=\"s100\"><Data ss:Type=\"String\">Desc ( - ) / Acrésc. ( + ) Médio ( % ): 0.00</Data></Cell>");

            stringBuilder.Append(InserirCelulasVazias(2));
            stringBuilder.Append(" <Cell ss:StyleID=\"s74\"><Data ss:Type=\"String\"> </Data></Cell>");
            stringBuilder.Append("</Row>");

            return stringBuilder.ToString();
        }

        static String InserirCelulasVazias(Int32 quantidade)
        {
            StringBuilder oStringBuilder = new StringBuilder();

            for (int i = 0; i < quantidade; i++)
                oStringBuilder.Append("<Cell ss:StyleID=\"s74\"/>");

            return oStringBuilder.ToString();
        }
        static String MontarLinhaCabecalhoItemPlanilhaOrcamentaria()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<Row ss:Height=\"38.25\">");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Nome</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Descrição</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Quant</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Unid</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Peso Unit</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Peso Total</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">VUL Tab Unit (R$)</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">VUL Tab Total (R$)</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">VUL Desc (R$)</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Total C/Desc (R$)</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Desc VUL (%)</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">R$/KG Desc (R$)</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Pç/m2</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Kg/m2</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Kg/m2 (%)</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Pç/m3</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Kg/m3</Data></Cell>");
            stringBuilder.Append(" <Cell ss:StyleID=\"s85\"><Data ss:Type=\"String\">Kg/m3 (%)</Data></Cell>");
            stringBuilder.Append("</Row>");

            return stringBuilder.ToString();
        }
        static String MontarLinhaPeriodoObjetoPlanilhaOrcamentaria(DateTime previsaoInicio, DateTime previsaoTermino, Decimal valorNegocio)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string previsaoUtilizacao;

            if (previsaoInicio.Year == 1 && previsaoTermino.Year == 1)
                previsaoUtilizacao = "Não foi informado";
            else
                previsaoUtilizacao = (previsaoTermino.Subtract(previsaoInicio).Days + 1).ToString();


            stringBuilder.Append("<Row> ");

            if (previsaoInicio.Year == 1 && previsaoTermino.Year == 1)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"3\" ss:StyleID=\"sPeriodo\"><Data ss:Type=\"String\">Previsão de utilização:</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"3\" ss:StyleID=\"sPeriodo\"><Data ss:Type=\"String\">Previsão de utilização: " + previsaoUtilizacao + " dias </Data></Cell>");

            if (previsaoInicio.Year == 1)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"3\" ss:StyleID=\"sPeriodo\"><Data ss:Type=\"String\">Previsão de Início:</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"3\" ss:StyleID=\"sPeriodo\"><Data ss:Type=\"String\">Previsão de Início: " + previsaoInicio.ToString("dd/MM/yyyy") + "</Data></Cell>");

            if (previsaoTermino.Year == 1)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"3\" ss:StyleID=\"sPeriodo\"><Data ss:Type=\"String\">Previsão de Término:</Data></Cell>");
            else
                stringBuilder.Append(" <Cell ss:MergeAcross=\"3\" ss:StyleID=\"sPeriodo\"><Data ss:Type=\"String\">Previsão de Término: " + previsaoTermino.ToString("dd/MM/yyyy") + "</Data></Cell>");

            if (previsaoUtilizacao == "Não foi informado" || valorNegocio == 0)
                stringBuilder.Append(" <Cell ss:MergeAcross=\"4\" ss:StyleID=\"sNegocio\"><Data ss:Type=\"String\">Negócio (R$):</Data></Cell>");
            else
            {
                stringBuilder.Append(" <Cell ss:MergeAcross=\"4\" ss:StyleID=\"sNegocio\"><Data ss:Type=\"String\">Negócio (R$): " + valorNegocio.ToString("N2") + "</Data></Cell>");
            }

            stringBuilder.Append("</Row>");

            return stringBuilder.ToString();
        }
    }
}