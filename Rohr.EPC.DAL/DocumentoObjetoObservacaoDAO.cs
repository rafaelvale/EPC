using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoObjetoObservacaoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        public DocumentoObjetoObservacao Obter(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            DocumentoObjetoObservacao oDocumentoObjetoObservacao = null;
            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoObservacao", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumentoObjetoObservacao = new DocumentoObjetoObservacao
                    {
                        IdDocumentoObjetoObservacao = Int32.Parse(dataReader["IdDocumentoObjetoObservacao"].ToString()),
                        Observacao = dataReader["Observacao"].ToString(),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoObjetoObservacao;
            }

        }

        public String ObterUltimaObservacao(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            String observacao = String.Empty;
            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoObservacao_Ultimo", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    observacao = dataReader["Observacao"].ToString();
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

            }

            return observacao;
        }
        public void Adicionar(DocumentoObjetoObservacao documentoObjetoObservacao)
        {
            DbParametros parametrosDocumentoObjetoObservacao = new DbParametros();
            parametrosDocumentoObjetoObservacao.Adicionar(new DbParametro("@Observacao", documentoObjetoObservacao.Observacao));
            parametrosDocumentoObjetoObservacao.Adicionar(new DbParametro("@IdDocumento", documentoObjetoObservacao.IdDocumento));

            documentoObjetoObservacao.IdDocumento = Convert.ToInt32(_dbHelper.ExecutarScalar("AddDocumentoObjetoObservacao", parametrosDocumentoObjetoObservacao, CommandType.StoredProcedure));
        }
    }
}
