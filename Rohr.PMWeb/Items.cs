using Rohr.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class Items
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 IdItem { get; private set; }
        public String Descricao { get; private set; }
        public Decimal ValorLocacao { get; private set; }
        public Decimal ValorIndenizacao { get; private set; }
        public String Unidade { get; private set; }
        public Decimal Peso { get; private set; }
        public Decimal MedidaControle { get; private set; }
        public String Tipo { get; private set; }
        public Int32 CodigoItemSistemaOrigem { get; private set; }
        public String DescricaoResumida { get; private set; }
        public Int32 CodigoGrupoItemSistemaOrigem { get; private set; }

        public IEnumerable<Items> ObterItemsDoSubGrupo(Int32 idTabelaPreco, Int32 idItem)
        {
            List<Items> listItems = new List<Items>();
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT i.id idItem, i.ItemGroupId, i.Description descricaoItem, tid.ValorIndenizacao, tid.ValorLocacao, i.ForeignKey, ");
            sql.Append("(SELECT Measure FROM dbo.Specifications WHERE SpecificationTemplateId = 23 AND EntityTypeId = 3 AND EntityId = i.Id) as peso, ");
            sql.Append("(SELECT Measure FROM dbo.Specifications WHERE SpecificationTemplateId = 22 AND EntityTypeId = 3 AND EntityId = i.Id) as medidaControle, ");
            sql.Append("(SELECT UOM FROM dbo.UOM WHERE Id = (SELECT ISNULL(ListItemId,0) FROM dbo.Specifications WHERE	SpecificationTemplateId = (case when i.Description like '$$%' then 130 else 20 end) AND EntityTypeId = 3 AND EntityId = i.Id)) AS unidade ");
            sql.Append("FROM dbo.Items i  ");
            sql.Append("INNER JOIN dbo.TabelaItens_Details_BR tid ON tid.ItemId = i.Id ");
            sql.Append(String.Format("WHERE i.ItemGroupId = (select ItemGroupId from Items where id = {1}) AND tid.TabelaItensID = {0} AND dbo.BuscaParametro(3,i.id, 'Totalizador de Perda', 'Detalhes Cadastro Itens') = 'false' AND LEFT(Description,2) != '$$' ORDER BY descricaoItem", idTabelaPreco, idItem));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Items item = new Items
                        {
                            IdItem = Int32.Parse(dataReader["IdItem"].ToString()),
                            Descricao = dataReader["descricaoItem"].ToString(),
                            CodigoGrupoItemSistemaOrigem = Int32.Parse(dataReader["ItemGroupId"].ToString()),
                            Unidade = dataReader["unidade"].ToString(),
                            CodigoItemSistemaOrigem = Int32.Parse(dataReader["IdItem"].ToString()),
                            DescricaoResumida = dataReader["ForeignKey"].ToString(),
                            Tipo = "item"
                        };

                        Decimal res;

                        Decimal.TryParse(dataReader["ValorLocacao"].ToString(), out res);
                        item.ValorLocacao = res;

                        Decimal.TryParse(dataReader["ValorIndenizacao"].ToString(), out res);
                        item.ValorIndenizacao = res;

                        Decimal.TryParse(dataReader["peso"].ToString().Replace('.', ','), out res);
                        item.Peso = res;

                        Decimal.TryParse(dataReader["medidaControle"].ToString().Replace('.', ','), out res);
                        item.MedidaControle = res;

                        listItems.Add(item);
                    }
                }
                else
                    throw new Exception("Não foi possível identificar os itens do subgrupo no PMWeb :(");
            }
            return listItems;
        }
        public Items ObterSubGrupoItem(Int32 idSubGrupo)
        {
            Items item;

            String sql = String.Format("SELECT g.Id, g.GroupName, g.ParentId FROM ItemGroups g WHERE id = {0}", idSubGrupo);

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql)))
            {
                if (dataReader.Read())
                {
                    item = new Items
                    {
                        IdItem = Int32.Parse(dataReader["Id"].ToString()),
                        Descricao = dataReader["GroupName"].ToString(),
                        CodigoGrupoItemSistemaOrigem = 0,
                        Unidade = "",
                        CodigoItemSistemaOrigem = Int32.Parse(dataReader["Id"].ToString()),
                        DescricaoResumida = dataReader["GroupName"].ToString(),
                        Tipo = "subgrupo",
                        ValorLocacao = 0,
                        ValorIndenizacao = 0,
                        Peso = 0
                    };

                }
                else
                    throw new Exception("Não foi possível identificar os itens do subgrupo no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return item;
        }
        public Int32 ObterIdGrupoItem(Int32 idItem)
        {
            Int32 idIGrupo;
            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(String.Format("SELECT ItemGroupId FROM Items WHERE Id = {0}", idItem))))
            {
                if (dataReader.Read())
                    idIGrupo = Int32.Parse(dataReader["ItemGroupId"].ToString());
                else
                    throw new Exception("Não foi possível identificar o Grupo do item no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return idIGrupo;
        }
    }
}
