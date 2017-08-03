using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Rohr.Data
{
    public class DbComando
    {
        internal IDbCommand PrepararComando(String comando, IDbConnection conexao, CommandType commandType)
        {
            IDbCommand command = new SqlCommand();

            command.CommandText = comando;
            command.Connection = conexao;
            command.CommandType = commandType;
            return command;
        }
        internal IDbCommand PrepararComando(String comando, IDbConnection conexao, CommandType commandType, DbParametros parametros)
        {
            IDbCommand command = PrepararComando(comando, conexao, commandType);

            foreach (SqlParameter sqlParameter in from DbParametro p in parametros
                                                  select new SqlParameter(p.Name, p.Value))
                command.Parameters.Add(sqlParameter);

            return command;
        }

        internal DataTable RetornarDataTable(String comandoSql, IDbConnection conexao, DbParametros parametros, CommandType tipoComando, IDbTransaction transacao)
        {
            DataTable dataTable = new DataTable();

            IDbCommand command = PrepararComando(comandoSql, conexao, tipoComando, parametros);
            command.Transaction = transacao;

            
            DbDataAdapter adapter = RetornarDataAdapter(command);
            
            adapter.Fill(dataTable);

            return dataTable;
        }
        internal DataSet RetornarDataSet(String comandoSql, IDbConnection conexao, DbParametros parametros, CommandType tipoComando, IDbTransaction transacao)
        {
            DataSet dataSet = new DataSet();

            IDbCommand command = PrepararComando(comandoSql, conexao, tipoComando, parametros);
            command.Transaction = transacao;

            DbDataAdapter adapter = RetornarDataAdapter(command);
            adapter.Fill(dataSet);

            return null;
        }

        static DbDataAdapter RetornarDataAdapter(IDbCommand command)
        {
            DbDataAdapter dataAdapter = new SqlDataAdapter();

            dataAdapter.SelectCommand = (DbCommand)command;
            return dataAdapter;
        }
    }
}
