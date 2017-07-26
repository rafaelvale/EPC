using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;

namespace Rohr.EPC.Business
{
    public class ChavePreenchidaBusiness
    {
        public Documento AdicionarChavePreenchida(Documento documento)
        {
            return new ChavePreenchidaDAO().Adicionar(documento);
        }
        public List<ChavePreenchida> ObterTodasPorIdDocumento(Int32 idDocumento)
        {
            return new ChavePreenchidaDAO().ObterTodasPorIdDocumento(idDocumento);
        }
    }
}
