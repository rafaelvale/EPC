using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class DocumentoComercialBusiness
    {
        public DocumentoComercial ObterPorIdDocumento(Int32 idDocumento)
        {
            return new DocumentoComercialDAO().ObterPorIdDocumento(idDocumento);
        }
        public void Atualizar(Documento documento)
        {
            new DocumentoComercialDAO().Atualizar(documento);
        }

      
        
    }
}
