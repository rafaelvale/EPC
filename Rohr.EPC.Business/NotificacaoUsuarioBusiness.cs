using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class NotificacaoUsuarioBusiness
    {
        public NotificacaoUsuario ObterPorIdUsuario(Int32 idUsuario)
        {
            NotificacaoUsuario oNotificacaoUsuario = new NotificacaoUsuarioDAO().ObterPorIdUsuario(idUsuario);

            if (oNotificacaoUsuario != null)
                return new NotificacaoUsuarioDAO().ObterPorIdUsuario(idUsuario);

            throw new MyException("Não foi possível recuperar o perfil de notificações do usuário");
        }
        public void Atualizar(Usuario usuario, NotificacaoUsuario notificacaoUsuario)
        {
            new NotificacaoUsuarioDAO().Atualizar(usuario, notificacaoUsuario);
        }
    }
}
