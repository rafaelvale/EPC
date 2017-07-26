using System;

namespace Rohr.EPC.Entity
{
    public class DocumentoFilial
    {
        public Int32 IdDocumentoFilial { get; set; }
        public String NomeUnidade { get; set; }
        public String Cidade { get; set; }
        public String Telefone { get; set; }
        public Int32 IdDocumento { get; set; }
    }
}
