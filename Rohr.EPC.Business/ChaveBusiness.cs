using Rohr.EPC.DAL;
using System.Collections.Generic;
using Rohr.EPC.Entity;

namespace Rohr.EPC.Business
{
    public class ChaveBusiness
    {
        public List<Chave> ObterTodas()
        {
            return new ChaveDAO().ObterTodos();
        }
    }
}
