using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class States
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public String StateKey { get; set; }

        public States(Int32 id)
        {
            ObterStates(id);
        }

        public States ObterStates(Int32 idStates)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT States.Id, States.StateKey ");
            sql.Append("FROM States ");
            sql.Append("WHERE ");
            sql.Append(String.Format(" States.Id = {0} ", idStates));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    StateKey = dataReader["StateKey"].ToString();
                }

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
    }
}
