using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class DocumentoResumoPropostaBusiness
    {
        public void AdicionarResumoProposta(Documento documento)
        {
            new DocumentoResumoPropostaDAO().AdicionarResumoProposta(documento);
        }

        public String ObterResumoFormatado(Documento documento)
        {
            return ObterResumoContrato(documento);
        }
        


        String ObterResumoContrato(Documento documento)
        {

            StringBuilder html = new StringBuilder();

            html.Append(String.Format("<link href='http://fonts.googleapis.com/css?family=Montserrat:400,700' rel=\"stylesheet\" type=\"text/css\"><div style=\"font-family: Montserrat, sans-serif;font-stretch: ultra-condensed;letter-spacing: 1px;font-size:15px;text-align:justify; height: 450px;\">{0}</div>", new DocumentoResumoBusiness().ObterUltimoResumo(documento.IdDocumento)));


            return html.ToString();
        }

    }
}
