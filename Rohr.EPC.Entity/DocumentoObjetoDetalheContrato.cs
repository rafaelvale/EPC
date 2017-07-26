using System;

namespace Rohr.EPC.Entity
{
    public class DocumentoObjetoDetalheContrato
    {
        public Int32 IdDocumentoOjetoDetalheContrato { get; set; }
        public String Descricao { get; set; }
        public Decimal ValorLocacao { get; set; }
        public Decimal ValorIndenizacao { get; set; }
        public String Unidade { get; set; }
        public String Tipo { get; set; }
        public Int32 IdDocumento { get; set; }
        public Boolean Totalizador { get; set; }
        public Int32 IdGrupo { get; set; }
        public String NomeGrupo { get; set; }
        public Decimal Peso { get; set; }
        public Int32 CodigoItemSistemaOrigem { get; set; }
        public String DescricaoResumida { get; set; }
        public Int32 CodigoGrupoItemSistemaOrigem { get; set; }
        public Boolean ExibirVUI { get; set; }
        public Boolean ExibirVUL { get; set; }
    }
}
