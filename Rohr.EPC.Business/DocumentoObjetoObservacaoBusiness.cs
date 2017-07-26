using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class DocumentoObjetoObservacaoBusiness
    {
        public DocumentoObjetoObservacao ObterPorIdDocumento(Int32 idDocumento)
        {
            DocumentoObjetoObservacao oDocumentoObjetoObservacao = new DocumentoObjetoObservacaoDAO().Obter(idDocumento);
            return oDocumentoObjetoObservacao ?? new DocumentoObjetoObservacao();
        }
        /// <summary>
        /// Obtem a última observação com base na revisão
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        public String ObterUltimaObservacao(Int32 idDocumento)
        {
            return new DocumentoObjetoObservacaoDAO().ObterUltimaObservacao(idDocumento);
        }
    }
}
