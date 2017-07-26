using Rohr.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Rohr.PMWeb
{
    public class ItemObjetoOrcamento
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public Int32 Codigo { get; set; }
        public String DescricaoResumida { get; set; }
        public String DescricaoOriginal { get; set; }
        public String Descricao { get; set; }
        public Double PrecoUnitario { get; set; }
        public String UnidadeMedida { get; set; }
        public Double Peso { get; set; }
        public Double Quantidade { get; set; }
        public Double ValorTabelaIndenizacao { get; set; }
        public Double ValorTabelaLocacao { get; set; }
        public Double ValorPraticadoLocacao { get; set; }
        public Double ValorPraticadoIndenizacao { get; set; }
        public Double Desconto { get; set; }
        public Double TotalLocacao { get; set; }
        public Int32 TabelaItensId { get; set; }
        public Int32 ItemGroupId { get; set; }
        public Boolean Exibir { get; set; }

        public List<ItemObjetoOrcamento> ObterItemObjetoOrcamento(Int32 idObjetoOrcamento)
        {
            List<ItemObjetoOrcamento> listItemObjetoOrcamento = new List<ItemObjetoOrcamento>();
            String sql = String.Format("EXEC GetItemsObjetoOrcamento_BR {0};", idObjetoOrcamento);

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql)))
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ItemObjetoOrcamento itemObjeto = new ItemObjetoOrcamento
                        {
                            Id = Int32.Parse(dataReader["Id"].ToString()),
                            Codigo = Int32.Parse(dataReader["Codigo"].ToString()),
                            DescricaoResumida = dataReader["Resumida"].ToString(),
                            DescricaoOriginal = dataReader["DescricaoOriginal"].ToString(),
                            Descricao = dataReader["Descricao"].ToString(),
                            PrecoUnitario = Double.Parse(dataReader["PrecoUnitario"].ToString()),
                            UnidadeMedida = dataReader["UM"].ToString(),
                            Peso = Double.Parse(dataReader["Peso"].ToString()),
                            Quantidade = Double.Parse(dataReader["Quantidade"].ToString()),
                            ValorTabelaIndenizacao = Double.Parse(dataReader["VUI"].ToString()),
                            ValorTabelaLocacao = Double.Parse(dataReader["VUL"].ToString()),
                            ValorPraticadoLocacao = Double.Parse(dataReader["ValorPraticado"].ToString()),
                            ValorPraticadoIndenizacao = Double.Parse(dataReader["valorPraticadoIndenizacao"].ToString()),
                            Desconto = Double.Parse(dataReader["Desconto"].ToString()),
                            TotalLocacao = Double.Parse(dataReader["TotalLoc"].ToString()),
                            TabelaItensId = Int32.Parse(dataReader["TabelaItensId"].ToString()),
                            ItemGroupId = Int32.Parse(dataReader["ItemGroupId"].ToString()),
                            Exibir =  true
                        };

                        listItemObjetoOrcamento.Add(itemObjeto);
                    }
                }
                else
                {
                    throw new Exception("Não foi possível identificar os itens do objeto no PMWeb. Verifique se não tem nenhum objeto sem itens :(");
                }

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return listItemObjetoOrcamento;
        }
    }
}
