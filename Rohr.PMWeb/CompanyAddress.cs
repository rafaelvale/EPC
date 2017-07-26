using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class CompanyAddress
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 IdCompanyAddresses { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String Zip { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public String Email { get; set; }
        public Boolean IsPrimary { get; set; }
        public States States { get; set; }
        public Countries Countries { get; set; }
        public ListValues City { get; set; }
        public String Type { get; set; }
        public String AltPhone { get; set; }

        public CompanyAddress ObterCompanyAddresses(Int32 idCompany)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT a.id, a.Address1, a.Address2, a.City, a.Zip, a.Phone, a.Fax, a.email, a.IsPrimary, a.StateId, a.CountryId, a.AltPhone ");
            sql.Append("FROM CompanyAddresses a ");
            sql.Append(String.Format("WHERE IsActive = 0 AND IsPrimary = 1 AND CompanyId = {0};", idCompany));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Address1 = dataReader["Address1"].ToString();
                    Address2 = dataReader["Address2"].ToString();
                    Zip = dataReader["Zip"].ToString();
                    Phone = dataReader["Phone"].ToString();
                    Fax = dataReader["Fax"].ToString();
                    Email = dataReader["Email"].ToString();
                    AltPhone = dataReader["AltPhone"].ToString();
                    IsPrimary = Boolean.Parse(dataReader["IsPrimary"].ToString());


                    Int32 resultConvert;

                    #region Estado
                    if (!Int32.TryParse(dataReader["StateId"].ToString(), out resultConvert))
                        throw new Exception("Não foi possível identificar no endereço do cliente o estado, verifique no PMWeb :(");

                    States oStates = new States(resultConvert);

                    if (oStates.Id != 0)
                        States = new States(resultConvert);
                    else
                        throw new Exception("Não foi possível identificar no endereço do cliente o estado, verifique no PMWeb :(");
                    #endregion
                    #region Cidade
                    if (!Int32.TryParse(dataReader["City"].ToString(), out resultConvert))
                        throw new Exception("Não foi possível identificar a cidade do cliente no PMWeb :(");

                    City = new ListValues().ObterCidade(resultConvert);
                    #endregion
                }
                else
                    throw new Exception("Não foi possível identificar o endereço do cliente no PMWeb. Verifique se o cliente tem um endereço marcado como primário :(");
                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }

    }
}
