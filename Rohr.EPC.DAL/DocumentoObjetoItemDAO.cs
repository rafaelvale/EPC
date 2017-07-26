using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoObjetoItemDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        public List<DocumentoObjetoItem> ObterPorIdObjeto(Int32 idDocumentoObjeto)
        {
            List<DocumentoObjetoItem> listDocumentoObjetoItem = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumentoObjeto", idDocumentoObjeto));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoObjetoItem", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listDocumentoObjetoItem = new List<DocumentoObjetoItem>();
                    while (dataReader.Read())
                    {
                        listDocumentoObjetoItem.Add(new DocumentoObjetoItem
                        {
                            IdDocumentoObjetoItem = Int32.Parse(dataReader["IdDocumentoObjetoItem"].ToString()),
                            DescricaoResumida = dataReader["DescricaoResumida"].ToString(),
                            DescricaoCliente = dataReader["DescricaoCliente"].ToString(),
                            Unidade = dataReader["Unidade"].ToString(),
                            Peso = Decimal.Parse(dataReader["Peso"].ToString()),
                            Quantidade = Decimal.Parse(dataReader["Quantidade"].ToString()),
                            ValorTabelaLocacao = Decimal.Parse(dataReader["ValorTabelaLocacao"].ToString()),
                            ValorTabelaIndenizacao = Decimal.Parse(dataReader["ValorTabelaIndenizacao"].ToString()),
                            ValorPraticadoLocacao = Decimal.Parse(dataReader["ValorPraticadoLocacao"].ToString()),
                            ValorPraticadoIndenizacao = Decimal.Parse(dataReader["ValorPraticadoIndenizacao"].ToString()),
                            Desconto = Decimal.Parse(dataReader["Desconto"].ToString()),
                            OrdemApresentacao = Int32.Parse(dataReader["OrdemApresentacao"].ToString()),
                            Exibir = Boolean.Parse(dataReader["Exibir"].ToString()),
                            CodigoTabelaPrecoSistemaOrigem = Int32.Parse(dataReader["CodigoTabelaPrecoSistemaOrigem"].ToString()),
                            CodigoItemSistemaOrigem = Int32.Parse(dataReader["CodigoItemSistemaOrigem"].ToString()),
                            CodigoSistemaOrigem = Int32.Parse(dataReader["CodigoSistemaOrigem"].ToString()),
                            IdDocumentoObjeto = Int32.Parse(dataReader["IdDocumentoObjeto"].ToString()),
                            CodigoGrupoSistemaOrigem = Int32.Parse(dataReader["CodigoGrupoSistemaOrigem"].ToString())
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listDocumentoObjetoItem;
            }
        }
    }
}
