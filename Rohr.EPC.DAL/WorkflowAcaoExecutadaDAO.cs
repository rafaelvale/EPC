using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.EPC.DAL
{
    public class WorkflowAcaoExecutadaDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public WorkflowAcaoExecutada ObterUltimaAcaoExecudaPorNumeroDocumento(Int32 idDocumento)
        {
            WorkflowAcaoExecutada oWorkflowAcaoExecutada = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@Id", idDocumento));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetWorkflowAcaoExecutada", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oWorkflowAcaoExecutada = new WorkflowAcaoExecutada
                    {
                        IdWorkflowAcaoExecutada = Int32.Parse(dataReader["IdWorkflowAcaoExecutada"].ToString()),
                        IdUsuario = Int32.Parse(dataReader["IdUsuario"].ToString()),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString()),
                        DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                        WorkflowAcao = new WorkflowAcao(Int32.Parse(dataReader["IdWorkflowAcao"].ToString())),
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oWorkflowAcaoExecutada;
            }
        }
        public WorkflowAcaoExecutada ObterUltimaReprovacaoGerenciaPorIdDocumento(Int32 idDocumento)
        {
            WorkflowAcaoExecutada oWorkflowAcaoExecutada = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetWorkflowUltimaAcaoExecutada", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oWorkflowAcaoExecutada = new WorkflowAcaoExecutada
                    {
                        IdWorkflowAcaoExecutada = Int32.Parse(dataReader["IdWorkflowAcaoExecutada"].ToString()),
                        IdUsuario = Int32.Parse(dataReader["IdUsuario"].ToString()),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString()),
                        DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                        WorkflowAcao = new WorkflowAcao(Int32.Parse(dataReader["IdWorkflowAcao"].ToString())),
                        Justificativa = dataReader["Justificativa"].ToString()
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oWorkflowAcaoExecutada;
            }
        }

        public List<WorkflowAcaoExecutada> ObterWorkflowAcaoExecutada(Int32 idWorkflowAcao, DateTime dataInicio, DateTime dataFim, Boolean eProposta)
        {
            List<WorkflowAcaoExecutada> listWorkflowAcaoExecutada = null;

            StringBuilder query = new StringBuilder();
            query.Append("select tempoTotalAcao ");
            query.Append("from workflow_acoes_executadas w ");
            query.Append("inner join documentos d using(idDocumento) ");
            query.Append("where w.idWorkflowAcao in (?) and w.dataCadastro between ? and ? ");
            query.Append("and d.eProposta = ? order by idDocumento;");

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@value1", idWorkflowAcao));
            parametros.Adicionar(new DbParametro("@value2", dataInicio));
            parametros.Adicionar(new DbParametro("@value3", dataFim));
            parametros.Adicionar(new DbParametro("@value4", eProposta));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader(query.ToString(), parametros, CommandType.Text)))
            {
                if (dataReader.HasRows)
                {
                    listWorkflowAcaoExecutada = new List<WorkflowAcaoExecutada>();
                    while (dataReader.Read())
                    {
                        listWorkflowAcaoExecutada.Add(new WorkflowAcaoExecutada
                        {
                            TempoTotalAcao = Int32.Parse(dataReader["tempoTotalAcao"].ToString())
                        });
                    }
                }
                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listWorkflowAcaoExecutada;
            }
        }
    }
}