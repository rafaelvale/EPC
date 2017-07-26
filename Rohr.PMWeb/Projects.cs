using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class Projects
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public String ProjectName { get; set; }
        public String ProjectNumber { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public String ProgramId { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String Zip { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public Int32 ClientId { get; set; }
        public Countries Countries { get; set; }
        public States States { get; set; }

        public Projects ObterProjects(Int32 idProject)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append("	a.Id, a.ProjectName, a.ProjectNumber, a.IsActive, a.CreateDate, a.ProgramId, ");
            sql.Append("	a.Address1, a.Address2, a.City, a.Zip, a.Phone, a.Fax, a.ClientId, a.StateId, a.CountryId ");
            sql.Append("FROM Projects as a ");
            sql.Append("WHERE ");
            sql.Append("a.IsDeleted = 0 and a.IsActive = 1 and ");
            sql.Append(String.Format("	a.id = {0} ", idProject));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    Int32 resultConvert;
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    ProjectName = dataReader["ProjectName"].ToString();
                    ProjectNumber = dataReader["ProjectNumber"].ToString();
                    IsActive = Boolean.Parse(dataReader["IsActive"].ToString());
                    CreateDate = DateTime.Parse(dataReader["CreateDate"].ToString());
                    ProgramId = dataReader["ProgramId"].ToString();
                    Address1 = dataReader["Address1"].ToString();
                    Address2 = dataReader["Address2"].ToString();
                    City = dataReader["City"].ToString();
                    Zip = dataReader["Zip"].ToString();
                    Phone = dataReader["Phone"].ToString();
                    Fax = dataReader["Fax"].ToString();

                    if (String.IsNullOrEmpty(Zip))
                        throw new Exception("Não foi possível identificar o CEP da obra no PMWeb :(");

                    if (String.IsNullOrEmpty(City))
                        throw new Exception("Não foi possível identificar a cidade da obra no PMWeb :(");

                    if (String.IsNullOrEmpty(Address1))
                        throw new Exception("Não foi possível identificar o endereco da obra no PMWeb :(");

                    if (String.IsNullOrEmpty(Address2))
                        throw new Exception("Não foi possível identificar o bairro da obra no PMWeb :(");

                    if (!Int32.TryParse(dataReader["ClientId"].ToString(), out resultConvert))
                        throw new Exception("Não foi possível identificar o cliente da obra no PMWeb :(");

                        ClientId = resultConvert;

                    if (!Int32.TryParse(dataReader["CountryId"].ToString(), out resultConvert))
                        throw new Exception("Não foi possível identificar o país da obra no PMWeb :(");
                        Countries oCountries = new Countries(resultConvert);

                        if (oCountries.Id != 0)
                            Countries = new Countries(resultConvert);
                        else
                            throw new Exception("Não foi possível identificar o país da obra no PMWeb :(");

                    if (!Int32.TryParse(dataReader["StateId"].ToString(), out resultConvert))
                        throw new Exception("Não foi possível identificar o estado da obra no PMWeb :(");
                    States oStates = new States(resultConvert);

                    if (oStates.Id != 0)
                        States = new States(resultConvert);
                    else
                        throw new Exception("Não foi possível identificar o estado da obra no PMWeb :(");
                }
                else
                    throw new Exception("Não foi possível encontrar a obra no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
    }
}
