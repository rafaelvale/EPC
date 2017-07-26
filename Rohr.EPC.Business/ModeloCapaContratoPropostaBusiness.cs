using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class ModeloCapaContratoPropostaBusiness
    {
        public ModeloCapaContratoProposta ObterPorId(Int32 idModelocapa)
        {
            return new ModeloCapaContratoPropostaDAO().ObterPorId(idModelocapa);
        }
        public List<ModeloCapaContratoProposta> ObterLista()
        {
            return new ModeloCapaContratoPropostaDAO().ObterLista();
        }

        public ModeloCapaContratoProposta ObterPorModeloDocumento(Documento documento)
        {
            if (documento.EProposta)
            {
                return new ModeloCapaContratoPropostaDAO().ObterPorId(1);//Proposta
            }
            else
                return new ModeloCapaContratoPropostaDAO().ObterPorId(2); //Contrato

            
        }
    }
}
