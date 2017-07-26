using System;
using System.Text;
using Rohr.Data;
using System.Data.SqlClient;
using System.Data;

namespace Rohr.PMWeb
{
    public class Document_TodoListDetails
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public String Description { get; set; }

        public Document_TodoListDetails ObterDocumentTodoListDetails(Int32 idTodoList)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT Id, Description FROM dbo.PMWeb_ListValues WHERE Id = ");
            sql.Append("(SELECT Field1 FROM Document_TodoListDetails WHERE Id =  ");
            sql.Append("(SELECT MAX(Id) FROM dbo.Document_TodoListDetails d WHERE d.Field1 IN (17098,17109) AND d.TodoListId = @value1)) ");

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@value1", idTodoList));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString(), parametros, CommandType.Text)))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    Description = dataReader["Description"].ToString();
                }
                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
    }
}
