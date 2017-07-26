using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoBloqueadoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        public void Adicionar(DocumentoBloqueado documentoBloqueado)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", documentoBloqueado.IdDocumento));
            parametros.Adicionar(new DbParametro("@IdUsuario", documentoBloqueado.IdUsuario));
            parametros.Adicionar(new DbParametro("@IdPerfil", documentoBloqueado.IdPerfil));

            _dbHelper.ExecutarNonQuery("AddDocumentoBloqueado", parametros, CommandType.StoredProcedure);
        }
        public void Atualizar(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            _dbHelper.ExecutarNonQuery("UpdateDocumentoBloqueado", parametros, CommandType.StoredProcedure);
        }
        public DataTable ObterListaBloqueios(Int32 idDocumento)
        {
            DbParametros parametrosBloqueio = new DbParametros();
            parametrosBloqueio.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            DataTable oDataTable = _dbHelper.ExecutarDataTable("GetDocumentoBloqueioById", parametrosBloqueio, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return oDataTable;
        }
        public DocumentoBloqueado ObterUltimoBloqueioAtivo(Int32 idDocumento)
        {
            DocumentoBloqueado oDocumentoBloqueado = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetUltimoBloqueioDocumentoById", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumentoBloqueado = new DocumentoBloqueado
                    {
                        IdUsuario = Int32.Parse(dataReader["idUsuario"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoBloqueado;
            }
        }
    }
}
