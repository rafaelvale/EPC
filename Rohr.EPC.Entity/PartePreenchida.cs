using System;

namespace Rohr.EPC.Entity
{
    public class PartePreenchida
    {
        public Int32 IdPartePreenchida { get; set; }
        public String Texto { get; set; }
        public Int32 IdDocumento { get; set; }
        public Int32 IdParte { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
