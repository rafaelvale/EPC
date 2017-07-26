using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class WorkflowAlcadaDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public WorkflowAlcada ObterPorId(Int32 id)
        {
            WorkflowAlcada oWorkflowAlcada = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@Id", id));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetWorkflowAlcada", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oWorkflowAlcada = new WorkflowAlcada
                    {
                        AlcadaPercentualDescontoMedio = Convert.ToDouble(dataReader["alcadaPercentualDescontoMedio"].ToString()),
                        AlcadaValorFaturamentoMensalMinimo = Convert.ToDouble(dataReader["AlcadaValorFaturamentoMensalMinimo"].ToString()),
                        AlcadaValorFaturamentoMensalMaximo = Convert.ToDouble(dataReader["AlcadaValorFaturamentoMensalMaximo"].ToString()),
                        DataCadastro = Convert.ToDateTime(dataReader["dataCadastro"].ToString()),
                        IdWorkflowAlcada = Convert.ToInt32(dataReader["idWorkflowAlcada"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oWorkflowAlcada;
            }
        }
    }
}
