using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class AuditoriaLogDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public List<AuditoriaLog> Obter(Usuario usuario)
        {
            List<AuditoriaLog> listLog = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdUsuario", usuario.IdUsuario));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetAuditoriaLog", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listLog = new List<AuditoriaLog>();
                    while (dataReader.Read())
                    {
                        listLog.Add(new AuditoriaLog
                        {
                            IdUsuario = dataReader.GetInt32(dataReader.GetOrdinal("idUsuario")),
                            IdTipoLog = dataReader.GetInt32(dataReader.GetOrdinal("idTipoLog")),
                            DataCadastro = dataReader.GetDateTime(dataReader.GetOrdinal("dataCadastro")),
                            Descricao = dataReader.GetString(dataReader.GetOrdinal("descricao")),
                            Ip = dataReader.GetString(dataReader.GetOrdinal("IP")),
                            NomeMaquina = dataReader.GetString(dataReader.GetOrdinal("nomeMaquina")),
                            AcessoMobile = dataReader.GetBoolean(dataReader.GetOrdinal("browserIsMobileDevice"))
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listLog;
            }
        }
        public void Adicionar(Int32 idTipoLog, Int32 idUsuario, Int32 idPerfil, String descricao, String ip, String nomeMaquina, String userWindows,
                              String browserType, String browserName, String browserVersion, Boolean browserIsMobileDevice)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdUsuario", idUsuario));
            parametros.Adicionar(new DbParametro("@IdPerfil", idPerfil));
            parametros.Adicionar(new DbParametro("@IdTipoLog", idTipoLog));
            parametros.Adicionar(new DbParametro("@Descricao", descricao));
            parametros.Adicionar(new DbParametro("@IP", ip));
            parametros.Adicionar(new DbParametro("@NomeMaquina", nomeMaquina));
            parametros.Adicionar(new DbParametro("@UserWindows", userWindows));
            parametros.Adicionar(new DbParametro("@BrowserType", browserType));
            parametros.Adicionar(new DbParametro("@BrowserName", browserName));
            parametros.Adicionar(new DbParametro("@BrowserVersion", browserVersion));
            parametros.Adicionar(new DbParametro("@BrowserIsMobileDevice", browserIsMobileDevice));

            _dbHelper.ExecutarNonQuery("Application_AddAuditoriaLog", parametros, CommandType.StoredProcedure);
        }
    }
}
