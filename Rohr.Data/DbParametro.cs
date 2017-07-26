using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Rohr.Data
{
    public class DbParametro
    {
        public String Name { get; set; }
        public Object Value { get; set; }
        public DbType Type { get; set; }
        public ParameterDirection ParameterDirection { get; set; }
        public DbParametro() { }

        public DbParametro(String nome, Object valor)
        {
            Name = nome;
            Value = valor;
            Type = DbType.String;
            ParameterDirection = ParameterDirection.Input;
        }
        public DbParametro(String nome, Object valor, DbType dbType)
        {
            Name = nome;
            Value = valor;
            Type = dbType;
            ParameterDirection = ParameterDirection.Input;
        }
    }

    public class DbParametros : IEnumerable
    {
        readonly List<DbParametro> _listDbParametro = new List<DbParametro>();
        public void Adicionar(DbParametro parametro)
        {
            _listDbParametro.Add(parametro);
        }
        public void Remover(DbParametro parametro)
        {
            _listDbParametro.Remove(parametro);
        }
        public List<DbParametro> Parametros
        {
            get { return _listDbParametro; }
        }
        public IEnumerator GetEnumerator()
        {
            return _listDbParametro.GetEnumerator();
        }
    }
}
