using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;

namespace Rohr.EPC.Business
{
    public class WorkflowAcaoExecutadaBusiness
    {
        public enum RelatorioSlaEtapas
        {
            AnaliseGerente,
            AnaliseDiretoriaOperacional,
            AnaliseJuridico,
            AnaliseDiretoria,
            ReceberDocumentoCliente,
            ArquivarContrato
        }

        public Double ObterTempoDesdeUltimaAcao(Int32 idDocumento)
        {
            WorkflowAcaoExecutada oWorkflowAcaoExecutada = new WorkflowAcaoExecutadaDAO().ObterUltimaAcaoExecudaPorNumeroDocumento(idDocumento);

            if (oWorkflowAcaoExecutada == null) return 0;
            TimeSpan diff = DateTime.Now.Subtract(oWorkflowAcaoExecutada.DataCadastro);
            return diff.TotalSeconds;
        }
        public Int32 ObterRelatorioSLA(RelatorioSlaEtapas etapa, DateTime dataInicio, DateTime dataFim, Boolean eProposta)
        {
            Int32 quantidadeAcoes = 0;
            Int32 totalAcoesNoPrazo = 0;

            List<int> listIdWorkflowAcao = !eProposta ? ObterEtapaContrato(etapa) : ObterEtapaProposta(etapa);

            foreach (Int32 idWorkflowAcao in listIdWorkflowAcao)
            {
                List<WorkflowAcaoExecutada> listWorkflowAcaoExecutada = new WorkflowAcaoExecutadaDAO().ObterWorkflowAcaoExecutada(idWorkflowAcao, dataInicio, dataFim, eProposta);

                if (listWorkflowAcaoExecutada == null) continue;
                foreach (WorkflowAcaoExecutada workflowAcaoExecutada in listWorkflowAcaoExecutada)
                {
                    quantidadeAcoes++;

                    if (workflowAcaoExecutada.TempoTotalAcao <= 3600)
                        totalAcoesNoPrazo++;
                }
            }

            if (quantidadeAcoes != 0)
                return ((100 * totalAcoesNoPrazo) / quantidadeAcoes);
            return 100;
        }

        List<Int32> ObterEtapaContrato(RelatorioSlaEtapas etapas)
        {
            List<Int32> listIdWorkflowAcao = new List<Int32>();
            switch (etapas)
            {
                case RelatorioSlaEtapas.AnaliseGerente:
                    listIdWorkflowAcao.Add(27);
                    listIdWorkflowAcao.Add(28);
                    break;
                case RelatorioSlaEtapas.AnaliseDiretoriaOperacional:
                    listIdWorkflowAcao.Add(45);
                    listIdWorkflowAcao.Add(46);
                    break;
                case RelatorioSlaEtapas.AnaliseJuridico:
                    listIdWorkflowAcao.Add(29);
                    listIdWorkflowAcao.Add(30);
                    break;
                case RelatorioSlaEtapas.AnaliseDiretoria:
                    listIdWorkflowAcao.Add(31);
                    listIdWorkflowAcao.Add(32);
                    break;
                case RelatorioSlaEtapas.ReceberDocumentoCliente:
                    listIdWorkflowAcao.Add(36);
                    break;
                case RelatorioSlaEtapas.ArquivarContrato:
                    listIdWorkflowAcao.Add(39);
                    break;
            }

            return listIdWorkflowAcao;
        }
        List<Int32> ObterEtapaProposta(RelatorioSlaEtapas etapas)
        {
            List<Int32> listIdWorkflowAcao = new List<Int32>();
            switch (etapas)
            {
                case RelatorioSlaEtapas.AnaliseGerente:
                    listIdWorkflowAcao.Add(41);
                    listIdWorkflowAcao.Add(42);
                    break;
                case RelatorioSlaEtapas.AnaliseDiretoriaOperacional:
                    listIdWorkflowAcao.Add(43);
                    listIdWorkflowAcao.Add(44);
                    break;
                case RelatorioSlaEtapas.ReceberDocumentoCliente:
                    listIdWorkflowAcao.Add(21);
                    listIdWorkflowAcao.Add(22);
                    break;
            }

            return listIdWorkflowAcao;
        }

        public WorkflowAcaoExecutada ObterUltimaAcaoPorIdDocumento(Int32 idDocumento)
        {
            WorkflowAcaoExecutada oWorkflowAcaoExecutada = new WorkflowAcaoExecutadaDAO().ObterUltimaAcaoExecudaPorNumeroDocumento(idDocumento);
            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(oWorkflowAcaoExecutada.WorkflowAcao.IdWorkflowAcao);
            return oWorkflowAcaoExecutada;
        }

        /// <summary>
        /// Obtem a ultima reprovação feita pela gerencia / diretoria operacional / juridico ou diretoria.
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns></returns>
        public WorkflowAcaoExecutada ObterUltimaReprovacaoGerenciaPorIdDocumento(Int32 idDocumento)
        {
            WorkflowAcaoExecutada oWorkflowAcaoExecutada = new WorkflowAcaoExecutadaDAO().ObterUltimaReprovacaoGerenciaPorIdDocumento(idDocumento);
            return oWorkflowAcaoExecutada;
        }
    }
}
