using System;

namespace Rohr.EPC.Entity
{
    public class Perfil
    {
        public Int32 IdPerfil { get; set; }
        public String Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public Boolean ExibirTodosDocumento { get; set; }
    }
}
