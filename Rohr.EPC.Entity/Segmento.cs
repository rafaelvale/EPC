using System;

namespace Rohr.EPC.Entity
{
    public class Segmento
    {
        public Int32 IdSegmento { get; set; }
        public String Descricao { get; set; }

        public Segmento() { }
        public Segmento(Int32 idSegmento)
        {
            IdSegmento = idSegmento;
        }
    }
}
