using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class ItemGroups
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public String GroupName { get; set; }
        public Int32 Id { get; set; }
        public ItemGroups ObterGrupoDoItem(Int32 idItem)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT GroupName, Id FROM ItemGroups WHERE id = ( ");
            sql.Append("SELECT ParentId FROM Items ");
            sql.Append("INNER JOIN ItemGroups ON Items.ItemGroupId = ItemGroups.Id ");
            sql.Append(String.Format("WHERE Items.id = {0})", idItem));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    GroupName = dataReader["GroupName"].ToString();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                }
                else
                    throw new Exception("Não foi possível identificar o grupo no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public Boolean ExibirItensSubGrupo(Int32 idItem)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("select 1 as res from ItemGroups where GroupName like '%CABINES ELEVADORES%' ");
            sql.Append("and id = ");
            sql.Append("(select ");
            sql.Append("(select ParentId from ItemGroups where id = ");
            sql.Append(String.Format("(select id from ItemGroups where Id = ItemGroupId)) as id from Items where Id= {0}) ", idItem));
            sql.Append("UNION ALL ");
            sql.Append("select 1 from ItemGroups where GroupName like '%EQUIPAMENTO DE ACESSO%' ");
            sql.Append("and id = ");
            sql.Append("(select ");
            sql.Append("(select ParentId from ItemGroups where id = ");
            sql.Append("(select ParentId from ItemGroups where id = ");
            sql.Append(String.Format("(select id from ItemGroups where Id = ItemGroupId))) as id from Items where Id= {0}) ", idItem));

            Boolean retorno;
            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                retorno = dataReader.HasRows;

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return retorno;
        }
    }
}
