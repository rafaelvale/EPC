using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Entity
{
    public class DocumentoPortfolioObras
    {
        public Int32 Id { get; set; }
        public Int32 IdDocumento { get; set; }
        public Int32 IdImagem { get; set; }
        public String DescricaoImagem { get; set; }
        public String Url { get; set; }
        public Int32 PartePortfolio { get; set; }
        public DateTime? DataInclusao { get; set; }

    }
}
