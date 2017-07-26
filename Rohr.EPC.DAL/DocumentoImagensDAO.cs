using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoImagensDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        public List<DocumentoImagens> GetDocumentoImagens(Int32 IdDoc)
        {
            List<DocumentoImagens> ListImagens = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", IdDoc));

            using (SqlDataReader datareader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoImagens", parametros, CommandType.StoredProcedure))
            {
                if (datareader.HasRows)
                {
                    ListImagens = new List<DocumentoImagens>();
                    while (datareader.Read())
                    {
                        ListImagens.Add(new DocumentoImagens
                        {
                            IdDocImagem = datareader.GetInt32(0),
                            IdDocumento = datareader.GetInt32(1),
                            IdImagem = datareader.GetInt32(2),
                            Url = datareader.GetString(3),
                            Descricao = datareader.GetString(4),
                            DtInclusao = datareader.GetDateTime(5),

                        });
                    }
                }
                datareader.Close();
                datareader.Dispose();
                _dbHelper.CloseConnection();


                return ListImagens;
            }
        }



        public void Adicionar(DocumentoImagens DocImg)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@IdDocumento", DocImg.IdDocumento));
            parametros.Adicionar(new DbParametro("@IdImagem", DocImg.IdImagem));
            parametros.Adicionar(new DbParametro("@Url", DocImg.Url));
            parametros.Adicionar(new DbParametro("@Descricao", DocImg.Descricao));
            _dbHelper.ExecutarNonQuery("AddDocumentoImagens", parametros, CommandType.StoredProcedure);
        }

        public void Update(DocumentoImagens DocImg)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@IdDocImagem", DocImg.IdDocImagem));
            parametros.Adicionar(new DbParametro("@Descricao", DocImg.Descricao));
            _dbHelper.ExecutarNonQuery("UpdateDocumentoImagens", parametros, CommandType.StoredProcedure);
        }

        public void Delete(int IdDocImagem)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@IdDocImagem", IdDocImagem));
            _dbHelper.ExecutarNonQuery("DeleteDocumentoImagens", parametros, CommandType.StoredProcedure);
        }

        public void AdicionarDescrGeral(DocumentoDescricaoGeralImagens DocDescImg)
        {
            DbParametros parametrosDocumentoResumo = new DbParametros();
            parametrosDocumentoResumo.Adicionar(new DbParametro("@Descricao", DocDescImg.DescricaoGeral));
            parametrosDocumentoResumo.Adicionar(new DbParametro("@IdDocumento", DocDescImg.IdDocumento));

            DocDescImg.IdDocumento = Convert.ToInt32(_dbHelper.ExecutarScalar("AddDocumentoDescricaoImagem", parametrosDocumentoResumo, CommandType.StoredProcedure));

        }

        public DocumentoDescricaoGeralImagens ObterDescrGeral(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@idDocumento", idDocumento));

            DocumentoDescricaoGeralImagens oDocumentoDescricaoImagem = null;
            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoDescricaoImagem", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumentoDescricaoImagem = new DocumentoDescricaoGeralImagens
                    {
                        IdDescrGeral = Int32.Parse(dataReader["IdDescrGeral"].ToString()),
                        DescricaoGeral = dataReader["DescricaoGeral"].ToString(),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoDescricaoImagem;
            }

        }
        public String ObterUltimoDescricao(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            String Resumo = String.Empty;
            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoDescricaoGeralFoto_ultimo", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    Resumo = dataReader["descricao"].ToString();
                }
                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();
            }

            return Resumo;
        }




        public void ObterUltimaFoto(Int32 idDocumento)
        {

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            _dbHelper.ExecutarNonQuery("GetDocumentoImagem_UltimaFoto", parametros, CommandType.StoredProcedure);

        }
    }
}
