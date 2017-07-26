using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class ListValues
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public String Description { get; set; }

        public ListValues ObterCidade(Int32 idCity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(" PMWeb_ListValues.Id, SUBSTRING(PMWeb_ListValues.Description, 0, (LEN(PMWeb_ListValues.Description)-CHARINDEX('-',REVERSE(PMWeb_ListValues.Description)))) as description ");
            sql.Append("FROM PMWeb_ListValues ");
            sql.Append("WHERE ");
            sql.Append(String.Format("PMWeb_ListValues.ListId = 10035 AND PMWeb_ListValues.id = {0} ", idCity));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    Description = dataReader["Description"].ToString();
                }
                else
                    throw new Exception("Não foi possível identificar a cidade do cliente no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
    }
}
