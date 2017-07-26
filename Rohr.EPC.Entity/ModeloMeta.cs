using System;

namespace Rohr.EPC.Entity
{
    public class ModeloMeta
    {
        public Int32 IdModeloMeta { get; set; }
        public Decimal Meta { get; set; }

        public ModeloMeta() { }
        public ModeloMeta(Int32 idModeloMeta)
        {
            IdModeloMeta = idModeloMeta;
        }
    }
}
