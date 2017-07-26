using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class DocumentoDescricaoGeralPortfolioBusiness
    {

        public void AdicionarDescricaoGeral(Documento documento)
        {
            new DocumentoDescricaoGeralPortfolioDAO().AdicionarDescricaoGeralPortfolio(documento);
            
        }

        public void AdicionarDescricaoGeralParte2(Documento documento)
        {
            new DocumentoDescricaoGeralPortfolioDAO().AdicionarDescricaoGeralPortfolioParte2(documento);

        }

        public String ObterDescricaoGeralFormatado(Documento documento)
        {
            return ObterDescricaoGeralImagemContrato(documento);
        }

        public String ObterDescricaoGeralFormatadoParte2(Documento documento)
        {
            return ObterDescricaoGeralImagemContratoParte2(documento);
        }


        String ObterDescricaoGeralImagemContrato(Documento documento)
        {

            StringBuilder html = new StringBuilder();
            String descricaop1 = String.Format("{0}", new DocumentoPortfolioBusiness().ObterUltimoDescricaoGeral(documento.IdDocumento));
            html = html.Replace("{documento.descricaogeral.portfolio}", descricaop1);
            
            return html.ToString();
        }


        String ObterDescricaoGeralImagemContratoParte2(Documento documento)
        {

            StringBuilder html = new StringBuilder();
            String descricaop2 = String.Format("{0}", new DocumentoPortfolioBusiness().ObterUltimoDescricaoGeralParte2(documento.IdDocumento));
            html = html.Replace("{documento.descricaogeralparte2.portfolio}", descricaop2);

            return html.ToString();
        }
    }
}
