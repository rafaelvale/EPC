using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class WorkflowAcaoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public WorkflowAcao Obter(Int32 id)
        {
            WorkflowAcao oWorkflowAcao = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdWorkflowAcao", id));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetWorkflowAcao", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oWorkflowAcao = new WorkflowAcao
                    {
                        IdWorkflowAcao = dataReader.GetInt32(dataReader.GetOrdinal("IdWorkflowAcao")),
                        Descricao = dataReader.GetString(dataReader.GetOrdinal("Descricao")),
                        Meta = dataReader.GetDecimal(dataReader.GetOrdinal("Meta")),
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oWorkflowAcao;
            }

        }
    }
}
