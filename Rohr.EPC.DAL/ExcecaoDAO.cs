using Rohr.Data;
using System;
using System.Data;

namespace Rohr.EPC.DAL
{
    public class ExcecaoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public void Adicionar(Exception exception, String url, Int32 idUsuario, Int32 idPerfil, Int32 idTipoExcecao)
        {
            Object stackTrace = DBNull.Value;
            if (!String.IsNullOrEmpty(exception.StackTrace))
                exception.StackTrace.ToString();

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@Message", exception.Message));
            parametros.Adicionar(new DbParametro("@StackTrace", stackTrace));
            parametros.Adicionar(new DbParametro("@URL", url));
            parametros.Adicionar(new DbParametro("@IdUsuario", idUsuario));
            parametros.Adicionar(new DbParametro("@IdPerfil", idPerfil));
            parametros.Adicionar(new DbParametro("@IdTipoExcecao", idTipoExcecao));
            _dbHelper.ExecutarNonQuery("Application_AddExcecao", parametros, CommandType.StoredProcedure);

            _dbHelper.CloseConnection();
        }
    }
}
