using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rohr.EPC.Business
{
    public class DocumentoObjetoDetalheContratoBusiness
    {
        public List<DocumentoObjetoDetalheContrato> Obter(Int32 idDocumentoObjeto)
        {
            return new DocumentoObjetoDetalheContratoDAO().Obter(idDocumentoObjeto);
        }

        public void AdicionarDocumentoObjetoDetalheContrato(Documento documento, List<ItemPreco> itemPreco)
        {
            new DocumentoObjetoDetalheContratoDAO().AdicionarDocumentoObjeto(documento, TratarPrecosContrato(documento, itemPreco));
        }

        private List<DocumentoObjetoDetalheContrato> TratarPrecosContrato(Documento documento, List<ItemPreco> itemPreco)
        {
            List<DocumentoObjetoDetalheContrato> listAux = new List<DocumentoObjetoDetalheContrato>();

            List<DocumentoObjetoItem> listSubgrupos = new List<DocumentoObjetoItem>();
            List<DocumentoObjetoItem> listItens = new List<DocumentoObjetoItem>();

            foreach (DocumentoObjeto objeto in documento.ListDocumentoObjeto)
            {
                foreach (DocumentoObjetoItem item in objeto.DocumentoObjetoItem)
                {
                    DocumentoObjetoItem item1 = item;

                    if (String.Compare(item.DescricaoResumida, "subgrupo", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        var res = listSubgrupos.Where(x => x.CodigoItemSistemaOrigem == item1.CodigoItemSistemaOrigem);
                        if (!res.Any())
                            listSubgrupos.Add(item);
                    }
                    else
                    {
                        var res = listItens.Where(x => x.CodigoItemSistemaOrigem == item1.CodigoItemSistemaOrigem);
                        if (!res.Any())
                            listItens.Add(item);
                    }
                }
            }

            listSubgrupos = listSubgrupos.OrderBy(x => x.DescricaoCliente).ToList();
            listItens = listItens.OrderBy(x => x.DescricaoCliente).ToList();

            ObterItensSubGrupo(listAux, listSubgrupos, listItens, documento, itemPreco);

            return listAux;
        }

        private void ObterItensSubGrupo(List<DocumentoObjetoDetalheContrato> listAux, IEnumerable<DocumentoObjetoItem> listSubgrupos, List<DocumentoObjetoItem> listItens, Documento documento, List<ItemPreco> itemPreco)
        {
            foreach (DocumentoObjetoItem subgrupo in listSubgrupos)
            {
                var resSubgrupo = (from p in itemPreco
                                   where p.idItemSistemaOrigem == subgrupo.CodigoItemSistemaOrigem
                                   select p).First();


                if (subgrupo.Unidade.Trim().Equals("M", StringComparison.OrdinalIgnoreCase) == false
                    && String.Compare(subgrupo.DescricaoResumida, "subgrupo", StringComparison.OrdinalIgnoreCase) == 0
                    && new ItemGroups().ExibirItensSubGrupo(subgrupo.CodigoItemSistemaOrigem) == false)
                {
                    foreach (Items item in new Items().ObterItemsDoSubGrupo(subgrupo.CodigoTabelaPrecoSistemaOrigem, subgrupo.CodigoItemSistemaOrigem))
                    {
                        DocumentoObjetoDetalheContrato oDocumentoObjetoDetalheContrato;

                        var res = listItens.Where(x => x.CodigoItemSistemaOrigem == item.IdItem);
                        if (res.Any())
                        {
                            DocumentoObjetoItem oDocumentoObjetoItem = res.Single();
                            oDocumentoObjetoDetalheContrato = new DocumentoObjetoDetalheContrato
                            {
                                Descricao = oDocumentoObjetoItem.DescricaoCliente,
                                ValorLocacao = oDocumentoObjetoItem.ValorPraticadoLocacao,
                                ValorIndenizacao = oDocumentoObjetoItem.ValorPraticadoIndenizacao,
                                Unidade = oDocumentoObjetoItem.Unidade,
                                Tipo = "item",
                                Peso = oDocumentoObjetoItem.Peso,
                                CodigoItemSistemaOrigem = oDocumentoObjetoItem.CodigoItemSistemaOrigem,
                                DescricaoResumida = oDocumentoObjetoItem.DescricaoResumida,
                                CodigoGrupoItemSistemaOrigem = new Items().ObterIdGrupoItem(oDocumentoObjetoItem.CodigoItemSistemaOrigem),
                                Totalizador = false,
                                ExibirVUL = resSubgrupo.exibirVUL,
                                ExibirVUI = resSubgrupo.exibirVUI
                            };

                            listAux.Add(oDocumentoObjetoDetalheContrato);
                        }
                        else
                        {
                            oDocumentoObjetoDetalheContrato = new DocumentoObjetoDetalheContrato
                            {
                                Descricao = item.Descricao,
                                Unidade = item.Unidade,
                                Tipo = "item",
                                Peso = item.Peso,
                                CodigoItemSistemaOrigem = item.CodigoItemSistemaOrigem,
                                DescricaoResumida = item.DescricaoResumida,
                                CodigoGrupoItemSistemaOrigem = item.CodigoGrupoItemSistemaOrigem,
                                Totalizador = false,
                                ExibirVUL = resSubgrupo.exibirVUL,
                                ExibirVUI = resSubgrupo.exibirVUI,
                                ValorLocacao = item.MedidaControle*subgrupo.ValorPraticadoLocacao
                            };


                            if (subgrupo.ValorPraticadoIndenizacao != 0)
                                oDocumentoObjetoDetalheContrato.ValorIndenizacao = item.MedidaControle * subgrupo.ValorPraticadoIndenizacao;
                            else
                                oDocumentoObjetoDetalheContrato.ValorIndenizacao = item.ValorIndenizacao;

                            listAux.Add(oDocumentoObjetoDetalheContrato);
                        }
                    }
                }
                else
                {
                    listAux.Add(new DocumentoObjetoDetalheContrato
                    {
                        Descricao = subgrupo.DescricaoCliente,
                        ValorLocacao = subgrupo.ValorPraticadoLocacao,
                        ValorIndenizacao = subgrupo.ValorPraticadoIndenizacao,
                        Unidade = subgrupo.Unidade,
                        Tipo = "subgrupo",
                        Peso = subgrupo.Peso,
                        CodigoItemSistemaOrigem = subgrupo.CodigoItemSistemaOrigem,
                        DescricaoResumida = subgrupo.DescricaoResumida,
                        CodigoGrupoItemSistemaOrigem = new Items().ObterIdGrupoItem(subgrupo.CodigoItemSistemaOrigem),
                        Totalizador = false,
                        ExibirVUL = resSubgrupo.exibirVUL,
                        ExibirVUI = resSubgrupo.exibirVUI
                    });
                }
            }

            foreach (DocumentoObjetoItem item in listItens)
            {
                var resItem = (from p in itemPreco
                               where p.idItemSistemaOrigem == item.CodigoItemSistemaOrigem
                               select p).First();

                listAux.Add(new DocumentoObjetoDetalheContrato
                {
                    Descricao = item.DescricaoCliente,
                    ValorLocacao = item.ValorPraticadoLocacao,
                    ValorIndenizacao = item.ValorPraticadoIndenizacao,
                    Unidade = item.Unidade,
                    Tipo = "item",
                    CodigoItemSistemaOrigem = item.CodigoItemSistemaOrigem,
                    DescricaoResumida = item.DescricaoResumida,
                    CodigoGrupoItemSistemaOrigem = new Items().ObterIdGrupoItem(item.CodigoItemSistemaOrigem),
                    Totalizador = false,
                    ExibirVUI = resItem.exibirVUI,
                    ExibirVUL = resItem.exibirVUL
                });
            }
            ObterSubGrupo(listAux, itemPreco);
        }
        private void ObterSubGrupo(List<DocumentoObjetoDetalheContrato> listAux, List<ItemPreco> itemPreco)
        {
            foreach (Int32 idGrupo in (from c in listAux
                                       where c.Tipo == "item" || (c.Tipo == "subgrupo" && c.Totalizador == false)
                                       select c.CodigoGrupoItemSistemaOrigem).Distinct().ToList())
            {

                Items item = new Items().ObterSubGrupoItem(idGrupo);

                listAux.Add(new DocumentoObjetoDetalheContrato
                {
                    Descricao = item.Descricao,
                    ValorLocacao = item.ValorLocacao,
                    ValorIndenizacao = item.ValorIndenizacao,
                    Unidade = item.Unidade,
                    Tipo = item.Tipo,
                    Peso = item.Peso,
                    CodigoItemSistemaOrigem = item.CodigoItemSistemaOrigem,
                    DescricaoResumida = item.DescricaoResumida,
                    CodigoGrupoItemSistemaOrigem = item.CodigoGrupoItemSistemaOrigem,
                    Totalizador = true,
                    ExibirVUL = false,
                    ExibirVUI = false,
                });
            }
        }
    }
}
