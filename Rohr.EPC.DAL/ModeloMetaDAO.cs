using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class ModeloMetaDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public ModeloMeta ObterPorId(Int32 id)
        {
            ModeloMeta oModeloMeta = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModeloMeta", id));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetModeloMeta", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oModeloMeta = new ModeloMeta
                    {
                        IdModeloMeta = dataReader.GetInt32(dataReader.GetOrdinal("IdModeloMeta")),
                        Meta = dataReader.GetDecimal(dataReader.GetOrdinal("Meta"))
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oModeloMeta;
            }
        }
    }
}
