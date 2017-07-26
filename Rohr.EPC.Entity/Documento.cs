using System;
using System.Collections.Generic;

namespace Rohr.EPC.Entity
{
    public class Documento
    {
        public Int32 IdDocumento { get; set; }
        public Int32 CodigoSistemaOrigem { get; set; }
        public Int32 NumeroDocumento { get; set; }
        public Int32 VersaoInterna { get; set; }
        public Int32 RevisaoCliente { get; set; }
        public DateTime DataCadastro { get; set; }
        public Decimal PercentualDesconto { get; set; }
        public Decimal ValorFaturamentoMensal { get; set; }
        public Decimal ValorNegocio { get; set; }
        public DateTime DataParaExibicao { get; set; }
        public Boolean Eminuta { get; set; }
        public Boolean EProposta { get; set; }
        public Boolean ExibirHistoria { get; set; }
        public Boolean PortfolioObras { get; set; }


        public Usuario Usuario { get; set; }
        public DocumentoStatus DocumentoStatus { get; set; }
        public DocumentoCliente DocumentoCliente { get; set; }
        public DocumentoComercial DocumentoComercial { get; set; }
        public DocumentoFilial DocumentoFilial { get; set; }
        public List<DocumentoObjeto> ListDocumentoObjeto { get; set; }
        public Filial Filial { get; set; }
        public DocumentoObra DocumentoObra { get; set; }
        public Modelo Modelo { get; set; }
        public List<PartePreenchida> ListPartePreenchida { get; set; }
        public List<ChavePreenchida> ListChavePreenchida { get; set; }
        public DocumentoObjetoObservacao DocumentoObjetoObservacao { get; set; }
        public DocumentoResumoProposta DocumentoResumoProposta { get; set; }
        public DocumentoHistoriaRohr DocumentoHistoriaRohr { get; set; }
        public List<ModeloHistoriaRohr> ListModeloHistoriaRohr { get; set; }
        public List<DocumentoHistoriaRohr> ListDocumentoHistoriaRohr { get; set; }
        public DocumentoDescricaoGeralImagens DocumentoDescricaoGeralFoto { get; set; }
        public DocumentoDescricaoGeralPortfolio DocumentoDescricaoGeralPortfolio { get; set; }
        public DocumentoDescricaoGeralPortfolio DocumentoDescricaoGeralPortfolioParte2 { get; set; }



        public Boolean Edicao { get; set; }


        public Documento()
        {
            Usuario = new Usuario();
            DocumentoStatus = new DocumentoStatus();
            DocumentoCliente = new DocumentoCliente();
            ListDocumentoObjeto = new List<DocumentoObjeto>();
            DocumentoObra = new DocumentoObra();
            Modelo = new Modelo();
            ListPartePreenchida = new List<PartePreenchida>();
            ListChavePreenchida = new List<ChavePreenchida>();
            ListModeloHistoriaRohr = new List<ModeloHistoriaRohr>();
            ListDocumentoHistoriaRohr = new List<DocumentoHistoriaRohr>();
            DocumentoObjetoObservacao = new DocumentoObjetoObservacao();
            DocumentoResumoProposta = new DocumentoResumoProposta();
            DocumentoHistoriaRohr = new DocumentoHistoriaRohr();
            DocumentoDescricaoGeralFoto = new DocumentoDescricaoGeralImagens();
            DocumentoDescricaoGeralPortfolio = new DocumentoDescricaoGeralPortfolio();
            DocumentoDescricaoGeralPortfolioParte2 = new DocumentoDescricaoGeralPortfolio();
        }

        public Decimal PercentualLimpeza { get; set; }
    }
}
