using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoComercialDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public DocumentoComercial ObterPorIdDocumento(Int32 idDocumento)
        {
            DocumentoComercial oDocumentoComercial = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoComercial", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumentoComercial = new DocumentoComercial
                    {
                        IdDocumentoComercial = Int32.Parse(dataReader["IdDocumentoComercial"].ToString()),
                        PrimeiroNome = dataReader["PrimeiroNome"].ToString(),
                        CodigoSistemaOrigem = Int32.Parse(dataReader["CodigoComercialOrigem"].ToString()),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString()),
                        Sobrenome = dataReader["Sobrenome"].ToString(),
                        Email = dataReader["Email"].ToString(),
                        Departamento = dataReader["Departamento"].ToString(),
                        Telefone = dataReader["Telefone"].ToString(),
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoComercial;
            }
        }
        public void Atualizar(Documento documento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@PrimeiroNome", documento.DocumentoComercial.PrimeiroNome));
            parametros.Adicionar(new DbParametro("@CodigoComercialOrigem", documento.DocumentoComercial.CodigoSistemaOrigem));
            parametros.Adicionar(new DbParametro("@Sobrenome", documento.DocumentoComercial.Sobrenome));
            parametros.Adicionar(new DbParametro("@Email", documento.DocumentoComercial.Email));
            parametros.Adicionar(new DbParametro("@Telefone", documento.DocumentoComercial.Telefone));
            parametros.Adicionar(new DbParametro("@Departamento", documento.DocumentoComercial.Departamento));
            parametros.Adicionar(new DbParametro("@IdDocumentoComercial", documento.DocumentoComercial.IdDocumentoComercial));

            _dbHelper.ExecutarNonQuery("UpdateDocumentoComercial", parametros, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();
        }
    }
}
