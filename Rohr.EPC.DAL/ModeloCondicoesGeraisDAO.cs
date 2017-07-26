using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class ModeloCondicoesGeraisDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public ModeloCondicoesGerais ObterPorId(Int32 id)
        {
            ModeloCondicoesGerais oModeloCondicoesGerais = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModeloCondicaoGeral", id));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetModeloCondicoesGerais", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oModeloCondicoesGerais = new ModeloCondicoesGerais
                    {
                        IdModeloCondicoesGerais = Int32.Parse(dataReader["IdModeloCondicaoGeral"].ToString()),
                        DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                        Texto = dataReader["Texto"].ToString(),
                        Versao = Int32.Parse(dataReader["Versao"].ToString()),
                        Descricao = (dataReader["Descricao"].ToString()),
                        Nome = (dataReader["Nome"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oModeloCondicoesGerais;
            }
        }
        public List<ModeloCondicoesGerais> ObterLista()
        {
            List<ModeloCondicoesGerais> listModelosCondicoesGerais = null;

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetListModeloCondicoesGerais", CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listModelosCondicoesGerais = new List<ModeloCondicoesGerais>();
                    while (dataReader.Read())
                    {
                        listModelosCondicoesGerais.Add(new ModeloCondicoesGerais
                        {
                            IdModeloCondicoesGerais = Int32.Parse(dataReader["IdModeloCondicaoGeral"].ToString()),
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

                return listModelosCondicoesGerais;
            }
        }
    }
}
