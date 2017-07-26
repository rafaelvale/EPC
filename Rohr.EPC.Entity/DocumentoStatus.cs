using System;

namespace Rohr.EPC.Entity
{
    public class DocumentoStatus
    {
        public Int32 IdDocumentoStatus { get; set; }
        public String Descricao { get; set; }

        public DocumentoStatus(){}

        public DocumentoStatus(Int32 id)
        {
            IdDocumentoStatus = id;
        }
    }
}
