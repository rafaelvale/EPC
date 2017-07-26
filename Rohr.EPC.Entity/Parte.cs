using System;
using System.Collections.Generic;

namespace Rohr.EPC.Entity
{
    public class Parte
    {
        public Int32 IdParte { get; set; }
        public String Nome { get; set; }
        public String TextoParte { get; set; }
        public DateTime DataCadastro { get; set; }
        public Boolean PermiteEdicao { get; set; }
        public Int32 OrdemExibicao { get; set; }
        public Boolean Exibir { get; set; }

        public List<Chave> ListChave { get; set; }
    }
}
