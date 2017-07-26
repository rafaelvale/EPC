using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class ModeloTipoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public ModeloTipo ObterPorId(Int32 id)
        {
            ModeloTipo oModeloTipo = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModeloTipo", id));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetModeloTipo", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oModeloTipo = new ModeloTipo
                    {
                        IdModeloTipo = dataReader.GetInt32(dataReader.GetOrdinal("IdModeloTipo")),
                        Descricao = dataReader.GetString(dataReader.GetOrdinal("Descricao"))
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oModeloTipo;
            }
        }
    }
}
