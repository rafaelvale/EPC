using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class DocumentoSemNegocioBusiness
    {
        public DocumentoSemNegocio ObterPorId(Int32 idDocumento)
        {
            return new DocumentoSemNegocioDAO().ObterPorIdDocumento(idDocumento);
        }
    }
}
