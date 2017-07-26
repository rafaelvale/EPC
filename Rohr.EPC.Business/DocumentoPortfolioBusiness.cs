using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class DocumentoPortfolioBusiness
    {

        public void AdicionarDocumentoPortfolio(DocumentoPortfolioObras DocImg)
        {
            new DocumentoPortfolioObrasDAO().Adicionar(DocImg);
        }
        public void AdicionarDocumentoPortfolioParte2(DocumentoPortfolioObras DocImg)
        {
            new DocumentoPortfolioObrasDAO().AdicionarParte2(DocImg);
        }

        public void UpdateDocumentoPortfolio(DocumentoPortfolioObras DocImg)
        {
            new DocumentoPortfolioObrasDAO().Update(DocImg);
        }
        public void UpdateDocumentoPortfolioParte2(DocumentoPortfolioObras DocImg)
        {
            new DocumentoPortfolioObrasDAO().UpdateParte2(DocImg);
        }
        public void DeleteDocumentoPortfolio(int IdDocImagem)
        {
            new DocumentoPortfolioObrasDAO().Delete(IdDocImagem);
        }
        public void DeleteDocumentoPortfolioParte2(int IdDocImagem)
        {
            new DocumentoPortfolioObrasDAO().DeleteParte2(IdDocImagem);
        }

        public List<DocumentoPortfolioObras> GetDocumentoImagens(Int32 IdDoc)
        {
            return new DocumentoPortfolioObrasDAO().GetDocumentoPortfolio(IdDoc);
        }
        public List<DocumentoPortfolioObras> GetDocumentoImagensParte2(Int32 IdDoc)
        {
            return new DocumentoPortfolioObrasDAO().GetDocumentoPortfolioParte2(IdDoc);
        }

        public void obterUltimaFoto(Int32 IdDocumento)
        {
            new DocumentoPortfolioObrasDAO().ObterUltimaFoto(IdDocumento);
        }
        public void obterUltimaFotoParte2(Int32 IdDocumento)
        {
            new DocumentoPortfolioObrasDAO().ObterUltimaFotoParte2(IdDocumento);
        }

        public DocumentoDescricaoGeralPortfolio ObterDescricaoGeralPorIdDocumento(Int32 idDocumento)
        {
            DocumentoDescricaoGeralPortfolio oDocumentoDescricaoGeralPortfolio = new DocumentoPortfolioObrasDAO().ObterDescrGeral(idDocumento);
            return oDocumentoDescricaoGeralPortfolio ?? new DocumentoDescricaoGeralPortfolio();
        }
        public DocumentoDescricaoGeralPortfolio ObterDescricaoGeralPorIdDocumentoParte2(Int32 idDocumento)
        {
            DocumentoDescricaoGeralPortfolio oDocumentoDescricaoGeralPortfolioParte2 = new DocumentoPortfolioObrasDAO().ObterDescrGeralParte2(idDocumento);
            return oDocumentoDescricaoGeralPortfolioParte2 ?? new DocumentoDescricaoGeralPortfolio();
        }

        /// <summary>
        /// Obtem o ultimo Resumo com base na revisão
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        public String ObterUltimoDescricaoGeral(Int32 idDocumento)
        {
            return new DocumentoPortfolioObrasDAO().ObterUltimoDescricao(idDocumento);
        }
        public String ObterUltimoDescricaoGeralParte2(Int32 idDocumento)
        {
            return new DocumentoPortfolioObrasDAO().ObterUltimoDescricaoParte2(idDocumento);
        }
    }
}
