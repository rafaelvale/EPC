using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;

namespace Rohr.EPC.Business
{
    public class PartePreenchidaBusiness
    {
        public List<PartePreenchida> ObterTodasPorIdDocumento(Int32 idDocumento)
        {
            return new PartePreenchidaDAO().ObterTodasPorIdDocumento(idDocumento);
        }
        public void AdicionarPartePreenchida(Documento documento)
        {
            new PartePreenchidaDAO().Adicionar(documento);
        }
    }
}
