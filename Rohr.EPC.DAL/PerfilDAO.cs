using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class PerfilDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public List<Perfil> ObterPorIdUsuario(Int32 idUsuario)
        {
            List<Perfil> listPerfil = new List<Perfil>();

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@idUsuario", idUsuario));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetPerfil", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Perfil perfil = new Perfil
                        {
                            IdPerfil = Int32.Parse(dataReader["idPerfil"].ToString()),
                            Descricao = dataReader["descricao"].ToString(),
                            ExibirTodosDocumento = Convert.ToBoolean(dataReader["ExibirTodosDocumento"].ToString())

                        };
                        listPerfil.Add(perfil);
                    }
                    dataReader.Close();
                    _dbHelper.CloseConnection();
                    dataReader.Dispose();
                }
                return listPerfil;
            }
        }
        public Perfil ObterPorId(Int32 idPerfil)
        {
            Perfil oPerfil = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdPerfil", idPerfil));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetPerfilById", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oPerfil = new Perfil
                    {
                        IdPerfil = dataReader.GetInt32(dataReader.GetOrdinal("IdPerfil")),
                        Descricao = dataReader.GetString(dataReader.GetOrdinal("Descricao"))
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oPerfil;
            }
        }
        public Boolean AtualizarExibirTodosDocumento(Int32 idUsuario, Int32 idPerfilAtivo)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@idUsuario", idUsuario));
            parametros.Adicionar(new DbParametro("@idPerfil", idPerfilAtivo));

            Boolean exibirTodosDocumento = Convert.ToBoolean(_dbHelper.ExecutarScalar("UpdateExibirTodosDocumento", parametros, CommandType.StoredProcedure));

            _dbHelper.CloseConnection();

            return exibirTodosDocumento;
        }
    }
}
