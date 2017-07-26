using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoObraDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public DocumentoObra ObterPorIdDocumento(Int32 idDocumento)
        {
            DocumentoObra oDocumentoObra = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoObra", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumentoObra = new DocumentoObra
                    {
                        IdDocumentoObra = Int32.Parse(dataReader["idDocumentoObra"].ToString()),
                        IdDocumento = Int32.Parse(dataReader["idDocumento"].ToString()),
                        Nome = dataReader["nome"].ToString(),
                        Endereco = dataReader["endereco"].ToString(),
                        Bairro = dataReader["bairro"].ToString(),
                        Cidade = dataReader["cidade"].ToString(),
                        Estado = dataReader["estado"].ToString(),
                        CEP = dataReader["cep"].ToString()
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoObra;
            }
        }
        public void Atualizar(Documento documento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumentoObra", documento.DocumentoObra.IdDocumentoObra));
            parametros.Adicionar(new DbParametro("@Nome", documento.DocumentoObra.Nome));
            parametros.Adicionar(new DbParametro("@Endereco", documento.DocumentoObra.Endereco));
            parametros.Adicionar(new DbParametro("@Bairro", documento.DocumentoObra.Bairro));
            parametros.Adicionar(new DbParametro("@Cidade", documento.DocumentoObra.Cidade));
            parametros.Adicionar(new DbParametro("@Estado", documento.DocumentoObra.Estado));
            parametros.Adicionar(new DbParametro("@CEP", documento.DocumentoObra.CEP));
            _dbHelper.ExecutarNonQuery("UpdateDocumentoObra", parametros, CommandType.StoredProcedure);
        }
    }
}
