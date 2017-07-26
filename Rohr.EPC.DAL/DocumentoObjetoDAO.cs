using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rohr.EPC.DAL
{
    public class DocumentoObjetoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");

        public void AdicionarDocumentoObjeto(Documento documento)
        {
            IDbTransaction transacao = _dbHelper.BeginTransacao();

            try
            {
                _dbHelper.ExecutarNonQuery(String.Format("DELETE FROM Documentos_ObjetosItens WHERE idDocumentoObjeto IN ( " +
                    "SELECT a.IdDocumentoObjeto " +
                    "FROM dbo.Documentos_Objetos a " +
                    "INNER JOIN Documentos_ObjetosItens b ON a.IdDocumentoObjeto = b.IdDocumentoObjeto " +
                    "WHERE a.idDocumento = {0}) ", documento.IdDocumento), transacao, CommandType.Text);

                _dbHelper.ExecutarNonQuery(
                    String.Format("DELETE FROM Documentos_Objetos WHERE IdDocumento = {0}", documento.IdDocumento),
                    transacao, CommandType.Text);

                _dbHelper.ExecutarNonQuery(
                    String.Format("DELETE FROM dbo.Documentos_ObjetosDetalhesContratos WHERE IdDocumento = {0}", documento.IdDocumento),
                    transacao, CommandType.Text);


                DbParametros oDbParametros = new DbParametros();
                oDbParametros.Adicionar(new DbParametro("@IdDocumento", documento.IdDocumento));
                oDbParametros.Adicionar(new DbParametro("@ValorFaturamentoMensal", documento.ValorFaturamentoMensal));
                oDbParametros.Adicionar(new DbParametro("@ValorNegocio", documento.ValorNegocio));
                oDbParametros.Adicionar(new DbParametro("@PercentualDesconto", documento.PercentualDesconto));
                _dbHelper.ExecutarNonQuery("UPDATE Documentos SET ValorFaturamentoMensal = @ValorFaturamentoMensal, ValorNegocio = @ValorNegocio, PercentualDesconto = @PercentualDesconto WHERE IdDocumento = @IdDocumento", oDbParametros, transacao, CommandType.Text);

                foreach (DocumentoObjeto documentoObjeto in documento.ListDocumentoObjeto)
                {
                    DbParametros parametrosDocumentoObjeto = new DbParametros();
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@DescricaoObjeto", documentoObjeto.DescricaoObjeto));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirDescricaoResumida", documentoObjeto.ExibirDescricaoResumida));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirDescricaoCliente", documentoObjeto.ExibirDescricaoCliente));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirUnidade", documentoObjeto.ExibirUnidade));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirPeso", documentoObjeto.ExibirPeso));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirQuantidade", documentoObjeto.ExibirQuantidade));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirValorTabelaLocacao", documentoObjeto.ExibirValorTabelaLocacao));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirValorPraticado", documentoObjeto.ExibirValorPraticadoLocacao));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirValorTabelaIndenizacao", documentoObjeto.ExibirValorTabelaIndenizacao));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirValorPraticadoIndenizacao", documentoObjeto.ExibirValorPraticadoIndenizacao));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirDesconto", documentoObjeto.ExibirDesconto));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirValorTotalItem", documentoObjeto.ExibirValorTotalItem));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirPesoTotalItem", documentoObjeto.ExibirPesoTotalItem));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirPesoTotalObjeto", documentoObjeto.ExibirSubTotalPeso));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@CodigoSistemaOrigem", documentoObjeto.CodigoSistemaOrigem));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@OrdemApresentacao", documentoObjeto.OrdemApresentacao));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@IdDocumento", documento.IdDocumento));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@CodigoOrcamentoSistemaOrigem", documentoObjeto.IdOrcamentoSistemaOrigem));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@CodigoRevOrcamentoSistemaOrigem", documentoObjeto.RevisaoOrcamentoSistemaOrigem));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@Volume", documentoObjeto.Volume));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@Area", documentoObjeto.Area));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@PrevisaoInicio", documentoObjeto.PrevisaoInicio.Year == 1 ? DBNull.Value : (object)documentoObjeto.PrevisaoInicio));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@PrevisaoTermino", documentoObjeto.PrevisaoTermino.Year == 1 ? DBNull.Value : (object)documentoObjeto.PrevisaoTermino));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirPrevisaoUtilizacao", documentoObjeto.ExibirPrevisaoUtilizacao));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirFaturamentoMensal", documentoObjeto.ExibirFaturamentoMensal));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirFaturamentoMensalObjeto", documentoObjeto.ExibirFaturamentoMensalObjeto));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirValorNegocio", documentoObjeto.ExibirValorNegocio));
                    parametrosDocumentoObjeto.Adicionar(new DbParametro("@ExibirValorNegocioObjeto", documentoObjeto.ExibirValorNegocioObjeto));

                    Int32 idDocumentoObjeto = Convert.ToInt32(_dbHelper.ExecutarScalar("AddDocumentoObjeto", parametrosDocumentoObjeto, transacao, CommandType.StoredProcedure));

                    foreach (DocumentoObjetoItem documentoObjetoItem in documentoObjeto.DocumentoObjetoItem)
                    {
                        DbParametros parametrosDocumentoObjetoItem = new DbParametros();
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@CodigoSistemaOrigem", documentoObjetoItem.CodigoSistemaOrigem));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@DescricaoResumida", documentoObjetoItem.DescricaoResumida));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@DescricaoCliente", documentoObjetoItem.DescricaoCliente));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@Unidade", documentoObjetoItem.Unidade));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@Peso", documentoObjetoItem.Peso));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@Quantidade", documentoObjetoItem.Quantidade));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@ValorTabelaLocacao", documentoObjetoItem.ValorTabelaLocacao));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@ValorPraticadoLocacao", documentoObjetoItem.ValorPraticadoLocacao));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@ValorTabelaIndenizacao", documentoObjetoItem.ValorTabelaIndenizacao));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@ValorPraticadoIndenizacao", documentoObjetoItem.ValorPraticadoIndenizacao));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@Desconto", documentoObjetoItem.Desconto));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@OrdemApresentacao", documentoObjetoItem.OrdemApresentacao));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@Exibir", documentoObjetoItem.Exibir));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@IdDocumentoObjeto", idDocumentoObjeto));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@CodigoTabelaPrecoSistemaOrigem", documentoObjetoItem.CodigoTabelaPrecoSistemaOrigem));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@CodigoItemSistemaOrigem", documentoObjetoItem.CodigoItemSistemaOrigem));
                        parametrosDocumentoObjetoItem.Adicionar(new DbParametro("@CodigoGrupoSistemaOrigem", documentoObjetoItem.CodigoGrupoSistemaOrigem));

                        _dbHelper.ExecutarNonQuery("AddDocumentoObjetoItem", parametrosDocumentoObjetoItem, transacao, CommandType.StoredProcedure);
                    }
                }

                _dbHelper.CommitTransacao(transacao);

                new DocumentoObjetoObservacaoDAO().Adicionar(documento.DocumentoObjetoObservacao);
            }
            catch (Exception)
            {
                _dbHelper.RollbackTransacao(transacao);
                throw;
            }
        }
        public List<DocumentoObjeto> ObterPorIdDocumento(Int32 idDocumento)
        {
            List<DocumentoObjeto> listDocumentoObjeto = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumento));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetDocumentoObjeto", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {
                    listDocumentoObjeto = new List<DocumentoObjeto>();
                    while (dataReader.Read())
                    {
                        listDocumentoObjeto.Add(new DocumentoObjeto
                        {
                            IdDocumentoObjeto = Int32.Parse(dataReader["idDocumentoObjeto"].ToString()),
                            DescricaoObjeto = dataReader["descricaoObjeto"].ToString(),
                            ExibirDescricaoResumida = Boolean.Parse(dataReader["exibirDescricaoResumida"].ToString()),
                            ExibirDescricaoCliente = Boolean.Parse(dataReader["exibirDescricaoCliente"].ToString()),
                            ExibirUnidade = Boolean.Parse(dataReader["exibirUnidade"].ToString()),
                            ExibirPeso = Boolean.Parse(dataReader["exibirPeso"].ToString()),
                            ExibirQuantidade = Boolean.Parse(dataReader["exibirQuantidade"].ToString()),
                            ExibirValorTabelaLocacao = Boolean.Parse(dataReader["exibirValorTabelaLocacao"].ToString()),
                            ExibirValorTabelaIndenizacao = Boolean.Parse(dataReader["exibirValorTabelaIndenizacao"].ToString()),
                            ExibirValorPraticadoLocacao = Boolean.Parse(dataReader["exibirValorPraticado"].ToString()),
                            ExibirValorPraticadoIndenizacao = Boolean.Parse(dataReader["exibirValorPraticadoIndenizacao"].ToString()),
                            ExibirDesconto = Boolean.Parse(dataReader["exibirDesconto"].ToString()),
                            ExibirValorTotalItem = Boolean.Parse(dataReader["exibirValorTotalItem"].ToString()),
                            ExibirPesoTotalItem = Boolean.Parse(dataReader["exibirPesoTotalItem"].ToString()),
                            ExibirSubTotalPeso = Boolean.Parse(dataReader["exibirPesoTotalObjeto"].ToString()),
                            DataCadastro = DateTime.Parse(dataReader["DataCadastro"].ToString()),
                            CodigoSistemaOrigem = Int32.Parse(dataReader["CodigoSistemaOrigem"].ToString()),
                            OrdemApresentacao = Int32.Parse(dataReader["OrdemApresentacao"].ToString()),
                            IdDocumento = Int32.Parse(dataReader["IdDocumento"].ToString()),
                            IdOrcamentoSistemaOrigem = Int32.Parse(dataReader["CodigoOrcamentoSistemaOrigem"].ToString()),
                            RevisaoOrcamentoSistemaOrigem = Int32.Parse(dataReader["CodigoRevOrcamentoSistemaOrigem"].ToString()),
                            Volume = Decimal.Parse(dataReader["Volume"].ToString()),
                            Area = Decimal.Parse(dataReader["Area"].ToString()),
                            DocumentoObjetoItem = new DocumentoObjetoItemDAO().ObterPorIdObjeto(Int32.Parse(dataReader["IdDocumentoObjeto"].ToString())),
                            PrevisaoInicio = String.IsNullOrEmpty(dataReader["PrevisaoInicio"].ToString()) ? DateTime.MinValue : DateTime.Parse(dataReader["PrevisaoInicio"].ToString()),
                            PrevisaoTermino = String.IsNullOrEmpty(dataReader["PrevisaoTermino"].ToString()) ? DateTime.MinValue : DateTime.Parse(dataReader["PrevisaoTermino"].ToString()),
                            ExibirPrevisaoUtilizacao = Boolean.Parse(dataReader["ExibirPrevisaoUtilizacao"].ToString()),
                            ExibirFaturamentoMensal = Boolean.Parse(dataReader["ExibirFaturamentoMensal"].ToString()),
                            ExibirFaturamentoMensalObjeto = Boolean.Parse(dataReader["ExibirFaturamentoMensalObjeto"].ToString()),
                            ExibirValorNegocio = Boolean.Parse(dataReader["ExibirValorNegocio"].ToString()),
                            ExibirValorNegocioObjeto = Boolean.Parse(dataReader["ExibirValorNegocioObjeto"].ToString())
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return listDocumentoObjeto;
            }
        }
    }
}
