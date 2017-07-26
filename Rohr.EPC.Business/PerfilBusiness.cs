using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;

namespace Rohr.EPC.Business
{
    public class PerfilBusiness
    {
        public List<Perfil> ObterPorIdUsuario(Int32 idUsuario)
        {
            List<Perfil> listPerfil = new PerfilDAO().ObterPorIdUsuario(idUsuario);

            if (listPerfil == null)
                throw new MyException("Não foi possível recuperar o perfil do usuário");

            return listPerfil;
        }
        public Perfil ObterPorId(Int32 idPerfil)
        {
            return new PerfilDAO().ObterPorId(idPerfil);
        }
        public Boolean AtualizarExibirTodosDocumento(Int32 idUsuario, Int32 idPerfil)
        {
            return new PerfilDAO().AtualizarExibirTodosDocumento(idUsuario, idPerfil);
        }
    }
}