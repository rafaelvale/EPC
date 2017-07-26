using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;

namespace Rohr.EPC.Business
{
    public class AplicacaoListaBusiness
    {
        public List<AplicacaoLista> ObterPorIdChave(Int32 idChave)
        {
            return new AplicacaoListaDAO().ObterPorIdChave(idChave);
        }
    }
}
