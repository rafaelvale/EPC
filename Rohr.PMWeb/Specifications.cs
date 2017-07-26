using Rohr.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class Specifications
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 SpecificationsId { get; set; }
        public Int32 EntityTypeId { get; set; }
        public Int32 EntityId { get; set; }
        public Int32 SpecificationTemplateId { get; set; }
        public String Measure { get; set; }
        public String Notes { get; set; }
        public Int32 ListItemId { get; set; }
        public String NomeTemplate { get; set; }

        public Specifications ObterEnderecoObraOportunidade(Int32 entityId, Boolean eProposta)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(73, entityId, 106);
                if (!eProposta && String.IsNullOrEmpty(oSpecifications.Measure.Trim()))
                    throw new Exception("Não foi possível recuperar da oportunidade o endereço da obra, verifique no PMWeb :(");

                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar da oportunidade o endereço da obra, verifique no PMWeb :(");
            }
        }
        public Specifications ObterBairroObraOportunidade(Int32 entityId, Boolean eProposta)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(74, entityId, 106);
                if (!eProposta && String.IsNullOrEmpty(oSpecifications.Measure.Trim()))
                    throw new Exception("Não foi possível recuperar da oportunidade o bairro da obra, verifique no PMWeb :(");
                return oSpecifications;

            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar da oportunidade o bairro da obra, verifique no PMWeb :(");
            }
        }
        public Specifications ObterCidadeObraOportunidade(Int32 entityId, Boolean eProposta)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(75, entityId, 106);
                if (!eProposta && String.IsNullOrEmpty(oSpecifications.Measure.Trim()))
                    throw new Exception("Não foi possível recuperar da oportunidade a cidade da obra, verifique no PMWeb :(");
                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar da oportunidade a cidade da obra, verifique no PMWeb :(");
            }
        }
        public Specifications ObterCepObraOportunidade(Int32 entityId, Boolean eProposta)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(76, entityId, 106);
                if (!eProposta && String.IsNullOrEmpty(oSpecifications.Measure.Trim()))
                    throw new Exception("Não foi possível recuperar da oportunidade o CEP da obra, verique no PMWeb :(");
                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar da oportunidade o CEP da obra, verique no PMWeb :(");
            }
        }
        public Specifications ObterEstadoObraOportunidade(Int32 entityId, Boolean eProposta)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(77, entityId, 106);
                if (eProposta && String.IsNullOrEmpty(oSpecifications.Measure.Trim()))
                    throw new Exception("Não foi possível recuperar da oportunidade o estado da obra, verifique no PMWeb :(");
                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar da oportunidade o estado da obra, verifique no PMWeb :(");
            }
        }

        public Specifications ObterEnderecoEntregaFaturaEndereco(Int32 entityId)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(100, entityId, 1);
                if (String.IsNullOrEmpty(oSpecifications.Measure.Trim()))
                    throw new Exception("Não foi possível recuperar o Endereço de entrega da fatura no PMWeb :(");

                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar o Endereço de entrega da fatura no PMWeb :(");
            }
        }
        public Specifications ObterEnderecoEntregaFaturaNumero(Int32 entityId)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(101, entityId, 1);
                if (String.IsNullOrEmpty(oSpecifications.Measure.Trim()))
                    throw new Exception("Não foi possível recuperar o número do endereço de entrega da fatura no PMWeb :(");

                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar o número do endereço de entrega da fatura no PMWeb :(");
            }
        }
        public Specifications ObterEnderecoEntregaFaturaBairro(Int32 entityId)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(103, entityId, 1);
                if (String.IsNullOrEmpty(oSpecifications.Measure.Trim()))
                    throw new Exception("Não foi possível recuperar o bairro do endereço de entrega da fatura no PMWeb :(");

                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar o bairro do endereço de entrega da fatura no PMWeb :(");
            }
        }
        public Specifications ObterEnderecoEntregaFaturaCidade(Int32 entityId)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(104, entityId, 1);
                if (oSpecifications.ListItemId < 0)
                    throw new Exception("Não foi possível recuperar a cidade do endereço de entrega da fatura no PMWeb :(");

                oSpecifications.Measure = new ListValues().ObterCidade(Convert.ToInt32(oSpecifications.ListItemId)).Description;
                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar a cidade do endereço de entrega da fatura no PMWeb :(");
            }
        }
        public Specifications ObterEnderecoEntregaFaturaCep(Int32 entityId)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(106, entityId, 1);
                if (String.IsNullOrEmpty(oSpecifications.Measure.Trim()))
                    throw new Exception("Não foi possível recuperar o CEP do endereço de entrega da fatura no PMWeb :(");

                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar o CEP do endereço de entrega da fatura no PMWeb :(");
            }
        }
        public Specifications ObterEnderecoEntregaFaturaEstado(Int32 entityId)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(105, entityId, 1);
                if (oSpecifications.ListItemId < 0)
                    throw new Exception("Não foi possível recuperar o estado do endereço de entrega da fatura no PMWeb :(");

                oSpecifications.Measure = new States(Convert.ToInt32(oSpecifications.ListItemId)).StateKey;

                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar o estado do endereço de entrega da fatura no PMWeb :(");
            }
        }

        public Specifications ObterPercentualLimpezaProposta(Int32 entityId)
        {
            try
            {
                Specifications oSpecifications = GetSpecifications(281, entityId, 2);

                if (!string.IsNullOrWhiteSpace(oSpecifications.Measure))
                    oSpecifications.Measure = oSpecifications.Measure.Substring(0, 1);
                else
                    oSpecifications.Measure = "0";

                return oSpecifications;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível verificar o percentual de limpeza no PMWeb :(");
            }
        }

        public Specifications ObterPercentualLimpezaContrato(Int32 entityId)
        {
            Specifications oSpecifications = GetSpecifications(280, entityId, 52);

            if (!string.IsNullOrWhiteSpace(oSpecifications.Measure))
                oSpecifications.Measure = oSpecifications.Measure.Substring(0, 1);
            else
                oSpecifications.Measure = "0";

            return oSpecifications;
        }

        public Specifications ObterOrcamentoValido(Int32 entityId)
        {
            try
            {
                return GetSpecifications(189, entityId, 2);
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível verificar se o orçamento é válido no PMWeb :(");
            }
        }

        public List<Specifications> ObterDetalhesPlataformas(Int32 entityId)
        {
            try
            {
                return GetListSpecifications(30, entityId);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível recuperar os Detalhes das Plataformas no PMWeb. " + ex.Message + " :(");
            }

        }

        public Specifications ObterDescricaoDoServico(Int32 entityId)
        {
            try
            {
                return GetSpecifications(82, entityId, 106);
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível recuperar a descrição do serviço no PMWeb :(");
            }
        }

        public Specifications ObterSpecificationById(Int32 idSpecification)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT s.id, s.EntityTypeId, s.EntityId, s.SpecificationTemplateId, s.Measure, s.Notes, s.ListItemId ");
            stringBuilder.Append("FROM Specifications s ");
            stringBuilder.Append(String.Format("WHERE Id = {0}", idSpecification));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(stringBuilder.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    SpecificationsId = Int32.Parse(dataReader["Id"].ToString());
                    EntityTypeId = Int32.Parse(dataReader["EntityTypeId"].ToString());
                    EntityId = Int32.Parse(dataReader["EntityId"].ToString());
                    SpecificationTemplateId = Int32.Parse(dataReader["SpecificationTemplateId"].ToString());
                    Measure = dataReader["Measure"].ToString();
                    Notes = dataReader["Notes"].ToString();
                    ListItemId = Int32.Parse(dataReader["ListItemId"].ToString());
                }
                else
                    throw new Exception("Especificações não encontrada :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }

        Specifications GetSpecifications(Int32 specificationTemplateId, Int32 entityId, Int32 entityTypeId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT s.id, s.EntityTypeId, s.EntityId, s.SpecificationTemplateId, s.Measure, s.Notes, s.ListItemId ");
            stringBuilder.Append("FROM Specifications s ");
            stringBuilder.Append(String.Format("WHERE SpecificationTemplateId = {0} AND EntityTypeId = {2} AND EntityId = {1}", specificationTemplateId, entityId, entityTypeId));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(stringBuilder.ToString())))
            {
                if (dataReader.HasRows)
                {
                    Int32 res;
                    dataReader.Read();
                    SpecificationsId = Int32.Parse(dataReader["Id"].ToString());
                    EntityTypeId = Int32.Parse(dataReader["EntityTypeId"].ToString());
                    EntityId = Int32.Parse(dataReader["EntityId"].ToString());
                    SpecificationTemplateId = Int32.Parse(dataReader["SpecificationTemplateId"].ToString());
                    Measure = dataReader["Measure"].ToString();
                    Notes = dataReader["Notes"].ToString();

                    if (Int32.TryParse(dataReader["ListItemId"].ToString(), out res))
                        ListItemId = res;
                    else
                        ListItemId = 0;
                }
                else
                    throw new Exception("Especificações não encontrada :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        List<Specifications> GetListSpecifications(Int32 groupId, Int32 entityId)
        {
            DbHelper dbHelperPmweb1 = new DbHelper("pmweb");

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT s.Id, s.EntityTypeId, s.EntityId, s.SpecificationTemplateId, s.Measure, s.Notes, s.ListItemId, t.Name ");
            stringBuilder.Append("FROM Specifications s  ");
            stringBuilder.Append("INNER JOIN SpecificationsTemplate t ON t.id = s.SpecificationTemplateId ");
            stringBuilder.Append("INNER JOIN SpecificationGroups g ON g.id = t.GroupId ");
            stringBuilder.Append(String.Format("WHERE g.id = {0} AND EntityId = {1} ", groupId, entityId));

            List<Specifications> listSpecifications = new List<Specifications>();
            using (SqlDataReader dataReader = ((SqlDataReader)dbHelperPmweb1.ExecutarDataReader(stringBuilder.ToString())))
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Specifications oSpecifications = new Specifications
                        {
                            SpecificationsId = Int32.Parse(dataReader["Id"].ToString()),
                            EntityTypeId = Int32.Parse(dataReader["EntityTypeId"].ToString()),
                            EntityId = Int32.Parse(dataReader["EntityId"].ToString()),
                            SpecificationTemplateId = Int32.Parse(dataReader["SpecificationTemplateId"].ToString()),
                            Measure = dataReader["Measure"].ToString(),
                            Notes = dataReader["Notes"].ToString(),
                            NomeTemplate = dataReader["Name"].ToString()
                        };

                        Int32 resultado;
                        if (Int32.TryParse(dataReader["ListItemId"].ToString(), out resultado))
                            oSpecifications.ListItemId = resultado;


                        listSpecifications.Add(oSpecifications);
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return listSpecifications;
        }

    }
}
