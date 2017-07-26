using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class Company
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; private set; }
        public String CompanyName { get; set; }
        public String Sigla { get; set; }
        public String CompanyCode { get; set; }
        public String ShortName { get; set; }
        public String StateTaxId { get; set; }
        private Boolean IsActive { get; set; }
        private Boolean IsDeleted { get; set; }
        private DateTime CreateDate { get; set; }
        private Int32 IdFilial { get; set; }

        public Company ObterCompany(Int32 idCompany)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.Id, a.CompanyName, a.CompanyCode, a.ShortName, a.StateTaxId, a.IsActive, a.IsDeleted, a.CreateDate ");
            sql.Append(String.Format("FROM Companies a WHERE Id = {0}", idCompany));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    CompanyName = dataReader["CompanyName"].ToString();
                    CompanyCode = dataReader["CompanyCode"].ToString();
                    ShortName = dataReader["ShortName"].ToString();
                    StateTaxId = dataReader["StateTaxId"].ToString();
                    IsActive = Boolean.Parse(dataReader["IsActive"].ToString());
                    IsDeleted = Boolean.Parse(dataReader["IsDeleted"].ToString());
                    CreateDate = DateTime.Parse(dataReader["CreateDate"].ToString());
                }
                else
                    throw new Exception("Não foi possível encontrar o cliente no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public Company ObterFilial(Int32 idCompany)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.Id, a.CompanyName, " +
                               "CASE WHEN a.Id = 3 THEN 'BH' " +
                                    "WHEN a.Id = 8 THEN 'DF' " +
                                    "WHEN a.Id = 9 THEN 'CB' " +
                                    "WHEN a.Id = 4 THEN 'RJ' " +
                                    "WHEN a.Id = 7 THEN 'CR' " +
                                    "WHEN a.Id = 2 THEN 'RS' " +
                                    "WHEN a.Id = 6 THEN 'RJ' " +
                                    "WHEN a.Id = 5 THEN 'SP' " +
                                    "WHEN a.Id = 1 THEN 'BA' END as Sigla, " +
                                 "a.CompanyCode, a.ShortName, a.StateTaxId, a.IsActive, a.IsDeleted, a.CreateDate, (SELECT dbo.BuscaParametro(19, a.Id, 'ID Filial', 'Vínculos IDs')) IdFilial ");
            sql.Append("FROM dbo.Companies a ");
            sql.Append("WHERE (SELECT dbo.BuscaParametro(19, a.Id, 'Filial', 'Características')) = 'True' ");
            sql.Append(String.Format(" AND a.Id = {0}", idCompany));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    CompanyName = dataReader["CompanyName"].ToString();
                    Sigla = dataReader["Sigla"].ToString();
                    CompanyCode = dataReader["CompanyCode"].ToString();
                    ShortName = dataReader["ShortName"].ToString();
                    StateTaxId = dataReader["StateTaxId"].ToString();
                    IsActive = Boolean.Parse(dataReader["IsActive"].ToString());
                    IsDeleted = Boolean.Parse(dataReader["IsDeleted"].ToString());
                    CreateDate = DateTime.Parse(dataReader["CreateDate"].ToString());
                    IdFilial = Int32.Parse(dataReader["IdFilial"].ToString());
                }
                else
                    throw new Exception("Não foi possível encontrar a filial no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }

        public Company ObterFilialNome(Int32 idCompany)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.Id, a.CompanyName, a.CompanyCode, a.ShortName, a.StateTaxId, a.IsActive, a.IsDeleted, a.CreateDate, (SELECT dbo.BuscaParametro(19, a.Id, 'ID Filial', 'Vínculos IDs')) IdFilial ");
            sql.Append("FROM dbo.Companies a ");
            sql.Append("WHERE (SELECT dbo.BuscaParametro(19, a.Id, 'Filial', 'Características')) = 'True' ");
            sql.Append(String.Format(" AND a.Id = {0}", idCompany));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    CompanyName = dataReader["CompanyName"].ToString();
                    CompanyCode = dataReader["CompanyCode"].ToString();
                    ShortName = dataReader["ShortName"].ToString();
                    StateTaxId = dataReader["StateTaxId"].ToString();
                    IsActive = Boolean.Parse(dataReader["IsActive"].ToString());
                    IsDeleted = Boolean.Parse(dataReader["IsDeleted"].ToString());
                    CreateDate = DateTime.Parse(dataReader["CreateDate"].ToString());
                    IdFilial = Int32.Parse(dataReader["IdFilial"].ToString());
                }
                else
                    throw new Exception("Não foi possível encontrar a filial no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public Company ObterCompanyProposta(Int32 idDocumentTodoList)
        {
            String sql = String.Empty;
            sql += "SELECT co.Id, co.CompanyName, co.CompanyCode, co.ShortName, co.StateTaxId, co.IsActive, co.IsDeleted, co.CreateDate ";
            sql += "FROM dbo.Specifications s ";
            sql += "LEFT JOIN dbo.SpecificationsTemplate st ON st.Id = s.SpecificationTemplateId ";
            sql += "LEFT JOIN dbo.PMWeb_Lists l on l.Id = st.ListId ";
            sql += "LEFT JOIN dbo.CompanyAddressesContacts C ON C.Id = s.ListItemId ";
            sql += "LEFT JOIN dbo.Companies co ON co.Id = c.CompanyId ";
            sql += "WHERE EntityTypeId = 106 and st.ID = 71 AND EntityId = " + idDocumentTodoList;

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql)))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    CompanyName = dataReader["CompanyName"].ToString();
                    CompanyCode = dataReader["CompanyCode"].ToString();
                    ShortName = dataReader["ShortName"].ToString();
                    StateTaxId = dataReader["StateTaxId"].ToString();
                    IsActive = Boolean.Parse(dataReader["IsActive"].ToString());
                    IsDeleted = Boolean.Parse(dataReader["IsDeleted"].ToString());
                    CreateDate = DateTime.Parse(dataReader["CreateDate"].ToString());
                }

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
    }
}
