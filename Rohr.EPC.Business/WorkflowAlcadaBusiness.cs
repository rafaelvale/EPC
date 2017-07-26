using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Linq;

namespace Rohr.EPC.Business
{
    public class WorkflowAlcadaBusiness
    {
        public WorkflowAlcada ObterPorId(Int32 idWorkflowAlcada)
        {
            return new WorkflowAlcadaDAO().ObterPorId(idWorkflowAlcada);
        }
        public Boolean DocumentoDentroAlcada(Int32 idPerfilAtivo, Documento documento)
        {
            if (String.IsNullOrEmpty(documento.ValorFaturamentoMensal.ToString()))
                throw new MyException("Valor do Faturamento Mensal inválido.");

            if (String.IsNullOrEmpty(documento.PercentualDesconto.ToString()))
                throw new MyException("Valor do Percentual de Desconto inválido.");

            if (documento.ValorFaturamentoMensal < 0)
                throw new MyException("Valor do Faturamento Mensal inválido.");

            if (documento.Modelo.Segmento.IdSegmento == 3) return true;

            if (idPerfilAtivo == 1)
            {
                if (VerificarAlcada(documento) == WorkflowAlcada.Alcada.Comercial)
                    return true;
            }
            else if (idPerfilAtivo == 7)
            {
                if (VerificarAlcada(documento) == WorkflowAlcada.Alcada.Gerente)
                    return true;
            }
            return false;
        }

        public WorkflowAlcada.Alcada VerificarAlcada(Documento documento)
        {
            if (documento.Modelo.Segmento.IdSegmento == 3) return WorkflowAlcada.Alcada.Comercial;

            WorkflowAlcada.Alcada alcadaDuracaoContrato = ObterAlcadaDuracaoContrato(documento);
            WorkflowAlcada.Alcada alcadaDesconto = ObterAlcadaDesconto(documento);
            WorkflowAlcada.Alcada alcadaFaturamento = ObterAlcadaFaturamento(documento);

            if (alcadaDesconto == WorkflowAlcada.Alcada.VicePresidencia ||
                alcadaDuracaoContrato == WorkflowAlcada.Alcada.VicePresidencia ||
                alcadaFaturamento == WorkflowAlcada.Alcada.VicePresidencia)
                return WorkflowAlcada.Alcada.VicePresidencia;

            if (alcadaDesconto == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                alcadaDuracaoContrato == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                alcadaFaturamento == WorkflowAlcada.Alcada.DiretoriaOperacional)
                return WorkflowAlcada.Alcada.DiretoriaOperacional;

            if (alcadaDesconto == WorkflowAlcada.Alcada.Superintendencia ||
                alcadaDuracaoContrato == WorkflowAlcada.Alcada.Superintendencia ||
                alcadaFaturamento == WorkflowAlcada.Alcada.Superintendencia)
                return WorkflowAlcada.Alcada.Superintendencia;

            if (alcadaDesconto == WorkflowAlcada.Alcada.Gerente ||
                alcadaDuracaoContrato == WorkflowAlcada.Alcada.Gerente ||
                alcadaFaturamento == WorkflowAlcada.Alcada.Gerente)
                return WorkflowAlcada.Alcada.Gerente;

            return WorkflowAlcada.Alcada.Comercial;
        }
        public WorkflowAlcada.Alcada ObterAlcadaDuracaoContrato(Documento documento)
        {
            return DocumentoObjetoBusiness.CalcularPrevisaoUtilizacaoContrato(documento) < 30 ? WorkflowAlcada.Alcada.VicePresidencia : WorkflowAlcada.Alcada.Comercial;
        }
        public WorkflowAlcada.Alcada ObterAlcadaDesconto(Documento documento)
        {
            if (documento.ListDocumentoObjeto == null)
                return WorkflowAlcada.Alcada.Comercial;

            foreach (DocumentoObjetoItem item in documento.ListDocumentoObjeto.SelectMany(documentoObjeto => documentoObjeto.DocumentoObjetoItem).OrderByDescending(o => o.Desconto))
            {
                if (documento.Filial.IdFilial == 2 && item.Desconto < 30 && item.Desconto >= 10)
                    return WorkflowAlcada.Alcada.Gerente;


                if (item.Desconto >= 30)
                    return WorkflowAlcada.Alcada.VicePresidencia;
                if (item.Desconto < 30 && item.Desconto >= 20)
                    return WorkflowAlcada.Alcada.Superintendencia;
                if (item.Desconto < 20 && item.Desconto >= 10)
                    return WorkflowAlcada.Alcada.Gerente;
            }
            return WorkflowAlcada.Alcada.Comercial;
        }
        public WorkflowAlcada.Alcada ObterAlcadaFaturamento(Documento documento)
        {
            return documento.ValorFaturamentoMensal < 1000 ? WorkflowAlcada.Alcada.VicePresidencia : WorkflowAlcada.Alcada.Comercial;
        }
        public String ObterDescricaoAlcada(Documento documento)
        {
            switch (VerificarAlcada(documento))
            {
                case WorkflowAlcada.Alcada.VicePresidencia:
                    return "Vice Presidência";
                case WorkflowAlcada.Alcada.Superintendencia:
                    return "Superintendência";
                case WorkflowAlcada.Alcada.DiretoriaOperacional:
                    return "Diretoria Operacional";
                case WorkflowAlcada.Alcada.Gerente:
                    return "Gerente";
                case WorkflowAlcada.Alcada.Comercial:
                    return "Comercial";
                default:
                    return "Não identificado";
            }
        }
}
}