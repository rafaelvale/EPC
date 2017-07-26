using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class ArquivoBusiness
    {
        public void AdicionarArquivo(Arquivo arquivo)
        {
            new ArquivoDAO().Adicionar(arquivo);
        }
        public Arquivo ObterArquivoPorIdDocumento(Int32 idDocumento)
        {
            return new ArquivoDAO().ObterPorIdDocumento(idDocumento);
        }
    }
}
