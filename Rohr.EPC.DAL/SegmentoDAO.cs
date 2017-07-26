using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class SegmentoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public Segmento ObterPorId(Int32 id)
        {
            Segmento oSegmento = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdSegmento", id));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetSegmento", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oSegmento = new Segmento
                    {
                        IdSegmento = dataReader.GetInt32(dataReader.GetOrdinal("idSegmento")),
                        Descricao = dataReader.GetString(dataReader.GetOrdinal("descricao"))
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oSegmento;
            }
        }
    }
}
