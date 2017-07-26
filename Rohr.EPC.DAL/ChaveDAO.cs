using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class ChaveDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public List<Chave> ObterChavePorIdParte(Int32 idParte)
        {
            List<Chave> listChave = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdParte", idParte));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetListChaveParte", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listChave = new List<Chave>();
                    while (dataReader.Read())
                    {
                        listChave.Add(new Chave
                        {
                            IdChave = dataReader.GetInt32(dataReader.GetOrdinal("IdChave")),
                            ChaveDescricao = dataReader.GetString(dataReader.GetOrdinal("chave")),
                            Descricao = dataReader.GetString(dataReader.GetOrdinal("Descricao")),
                            DataCadastro = dataReader.GetDateTime(dataReader.GetOrdinal("DataCadastro")),
                            PermiteEdicao = dataReader.GetBoolean(dataReader.GetOrdinal("PermiteEdicao")),
                            Exibir = dataReader.GetBoolean(dataReader.GetOrdinal("Exibir"))
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listChave;
            }
        }
        public List<Chave> ObterTodos()
        {
            List<Chave> listChave = null;

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("SELECT IdChave, Chave, Descricao FROM Chaves ORDER BY Chave", CommandType.Text))
            {
                if (dataReader.HasRows)
                {
                    listChave = new List<Chave>();
                    while (dataReader.Read())
                    {
                        listChave.Add(new Chave
                        {
                            IdChave = dataReader.GetInt32(dataReader.GetOrdinal("IdChave")),
                            ChaveDescricao = dataReader.GetString(dataReader.GetOrdinal("chave")),
                            Descricao = dataReader.GetString(dataReader.GetOrdinal("Descricao")),
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listChave;
            }
        }
    }
}
