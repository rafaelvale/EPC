using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class ModeloPortfolioObrasBusiness
    {
        public ModeloPortfolioObras ObterPorId(Int32 idModeloPortfolioObras)
        {
            return new PortfolioObraDAO().ObterPorId(idModeloPortfolioObras);
        }

        public List<ModeloPortfolioObras> ObterLista()
        {
            return new PortfolioObraDAO().ObterLista();
        }


        public ModeloPortfolioObras ObterPorModeloDocumento(Documento documento)
        {
            return new PortfolioObraDAO().ObterPorId(1);

        }
    }
}
