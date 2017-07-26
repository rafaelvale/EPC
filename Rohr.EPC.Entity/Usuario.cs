using System;
using System.Collections.Generic;

namespace Rohr.EPC.Entity
{
    public class Usuario
    {
        public Int32 IdUsuario { get; set; }
        public Int32 Matricula { get; set; }
        public String PrimeiroNome { get; set; }
        public String Sobrenome { get; set; }
        public String Login { get; set; }
        public String Senha { get; set; }
        public DateTime DataCadastro { get; set; }
        public Boolean PrimeiroAcesso { get; set; }
        public DateTime? DataUltimoLogin { get; set; }
        public String IP { get; set; }
        public Boolean Ativo { get; set; }
        public DateTime? DataDesativacao { get; set; }
        public DateTime? DataUltimaAlteracaoSenha { get; set; }
        public Int32 TentativasAcessoFalho { get; set; }
        public DateTime? DataUltimoBloqueioSenha { get; set; }
        public Boolean Bloqueado { get; set; }
        public String Email { get; set; }
        public Boolean ExibirModal { get; set; }

        public List<Perfil> Perfis { get; set; }
        public List<Filial> Filiais { get; set; }

        public Usuario() { }
        public Usuario(Int32 idUsuario)
        {
            IdUsuario = idUsuario;
        }
        public Usuario(Int32 idUsuario, String primeiroNome)
        {
            IdUsuario = idUsuario;
            PrimeiroNome = primeiroNome;
        }
    }
}
