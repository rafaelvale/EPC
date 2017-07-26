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
    public class HistoriaRohrDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public ModeloHistoriaRohr ObterPorId(Int32 id)
        {
            ModeloHistoriaRohr oModeloHistoriaRohr = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModeloHistoriaRohr", id));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetModeloHistoriaRohr", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oModeloHistoriaRohr = new ModeloHistoriaRohr
                    {
                        IdModeloHistoriaRohr = Int32.Parse(dataReader["IdModeloHistoriaRohr"].ToString()),
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

                return oModeloHistoriaRohr;

            }
        }

       

        public List<ModeloHistoriaRohr> ObterLista()
        {
            List<ModeloHistoriaRohr> listModeloHistoriaRohr = null;

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetListModeloHistoriaRohr", CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listModeloHistoriaRohr = new List<ModeloHistoriaRohr>();
                    while (dataReader.Read())
                    {
                        listModeloHistoriaRohr.Add(new ModeloHistoriaRohr
                        {
                            IdModeloHistoriaRohr = Int32.Parse(dataReader["IdModeloHistoriaRohr"].ToString()),
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

                return listModeloHistoriaRohr;
            }
        }
    }
}
