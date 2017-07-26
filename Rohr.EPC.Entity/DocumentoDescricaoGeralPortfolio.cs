using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Entity
{
    public class DocumentoDescricaoGeralPortfolio
    {
        public Int32 Id { get; set; }
        public Int32 IdDocumento { get; set; }
        public Int32 PartePortfolio { get; set; }
        public String Descricao { get; set; }
    }
}
