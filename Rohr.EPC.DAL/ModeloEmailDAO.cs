using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.EPC.DAL
{
    public class ModeloEmailDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public ModeloEmail ObterPorId(Int32 id)
        {
            ModeloEmail oModeloEmail = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdModeloEmail", id));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetModeloEmailById", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oModeloEmail = new ModeloEmail
                    {
                        IdModeloEmail = Int32.Parse(dataReader["IdModeloEmail"].ToString()),
                        Modelo = dataReader["Modelo"].ToString()
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oModeloEmail;
            }
        }
        public void EnviarEmail(String destinatario, String assunto, String mensagem)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@Destinatario", destinatario));
            parametros.Adicionar(new DbParametro("@Assunto", assunto));
            parametros.Adicionar(new DbParametro("@Mensagem", mensagem));

            if (destinatario != String.Empty)
                _dbHelper.ExecutarScalar("Application_EnviarEmail", parametros, CommandType.StoredProcedure);

            _dbHelper.CloseConnection();
        }

        public DataTable ObterListaEmail(Int32 idFilial, Int32 idPerfilDestino)
        {
            DataTable dataTable = new DataTable();
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdPerfil", idPerfilDestino));
            parametros.Adicionar(new DbParametro("@IdFilial", idFilial));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("Application_GetListaEmail", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                    dataTable.Load(dataReader);

                dataReader.Close();
                _dbHelper.CloseConnection();
                dataReader.Dispose();

                return dataTable;
            }
        }
        public DataTable ObterListaEmail(Int32 idFilial, Int32 idPerfilDestino, Boolean aprovado)
        {
            DataTable dataTable = new DataTable();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT b.PrimeiroNome, b.email ");
            sql.Append("FROM UsuariosXFiliais a ");
            sql.Append("INNER JOIN Usuarios b ON a.IdUsuario = b.IdUsuario ");
            sql.Append("INNER JOIN UsuariosXPerfis c ON c.IdUsuario = b.IdUsuario ");
            sql.Append("INNER JOIN Notificacoes_Usuarios d ON d.IdUsuario = b.IdUsuario ");
            sql.Append(String.Format("WHERE a.IdFilial = {0} ", idFilial));
            sql.Append("AND a.Ativo = 1 ");
            sql.Append("AND b.Ativo = 1 ");
            sql.Append("AND c.Ativo = 1 ");
            sql.Append(String.Format("AND c.IdPerfil = {0} ", idPerfilDestino));

            if (aprovado)
            {
                switch (idPerfilDestino)
                {
                    case 3:
                        sql.Append("AND d.AnaliseJuridico = 1 ");
                        break;
                    case 4:
                        sql.Append("AND d.AnaliseDiretoria = 1 ");
                        break;
                    case 5:
                        sql.Append("AND d.AnaliseSuperintendencia = 1 ");
                        break;
                    case 8:
                        sql.Append("AND d.AnaliseGerente = 1 ");
                        break;
                    case 9:
                        sql.Append("AND d.AnaliseDiretoriaOperacional = 1 ");
                        break;
                    case 10:
                        sql.Append("AND d.AnaliseVicePresidencia = 1 ");
                        break;
                }
            }


            sql.Append("AND b.Email IS NOT NULL ");

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader(sql.ToString(), CommandType.Text))
            {
                if (dataReader.HasRows)
                    dataTable.Load(dataReader);

                dataReader.Close();
                _dbHelper.CloseConnection();
                dataReader.Dispose();

                return dataTable;
            }
        }
    }
}