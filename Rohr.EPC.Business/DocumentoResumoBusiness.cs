using Rohr.EPC.Entity;
using Rohr.EPC.DAL;
using System;

namespace Rohr.EPC.Business
{
    public class DocumentoResumoBusiness
    {
        public DocumentoResumoProposta ObterPorIdDocumento(Int32 idDocumento)
        {
            DocumentoResumoProposta oDocumentoResumoProposta = new DocumentoResumoDAO().Obter(idDocumento);
            return oDocumentoResumoProposta ?? new DocumentoResumoProposta();
        }
        /// <summary>
        /// Obtem o ultimo Resumo com base na revisão
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        public String ObterUltimoResumo(Int32 idDocumento)
        {
            return new DocumentoResumoDAO().ObterUltimoResumo(idDocumento);
        }
    }
}
