using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Rohr.Data
{
    public class DbGerenciador
    {
        internal static IDbConnection PegarConexao(String nomeConexao)
        {
            return PegarConexao(nomeConexao, "sqlserver");
        }
        internal static IDbConnection PegarConexao(String nomeConexao, String servidor)
        {
            IDbConnection conexao = new SqlConnection();

            conexao.ConnectionString = PegarStringConexao(nomeConexao);
            conexao.Open();
            return conexao;
        }
        static String PegarStringConexao(String nomeConexao)
        {
            return ConfigurationManager.ConnectionStrings[nomeConexao].ConnectionString;
        }
    }
}
