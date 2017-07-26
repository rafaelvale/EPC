using Rohr.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class Estimates
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public Int32 RevisionNumber { get; set; }
        public Double RevisionId { get; set; }
        public DateTime RevisionDate { get; set; }
        public Int32 DocStatusId { get; set; }

        public Estimates ObterEstimatePorDocumentoId(Int32 documentId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT LinkedRecordId as id FROM DocumentAttachments ");
            sql.Append("WHERE ");
            sql.Append(String.Format("DocumentTypeId = 106 AND LinkedRecordTypeId = 2  AND DocumentId = {0}; ", documentId));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                }

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public Estimates ObterEstimate(Int32 idEstimate)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM ");
            sql.Append("(SELECT e.Id, e.RevisionNumber, e.RevisionDate, e.DocStatusId, e.RevisionId, ISNULL(s.Measure, 'False') Measure ");
            sql.Append("	FROM estimates e LEFT JOIN Specifications s on s.EntityId = e.Id  ");
            sql.Append("	and s.EntityTypeId = 2 and s.SpecificationTemplateId = 189 ");
            sql.Append("	WHERE LEFT(RevisionId, CHARINDEX('.', RevisionId)-1) = @value1) AS a ");
            sql.Append("WHERE a.Measure = 'True' ");

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@value1", (Double)idEstimate));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString(), parametros, CommandType.Text)))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    RevisionId = Double.Parse(dataReader["RevisionId"].ToString().Replace('.', ','));
                    RevisionNumber = Int32.Parse(dataReader["RevisionNumber"].ToString());
                    RevisionDate = DateTime.Parse(dataReader["RevisionDate"].ToString());
                    DocStatusId = Int32.Parse(dataReader["DocStatusId"].ToString());
                }
                else
                    throw new Exception("Orçamento não encontrado no PMWeb. Verifique se o orçamento está marcado como valido :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public Estimates ObterEstimateContrato(Int32 idEstimate)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM ");
            sql.Append("(SELECT e.Id, e.RevisionNumber, e.RevisionDate, e.DocStatusId, e.RevisionId, ISNULL(s.Measure, 'False') Measure ");
            sql.Append("	FROM estimates e LEFT JOIN Specifications s on s.EntityId = e.Id  ");
            sql.Append("	and s.EntityTypeId = 2 and s.SpecificationTemplateId = 189 ");
            sql.Append("	WHERE e.Id = @value1) AS a ");
            sql.Append("WHERE a.Measure = 'True' ");

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@value1", (Double)idEstimate));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString(), parametros, CommandType.Text)))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    RevisionId = Double.Parse(dataReader["RevisionId"].ToString().Replace('.', ','));
                    RevisionNumber = Int32.Parse(dataReader["RevisionNumber"].ToString());
                    RevisionDate = DateTime.Parse(dataReader["RevisionDate"].ToString());
                    DocStatusId = Int32.Parse(dataReader["DocStatusId"].ToString());
                }
                else
                    throw new Exception("Orçamento não encontrado no PMWeb. Verifique se o orçamento está marcado como valido :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public void OrcamentoFinalizado(Int32 projectId)
        {
            const string sql = "SELECT dbo.VerificaOrcamentoFinalizado(@ProjectId) Orcamento";
            DbParametros parametros = new DbParametros();
            DbParametro parameter = new DbParametro("@ProjectId", projectId);

            parametros.Adicionar(parameter);

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql, parametros, CommandType.Text)))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    if (Int32.Parse(dataReader["Orcamento"].ToString()) == 0)
                        throw new Exception("O contrato não tem nenhum orçamento finalizado no PMWeb. Finalize o orçamento para continuar :(");
                }
                else
                    throw new Exception("O contrato não tem nenhum orçamento finalizado no PMWeb. Finalize o orçamento para continuar :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();
        }
        public void LiberarOrcamento(Int32 idEstimate, Boolean liberacaoExtraordinaria)
        {
            String sql = "LiberarOrcamentoParaEdicao_BR";
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@estimateId", idEstimate));
            parametros.Adicionar(new DbParametro("@liberacaoExtraordinaria", liberacaoExtraordinaria));

            _dbHelperPmweb.ExecutarNonQuery(sql, parametros, CommandType.StoredProcedure);
            _dbHelperPmweb.CloseConnection();
        }
        //public void AtualizarPercentualDesconto(Int32 commitmentId)
        //{
        //    String sql = "AtualizaPercentualLimpeza_BR";
        //    DbParametros parametros = new DbParametros();
        //    parametros.Adicionar(new DbParametro("@CommitmentId", commitmentId));

        //    _dbHelperPmweb.ExecutarNonQuery(sql, parametros, CommandType.StoredProcedure);
        //    _dbHelperPmweb.CloseConnection();
        //}
    }
}
