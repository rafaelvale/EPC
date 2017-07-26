using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class DocumentTodoList
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 IdDocumentoTodoList { get; set; }
        public String DocNumber { get; set; }
        public String Description { get; set; }
        public Int32 ProjectId { get; set; }
        public Int32 DocStatusId { get; set; }
        public Int32 Revision { get; set; }
        public DateTime DocumentDate { get; set; }
        public Boolean Inactive { get; set; }
        public DateTime CreateDate { get; set; }

        public DocumentTodoList ObterDocumentTodoLists(Int32 docNumber)
        {
            StringBuilder sql = new StringBuilder();


            sql.Append("SELECT b.Id, b.DocNumber, b.Description, b.ProjectId, b.DocStatusId, b.Revision, b.DocumentDate, b.Inactive, b.CreateDate ");
            sql.Append("FROM dbo.Document_TodoLists b ");
            sql.Append(String.Format("WHERE b.Inactive = 0 AND RIGHT(DocNumber, 6) = '{0}' ", docNumber));


            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    IdDocumentoTodoList = Int32.Parse(dataReader["Id"].ToString());
                    DocNumber = dataReader["DocNumber"].ToString();

                    if (dataReader["Description"].ToString() == String.Empty)
                        throw new Exception("Não foi possível recuperar o nome da obra no PMWeb :(");

                    Description = dataReader["Description"].ToString();

                    ProjectId = Int32.Parse(dataReader["ProjectId"].ToString());
                    DocStatusId = Int32.Parse(dataReader["DocStatusId"].ToString());
                    Revision = Int32.Parse(dataReader["Revision"].ToString());
                    DocumentDate = DateTime.Parse(dataReader["DocumentDate"].ToString());
                    Inactive = Boolean.Parse(dataReader["Inactive"].ToString());
                    CreateDate = DateTime.Parse(dataReader["CreateDate"].ToString());
                }
                else
                    throw new Exception("Oportunidade não encontrada ou não está disponível no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }

            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public Boolean ObterStatusDocumentTodoLists(Int32 idDocumentTodoLists)
        {
            Int32 docStatusId = 0;
            Boolean inactive = true;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT a.DocStatusId, a.Inactive ");
            stringBuilder.Append("FROM Document_TodoLists a ");
            stringBuilder.Append(String.Format("WHERE a.Id = {0}", idDocumentTodoLists));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(stringBuilder.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    docStatusId = Int32.Parse(dataReader["DocStatusId"].ToString());
                    inactive = Boolean.Parse(dataReader["Inactive"].ToString());
                }

                dataReader.Close();
                dataReader.Dispose();
            }

            _dbHelperPmweb.CloseConnection();
            return docStatusId == 1 && !inactive;
        }
        public void MarcarProspostaAprovada(Int32 idDocumentTodoLists)
        {
            try
            {
                if (idDocumentTodoLists == 0)
                    throw new ApplicationException("Não foi possível marcar a oportunidade no PMWeb");

                DbParametros parametros = new DbParametros();
                parametros.Adicionar(new DbParametro("@IdDocumentTodoList", idDocumentTodoLists));

                _dbHelperPmweb.ExecutarNonQuery("UPDATE Document_TodoLists SET PhaseId = 1 WHERE Id = @IdDocumentTodoList", parametros);

                _dbHelperPmweb.CloseConnection();
            }
            catch
            {
                _dbHelperPmweb.CloseConnection();
                throw;
            }
        }
    }
}
