using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class ParteDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public List<Parte> ObterPorIdModelo(Int32 idModelo)
        {
            List<Parte> listPartes = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModelo", idModelo));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetListParteModelo", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listPartes = new List<Parte>();
                    while (dataReader.Read())
                    {
                        listPartes.Add(new Parte
                        {
                            IdParte = dataReader.GetInt32(dataReader.GetOrdinal("IdParte")),
                            Nome = dataReader.GetString(dataReader.GetOrdinal("Nome")),
                            TextoParte = dataReader.GetString(dataReader.GetOrdinal("TextoParte")),
                            DataCadastro = dataReader.GetDateTime(dataReader.GetOrdinal("DataCadastro")),
                            PermiteEdicao = dataReader.GetBoolean(dataReader.GetOrdinal("PermiteEdicao")),
                            OrdemExibicao = dataReader.GetInt32(dataReader.GetOrdinal("PosicaoExibicao")),
                            Exibir = dataReader.GetBoolean(dataReader.GetOrdinal("Exibir"))
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listPartes;
            }
        }
    }
}
