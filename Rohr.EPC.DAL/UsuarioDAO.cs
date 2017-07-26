using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class UsuarioDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public Usuario ObterPorId(Int32 id)
        {
            Usuario oUsuario = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdUsuario", id));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetUsuario", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oUsuario = new Usuario
                    {
                        IdUsuario = dataReader.GetInt32(dataReader.GetOrdinal("IdUsuario")),
                        Matricula = dataReader.GetInt32(dataReader.GetOrdinal("Matricula")),
                        PrimeiroNome = dataReader.GetString(dataReader.GetOrdinal("PrimeiroNome")),
                        Sobrenome = dataReader.GetString(dataReader.GetOrdinal("Sobrenome")),
                        Ativo = dataReader.GetBoolean(dataReader.GetOrdinal("Ativo")),
                        Login = dataReader.GetString(dataReader.GetOrdinal("Login")),
                        DataCadastro = dataReader.GetDateTime(dataReader.GetOrdinal("DataCadastro")),
                        PrimeiroAcesso = dataReader.GetBoolean(dataReader.GetOrdinal("PrimeiroAcesso")),
                        DataUltimaAlteracaoSenha = dataReader["DataUltimaAlteracaoSenha"] as DateTime?,
                        Email = dataReader.GetString(dataReader.GetOrdinal("Email")),
                        ExibirModal = dataReader.GetBoolean(dataReader.GetOrdinal("ExibirModal"))
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oUsuario;
            }
        }
        public Usuario ValidarAcesso(String login, String senha, String IP)
        {
            Usuario oUsuario = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@Login", login));
            parametros.Adicionar(new DbParametro("@Senha", senha));
            parametros.Adicionar(new DbParametro("@IP", IP));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("Application_ValidarAcesso", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.Read())
                {
                    oUsuario = new Usuario
                    {
                        IdUsuario = dataReader.GetInt32(dataReader.GetOrdinal("IdUsuario")),
                        Matricula = dataReader.GetInt32(dataReader.GetOrdinal("Matricula")),
                        PrimeiroNome = dataReader.GetString(dataReader.GetOrdinal("PrimeiroNome")),
                        Sobrenome = dataReader.GetString(dataReader.GetOrdinal("Sobrenome")),
                        Ativo = dataReader.GetBoolean(dataReader.GetOrdinal("Ativo")),
                        Login = dataReader.GetString(dataReader.GetOrdinal("Login")),
                        DataCadastro = dataReader.GetDateTime(dataReader.GetOrdinal("DataCadastro")),
                        PrimeiroAcesso = dataReader.GetBoolean(dataReader.GetOrdinal("PrimeiroAcesso")),
                        DataUltimaAlteracaoSenha = dataReader["DataUltimaAlteracaoSenha"] as DateTime?,
                        TentativasAcessoFalho = dataReader.GetInt32(dataReader.GetOrdinal("TentativasAcessoFalho")),
                        DataUltimoBloqueioSenha = dataReader["DataUltimoBloqueioSenha"] as DateTime?,
                        Bloqueado = dataReader.GetBoolean(dataReader.GetOrdinal("Bloqueado")),
                        Email = dataReader.GetString(dataReader.GetOrdinal("Email")),
                        ExibirModal = dataReader.GetBoolean(dataReader.GetOrdinal("ExibirModal"))
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oUsuario;
            }
        }
        public void AtualizarSenha(String senha, Int32 idUsuario)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@Senha", senha));
            parametros.Adicionar(new DbParametro("@IdUsuario", idUsuario));

            _dbHelper.ExecutarNonQuery("UpdateSenha", parametros, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();
        }

        public void AtualizarEmail(String email, Int32 idUsuario)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@Email", email));
            parametros.Adicionar(new DbParametro("@IdUsuario", idUsuario));

            _dbHelper.ExecutarNonQuery("UPDATE Usuarios SET email = @Email WHERE IdUsuario = @IdUsuario", parametros, CommandType.Text);
            _dbHelper.CloseConnection();            
        }
        public void AtualizarExibicaoModal(Int32 idUsuario)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdUsuario", idUsuario));

            _dbHelper.ExecutarNonQuery("UPDATE Usuarios SET ExibirModal = 0 WHERE IdUsuario = @IdUsuario", parametros, CommandType.Text);
            _dbHelper.CloseConnection();
        }
        public Boolean ValidarSenhaAtual(String senha, Int32 idUsuario)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdUsuario", idUsuario));
            parametros.Adicionar(new DbParametro("@Senha", senha));


            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("Application_ValidarSenha", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.HasRows)
                    return true;

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return false;
            }
        }

        public List<Usuario> ObterTodos()
        {
            List<Usuario> listUsuarios = null;

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetListUsuario", CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listUsuarios = new List<Usuario>();
                    while (dataReader.Read())
                    {
                        listUsuarios.Add(new Usuario
                        {
                            IdUsuario = dataReader.GetInt32(0),
                            Matricula = dataReader.GetInt32(1),
                            PrimeiroNome = dataReader.GetString(2),
                            Sobrenome = dataReader.GetString(3),
                            Login = dataReader.GetString(4),
                            IP = dataReader.GetString(8),
                            Ativo = dataReader.GetBoolean(9),
                            Bloqueado = dataReader.GetBoolean(12),
                            Email = dataReader.GetString(13)
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listUsuarios;
            }
        }
    }
}
