using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Entity
{
    public class Imagens
    {
        public Int32 IdImagem { get; set; }
        public String TipoConteudo { get; set; }
        public byte[] Conteudo { get; set; }
        public String NomeOriginal { get; set; }
        public String NomeInterno { get; set; }
        public String DirArquivo { get; set; }
        public String DescrArquivo { get; set; }
        public String Url { get; set; }
        public Int32 LarguraArquivo { get; set; }
        public Int32 AlturaArquivo { get; set; }
        public Boolean FlagAtivo { get; set; }
        public Int32 IdUsuarioUpload { get; set; }
        public String Tag { get; set; }


    }
}
