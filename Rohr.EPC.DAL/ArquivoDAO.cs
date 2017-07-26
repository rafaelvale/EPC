using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class ArquivoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        public void Adicionar(Arquivo arquivo)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@Nome", arquivo.Nome));
            parametros.Adicionar(new DbParametro("@Extensao", arquivo.Extensao));
            parametros.Adicionar(new DbParametro("@Tipo", arquivo.Tipo));
            parametros.Adicionar(new DbParametro("@Tamanho", arquivo.Tamanho));
            parametros.Adicionar(new DbParametro("@Conteudo", arquivo.Conteudo));
            parametros.Adicionar(new DbParametro("@IdDocumento", arquivo.IdDocumento));

            _dbHelper.ExecutarNonQuery("AddArquivo", parametros, CommandType.StoredProcedure);
        }
        public Arquivo ObterPorIdDocumento(Int32 idDocumento)
        {
            Arquivo oArquivo = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetArquivoByDocumentoId", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oArquivo = new Arquivo
                    {
                        Nome = dataReader.GetString(dataReader.GetOrdinal("Nome")),
                        Tamanho = dataReader.GetInt32(dataReader.GetOrdinal("Tamanho")),
                        Tipo = dataReader.GetString(dataReader.GetOrdinal("Tipo")),
                        Extensao = dataReader.GetString(dataReader.GetOrdinal("Extensao")),
                        Conteudo =  (byte[])dataReader["Conteudo"],
                        IdArquivo = dataReader.GetInt32(dataReader.GetOrdinal("IdArquivo")),
                        IdDocumento = dataReader.GetInt32(dataReader.GetOrdinal("IdDocumento")),
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oArquivo;
            }
        }
    }
}