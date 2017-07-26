using System;

namespace Rohr.EPC.Entity
{
    public class Chave
    {
        public Int32 IdChave { get; set; }
        public String ChaveDescricao { get; set; }
        public String Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public Boolean PermiteEdicao { get; set; }
        public Boolean Exibir { get; set; }
    }
}
