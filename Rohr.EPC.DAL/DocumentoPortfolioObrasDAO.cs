using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.DAL
{
    public class DocumentoPortfolioObrasDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        public List<DocumentoPortfolioObras> GetDocumentoPortfolio(Int32 IdDoc)
        {
            List<DocumentoPortfolioObras> ListPortfolio = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", IdDoc));

            using(SqlDataReader datareader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoPortfolio", parametros, CommandType.StoredProcedure))
            {
                if (datareader.HasRows)
                {
                    ListPortfolio = new List<DocumentoPortfolioObras>();
                    while (datareader.Read())
                    {
                        ListPortfolio.Add(new DocumentoPortfolioObras
                        {
                            Id = datareader.GetInt32(0),
                            IdDocumento = datareader.GetInt32(1),
                            IdImagem = datareader.GetInt32(2),
                            Url = datareader.GetString(3),
                            DescricaoImagem = datareader.GetString(4),
                            PartePortfolio = datareader.GetInt32(5),
                            DataInclusao = datareader.GetDateTime(6),
                        });
                    }
                }
                datareader.Close();
                datareader.Dispose();
                _dbHelper.CloseConnection();

                return ListPortfolio;
            }
        }

        public List<DocumentoPortfolioObras> GetDocumentoPortfolioParte2(Int32 IdDoc)
        {
            List<DocumentoPortfolioObras> ListPortfolio = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", IdDoc));

            using (SqlDataReader datareader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoPortfolioParte2", parametros, CommandType.StoredProcedure))
            {
                if (datareader.HasRows)
                {
                    ListPortfolio = new List<DocumentoPortfolioObras>();
                    while (datareader.Read())
                    {
                        ListPortfolio.Add(new DocumentoPortfolioObras
                        {
                            Id = datareader.GetInt32(0),
                            IdDocumento = datareader.GetInt32(1),
                            IdImagem = datareader.GetInt32(2),
                            Url = datareader.GetString(3),
                            DescricaoImagem = datareader.GetString(4),
                            PartePortfolio = datareader.GetInt32(5),
                            DataInclusao = datareader.GetDateTime(6),
                        });
                    }
                }
                datareader.Close();
                datareader.Dispose();
                _dbHelper.CloseConnection();

                return ListPortfolio;
            }
        }

        public void Adicionar(DocumentoPortfolioObras DocPort)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@IdDocumento", DocPort.IdDocumento));
            parametros.Adicionar(new DbParametro("@IdImagem", DocPort.IdImagem));
            parametros.Adicionar(new DbParametro("@Url", DocPort.Url));
            parametros.Adicionar(new DbParametro("@Descricao", DocPort.DescricaoImagem));
            _dbHelper.ExecutarNonQuery("AddDocumentoPortfolio", parametros, CommandType.StoredProcedure);
        }

        public void AdicionarParte2(DocumentoPortfolioObras DocPort)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@IdDocumento", DocPort.IdDocumento));
            parametros.Adicionar(new DbParametro("@IdImagem", DocPort.IdImagem));
            parametros.Adicionar(new DbParametro("@Url", DocPort.Url));
            parametros.Adicionar(new DbParametro("@Descricao", DocPort.DescricaoImagem));
            _dbHelper.ExecutarNonQuery("AddDocumentoPortfolioParte2", parametros, CommandType.StoredProcedure);
        }


        public void Update(DocumentoPortfolioObras DocPort)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@IdDocImagem", DocPort.Id));
            parametros.Adicionar(new DbParametro("@Descricao", DocPort.DescricaoImagem));
            _dbHelper.ExecutarNonQuery("UpdateDocumentoPortfolio", parametros, CommandType.StoredProcedure);
        }
        public void UpdateParte2 (DocumentoPortfolioObras DocPort)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@IdDocImagem", DocPort.Id));
            parametros.Adicionar(new DbParametro("@Descricao", DocPort.DescricaoImagem));
            _dbHelper.ExecutarNonQuery("UpdateDocumentoPortfolioParte2", parametros, CommandType.StoredProcedure);
        }

        public void Delete(int Id)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@Id", Id));
            _dbHelper.ExecutarNonQuery("DeleteDocumentoPortfolio", parametros, CommandType.StoredProcedure);
        }
        public void DeleteParte2(int Id)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@Id", Id));
            _dbHelper.ExecutarNonQuery("DeleteDocumentoPortfolioParte2", parametros, CommandType.StoredProcedure);
        }

        public void AdicionarDescrGeral(DocumentoDescricaoGeralPortfolio DocDescPort)
        {
            DbParametros parametrosDocumentoResumo = new DbParametros();
            parametrosDocumentoResumo.Adicionar(new DbParametro("@Descricao", DocDescPort.Descricao));
            parametrosDocumentoResumo.Adicionar(new DbParametro("@IdDocumento", DocDescPort.IdDocumento));

            DocDescPort.IdDocumento = Convert.ToInt32(_dbHelper.ExecutarScalar("AddDocumentoDescricaoPortfolio", parametrosDocumentoResumo, CommandType.StoredProcedure));

        }

        public void AdicionarDescrGeralParte2(DocumentoDescricaoGeralPortfolio DocDescPort)
        {
            DbParametros parametrosDocumentoResumo = new DbParametros();
            parametrosDocumentoResumo.Adicionar(new DbParametro("@Descricao", DocDescPort.Descricao));
            parametrosDocumentoResumo.Adicionar(new DbParametro("@IdDocumento", DocDescPort.IdDocumento));

            DocDescPort.IdDocumento = Convert.ToInt32(_dbHelper.ExecutarScalar("AddDocumentoDescricaoPortfolioParte2", parametrosDocumentoResumo, CommandType.StoredProcedure));

        }

        public DocumentoDescricaoGeralPortfolio ObterDescrGeral(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@idDocumento", idDocumento));

            DocumentoDescricaoGeralPortfolio oDocumentoDescricaoPortfolio = null;
            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoDescricaoPortfolio", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumentoDescricaoPortfolio = new DocumentoDescricaoGeralPortfolio
                    {
                        Id = Int32.Parse(dataReader["Id"].ToString()),
                        Descricao = dataReader["DescricaoGeral"].ToString(),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoDescricaoPortfolio;
            }

        }

        public DocumentoDescricaoGeralPortfolio ObterDescrGeralParte2(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@idDocumento", idDocumento));

            DocumentoDescricaoGeralPortfolio oDocumentoDescricaoPortfolio = null;
            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoDescricaoPortfolioParte2", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumentoDescricaoPortfolio = new DocumentoDescricaoGeralPortfolio
                    {
                        Id = Int32.Parse(dataReader["Id"].ToString()),
                        Descricao = dataReader["DescricaoGeral"].ToString(),
                        IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumentoDescricaoPortfolio;
            }

        }

        public String ObterUltimoDescricao(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            String Resumo = String.Empty;

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoDescricaoGeralPortfolio_ultimo", parametros, CommandType.StoredProcedure))
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


        public String ObterUltimoDescricaoParte2(Int32 idDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            String Resumo = String.Empty;

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoDescricaoGeralPortfolio_ultimoParte2", parametros, CommandType.StoredProcedure))
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

            _dbHelper.ExecutarNonQuery("GetDocumentoPortfolio_UltimaFoto", parametros, CommandType.StoredProcedure);

        }

        public void ObterUltimaFotoParte2(Int32 idDocumento)
        {

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            _dbHelper.ExecutarNonQuery("GetDocumentoPortfolio_UltimaFotoParte2", parametros, CommandType.StoredProcedure);

        }

    }
}
