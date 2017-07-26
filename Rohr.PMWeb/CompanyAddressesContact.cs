using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class CompanyAddressesContact
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Title { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public String Cell { get; set; }
        public String Email { get; set; }
        public String WebSite { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsPrimary { get; set; }
        public Int32 CompanyId { get; set; }
        public String Notes { get; set; }

        public CompanyAddressesContact ObterCompanyAddressesContacts(Int32 projectId)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");
            sql.Append("	a.UserName, a.FirstName, a.LastName, a.Title, a.Phone, a.Fax, a.Cell, a.Email, a.Website, ");
            sql.Append("	a.IsPrimary, a.Notes, a.IsActive, a.IsPrimary, a.CompanyId ");
            sql.Append("FROM ");
            sql.Append("	ProjectContacts c ");
            sql.Append("	INNER JOIN CompanyAddressesContacts a ON c.ContactId = a.Id ");
            sql.Append(String.Format("WHERE ProjectId = {0};", projectId));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    UserName = dataReader["UserName"].ToString();
                    FirstName = dataReader["FirstName"].ToString();
                    LastName = dataReader["LastName"].ToString();
                    Title = dataReader["Title"].ToString();
                    Phone = dataReader["Phone"].ToString();
                    Fax = dataReader["Fax"].ToString();
                    Cell = dataReader["Cell"].ToString();
                    Email = dataReader["Email"].ToString();
                    WebSite = dataReader["WebSite"].ToString();
                    IsActive = Boolean.Parse(dataReader["IsActive"].ToString());
                    IsPrimary = Boolean.Parse(dataReader["IsPrimary"].ToString());
                    CompanyId = Int32.Parse(dataReader["CompanyId"].ToString());
                    Notes = dataReader["Notes"].ToString();
                }
                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public CompanyAddressesContact ObterCompanyAddressesContactsById(Int32 idCompanyAddressesContact)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");
            sql.Append("	a.UserName, a.FirstName, a.LastName, a.Title, a.Phone, a.Fax, a.Cell, a.Email, a.Website, ");
            sql.Append("	a.IsPrimary, a.Notes, a.IsActive, a.IsPrimary, a.CompanyId ");
            sql.Append("FROM CompanyAddressesContacts a ");
            sql.Append(String.Format("WHERE a.Id = {0};", idCompanyAddressesContact));


            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    UserName = dataReader["UserName"].ToString();
                    FirstName = dataReader["FirstName"].ToString();
                    LastName = dataReader["LastName"].ToString();
                    Title = dataReader["Title"].ToString();
                    Phone = dataReader["Phone"].ToString();
                    Fax = dataReader["Fax"].ToString();
                    Cell = dataReader["Cell"].ToString();
                    Email = dataReader["Email"].ToString();
                    WebSite = dataReader["WebSite"].ToString();
                    IsActive = Boolean.Parse(dataReader["IsActive"].ToString());
                    IsPrimary = Boolean.Parse(dataReader["IsPrimary"].ToString());
                    CompanyId = Int32.Parse(dataReader["CompanyId"].ToString());
                    Notes = dataReader["Notes"].ToString();
                }
                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
    }
}
