using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Entity
{
    public class ModeloPortfolioObras
    {
        public Int32 IdPortfolio { get; set; }
        public String Descricao { get; set; }
        public String Texto { get; set; }
        public Int32 Versao { get; set; }
        public DateTime? DataCadastro { get; set; }
        public Boolean Ativo { get; set; }
        public DateTime? DataDesativacao { get; set; }
        public String Nome { get; set; }
    }
}
