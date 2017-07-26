using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoSemNegocioDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public DocumentoSemNegocio ObterPorIdDocumento(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoSemNegocioByIdDocumento", parametros, CommandType.StoredProcedure))
            {
                DocumentoSemNegocio oDocumentoSemNegocio = new DocumentoSemNegocio();

                if (dataReader.Read())
                {

                    oDocumentoSemNegocio = new DocumentoSemNegocio
                    {
                        IdDocumentoSemNegocio = Int32.Parse(dataReader["idDocumentoSemNegocio"].ToString()),
                        Motivo = dataReader["Motivo"].ToString(),
                        Observacao = dataReader["Observacao"].ToString(),
                        DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoSemNegocio;
            }
        }
    }
}
