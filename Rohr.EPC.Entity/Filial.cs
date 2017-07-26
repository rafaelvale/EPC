using System;

namespace Rohr.EPC.Entity
{
    public class Filial
    {
        public Int32 IdFilial { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int32 CodigoOrigem { get; set; }
        public String Descricao { get; set; }

        public Filial() { }
        public Filial(Int32 idFilial)
        {
            IdFilial = idFilial;
        }
    }
}
