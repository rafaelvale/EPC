using System;
using System.Collections.Generic;

namespace Rohr.EPC.Entity
{
    public class DocumentoObjeto
    {
        public Int32 IdDocumentoObjeto { get; set; }
        public String DescricaoObjeto { get; set; }
        public Boolean ExibirDescricaoResumida { get; set; }
        public Boolean ExibirDescricaoCliente { get; set; }
        public Boolean ExibirUnidade { get; set; }
        public Boolean ExibirPeso { get; set; }
        public Boolean ExibirQuantidade { get; set; }
        public Boolean ExibirValorTabelaLocacao { get; set; }
        public Boolean ExibirValorTabelaIndenizacao { get; set; }
        public Boolean ExibirValorPraticadoLocacao { get; set; }
        public Boolean ExibirValorPraticadoIndenizacao { get; set; }
        public Boolean ExibirDesconto { get; set; }
        public Boolean ExibirValorTotalItem { get; set; }
        public Boolean ExibirPesoTotalItem { get; set; }
        public Boolean ExibirSubTotalPeso { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int32 CodigoSistemaOrigem { get; set; }
        public Int32 IdOrcamentoSistemaOrigem { get; set; }
        public Int32 RevisaoOrcamentoSistemaOrigem { get; set; }
        public Int32 IdDocumento { get; set; }
        public Int32 OrdemApresentacao { get; set; }
        public Decimal Volume { get; set; }
        public Decimal Area { get; set; }
        public List<DocumentoObjetoItem> DocumentoObjetoItem { get; set; }
        public Boolean ExibirPrevisaoUtilizacao { get; set; }
        public DateTime PrevisaoInicio { get; set; }
        public DateTime PrevisaoTermino { get; set; }
        public Boolean ExibirValorNegocioObjeto { get; set; }
        public Boolean ExibirFaturamentoMensalObjeto { get; set; }
        public Boolean ExibirValorNegocio { get; set; }
        public Boolean ExibirFaturamentoMensal { get; set; }        

        public DocumentoObjeto()
        {
            DocumentoObjetoItem = new List<DocumentoObjetoItem>();
        }
    }
}
