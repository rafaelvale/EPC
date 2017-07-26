using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoStatusDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public DocumentoStatus ObterDocumentoStatusPorId(Int32 idDocumentoStatus)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumentoStatus", idDocumentoStatus));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoStatus", parametros, CommandType.StoredProcedure))
            {
                DocumentoStatus oDocumentoStatus = null;

                if (dataReader.Read())
                {
                    oDocumentoStatus = new DocumentoStatus
                    {
                        IdDocumentoStatus = Int32.Parse(dataReader["IdDocumentoStatus"].ToString()),
                        Descricao = dataReader["Descricao"].ToString()
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoStatus;
            }
        }
    }
}
