using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class NotificacaoUsuarioDAO
    {
        private readonly DbHelper _dbHelper = new DbHelper("epc");

        public NotificacaoUsuario ObterPorIdUsuario(Int32 idUsuario)
        {
            NotificacaoUsuario oNotificacaoUsuario = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdUsuario", idUsuario));

            using (
                SqlDataReader dataReader =
                    (SqlDataReader)
                        _dbHelper.ExecutarDataReader("GetNotificacaoUsuario", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oNotificacaoUsuario = new NotificacaoUsuario
                    {
                        IdNotificaoUsuario = dataReader.GetInt32(dataReader.GetOrdinal("idNotificacaoUsuario")),
                        AnaliseGerente = dataReader.GetBoolean(dataReader.GetOrdinal("analiseGerente")),
                        AnaliseSuperintendencia = dataReader.GetBoolean(dataReader.GetOrdinal("AnaliseSuperintendencia")),
                        AnaliseDiretoriaOperacional = dataReader.GetBoolean(dataReader.GetOrdinal("analiseDiretoriaOperacional")),
                        AnaliseVicePresidencia = dataReader.GetBoolean(dataReader.GetOrdinal("analiseVicePresidencia")),
                        AnaliseJuridico = dataReader.GetBoolean(dataReader.GetOrdinal("AnaliseJuridico")),
                        AnaliseDiretoria = dataReader.GetBoolean(dataReader.GetOrdinal("AnaliseDiretoria")),
                        ContratoAssinadoCliente = dataReader.GetBoolean(dataReader.GetOrdinal("ContratoAssinadoCliente")),
                        ContratoArquivado = dataReader.GetBoolean(dataReader.GetOrdinal("ContratoArquivado")),
                        ResumoDiario = dataReader.GetBoolean(dataReader.GetOrdinal("ResumoDiario")),
                        ResumoSemanal = dataReader.GetBoolean(dataReader.GetOrdinal("ResumoSemanal")),
                        ResumoMensal = dataReader.GetBoolean(dataReader.GetOrdinal("ResumoMensal")),
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oNotificacaoUsuario;
            }
        }

        public void Atualizar(Usuario usuario, NotificacaoUsuario notificacaoUsuario)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@AnaliseGerente", notificacaoUsuario.AnaliseGerente));
            parametros.Adicionar(new DbParametro("@AnaliseSuperintendencia",notificacaoUsuario.AnaliseSuperintendencia));
            parametros.Adicionar(new DbParametro("@AnaliseDiretoriaOperacional", notificacaoUsuario.AnaliseDiretoriaOperacional));
            parametros.Adicionar(new DbParametro("@AnaliseVicePresidencia", notificacaoUsuario.AnaliseVicePresidencia));
            parametros.Adicionar(new DbParametro("@AnaliseJuridico", notificacaoUsuario.AnaliseJuridico));
            parametros.Adicionar(new DbParametro("@AnaliseDiretoria", notificacaoUsuario.AnaliseDiretoria));
            parametros.Adicionar(new DbParametro("@ContratoAssinadoCliente", notificacaoUsuario.ContratoAssinadoCliente));
            parametros.Adicionar(new DbParametro("@ContratoArquivado", notificacaoUsuario.ContratoArquivado));
            parametros.Adicionar(new DbParametro("@ResumoDiario", notificacaoUsuario.ResumoDiario));
            parametros.Adicionar(new DbParametro("@ResumoSemanal", notificacaoUsuario.ResumoSemanal));
            parametros.Adicionar(new DbParametro("@ResumoMensal", notificacaoUsuario.ResumoMensal));
            parametros.Adicionar(new DbParametro("@IdUsuario", usuario.IdUsuario));
            _dbHelper.ExecutarNonQuery("UpdateNotificacaoUsuario", parametros, CommandType.StoredProcedure);

            _dbHelper.CloseConnection();
        }
    }
}
