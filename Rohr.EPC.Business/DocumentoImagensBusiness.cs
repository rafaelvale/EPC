using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class DocumentoImagensBusiness
    {

        public void AdicionarDocumentoImagens(DocumentoImagens DocImg)
        {
            new DocumentoImagensDAO().Adicionar(DocImg);
        }

        public void UpdateDocumentoImagens(DocumentoImagens DocImg)
        {
            new DocumentoImagensDAO().Update(DocImg);
        }
        public void DeleteDocumentoImagens(int IdDocImagem)
        {
            new DocumentoImagensDAO().Delete(IdDocImagem);
        }

        public List<DocumentoImagens> GetDocumentoImagens(Int32 IdDoc)
        {
            return new DocumentoImagensDAO().GetDocumentoImagens(IdDoc);
        }
        
        public void obterUltimaFoto(Int32 IdDocumento)
        {
             new DocumentoImagensDAO().ObterUltimaFoto(IdDocumento);
        }

        public DocumentoDescricaoGeralImagens ObterDescricaoGeralPorIdDocumento(Int32 idDocumento)
        {
            DocumentoDescricaoGeralImagens oDocumentoDescricaoGeral = new DocumentoImagensDAO().ObterDescrGeral(idDocumento);
            return oDocumentoDescricaoGeral ?? new DocumentoDescricaoGeralImagens();
        }

        /// <summary>
        /// Obtem o ultimo Resumo com base na revisão
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        public String ObterUltimoDescricaoGeral(Int32 idDocumento)
        {
            return new DocumentoImagensDAO().ObterUltimoDescricao(idDocumento);
        }
    }
}
