using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class DocumentoObraBusiness
    {
        public DocumentoObra ObterPorIdDocumento(Int32 idDocumento)
        {
            return new DocumentoObraDAO().ObterPorIdDocumento(idDocumento);
        }
        public void Atualizar(Documento documento)
        {
            new DocumentoObraDAO().Atualizar(documento);
        }
    }
}
