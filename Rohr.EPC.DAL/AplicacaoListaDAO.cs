using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class AplicacaoListaDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public List<AplicacaoLista> ObterPorIdChave(Int32 idChave)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdChave", idChave));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetAplicacaoLista", parametros, CommandType.StoredProcedure))
            {
                List<AplicacaoLista> listAplicacaoLista;
                if (dataReader.HasRows)
                {
                    listAplicacaoLista = new List<AplicacaoLista>();
                    while (dataReader.Read())
                    {
                        listAplicacaoLista.Add(new AplicacaoLista
                        {
                            IdAplicacaoLista = dataReader.GetInt32(dataReader.GetOrdinal("IdAplicacaoLista")),
                            IdAplicacaoGrupoLista = dataReader.GetInt32(dataReader.GetOrdinal("IdAplicacaoGrupoLista")),
                            Descricao = dataReader.GetString(dataReader.GetOrdinal("Descricao"))
                        });
                    }
                }
                else
                    throw new Exception("Não foi possível identificar o valor padrão ;(");

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listAplicacaoLista;
            }
        }
    }
}
