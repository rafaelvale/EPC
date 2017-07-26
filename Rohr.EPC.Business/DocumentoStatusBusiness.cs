using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class DocumentoStatusBusiness
    {
        public DocumentoStatus ObterPorId(Int32 idDocumentoStatus)
        {
            return new DocumentoStatusDAO().ObterDocumentoStatusPorId(idDocumentoStatus);
        }
    }
}
