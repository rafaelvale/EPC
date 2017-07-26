using System;

namespace Rohr.EPC.Entity
{
    public class ModeloTipo
    {
        public Int32 IdModeloTipo { get; set; }
        public String Descricao { get; set; }

        public ModeloTipo() { }
        public ModeloTipo(Int32 idModeloTipo)
        {
            IdModeloTipo = idModeloTipo;
        }
    }
}
