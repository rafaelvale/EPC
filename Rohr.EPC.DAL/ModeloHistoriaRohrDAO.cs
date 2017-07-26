using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.DAL
{
    public class ModeloHistoriaRohrDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public void Adicionar(DocumentoHistoriaRohr documentoHistoriaRohr)
        {
            DbParametros parametroDocumentoHistoriaRohr = new DbParametros();
            parametroDocumentoHistoriaRohr.Adicionar(new DbParametro("@DescricaoDocumento", documentoHistoriaRohr.DescricaoDocumento));
            parametroDocumentoHistoriaRohr.Adicionar(new DbParametro("@DataCadastro", DateTime.Now));
            parametroDocumentoHistoriaRohr.Adicionar(new DbParametro("@CodigoSistemaOrigem", documentoHistoriaRohr.CodigoSistemaOrigem));
            parametroDocumentoHistoriaRohr.Adicionar(new DbParametro("@IdDocumento", documentoHistoriaRohr.IdDocumento));
            parametroDocumentoHistoriaRohr.Adicionar(new DbParametro("@IdModeloHistoriaRohr", documentoHistoriaRohr.IdModeloHistoriaRohr));
            parametroDocumentoHistoriaRohr.Adicionar(new DbParametro("@Exibir", 1));

            documentoHistoriaRohr.IdDocumento = Convert.ToInt32(_dbHelper.ExecutarScalar("AddDocumentoHistoriaRohr", parametroDocumentoHistoriaRohr, CommandType.StoredProcedure));

        }
    }
}
