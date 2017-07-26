using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class Contacts
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 SpecificationsId { get; set; }
        public Int32 EntityTypeId { get; set; }
        public Int32 ListItemId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Phone { get; set; }
        public String Cell { get; set; }
        public String Email { get; set; }
        public String Website { get; set; }
        public Int32 CompanyId { get; set; }
        public String DepartmentName { get; set; }

        Contacts ObterContact(String sql)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ");
            stringBuilder.Append("Specifications.Id, EntityId, ListItemId, CompanyAddressesContacts.FirstName, CompanyAddressesContacts.LastName, ");
            stringBuilder.Append("CompanyAddressesContacts.Phone, CompanyAddressesContacts.Cell, CompanyAddressesContacts.Email, ");
            stringBuilder.Append("CompanyAddressesContacts.Website, CompanyAddressesContacts.CompanyId, CompanyAddressesContacts.DepartmentId, CompanyDepartments.DepartmentName ");
            stringBuilder.Append("FROM Specifications ");
            stringBuilder.Append("INNER JOIN CompanyAddressesContacts on Specifications.ListItemId = CompanyAddressesContacts.Id ");
            stringBuilder.Append("LEFT JOIN CompanyDepartments on CompanyAddressesContacts.DepartmentId = CompanyDepartments.Id ");
            stringBuilder.Append("WHERE CompanyAddressesContacts.IsActive = 0 ");
            stringBuilder.Append(sql);

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelperPmweb.ExecutarDataReader(stringBuilder.ToString()))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    SpecificationsId = Int32.Parse(dataReader["Id"].ToString());
                    EntityTypeId = Int32.Parse(dataReader["EntityId"].ToString());
                    ListItemId = Int32.Parse(dataReader["ListItemId"].ToString());
                    FirstName = dataReader["FirstName"].ToString();
                    LastName = dataReader["LastName"].ToString();
                    Phone = dataReader["Phone"].ToString();
                    Cell = dataReader["Cell"].ToString();
                    Email = dataReader["Email"].ToString();
                    Website = dataReader["Website"].ToString();
                    CompanyId = Int32.Parse(dataReader["CompanyId"].ToString());
                    DepartmentName = dataReader["DepartmentName"].ToString();
                }
                else
                    throw new Exception();

                if (dataReader.Read())
                    throw new Exception();

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }

        public Contacts ObterComercialResponsavelOportunidade(Int32 entityId)
        {
            try
            {
                return ObterContact(String.Format("AND SpecificationTemplateId = 140 AND EntityTypeId = 106 AND EntityId = {0}", entityId));
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível encontrar o comercial responsável no PMWeb :(");
            }

        }
        public Contacts ObterComercialResponsavelContrato(Int32 entityId)
        {
            try
            {
                return ObterContact(String.Format("AND SpecificationTemplateId = 123 AND EntityTypeId = 1 AND EntityId = {0}", entityId));
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível encontrar o comercial responsável no PMWeb :(");
            }
        }
        public Contacts ObterContatoComercialOportunidade(Int32 entityId)
        {
            try
            {
                return ObterContact(String.Format("AND SpecificationTemplateId = 71 AND EntityTypeId = 106 AND EntityId = {0}", entityId));
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível encontrar o contato comercial no PMWeb :(");
            }
        }
    }
}
