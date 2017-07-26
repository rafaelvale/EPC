using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class DocumentoDescricaoGeralImagemBusiness
    {

        public void AdicionarDescricaoGeral(Documento documento)
        {
            new DocumentoDescricaoGeralImagemDAO().AdicionarDescricaoGeralImagem(documento);
        }


        public String ObterDescricaoGeralFormatado(Documento documento)
        {
            return ObterDescricaoGeralImagemContrato(documento);
        }



        String ObterDescricaoGeralImagemContrato(Documento documento)
        {

            StringBuilder html = new StringBuilder();

            html.Append(String.Format("<div style=\"text-align: justify; height:500px;\">{0}</div>", new DocumentoImagensBusiness().ObterUltimoDescricaoGeral(documento.IdDocumento)));


            return html.ToString();
        }

    }
}
