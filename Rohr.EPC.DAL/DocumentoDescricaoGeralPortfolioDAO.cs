using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.DAL
{
    public class DocumentoDescricaoGeralPortfolioDAO
    {
        readonly DbHelper _dbhelper = new DbHelper("epc");

        public void AdicionarDescricaoGeralPortfolio(Documento documento)
        {

            IDbTransaction transacao = _dbhelper.BeginTransacao();

            try
            {
                _dbhelper.CommitTransacao(transacao);

                new DocumentoPortfolioObrasDAO().AdicionarDescrGeral(documento.DocumentoDescricaoGeralPortfolio);

            }
            catch (Exception)
            {
                _dbhelper.RollbackTransacao(transacao);
                throw;
            }
        }

        public void AdicionarDescricaoGeralPortfolioParte2(Documento documento)
        {

            IDbTransaction transacao = _dbhelper.BeginTransacao();

            try
            {
                _dbhelper.CommitTransacao(transacao);

                new DocumentoPortfolioObrasDAO().AdicionarDescrGeralParte2(documento.DocumentoDescricaoGeralPortfolioParte2);

            }
            catch (Exception)
            {
                _dbhelper.RollbackTransacao(transacao);
                throw;
            }
        }
    }
}
