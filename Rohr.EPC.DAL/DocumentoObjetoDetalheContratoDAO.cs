using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoObjetoDetalheContratoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        public List<DocumentoObjetoDetalheContrato> Obter(Int32 idDocumentoObjeto)
        {
            List<DocumentoObjetoDetalheContrato> listDocumentoObjetoDetalheContrato = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumentoObjeto));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoObjetoDetalheContrato", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listDocumentoObjetoDetalheContrato = new List<DocumentoObjetoDetalheContrato>();
                    while (dataReader.Read())
                    {
                        listDocumentoObjetoDetalheContrato.Add(new DocumentoObjetoDetalheContrato
                        {
                            IdDocumentoOjetoDetalheContrato = Int32.Parse(dataReader["IdDocumentoObjetoDetalheContrato"].ToString()),
                            Descricao = dataReader["Descricao"].ToString(),
                            ValorLocacao = Decimal.Parse(dataReader["ValorLocacao"].ToString()),
                            ValorIndenizacao = Decimal.Parse(dataReader["ValorIndenizacao"].ToString()),
                            Unidade = dataReader["Unidade"].ToString(),
                            Tipo = dataReader["Tipo"].ToString(),
                            IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString()),
                            Totalizador = Boolean.Parse(dataReader["Totalizador"].ToString()),
                            DescricaoResumida = dataReader["DescricaoResumida"].ToString(),
                            CodigoItemSistemaOrigem = Int32.Parse(dataReader["CodigoItemSistemaOrigem"].ToString()),
                            CodigoGrupoItemSistemaOrigem = Int32.Parse(dataReader["CodigoGrupoItemSistemaOrigem"].ToString()),
                            ExibirVUI = Boolean.Parse(dataReader["ExibirVUI"].ToString()),
                            ExibirVUL = Boolean.Parse(dataReader["ExibirVUL"].ToString())
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listDocumentoObjetoDetalheContrato;
            }
        }
        public void AdicionarDocumentoObjeto(Documento documento, List<DocumentoObjetoDetalheContrato> listDocumentoObjetoDetalheContrato)
        {
            IDbTransaction transacao = _dbHelper.BeginTransacao();

            try
            {
                _dbHelper.ExecutarNonQuery(
                    String.Format("DELETE FROM Documentos_ObjetosDetalhesContratos WHERE IdDocumento = {0}", documento.IdDocumento),
                    transacao, CommandType.Text);

                foreach (DocumentoObjetoDetalheContrato item in listDocumentoObjetoDetalheContrato)
                {
                    DbParametros parametrosDocumentoObjetoDetalhes = new DbParametros();
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@Descricao", item.Descricao));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@ValorLocacao", item.ValorLocacao));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@ValorIndenizacao", item.ValorIndenizacao));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@Unidade", item.Unidade));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@Tipo", item.Tipo));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@IdDocumento", documento.IdDocumento));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@Totalizador", item.Totalizador));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@Peso", item.Peso));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@CodigoItemSistemaOrigem", item.CodigoItemSistemaOrigem));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@DescricaoResumida", item.DescricaoResumida));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@CodigoGrupoItemSistemaOrigem", item.CodigoGrupoItemSistemaOrigem));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@ExibirVUI", item.ExibirVUI));
                    parametrosDocumentoObjetoDetalhes.Adicionar(new DbParametro("@ExibirVUL", item.ExibirVUL));

                    _dbHelper.ExecutarNonQuery("AddDocumentoObjetosDetalheContrato", parametrosDocumentoObjetoDetalhes, transacao, CommandType.StoredProcedure);
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
