using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Entity
{
    public class DocumentoHistoriaRohr
    {
        public Int32 IdDocumentoHistoria { get; set; }
        public String CodigoSistemaOrigem { get; set; }
        public String DescricaoDocumento { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int32 IdDocumento { get; set; }
        public Int32 IdModeloHistoriaRohr { get; set; }
        public Boolean Exibir { get; set; }



        public List<ModeloHistoriaRohr> ModeloHistoriaRohr { get; set; }

        public DocumentoHistoriaRohr()
        {
            ModeloHistoriaRohr = new List<ModeloHistoriaRohr>();
        }


    }
}
