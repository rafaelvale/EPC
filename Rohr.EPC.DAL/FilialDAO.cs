using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class FilialDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public Filial ObterPorId(Int32 id)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdFilial", id));
            Filial filial = null;

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetFilial", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    filial = new Filial
                    {
                        IdFilial = Int32.Parse(dataReader["idFilial"].ToString()),
                        CodigoOrigem = Int32.Parse(dataReader["codigoOrigem"].ToString()),
                        DataCadastro = DateTime.Parse(dataReader["dataCadastro"].ToString()),
                        Descricao = dataReader["Descricao"].ToString()
                    };
                }

                dataReader.Close();
                _dbHelper.CloseConnection();
                dataReader.Dispose();

                return filial;
            }
        }
        public List<Filial> ObterFilialPorIdUsuario(Int32 idUsuario)
        {
            List<Filial> listFiliais = new List<Filial>();
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@idUsuario", idUsuario));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetFilialByIdUsuario", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Filial filial = new Filial
                        {
                            IdFilial = dataReader.GetInt32(dataReader.GetOrdinal("idFilial")),
                            CodigoOrigem = dataReader.GetInt32(dataReader.GetOrdinal("codigoOrigem")),
                            DataCadastro = dataReader.GetDateTime(dataReader.GetOrdinal("dataCadastro")),
                        };

                        listFiliais.Add(filial);
                    }
                }

                dataReader.Close();
                _dbHelper.CloseConnection();
                dataReader.Dispose();

                return listFiliais;
            }
        }
    }
}
