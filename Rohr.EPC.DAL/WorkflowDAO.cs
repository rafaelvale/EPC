using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.EPC.DAL
{
    public class WorkflowDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public Workflow ObterAcaoReceberClientePorIdDocumento(Int32 idDocumento)
        {
            Workflow oWorkflow = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("CheckDocumentoEnviadoCliente", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oWorkflow = new Workflow
                    {
                        IdWorkflow = Int32.Parse(dataReader["IdWorkflow"].ToString()),
                        DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                        Justificativa = dataReader["Justificativa"].ToString(),
                        IdUsuario = Int32.Parse(dataReader["IdUsuario"].ToString()),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString()),
                        WorkflowEtapa = new WorkflowEtapa(Int32.Parse(dataReader["IdWorkflowEtapa"].ToString())),
                        WorkflowAcao = new WorkflowAcao(Int32.Parse(dataReader["IdWorkFlowAcao"].ToString())),
                        IdPerfil = Int32.Parse(dataReader["IdPerfil"].ToString()),
                        Meta = Decimal.Parse(dataReader["Meta"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oWorkflow;
            }
        }

        public DataTable ObterEnderecoDne(String cep)
        {
            DbParametros parametrosHistoricoAcoes = new DbParametros();
            parametrosHistoricoAcoes.Adicionar(new DbParametro("@CEP", cep));

            DataTable oDataTable = _dbHelper.ExecutarDataTable("GetEnderecoByCep", parametrosHistoricoAcoes, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return oDataTable;

        }

        public DataTable GetVisitaSemOportunidade(String ComercialResponsavel, int opcao)
        {
            DbParametros parametrosHistoricoAcoes = new DbParametros();
            parametrosHistoricoAcoes.Adicionar(new DbParametro("@Valor", ComercialResponsavel));
            parametrosHistoricoAcoes.Adicionar(new DbParametro("@opcao", opcao));
            DataTable oDataTable = _dbHelper.ExecutarDataTable("GetVisitaSemOportunidade", parametrosHistoricoAcoes, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();
            return oDataTable;

        }
        


        public Workflow ObterUltimaAcaoPorIdDocumento(Int32 idDocumento)
        {
            Workflow oWorkflow = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetWorkflowUltimaAcao", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oWorkflow = new Workflow
                    {
                        IdWorkflow = dataReader.GetInt32(dataReader.GetOrdinal("IdWorkflow")),
                        DataCadastro = dataReader.GetDateTime(dataReader.GetOrdinal("DataCadastro")),
                        IdUsuario = dataReader.GetInt32(dataReader.GetOrdinal("IdUsuario")),
                        IdDocumento = dataReader.GetInt32(dataReader.GetOrdinal("IdDocumento")),
                        WorkflowEtapa = new WorkflowEtapa(dataReader.GetInt32(dataReader.GetOrdinal("IdWorkflowEtapa"))),
                        WorkflowAcao = new WorkflowAcao(dataReader.GetInt32(dataReader.GetOrdinal("IdWorkflowAcao"))),
                        IdPerfil = dataReader.GetInt32(dataReader.GetOrdinal("IdPerfil")),
                        Meta = dataReader.GetDecimal(dataReader.GetOrdinal("Meta"))
                    };
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("Justificativa")))
                        oWorkflow.Justificativa = dataReader.GetString(dataReader.GetOrdinal("Justificativa"));
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oWorkflow;
            }
        }
        public DataTable ObterTodosPorIdDocumento(Int32 idDocumento)
        {
            StringBuilder query = new StringBuilder();

            query.Append("select acoes.dataCadastro, acoes.justificativa, usuarios.primeiroNome, workflow_acoes.descricao, perfis.idPerfil, perfis.descricao descricaoPerfil, acoes.MetaEmMinuto ");
            query.Append("from workflow_acoes_executadas as acoes ");
            query.Append("inner join usuarios on usuarios.idUsuario = acoes.idUsuario ");
            query.Append("inner join documentos on documentos.idDocumento = acoes.idDocumento ");
            query.Append("inner join workflow_acoes on workflow_acoes.idWorkflowAcao = acoes.idWorkflowAcao ");
            query.Append("inner join perfis on perfis.idPerfil = acoes.idPerfil ");
            query.Append("where documentos.eProposta = 1 AND acoes.idDocumento = ? ORDER BY acoes.dataCadastro DESC");

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@value", idDocumento));

            DataTable oDataTable = _dbHelper.ExecutarDataTable(query.ToString(), parametros, CommandType.Text);
            _dbHelper.CloseConnection();

            return oDataTable;
        }
        public DataTable ObterHistoricoAcoes(Int32 idDocumento)
        {
            DbParametros parametrosHistoricoAcoes = new DbParametros();
            parametrosHistoricoAcoes.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            DataTable oDataTable = _dbHelper.ExecutarDataTable("GetHistoricoAcoes", parametrosHistoricoAcoes, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return oDataTable;
        }


        public DataTable GetTempoMedioPropostas()
        {

            DataTable oDataTable = _dbHelper.ExecutarDataTable("GetTempoMedioPropostas", CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return oDataTable;
        }



        public DataTable GetTempoMedioContratos()
        {

            DataTable oDataTable = _dbHelper.ExecutarDataTable("GetTempoMedioContratos", CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return oDataTable;
        }


        public DataTable GetDocumentos()
        {            

            DataTable oDataTable = _dbHelper.ExecutarDataTable("GetDocumentos", CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return oDataTable;
        }



        public DataTable ObterAprovacaoJuridico(Int32 idDocumento)
        {
            DbParametros param = new DbParametros();
            param.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            DataTable oDataTable = _dbHelper.ExecutarDataTable("GetAprovacaoJuridico", param, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return oDataTable;
        }

        public void AdicionarAcao(Workflow workflow, WorkflowAcaoExecutada workflowAcaoExecutada, Boolean atualizarDocumento)
        {
            IDbTransaction transacao = _dbHelper.BeginTransacao();

            try
            {
                DbParametros parametrosWorkflow = new DbParametros();
                parametrosWorkflow.Adicionar(new DbParametro("@IdWorkflowAcao", workflow.WorkflowAcao.IdWorkflowAcao));
                parametrosWorkflow.Adicionar(new DbParametro("@IdUsuario", workflow.IdUsuario));
                parametrosWorkflow.Adicionar(new DbParametro("@IdWorkflowEtapa", workflow.WorkflowEtapa.IdWorkflowEtapa));
                parametrosWorkflow.Adicionar(new DbParametro("@IdDocumento", workflow.IdDocumento));
                parametrosWorkflow.Adicionar(new DbParametro("@Justificativa", workflow.Justificativa));
                parametrosWorkflow.Adicionar(new DbParametro("@IdPerfil", workflow.IdPerfil));
                Int32 idWorkflow = Convert.ToInt32(_dbHelper.ExecutarScalar("AddWorkflow", parametrosWorkflow, transacao, CommandType.StoredProcedure));

                DbParametros parametrosWorkflowAcaoExecutada = new DbParametros();
                parametrosWorkflowAcaoExecutada.Adicionar(new DbParametro("@IdUsuario", workflowAcaoExecutada.IdUsuario));
                parametrosWorkflowAcaoExecutada.Adicionar(new DbParametro("@IdDocumento", workflowAcaoExecutada.IdDocumento));
                parametrosWorkflowAcaoExecutada.Adicionar(new DbParametro("@Justificativa", workflowAcaoExecutada.Justificativa));
                parametrosWorkflowAcaoExecutada.Adicionar(new DbParametro("@IdWorkflowAcao", workflowAcaoExecutada.WorkflowAcao.IdWorkflowAcao));
                parametrosWorkflowAcaoExecutada.Adicionar(new DbParametro("@IdPerfil", workflowAcaoExecutada.IdPerfil));
                parametrosWorkflowAcaoExecutada.Adicionar(new DbParametro("@TempoTotalAcao", workflowAcaoExecutada.TempoTotalAcao));
                parametrosWorkflowAcaoExecutada.Adicionar(new DbParametro("@IdWorkflow", idWorkflow));
                _dbHelper.ExecutarNonQuery("AddWorkflowAcaoExecutada", parametrosWorkflowAcaoExecutada, transacao, CommandType.StoredProcedure);


                DbParametros parametrosEMinutaDocumento = new DbParametros();
                parametrosEMinutaDocumento.Adicionar(new DbParametro("@EMinuta", atualizarDocumento));
                parametrosEMinutaDocumento.Adicionar(new DbParametro("@IdDocumento", workflow.IdDocumento));
                _dbHelper.ExecutarNonQuery("UpdateEMinutaDocumento", parametrosEMinutaDocumento, transacao, CommandType.StoredProcedure);

                DbParametros parametrosWorkflowEtapaExecutada = new DbParametros();

                parametrosWorkflowEtapaExecutada.Adicionar(new DbParametro("@IdWorkflowEtapa", workflow.WorkflowEtapa.IdWorkflowEtapa));
                parametrosWorkflowEtapaExecutada.Adicionar(new DbParametro("@IdDocumento", workflow.IdDocumento));
                parametrosWorkflowEtapaExecutada.Adicionar(new DbParametro("@IdPerfil", workflow.IdPerfil));
                parametrosWorkflowEtapaExecutada.Adicionar(new DbParametro("@IdUsuario", workflow.IdUsuario));
                _dbHelper.ExecutarNonQuery("AddWorkflowEtapaExecutada", parametrosWorkflowEtapaExecutada, transacao, CommandType.StoredProcedure);
                

                _dbHelper.CommitTransacao(transacao);
                _dbHelper.CloseConnection();
            }
            catch (Exception)
            {
                _dbHelper.RollbackTransacao(transacao);
                _dbHelper.CloseConnection();
                throw;
            }
        }
    }
}
