using System;
using System.Data;

namespace Rohr.Data
{
    public class DbHelper
    {
        private IDbConnection _connection;

        public DbHelper(String nomeConexao)
        {
            _connection = DbGerenciador.PegarConexao(nomeConexao);
        }
        public IDbTransaction BeginTransacao()
        {
            return _connection.BeginTransaction();
        }
        public void CommitTransacao(IDbTransaction transacao)
        {
            transacao.Commit();
            if (_connection == null) return;
            _connection.Close();
            _connection.Dispose();
        }
        public void RollbackTransacao(IDbTransaction transacao)
        {
            transacao.Rollback();
            if (_connection == null) return;
            _connection.Close();
            _connection.Dispose();
        }

        public void CloseConnection()
        {
            _connection.Close();
        }
        public void DisposeComando(IDbCommand comando)
        {
            if (comando == null) return;

            if (comando.Connection != null)
            {
                comando.Connection.Close();
                comando.Connection.Dispose();
            }

            comando.Dispose();
        }

        #region Scalar
        public Object ExecutarScalar(String comandoSql)
        {
            return ExecutarScalar(comandoSql, (IDbTransaction)null);
        }
        public Object ExecutarScalar(String comandoSql, CommandType tipoComando)
        {
            return ExecutarScalar(comandoSql, (IDbTransaction)null, tipoComando);
        }
        public Object ExecutarScalar(String comandoSql, IDbTransaction transacao)
        {
            return ExecutarScalar(comandoSql, transacao, CommandType.Text);
        }
        public Object ExecutarScalar(String comandoSql, DbParametros parametros)
        {
            return ExecutarScalar(comandoSql, parametros, null);
        }
        public Object ExecutarScalar(String comandoSql, IDbTransaction transacao, CommandType tipoComando)
        {
            return ExecutarScalar(comandoSql, new DbParametros(), transacao, tipoComando);
        }
        public Object ExecutarScalar(String comandoSql, DbParametros parametros, CommandType tipoComando)
        {
            return ExecutarScalar(comandoSql, parametros, null, tipoComando);
        }
        public Object ExecutarScalar(String comandoSql, DbParametros parametros, IDbTransaction transacao)
        {
            return ExecutarScalar(comandoSql, parametros, transacao, CommandType.Text);
        }
        public Object ExecutarScalar(String comandoSql, DbParametros parametros, IDbTransaction transacao, CommandType tipoComando)
        {
            Object objectScalar;
            if (transacao != null)
                _connection = transacao.Connection;

            IDbCommand command = new DbComando().PrepararComando(comandoSql, _connection, tipoComando, parametros);
            command.Transaction = transacao;

            try
            {
                objectScalar = command.ExecuteScalar();
            }
            finally
            {
                if (transacao == null)
                {
                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }

                command.Dispose();
            }

            return objectScalar;
        }
        #endregion
        #region NonQuery
        public Int32 ExecutarNonQuery(String comandoSql)
        {
            return ExecutarNonQuery(comandoSql, (IDbTransaction)null);
        }
        public Int32 ExecutarNonQuery(String comandoSql, CommandType tipoComando)
        {
            return ExecutarNonQuery(comandoSql, (IDbTransaction)null, tipoComando);
        }
        public Int32 ExecutarNonQuery(String comandoSql, IDbTransaction transacao)
        {
            return ExecutarNonQuery(comandoSql, transacao, CommandType.Text);
        }
        public Int32 ExecutarNonQuery(String comandoSql, DbParametros parametros)
        {
            return ExecutarNonQuery(comandoSql, parametros, null);
        }
        public Int32 ExecutarNonQuery(String comandoSql, IDbTransaction transacao, CommandType tipoComando)
        {
            return ExecutarNonQuery(comandoSql, new DbParametros(), transacao, tipoComando);
        }
        public Int32 ExecutarNonQuery(String comandoSql, DbParametros parametros, CommandType tipoComando)
        {
            return ExecutarNonQuery(comandoSql, parametros, null, tipoComando);
        }
        public Int32 ExecutarNonQuery(String comandoSql, DbParametros parametros, IDbTransaction transacao)
        {
            return ExecutarNonQuery(comandoSql, parametros, transacao, CommandType.Text);
        }
        public Int32 ExecutarNonQuery(String comandoSql, DbParametros parametros, IDbTransaction transacao, CommandType tipoComando)
        {
            Int32 linhasAfetadas;

            if (transacao != null)
                _connection = transacao.Connection;

            IDbCommand command = new DbComando().PrepararComando(comandoSql, _connection, tipoComando, parametros);
            command.Transaction = transacao;

            try
            {
                linhasAfetadas = command.ExecuteNonQuery();
            }
            finally
            {
                if (transacao == null)
                {
                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }

                command.Dispose();
            }

            return linhasAfetadas;
        }
        #endregion
        #region DataTable
        public DataTable ExecutarDataTable(String comandoSql)
        {
            return ExecutarDataTable(comandoSql, (IDbTransaction)null);
        }
        public DataTable ExecutarDataTable(String comandoSql, CommandType tipoComando)
        {
            return ExecutarDataTable(comandoSql, (IDbTransaction)null, tipoComando);
        }
        public DataTable ExecutarDataTable(String comandoSql, IDbTransaction transacao)
        {
            return ExecutarDataTable(comandoSql, transacao, CommandType.Text);
        }
        public DataTable ExecutarDataTable(String comandoSql, DbParametros parametros)
        {
            return ExecutarDataTable(comandoSql, parametros, null);
        }
        public DataTable ExecutarDataTable(String comandoSql, IDbTransaction transacao, CommandType tipoComando)
        {
            return ExecutarDataTable(comandoSql, new DbParametros(), transacao, tipoComando);
        }
        public DataTable ExecutarDataTable(String comandoSql, DbParametros parametros, CommandType tipoComando)
        {
            return ExecutarDataTable(comandoSql, parametros, null, tipoComando);
        }
        public DataTable ExecutarDataTable(String comandoSql, DbParametros parametros, IDbTransaction transacao)
        {
            return ExecutarDataTable(comandoSql, parametros, transacao, CommandType.Text);
        }
        public DataTable ExecutarDataTable(String comandoSql, DbParametros parametros, IDbTransaction transacao, CommandType tipoComando)
        {
            DataTable dataTable;
            if (transacao != null)
                _connection = transacao.Connection;

            try
            {
                dataTable = new DbComando().RetornarDataTable(comandoSql, _connection, parametros, tipoComando, transacao);
            }
            finally
            {
                if (transacao == null)
                {
                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }
            }
            return dataTable;
        }
        #endregion
        #region DataSet
        public DataSet ExecutarDataSet(String comandoSql)
        {
            return ExecutarDataSet(comandoSql, (IDbTransaction)null);
        }
        public DataSet ExecutarDataSet(String comandoSql, CommandType tipoComando)
        {
            return ExecutarDataSet(comandoSql, (IDbTransaction)null, tipoComando);
        }
        public DataSet ExecutarDataSet(String comandoSql, IDbTransaction transacao)
        {
            return ExecutarDataSet(comandoSql, transacao, CommandType.Text);
        }
        public DataSet ExecutarDataSet(String comandoSql, DbParametros parametros)
        {
            return ExecutarDataSet(comandoSql, parametros, null);
        }
        public DataSet ExecutarDataSet(String comandoSql, IDbTransaction transacao, CommandType tipoComando)
        {
            return ExecutarDataSet(comandoSql, new DbParametros(), transacao, tipoComando);
        }
        public DataSet ExecutarDataSet(String comandoSql, DbParametros parametros, CommandType tipoComando)
        {
            return ExecutarDataSet(comandoSql, parametros, null, tipoComando);
        }
        public DataSet ExecutarDataSet(String comandoSql, DbParametros parametros, IDbTransaction transacao)
        {
            return ExecutarDataSet(comandoSql, parametros, transacao, CommandType.Text);
        }
        public DataSet ExecutarDataSet(String comandoSql, DbParametros parametros, IDbTransaction transacao, CommandType tipoComando)
        {
            DataSet dataSet;
            if (transacao != null)
                _connection = transacao.Connection;

            try
            {
                dataSet = new DbComando().RetornarDataSet(comandoSql, _connection, parametros, tipoComando, transacao);
            }
            finally
            {
                if (transacao == null)
                {
                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }
            }

            return dataSet;
        }
        #endregion
        #region DataReader
        public IDataReader ExecutarDataReader(String comandoSql)
        {
            return ExecutarDataReader(comandoSql, (IDbTransaction)null);
        }
        public IDataReader ExecutarDataReader(String comandoSql, CommandType tipoComando)
        {
            return ExecutarDataReader(comandoSql, (IDbTransaction)null, tipoComando);
        }
        public IDataReader ExecutarDataReader(String comandoSql, IDbTransaction transacao)
        {
            return ExecutarDataReader(comandoSql, transacao, CommandType.Text);
        }
        public IDataReader ExecutarDataReader(String comandoSql, DbParametros parametros)
        {
            return ExecutarDataReader(comandoSql, parametros, null);
        }
        public IDataReader ExecutarDataReader(String comandoSql, IDbTransaction transacao, CommandType tipoComando)
        {
            return ExecutarDataReader(comandoSql, new DbParametros(), transacao, tipoComando);
        }
        public IDataReader ExecutarDataReader(String comandoSql, DbParametros parametros, CommandType tipoComando)
        {
            return ExecutarDataReader(comandoSql, parametros, null, tipoComando);
        }
        public IDataReader ExecutarDataReader(String comandoSql, DbParametros parametros, IDbTransaction transacao)
        {
            return ExecutarDataReader(comandoSql, parametros, transacao, CommandType.Text);
        }
        public IDataReader ExecutarDataReader(String comandoSql, DbParametros parametros, IDbTransaction transacao, CommandType tipoComando)
        {
            IDataReader dataReader;

            if (transacao != null)
                _connection = transacao.Connection;

            IDbCommand command = new DbComando().PrepararComando(comandoSql, _connection, tipoComando, parametros);
            command.Transaction = transacao;

            try
            {
                dataReader = command.ExecuteReader();
            }
            finally
            {
                if (transacao == null)
                {
                    //if (_connection != null)
                    //{
                    //    _connection.Close();
                    //    _connection.Dispose();
                    //}
                }

                command.Dispose();
            }

            return dataReader;
        }
        #endregion
    }
}
