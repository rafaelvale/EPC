using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoClienteDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public DocumentoCliente ObterPorId(Int32 id)
        {
            DocumentoCliente oDocumentoCliente = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", id));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoCliente", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumentoCliente = new DocumentoCliente
                    {
                        IdDocumentoCliente = Int32.Parse(dataReader["idDocumentoCliente"].ToString()),
                        Nome = dataReader["nome"].ToString(),
                        CpfCnpj = dataReader["cpf_cnpj"].ToString(),
                        RgIe = dataReader["rg_ie"].ToString(),
                        CodigoOrigem = Int32.Parse(dataReader["CodigoClienteOrigem"].ToString()),
                        Endereco = dataReader["endereco"].ToString(),
                        Bairro = dataReader["bairro"].ToString(),
                        Cidade = dataReader["cidade"].ToString(),
                        Estado = dataReader["estado"].ToString(),
                        Cep = dataReader["cep"].ToString(),
                        IdDocumento = Int32.Parse(dataReader["idDocumento"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoCliente;
            }
        }
        public void Atualizar(Documento documento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@CodigoClienteOrigem", documento.DocumentoCliente.CodigoOrigem));
            parametros.Adicionar(new DbParametro("@CPF_CNPJ", documento.DocumentoCliente.CpfCnpj));
            parametros.Adicionar(new DbParametro("@RG_IE", documento.DocumentoCliente.RgIe));
            parametros.Adicionar(new DbParametro("@Nome", documento.DocumentoCliente.Nome));
            parametros.Adicionar(new DbParametro("@IdDocumentoCliente", documento.DocumentoCliente.IdDocumentoCliente));
            parametros.Adicionar(new DbParametro("@Endereco", documento.DocumentoCliente.Endereco + " - " + documento.DocumentoCliente.Numero));
            parametros.Adicionar(new DbParametro("@Bairro", documento.DocumentoCliente.Bairro));
            parametros.Adicionar(new DbParametro("@Cidade", documento.DocumentoCliente.Cidade));
            parametros.Adicionar(new DbParametro("@Estado", documento.DocumentoCliente.Estado));
            parametros.Adicionar(new DbParametro("@CEP", documento.DocumentoCliente.Cep));
            _dbHelper.ExecutarNonQuery("UpdateDocumentoCliente", parametros, CommandType.StoredProcedure);
        }
    }
}
