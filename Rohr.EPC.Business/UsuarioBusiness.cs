using System.Web;
using Rohr.EPC.Entity;
using Rohr.EPC.DAL;
using System;
using System.Collections.Generic;

namespace Rohr.EPC.Business
{
    public static class UsuarioBusiness
    {
        public static Usuario ValidarUsuarioSenha(String login, String senha)
        {
            Usuario oUsuario = new UsuarioDAO().ValidarAcesso(login, senha, HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            if (oUsuario == null)
                throw new MyException("Combinação usuário e senha inválidos");

            if (oUsuario.Bloqueado)
                throw new MyException("Usuário bloqueado devido as tentativas inválidas de acesso");

            if (!oUsuario.Ativo)
                throw new MyException("Usuário desativado");

            oUsuario.Filiais = new FilialBusiness().ObterFilialPorIdUsuario(oUsuario.IdUsuario);
            oUsuario.Perfis = new PerfilBusiness().ObterPorIdUsuario(oUsuario.IdUsuario);

            return oUsuario;
        }

        static Boolean ValidarSenhaAtual(String senhaAtual, Usuario usuario)
        {
            return new UsuarioDAO().ValidarSenhaAtual(senhaAtual, usuario.IdUsuario);
        }
        public static void AlterarSenha(String senhaAtual, String novaSenha, String confirmarSenha, Usuario usuario)
        {
            if (String.IsNullOrWhiteSpace(senhaAtual) || String.IsNullOrWhiteSpace(novaSenha) || String.IsNullOrWhiteSpace(confirmarSenha))
                throw new MyException("Todos os campos da seção segurança são obrigatórios");

            if (String.Compare(senhaAtual, novaSenha, StringComparison.Ordinal) == 0)
                throw new MyException("A nova senha não pode ser igual a senha atual");

            if (String.Compare(novaSenha, confirmarSenha, StringComparison.Ordinal) != 0)
                throw new MyException("A nova senha não corresponde com a confirmação");

            if (novaSenha.Length < 6)
                throw new MyException("A nova senha é muito curta");

            if (!ValidarSenhaAtual(senhaAtual, usuario))
                throw new MyException("A senha atual não corresponde com a senha cadastrada");

            new UsuarioDAO().AtualizarSenha(novaSenha, usuario.IdUsuario);
        }
        public static Usuario ObterPorId(Int32 idUsuario)
        {
            Usuario oUsuario = new UsuarioDAO().ObterPorId(idUsuario);
            oUsuario.Filiais = new FilialBusiness().ObterFilialPorIdUsuario(oUsuario.IdUsuario);
            oUsuario.Perfis = new PerfilBusiness().ObterPorIdUsuario(oUsuario.IdUsuario);
            return oUsuario;
        }
        public static void AtualizarEmail(String email, Usuario usuario)
        {
            if (email.Contains("@"))
                throw new MyException("Informe o email sem o @");

            if (email.Contains("rohr"))
                throw new MyException("Informe o email sem o domínio");

            if (email.Contains(".com.br"))
                throw new MyException("Informe o email sem o domínio");

            if (!String.IsNullOrEmpty(email))
                email = String.Format("{0}@rohr.com.br", email.Trim());
            else
                email = String.Empty;


            new UsuarioDAO().AtualizarEmail(email, usuario.IdUsuario);
            new UsuarioDAO().AtualizarExibicaoModal(usuario.IdUsuario);

            usuario.Email = email;
            usuario.ExibirModal = false;
        }
        public static void AtualizarExibicaoModal(Usuario usuario)
        {
            new UsuarioDAO().AtualizarExibicaoModal(usuario.IdUsuario);
            usuario.ExibirModal = false;
        }

        public static List<Usuario> ObterTodos()
        {
            return new UsuarioDAO().ObterTodos();
        }
    }
}
