using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class ModeloHistoriaRohrBusiness
    {
        public ModeloHistoriaRohr ObterPorId(Int32 idModeloHistoriaRohr)
        {
            return new HistoriaRohrDAO().ObterPorId(idModeloHistoriaRohr);
        }

        public List<ModeloHistoriaRohr> ObterLista()
        {
            return new HistoriaRohrDAO().ObterLista();
        }
        

        public ModeloHistoriaRohr ObterPorModeloDocumento(Documento documento)
        {
            return new HistoriaRohrDAO().ObterPorId(1);

        }
    }
}
