using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Entity
{
    public class DocumentoImagens
    {
        public Int32 IdDocImagem { get; set; }
        public Int32 IdDocumento { get; set; }
        public Int32 IdImagem { get; set; }
        public String Url { get; set; }
        public String Descricao { get; set; }
        public DateTime? DtInclusao { get; set; }
    }

   

}
