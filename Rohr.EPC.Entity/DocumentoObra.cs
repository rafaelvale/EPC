using System;

namespace Rohr.EPC.Entity
{
    public class DocumentoObra
    {
        public Int32 IdDocumentoObra { get; set; }
        public String Nome { get; set; }
        public String Endereco { get; set; }
        public String Bairro { get; set; }
        public String Cidade { get; set; }
        public String Estado { get; set; }
        public String CEP { get; set; }
        public Int32 IdDocumento { get; set; }
        public Int32 CodigoOrigem { get; set; }
    }
}
