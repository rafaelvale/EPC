using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;


namespace Rohr.EPC.DAL
{
    public class DocumentoResumoDAO
    {
        readonly DbHelper _dbhelper = new DbHelper("epc");
        public DocumentoResumoProposta Obter(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@idDocumento", idDocumento));

            DocumentoResumoProposta oDocumentoResumoProposta = null;
            using (SqlDataReader dataReader =(SqlDataReader)_dbhelper.ExecutarDataReader("GetDocumentoResumo", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumentoResumoProposta = new DocumentoResumoProposta
                    {
                        IdDocumentoResumo = Int32.Parse(dataReader["IdDocumentoResumo"].ToString()),
                        Resumo = dataReader["Resumo"].ToString(),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbhelper.CloseConnection();

                return oDocumentoResumoProposta;
            }

        }
        
        public String ObterUltimoResumo(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            String Resumo = String.Empty;
            using(SqlDataReader dataReader = (SqlDataReader)_dbhelper.ExecutarDataReader("GetDocumentoResumo_ultimo", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    Resumo = dataReader["Resumo"].ToString();
                }
                dataReader.Close();
                dataReader.Dispose();
                _dbhelper.CloseConnection();
            }

            return Resumo;
        }

        public void Adicionar(DocumentoResumoProposta documentoResumoProposta)
        {
            DbParametros parametrosDocumentoResumo = new DbParametros();
            parametrosDocumentoResumo.Adicionar(new DbParametro("@Resumo", documentoResumoProposta.Resumo));
            parametrosDocumentoResumo.Adicionar(new DbParametro("@IdDocumento", documentoResumoProposta.IdDocumento));

            documentoResumoProposta.IdDocumento = Convert.ToInt32(_dbhelper.ExecutarScalar("AddDocumentoResumoProposta", parametrosDocumentoResumo, CommandType.StoredProcedure));
        }
    }
}
