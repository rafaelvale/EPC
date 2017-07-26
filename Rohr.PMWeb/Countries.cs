using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class Countries
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public String Nationality { get; set; }

        public Countries(Int32 id)
        {
            ObterCountries(id);
        }
        public Countries ObterCountries(Int32 idCountry)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(" Countries.Id, Countries.Nationality ");
            sql.Append("FROM ");
            sql.Append(" Countries ");
            sql.Append("WHERE ");
            sql.Append(String.Format(" Countries.id = {0} ", idCountry));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    Nationality = dataReader["Nationality"].ToString();
                }

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
    }
}
