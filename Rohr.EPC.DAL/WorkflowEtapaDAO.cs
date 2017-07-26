using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class WorkflowEtapaDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public WorkflowEtapa ObterPorId(Int32 id)
        {
            WorkflowEtapa oWorkflowEtapa = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdWorkflowEtapa", id));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetWorkflowEtapa", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oWorkflowEtapa = new WorkflowEtapa
                    {
                        IdWorkflowEtapa = dataReader.GetInt32(dataReader.GetOrdinal("IdWorkflowEtapa")),
                        Meta = dataReader.GetDecimal(dataReader.GetOrdinal("Meta")),
                        Etapa = dataReader.GetString(dataReader.GetOrdinal("Etapa"))
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oWorkflowEtapa;
            }

        }
    }
}
