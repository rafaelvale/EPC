using System;
using System.Text;

namespace Rohr.EPC.Entity
{
    public class DocumentoObjetoItem
    {
        public Int32 IdDocumentoObjetoDetalheContrato { get; set; }
        public Int32 IdDocumentoObjetoItem { get; set; }
        public Int32 IdDocumentoObjeto { get; set; }
        public Int32 CodigoSistemaOrigem { get; set; }
        public String DescricaoResumida { get; set; }
        public String DescricaoCompleta { get; set; }
        public String DescricaoCliente { get; set; }
        public String Unidade { get; set; }
        public Decimal Peso { get; set; }
        public Decimal Quantidade { get; set; }
        public Decimal ValorTabelaLocacao { get; set; }
        public Decimal ValorTabelaIndenizacao { get; set; }
        public Decimal ValorPraticadoLocacao { get; set; }
        public Decimal ValorPraticadoIndenizacao { get; set; }
        public Decimal Desconto { get; set; }
        public Int32 OrdemApresentacao { get; set; }
        public Boolean Exibir { get; set; }
        public Int32 CodigoTabelaPrecoSistemaOrigem { get; set; }
        public Int32 CodigoItemSistemaOrigem { get; set; }
        public Int32 CodigoGrupoSistemaOrigem { get; set; }
    }
}
