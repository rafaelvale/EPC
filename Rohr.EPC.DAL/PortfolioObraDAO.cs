using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.DAL
{
    public class PortfolioObraDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public ModeloPortfolioObras ObterPorId(Int32 id)
        {
            ModeloPortfolioObras oModeloPortfolioObras = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModeloHistoriaRohr", id));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetModeloPortfolioObras", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oModeloPortfolioObras = new ModeloPortfolioObras
                    {
                        IdPortfolio = Int32.Parse(dataReader["IdPotfolio"].ToString()),
                        DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                        Texto = dataReader["Texto"].ToString(),
                        Versao = Int32.Parse(dataReader["Versao"].ToString()),
                        Descricao = dataReader["Descricao"].ToString(),
                        Nome = dataReader["Nome"].ToString()

                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oModeloPortfolioObras;

            }
        }



        public List<ModeloPortfolioObras> ObterLista()
        {
            List<ModeloPortfolioObras> listModeloPortfolioObras = null;

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetListModeloPortfolioObras", CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listModeloPortfolioObras = new List<ModeloPortfolioObras>();
                    while (dataReader.Read())
                    {
                        listModeloPortfolioObras.Add(new ModeloPortfolioObras
                        {
                            IdPortfolio = Int32.Parse(dataReader["IdPortfolio"].ToString()),
                            DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                            Texto = dataReader["Texto"].ToString(),
                            Versao = Int32.Parse(dataReader["Versao"].ToString()),
                            Descricao = (dataReader["Descricao"].ToString()),
                            Nome = (dataReader["Nome"].ToString())
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listModeloPortfolioObras;
            }
        }
    }
}
