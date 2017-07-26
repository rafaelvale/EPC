using System;

namespace Rohr.EPC.Entity
{
    public class DocumentoSemNegocio
    {
        public Int32 IdDocumentoSemNegocio { get; set; }
        public String Motivo { get; set; }
        public String Observacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int32 IdDocumento { get; set; }
    }
}
