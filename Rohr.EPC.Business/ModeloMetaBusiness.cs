using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class ModeloMetaBusiness
    {
        public ModeloMeta ObterPorId(Int32 idModeloMeta)
        {
            return new ModeloMetaDAO().ObterPorId(idModeloMeta);
        }
    }
}
