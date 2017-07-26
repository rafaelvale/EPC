using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class ChavePreenchidaDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public List<ChavePreenchida> ObterTodasPorIdDocumento(Int32 idDocumento)
        {
            List<ChavePreenchida> listChavesPreenchidas = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetChavePreenchida", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listChavesPreenchidas = new List<ChavePreenchida>();
                    while (dataReader.Read())
                    {
                        listChavesPreenchidas.Add(new ChavePreenchida
                        {
                            IdChavePreenchida = Int32.Parse(dataReader["IdChavePreenchida"].ToString()),
                            Texto = dataReader["Texto"].ToString(),
                            DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                            IdChave = dataReader.GetInt32(dataReader.GetOrdinal("IdChave"))
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listChavesPreenchidas;
            }
        }
        public Documento Adicionar(Documento documento)
        {
            IDbTransaction transacao = _dbHelper.BeginTransacao();

            _dbHelper.ExecutarNonQuery(
                String.Format("DELETE FROM Chaves_Preenchidas WHERE IdDocumento = {0}", documento.IdDocumento), transacao, CommandType.Text);

            try
            {
                foreach (ChavePreenchida chavePreenchida in documento.ListChavePreenchida)
                {
                    DbParametros parametros = new DbParametros();
                    parametros.Adicionar(new DbParametro("@Texto", chavePreenchida.Texto));
                    parametros.Adicionar(new DbParametro("@IdDocumento", documento.IdDocumento));
                    parametros.Adicionar(new DbParametro("@IdChave", chavePreenchida.IdChave));

                    chavePreenchida.IdChavePreenchida = (Int32)_dbHelper.ExecutarScalar("AddChavePreenchida", parametros, transacao, CommandType.StoredProcedure);
                }

                _dbHelper.CommitTransacao(transacao);

                return documento;
            }
            catch (Exception)
            {
                _dbHelper.RollbackTransacao(transacao);
                throw;
            }
        }
    }
}
