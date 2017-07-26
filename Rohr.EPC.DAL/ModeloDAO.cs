using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.EPC.DAL
{
    public class ModeloDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public Modelo ObterPorId(Int32 idModelo)
        {
            Modelo oModelo = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModelo", idModelo));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetModeloById", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oModelo = new Modelo
                    {
                        IdModelo = dataReader.GetInt32(dataReader.GetOrdinal("IdModelo")),
                        Titulo = dataReader.GetString(dataReader.GetOrdinal("Titulo")),
                        OrcamentoObrigatorio = dataReader.GetBoolean(dataReader.GetOrdinal("OrcamentoObrigatorio")),
                        DataCadastro = dataReader.GetDateTime(dataReader.GetOrdinal("DataCadastro")),
                        Versao = dataReader.GetInt32(dataReader.GetOrdinal("Versao")),
                        ModeloTipo = new ModeloTipo(dataReader.GetInt32(dataReader.GetOrdinal("IdModeloTipo"))),
                        ModeloMeta = new ModeloMeta(dataReader.GetInt32(dataReader.GetOrdinal("IdModeloMeta"))),
                        Segmento = new Segmento(dataReader.GetInt32(dataReader.GetOrdinal("IdSegmento"))),
                        CondicoesGeraisObrigatorio = dataReader.GetBoolean(dataReader.GetOrdinal("CondicoesGeraisObrigatorio"))
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oModelo;
            }
        }

        public List<Modelo> ObterTodosModeloProposta(Boolean transporteVertical)
        {
            List<Modelo> listModelos = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModeloTipo", 1));
            parametros.Adicionar(new DbParametro("@TransporteVertical", transporteVertical));
            parametros.Adicionar(new DbParametro("@IdModelo", DBNull.Value));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetModelo", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listModelos = new List<Modelo>();
                    while (dataReader.Read())
                    {
                        listModelos.Add(new Modelo
                        {
                            IdModelo = dataReader.GetInt32(dataReader.GetOrdinal("idModelo")),
                            Titulo = dataReader.GetString(dataReader.GetOrdinal("titulo"))
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listModelos;
            }
        }
        public List<Modelo> ObterTodosModeloContrato(Int32 idModelo)
        {
            List<Modelo> listModelos = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModeloProposta", idModelo));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetModeloContrato", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listModelos = new List<Modelo>();
                    while (dataReader.Read())
                    {
                        listModelos.Add(new Modelo
                        {
                            IdModelo = dataReader.GetInt32(dataReader.GetOrdinal("idModelo")),
                            Titulo = dataReader.GetString(dataReader.GetOrdinal("titulo"))
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

            }
            return listModelos;
        }

        public List<Modelo> ObterTodosModelosRelatorio()
        {
            List<Modelo> listModelos = null;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * ");
            query.Append("FROM dbo.Modelos m ");
            query.Append("WHERE m.IdModelo IN (SELECT MAX(IdModelo) FROM dbo.Modelos WHERE Ativo = 1 GROUP BY titulo) ");
            query.Append("AND m.IdModeloTipo != 3 ");
            query.Append("ORDER BY titulo ");

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader(query.ToString(), CommandType.Text))
            {
                if (dataReader.HasRows)
                {
                    listModelos = new List<Modelo>();
                    while (dataReader.Read())
                    {
                        listModelos.Add(new Modelo
                        {
                            IdModelo = Int32.Parse(dataReader["idModelo"].ToString()),
                            Titulo = dataReader["titulo"].ToString(),
                            DataCadastro = DateTime.Parse(dataReader["dataCadastro"].ToString()),
                            ModeloMeta = new ModeloMetaDAO().ObterPorId(Int32.Parse(dataReader["idModeloMeta"].ToString())),
                            CondicoesGeraisObrigatorio = Boolean.Parse(dataReader["CondicoesGeraisObrigatorio"].ToString()),
                            Versao = Int32.Parse(dataReader["versao"].ToString())
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listModelos;
            }
        }
    }
}
