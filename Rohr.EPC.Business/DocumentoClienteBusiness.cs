using Rohr.EPC.DAL;
using Rohr.EPC.Entity;

namespace Rohr.EPC.Business
{
    public class DocumentoClienteBusiness
    {
        public void Atualizar(Documento documento)
        {
            new DocumentoClienteDAO().Atualizar(documento);
        }
    }
}
