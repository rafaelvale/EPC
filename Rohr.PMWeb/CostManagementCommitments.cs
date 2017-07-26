using Rohr.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class CostManagementCommitments
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public String CommitmentCode { get; set; }
        public Int32 CompanyId { get; set; }
        public Int32 ProjectId { get; set; }
        public Boolean IsActive { get; set; }
        public Int32 EstimatedId { get; set; }
        public DateTime CreateDate { get; set; }

        public CostManagementCommitments ObterContratoPorOportunidade(Int32 documentTodoListId)
        {
            StringBuilder sql = new StringBuilder();
            IDbTransaction transacao = _dbHelperPmweb.BeginTransacao();

            try
            {
                sql.Clear();
                sql.Append(String.Format("SELECT d.LinkedRecordId FROM DocumentAttachments d WHERE d.DocumentId = {0} AND d.DocumentTypeId = 106", documentTodoListId));
                var estimateId = _dbHelperPmweb.ExecutarScalar(sql.ToString(), transacao);

                if (estimateId == null)
                    throw new Exception("Não foi possível encontrar o contrato associado a essa proposta no PMWeb. Verifique se o contrato foi gerado. :(");

                sql.Clear();
                sql.Append("SELECT LinkedRecordId ");
                sql.Append("FROM DocumentAttachments d ");
                sql.Append(String.Format("WHERE d.LinkedRecordId IN ((SELECT Id FROM ESTIMATES WHERE FLOOR(RevisionId) = {0})) ", estimateId));
                sql.Append("	  AND d.DocumentTypeId = 52 ");
                sql.Append("	  AND d.LinkedRecordTypeId = 2 ");
                estimateId = _dbHelperPmweb.ExecutarScalar(sql.ToString(), transacao);

                if(estimateId == null)
                    throw new Exception("Não foi possível encontrar o contrato associado a essa proposta no PMWeb. Verifique se o contrato foi gerado. :(");

                sql.Clear();
                sql.Append("SELECT a.Id, a.CommitmentCode, a.CompanyId as IdFilial, a.ProjectId, a.IsActive, a.EstimateId, ");
                sql.Append("(SELECT ClientId FROM Projects where Id = a.ProjectId) as CompanyId ");
                sql.Append(String.Format("FROM CostManagement_Commitments a WHERE a.IsActive = 1 AND a.EstimateId = {0} ", estimateId));

                using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString(), transacao)))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        Id = Int32.Parse(dataReader["Id"].ToString());
                        CommitmentCode = dataReader["CommitmentCode"].ToString();
                        CompanyId = Int32.Parse(dataReader["CompanyId"].ToString());
                        ProjectId = Int32.Parse(dataReader["ProjectId"].ToString());
                        IsActive = Boolean.Parse(dataReader["IsActive"].ToString());
                        EstimatedId = Int32.Parse(dataReader["EstimateId"].ToString());
                    }
                    else
                        throw new Exception("Não foi possível encontrar o contrato associado a essa proposta no PMWeb. Verifique se o contrato foi gerado. :(");
                }

                _dbHelperPmweb.CommitTransacao(transacao);
                _dbHelperPmweb.CloseConnection();

                return this;
            }
            catch
            {
                _dbHelperPmweb.RollbackTransacao(transacao);
                _dbHelperPmweb.CloseConnection();
                throw;
            }
        }
        public CostManagementCommitments ObterContrato(Int32 idCostManagementCommitments)
        {
            StringBuilder sql = new StringBuilder();
            IDbTransaction transacao = _dbHelperPmweb.BeginTransacao();

            try
            {
                sql.Clear();
                sql.Append("SELECT a.Id, a.CommitmentCode, a.CompanyId, a.ProjectId, a.IsActive, a.EstimateId, a.CreateDate ");
                sql.Append(String.Format("FROM CostManagement_Commitments a WHERE a.IsActive = 1 AND a.id = {0} ", idCostManagementCommitments));

                using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString(), transacao)))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        Id = Int32.Parse(dataReader["Id"].ToString());
                        CommitmentCode = dataReader["CommitmentCode"].ToString();
                        CompanyId = Int32.Parse(dataReader["CompanyId"].ToString());
                        ProjectId = Int32.Parse(dataReader["ProjectId"].ToString());
                        IsActive = Boolean.Parse(dataReader["IsActive"].ToString());
                        EstimatedId = Int32.Parse(dataReader["EstimateId"].ToString());
                        CreateDate = DateTime.Parse(dataReader["CreateDate"].ToString());
                    }
                    else
                        throw new Exception("Não foi possível encontrar o contrato associado a essa proposta no PMWeb :(");
                }

                _dbHelperPmweb.CommitTransacao(transacao);
                _dbHelperPmweb.CloseConnection();

                return this;
            }
            catch
            {
                _dbHelperPmweb.RollbackTransacao(transacao);
                _dbHelperPmweb.CloseConnection();
                throw;
            }
        }
    }
}
