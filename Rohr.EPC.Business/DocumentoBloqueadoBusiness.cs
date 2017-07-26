using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class DocumentoBloqueadoBusiness
    {
        public void AdicionarDocumentoBloqueado(List<DocumentoBloqueado> listDocumentoBloqueado)
        {
            foreach (DocumentoBloqueado documentoBloqueado in listDocumentoBloqueado)
            {
                new DocumentoBloqueadoDAO().Adicionar(documentoBloqueado);
            }
        }
        public void RemoverBloqueio(List<Int32> listIdDocumento)
        {
            foreach (Int32 idDocumento in listIdDocumento)
            {
                if (new DocumentoBloqueadoBusiness().ObterBloqueioAtivo(idDocumento) != null)
                {
                    Int32 idUsuario = new DocumentoBloqueadoBusiness().ObterBloqueioAtivo(idDocumento).IdUsuario;
                    if (idUsuario == new Util().GetSessaoUsuario().IdUsuario)
                        new DocumentoBloqueadoDAO().Atualizar(idDocumento);
                    else
                        throw new MyException(String.Format("O documento nº: {0}, está bloqueado. Não é possível executar a ação :(",
                    new DocumentoBusiness().RecuperarDocumento(idDocumento).NumeroDocumento));
                }
            }
        }
        public void RemoverBloqueio(Int32 idDocumento)
        {
            if (new DocumentoBloqueadoBusiness().ObterBloqueioAtivo(idDocumento) != null)
            {
                Int32 idUsuario = new DocumentoBloqueadoBusiness().ObterBloqueioAtivo(idDocumento).IdUsuario;
                if (idUsuario == new Util().GetSessaoUsuario().IdUsuario)
                    new DocumentoBloqueadoDAO().Atualizar(idDocumento);
                else
                    throw new MyException(String.Format("O documento nº: {0}, está bloqueado. Não é possível executar a ação :(",
                new DocumentoBusiness().RecuperarDocumento(idDocumento).NumeroDocumento));
            }
        }
        public DataTable ObterListaBloqueios(Documento documento)
        {
            return new DocumentoBloqueadoDAO().ObterListaBloqueios(documento.IdDocumento);
        }
        public DocumentoBloqueado ObterBloqueioAtivo(Int32 idDocumento)
        {
            return new DocumentoBloqueadoDAO().ObterUltimoBloqueioAtivo(idDocumento);
        }
    }
}
