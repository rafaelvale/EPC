using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.EPC.DAL
{
    public class PartePreenchidaDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public PartePreenchida ObterPorIdDocumento(Int32 idDocumento)
        {
            PartePreenchida oPartePreenchida = null;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT idPartePreenchida, texto, idDocumento, idParte, dataCadastro ");
            query.Append("FROM partes_preenchidas ");
            query.Append("WHERE atual = 1 AND idDocumento = ?");

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@value", idDocumento));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader(query.ToString(), parametros, CommandType.Text)))
            {
                if (dataReader.Read())
                {
                    oPartePreenchida = new PartePreenchida
                    {
                        IdPartePreenchida = Int32.Parse(dataReader["idPartePreenchida"].ToString()),
                        Texto = dataReader["texto"].ToString(),
                        IdDocumento = Int32.Parse(dataReader["idDocumento"].ToString()),
                        IdParte = Int32.Parse(dataReader["idParte"].ToString()),
                        DataCadastro = DateTime.Parse(dataReader["dataCadastro"].ToString())
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oPartePreenchida;
            }
        }
        public List<PartePreenchida> ObterTodasPorIdDocumento(Int32 idDocumento)
        {
            List<PartePreenchida> listPartesPreenchidas = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelper.ExecutarDataReader("GetPartePreenchida", parametros, CommandType.StoredProcedure)))
            {
                if (dataReader.HasRows)
                {
                    listPartesPreenchidas = new List<PartePreenchida>();
                    while (dataReader.Read())
                    {
                        listPartesPreenchidas.Add(new PartePreenchida
                        {
                            IdPartePreenchida = Int32.Parse(dataReader["idPartePreenchida"].ToString()),
                            Texto = dataReader["texto"].ToString(),
                            IdDocumento = Int32.Parse(dataReader["idDocumento"].ToString()),
                            IdParte = Int32.Parse(dataReader["idParte"].ToString())
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listPartesPreenchidas;
            }
        }

        public void Adicionar(Documento documento)
        {
            IDbTransaction transacao = _dbHelper.BeginTransacao();

            try
            {
                _dbHelper.ExecutarNonQuery(
                    String.Format("DELETE FROM Partes_Preenchidas WHERE IdDocumento = {0}", documento.IdDocumento),
                    transacao, CommandType.Text);

                foreach (PartePreenchida partePreenchida in documento.ListPartePreenchida)
                {
                    DbParametros parametrosPartePreenchida = new DbParametros();
                    parametrosPartePreenchida.Adicionar(new DbParametro("@Texto", partePreenchida.Texto));
                    parametrosPartePreenchida.Adicionar(new DbParametro("@IdDocumento", documento.IdDocumento));
                    parametrosPartePreenchida.Adicionar(new DbParametro("@IdParte", partePreenchida.IdParte));

                    partePreenchida.IdPartePreenchida =
                        Convert.ToInt32(_dbHelper.ExecutarScalar("AddPartePreenchida", parametrosPartePreenchida, transacao, CommandType.StoredProcedure));
                }

                _dbHelper.CommitTransacao(transacao);
            }
            catch (Exception)
            {
                _dbHelper.RollbackTransacao(transacao);
                throw;
            }
        }
    }
}
