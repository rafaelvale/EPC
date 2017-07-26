using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Data;
using Rohr.PMWeb;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;

namespace Rohr.EPC.Business
{
    public class WorkflowBusiness
    {
        public enum Acao
        {
            Aprovar,
            Reprovar
        }

        public Boolean VerificarDocumentoEncaminhadoCliente(Int32 idDocumento)
        {
            Workflow oWorkflow = new WorkflowDAO().ObterAcaoReceberClientePorIdDocumento(idDocumento);

            return oWorkflow != null;
        }
        public Workflow ObterUltimaAcaoPorIdDocumento(Int32 idDocumento)
        {
            Workflow oWorkflow = new WorkflowDAO().ObterUltimaAcaoPorIdDocumento(idDocumento);
            oWorkflow.WorkflowEtapa = new WorkflowEtapaDAO().ObterPorId(oWorkflow.WorkflowEtapa.IdWorkflowEtapa);
            oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(oWorkflow.WorkflowAcao.IdWorkflowAcao);
            return oWorkflow;
        }
        public void ExecutarAcao(Workflow workflow, Acao acao, Documento documento)
        {
            Boolean eMinuta = true;
            Workflow oWorkflow = new Workflow
            {
                DataCadastro = DateTime.Now,
                Justificativa = workflow.Justificativa,
                IdUsuario = workflow.IdUsuario,
                IdPerfil = workflow.IdPerfil,
                IdDocumento = workflow.IdDocumento
            };
            oWorkflow.Justificativa = workflow.Justificativa;

            WorkflowAcaoExecutada oWorkflowAcaoExecutada = new WorkflowAcaoExecutada
            {
                DataCadastro = DateTime.Now,
                IdUsuario = oWorkflow.IdUsuario,
                IdPerfil = workflow.IdPerfil,
                IdDocumento = oWorkflow.IdDocumento,
                Justificativa = oWorkflow.Justificativa,
                NumeroDocumento = new DocumentoBusiness().ObterPorId(workflow.IdDocumento).NumeroDocumento,
                TempoTotalAcao = new WorkflowAcaoExecutadaBusiness().ObterTempoDesdeUltimaAcao(documento.IdDocumento)
            };

            if (Acao.Reprovar == acao)
            {
                oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(17);

                oWorkflow.WorkflowEtapa = documento.Modelo.Segmento.IdSegmento != 3 ? new WorkflowEtapa(1) : new WorkflowEtapa(7);

                if (documento.EProposta)
                {
                    switch (workflow.WorkflowAcao.IdWorkflowAcao)
                    {
                        case 3:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(23);
                            break;
                        case 4:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(22);
                            break;
                        case 1:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(42);
                            break;
                        case 2:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(44);
                            break;
                        case 49:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(50);
                            break;
                        case 55:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(58);
                            break;
                        case 56:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(60);
                            break;
                    }
                }
                else
                {
                    switch (workflow.WorkflowAcao.IdWorkflowAcao)
                    {
                        case 1:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(28);
                            break;
                        case 2:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(46);
                            break;
                        case 6:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(30);
                            break;
                        case 7:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(32);
                            break;
                        case 49:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(52);
                            break;
                        case 54:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(54);
                            break;
                        case 55:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(62);
                            break;
                        case 56:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(64);
                            break;
                        case 70:
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(73);
                            break;
                    }

                    new Estimates().LiberarOrcamento(documento.ListDocumentoObjeto[0].IdOrcamentoSistemaOrigem, workflow.WorkflowAcao.IdWorkflowAcao == 54);
                }
            }
            else switch (workflow.WorkflowAcao.IdWorkflowAcao)
                {
                    case 1:
                        if (documento.EProposta)
                        {
                            if (new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.Superintendencia ||
                                new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                                new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.VicePresidencia)
                            {
                                if (documento.Filial.IdFilial == 2)
                                {
                                    oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(55);
                                    oWorkflow.WorkflowEtapa = new WorkflowEtapa(11);
                                }
                                else
                                {
                                    oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(2);
                                    oWorkflow.WorkflowEtapa = new WorkflowEtapa(6);
                                }
                            }
                            else
                            {
                                oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(3);
                                oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                                eMinuta = false;
                            }
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(41);
                        }
                        else
                        {
                            if (new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.Superintendencia ||
                                new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                                new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.VicePresidencia)
                            {
                                if (documento.Filial.IdFilial == 2)
                                {
                                    oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(55);
                                    oWorkflow.WorkflowEtapa = new WorkflowEtapa(11);
                                }
                                else
                                {
                                    oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(2);
                                    oWorkflow.WorkflowEtapa = new WorkflowEtapa(6);
                                }

                                oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(27);
                            }
                            else
                            {
                                oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(6);
                                oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                                oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(27);
                                eMinuta = false;
                            }
                        }
                        break;
                    case 49:
                        eMinuta = false;
                        if (documento.EProposta)
                        {
                            oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(3);
                            oWorkflow.WorkflowEtapa = new WorkflowEtapa(7);
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(51);
                        }
                        else
                        {
                            oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(10);
                            oWorkflow.WorkflowEtapa = new WorkflowEtapa(7);
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(53);
                        }
                        break;
                    case 2:
                        if (documento.EProposta)
                        {
                            if (new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                                new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.VicePresidencia)
                            {
                                oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(55);
                                oWorkflow.WorkflowEtapa = new WorkflowEtapa(11);
                                oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(43);
                            }
                            else
                            {
                                oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(3);
                                oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                                oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(43);
                                eMinuta = false;
                            }
                        }
                        else
                        {
                            if (new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.DiretoriaOperacional ||
                                new WorkflowAlcadaBusiness().VerificarAlcada(documento) == WorkflowAlcada.Alcada.VicePresidencia)
                            {
                                oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(55);
                                oWorkflow.WorkflowEtapa = new WorkflowEtapa(11);
                                oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(45);
                            }
                            else
                            {
                                oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(6);
                                oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                                oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(45);
                                eMinuta = false;
                            }
                        }
                        break;
                    case 3:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(4);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(10);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(20);
                        eMinuta = false;
                        break;
                    case 4:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(5);
                        oWorkflow.WorkflowEtapa = documento.Modelo.Segmento.IdSegmento == 3 ? new WorkflowEtapa(7) : new WorkflowEtapa(1);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(21);
                        eMinuta = false;
                        new DocumentTodoList().MarcarProspostaAprovada(documento.CodigoSistemaOrigem);
                        break;
                    case 6:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(65);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(29);
                        break;
                    case 7:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(8);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(31);
                        break;
                    case 8:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(9);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(33);
                        break;
                    case 9:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(10);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(34);
                        break;
                    case 10:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(11);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(10);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(35);
                        break;
                    case 11:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(68);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(36);

                        if (documento.Modelo.Segmento.IdSegmento == 3)
                        {
                            oWorkflow.WorkflowEtapa = new WorkflowEtapa(7);
                            eMinuta = false;
                        }
                        else
                        {
                            oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                        }

                        break;
                    case 12:

                        if (documento.Modelo.Segmento.IdSegmento == 3)
                        {
                            oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(13);
                            oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(38);
                            eMinuta = false;
                        }
                        else
                        {
                            oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(70);
                            oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(71);
                        }
                        break;
                    case 13:
                        new DocumentoBusiness().AtualizarStatusDocumento(oWorkflow.IdDocumento);
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(40);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(39);
                        eMinuta = false;
                        break;
                    case 14:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(12);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(37);
                        eMinuta = false;
                        break;
                    case 55:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(56);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(12);

                        if (documento.EProposta)
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(57);
                        else
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(61);
                        break;
                    case 56:
                        if (documento.EProposta)
                        {
                            oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(3);
                            oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(59);
                            eMinuta = false;
                        }
                        else
                        {
                            oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(6);
                            oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                            oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(63);
                        }
                        break;
                    case 65:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(66);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(33);
                        break;
                    case 66:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(10);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(67);
                        break;
                    case 68:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(12);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(69);
                        break;
                    case 70:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(74);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(4);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(72);
                        break;
                    case 74:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(76);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(75);
                        break;
                    case 76:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(77);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(80);
                        break;
                    case 77:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(78);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(81);
                        break;
                    case 78:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(79);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(1);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(83);
                        break;
                    case 79:
                        oWorkflow.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(40);
                        oWorkflow.WorkflowEtapa = new WorkflowEtapa(3);
                        oWorkflowAcaoExecutada.WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(40);
                        new DocumentoBusiness().AtualizarStatusDocumento(oWorkflow.IdDocumento);
                        break;

                    default:
                        throw new MyException("Não foi possível recuperar a ação");
                }

            new WorkflowDAO().AdicionarAcao(oWorkflow, oWorkflowAcaoExecutada, eMinuta);

            if (acao == Acao.Reprovar && (oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 1 || oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 7))
                new ModeloEmailBusiness().EnviarEmail(documento, oWorkflow, acao);
            else
            {
                if (
                    oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 1 ||
                    oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 3 ||
                    oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 4 ||
                    oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 5 ||
                    oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 6 ||
                    oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 7 ||
                    oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 5 ||
                    oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 11 ||
                    oWorkflow.WorkflowEtapa.IdWorkflowEtapa == 12)
                    new ModeloEmailBusiness().EnviarEmail(documento, oWorkflow, acao);
            }
        }
        public DataTable ObterHistoricoAcoes(Documento documento)
        {
            return new WorkflowDAO().ObterHistoricoAcoes(documento.IdDocumento);
        }
        public String ObterHistoricoAcoesFormatadoEmail(Documento documento)
        {
            DataTable dtHistoricoAcoes = ObterHistoricoAcoes(documento);
            StringBuilder sb = new StringBuilder();
            Boolean existeHistorico = false;

            sb.Append("<table align='center' border='0' cellpadding='0' cellspacing='0' style='width: 100%; margin-bottom: 30px;'>");
            sb.Append("<tr>");
            sb.Append("<td style=\"text-align:center;\" colspan=\"5\"><br /><b>Histórico de ações</b><br /><br /></td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<th style='border-bottom: 1px solid;border-color: #ddd; padding:1px; width: 120px; text-align:left;'>Ação</th>");
            sb.Append("<th style='border-bottom: 1px solid;border-color: #ddd; padding:1px; width: 70px;' text-align:left;'>Executada em</th>");
            sb.Append("<th style='border-bottom: 1px solid;border-color: #ddd; padding:1px; width: 90px;' text-align:left;'>Responsável</th>");
            sb.Append("<th style='border-bottom: 1px solid;border-color: #ddd; padding:1px; width: 120px; text-align:left;'>Perfil</th>");
            sb.Append("<th style='border-bottom: 1px solid;border-color: #ddd; padding:1px; width: 120px; text-align:left;'>Observação</th>");
            sb.Append("</tr>");

            for (int i = 0; i < dtHistoricoAcoes.Rows.Count; i++)
            {
                sb.Append("<tr>");
                sb.Append(String.Format("<td style='border-bottom: 1px solid;border-color: #ddd; padding:3px;'>{0}</td>", dtHistoricoAcoes.Rows[i][5]));
                sb.Append(String.Format("<td style='border-bottom: 1px solid;border-color: #ddd; padding:3px;'>{0}</td>", Convert.ToDateTime(dtHistoricoAcoes.Rows[i][0]).ToString("dd/MM/yyyy")));
                sb.Append(String.Format("<td style='border-bottom: 1px solid;border-color: #ddd; padding:3px;'>{0}</td>", dtHistoricoAcoes.Rows[i][4]));
                sb.Append(String.Format("<td style='border-bottom: 1px solid;border-color: #ddd; padding:3px;'>{0}</td>", dtHistoricoAcoes.Rows[i][6]));
                sb.Append(String.Format("<td style='border-bottom: 1px solid;border-color: #ddd; padding:3px;'>{0}</td>", dtHistoricoAcoes.Rows[i][1]));
                sb.Append("</tr>");
                existeHistorico = true;
            }
            sb.Append("</table>");

            if (!existeHistorico)
                sb.Clear();

            return sb.ToString();
        }
        public DataTable ObterTodosPorIdDocumento(Int32 idDocumento)
        {
            return new WorkflowDAO().ObterTodosPorIdDocumento(idDocumento);
        }

        

        public static String ObterObterCaminhoAssinatura(Int32 idDocumento, Int32 idUsuario)
        {
            String caminho = String.Empty;
            DataTable dt = new WorkflowDAO().ObterAprovacaoJuridico(idDocumento);

            if (dt.Rows.Count == 0)
                caminho = ObterCaminho(Convert.ToInt32(idUsuario));
            else if (dt.Rows.Count == 1)
            {
                caminho = ObterCaminho(Convert.ToInt32(dt.Rows[0][0].ToString()));
            }

            return caminho;
        }

        private static String ObterCaminho(Int32 idUsuario)
        {
            switch (idUsuario)
            {
                case 35:
                    return "/Content/Image/assKatia.jpg";
                case 52:
                    return "/Content/Image/assRafael.jpg";
                default:
                    return "/Content/Image/aprovadoJuridico.jpg";
            }
        }

        public List<Workflow> ObterListaWorkflowCockpit(Repeater repeaterControls)
        {
            List<Workflow> listWorkflow = new List<Workflow>();

            foreach (RepeaterItem item in ((Repeater)repeaterControls).Items)
            {
                if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem) continue;
                CheckBox checkbox = (CheckBox)item.FindControl("ckDocumento");

                if (checkbox.Checked)
                {
                    Workflow oWorkflow = new WorkflowBusiness().ObterUltimaAcaoPorIdDocumento(Int32.Parse(checkbox.Attributes["idDocumento"]));
                    listWorkflow.Add(oWorkflow);
                }
                checkbox.Checked = false;
            }

            return listWorkflow;
        }
    }
}
