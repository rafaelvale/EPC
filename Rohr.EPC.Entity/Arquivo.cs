using System;

namespace Rohr.EPC.Entity
{
    public class Arquivo
    {
        public Int32 IdArquivo { get; set; }
        public String Nome { get; set; }
        public String Extensao { get; set; }
        public String Tipo { get; set; }
        public Int32 Tamanho { get; set; }
        public Byte[] Conteudo { get; set; }
        public Int32 IdDocumento { get; set; }
    }
}