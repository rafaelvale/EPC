using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Rohr.EPC.Business
{
    public class DocumentoBusiness
    {

        

        public Int32 ObterTermoID(Int32 TermoNumero)
        {
            Int32 Termo = 0;            
            Termo = new DocumentoDAO().ObterTermoID(TermoNumero);            
            return Termo;

        }

        public DataTable ObterDadosTermo(Int32 IDOportunidade)
        {  
                return new DocumentoDAO().ObterDadosTermo(IDOportunidade);
        }

        public DataTable ObterTermoPMweb(Int32 TermoNumero)
        {
            return new DocumentoDAO().ObterTermoPMweb(TermoNumero);
        }

        public Int32 AdicionarProposta(Documento documento, Int32 idUsuario, Int32 idPerfil)
        {
            Int32 idWorkflowEtapa = 1;
            if (documento.Modelo.Segmento.IdSegmento == 3)
                idWorkflowEtapa = 7;

            Workflow oWorkflow = new Workflow
            {
                WorkflowEtapa = new WorkflowEtapa(idWorkflowEtapa),
                IdPerfil = idPerfil,
                IdUsuario = idUsuario,
                WorkflowAcao = new WorkflowAcao(15)
            };


            return new DocumentoDAO().AdicionarProposta(documento, oWorkflow);
        }
        public Int32 AdicionarRevisaoProposta(Documento documento, Int32 idUsuario, Int32 idPerfil)
        {
            Int32 idWorkflowEtapa = 1;
            if (documento.Modelo.Segmento.IdSegmento == 3)
                idWorkflowEtapa = 7;

            Workflow oWorkflow = new Workflow
            {
                WorkflowEtapa = new WorkflowEtapa(idWorkflowEtapa),
                IdPerfil = idPerfil,
                IdUsuario = idUsuario,
                WorkflowAcao = new WorkflowAcao(17)
            };

            return new DocumentoDAO().AdicionarProposta(documento, oWorkflow);
        }

        public Int32 AdicionarContrato(Documento documentoContrato, Documento documentoProposta, Int32 idUsuario, Int32 idPerfil)
        {
            WorkflowEtapa oWorkflowEtapa = documentoContrato.Modelo.Segmento.IdSegmento == 3 ? new WorkflowEtapa(7) : new WorkflowEtapa(1);

            Workflow oWorkflow = new Workflow
            {
                IdUsuario = idUsuario,
                IdPerfil = idPerfil,
                WorkflowAcao = WorkflowAcaoBusiness.ObterPorId(16),
                WorkflowEtapa = oWorkflowEtapa
            };

            return new DocumentoDAO().AdicionarContrato(documentoContrato, documentoProposta, oWorkflow);
        }
        public Int32 AdicionarRevisaoContrato(Documento documentoContrato, Documento documentoProposta, Int32 idUsuario, Int32 idPerfil)
        {
            Int32 idWorkflowEtapa = 1;
            if (documentoContrato.Modelo.Segmento.IdSegmento == 3)
                idWorkflowEtapa = 7;

            Workflow oWorkflow = new Workflow
            {
                WorkflowEtapa = new WorkflowEtapa(idWorkflowEtapa),
                IdPerfil = idPerfil,
                IdUsuario = idUsuario,
                WorkflowAcao = new WorkflowAcao(17)
            };

            return new DocumentoDAO().AdicionarContrato(documentoContrato, documentoProposta, oWorkflow);
        }

        public Documento RecuperarDocumento(Int32 idDocumento)
        {
            Documento oDocumento = ObterPorId(idDocumento);
            oDocumento.Modelo = new ModeloBusiness().ObterModeloPorId(oDocumento.Modelo.IdModelo);
            oDocumento.ListPartePreenchida = new PartePreenchidaBusiness().ObterTodasPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.ListChavePreenchida = new ChavePreenchidaBusiness().ObterTodasPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.DocumentoObra = new DocumentoObraBusiness().ObterPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.DocumentoComercial = new DocumentoComercialBusiness().ObterPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.Usuario = new UsuarioDAO().ObterPorId(oDocumento.Usuario.IdUsuario);
            oDocumento.Filial = new FilialBusiness().ObterFilial(oDocumento.Filial.IdFilial);
            oDocumento.ListDocumentoObjeto = new DocumentoObjetoBusiness().ObterPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.DocumentoObjetoObservacao = new DocumentoObjetoObservacaoBusiness().ObterPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.DocumentoResumoProposta = new DocumentoResumoBusiness().ObterPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.DocumentoDescricaoGeralFoto = new DocumentoImagensBusiness().ObterDescricaoGeralPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.DocumentoDescricaoGeralPortfolio = new DocumentoPortfolioBusiness().ObterDescricaoGeralPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.DocumentoDescricaoGeralPortfolioParte2 = new DocumentoPortfolioBusiness().ObterDescricaoGeralPorIdDocumentoParte2(oDocumento.IdDocumento);


            return oDocumento;
        }
        public Documento ObterPorId(Int32 idDocumento)
        {
            Documento oDocumento = new DocumentoDAO().ObterPorId(idDocumento);
            oDocumento.DocumentoObra = new DocumentoObraBusiness().ObterPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.DocumentoCliente = new DocumentoClienteDAO().ObterPorId(oDocumento.IdDocumento);
            oDocumento.ListChavePreenchida = new ChavePreenchidaBusiness().ObterTodasPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.ListDocumentoObjeto = new DocumentoObjetoBusiness().ObterPorIdDocumento(oDocumento.IdDocumento);
            oDocumento.DocumentoStatus = new DocumentoStatusBusiness().ObterPorId(oDocumento.DocumentoStatus.IdDocumentoStatus);
            return oDocumento;
        }

        public Boolean PropostaExiste(Int32 oportunidade)
        {
            return new DocumentoDAO().PropostaExistente(oportunidade);
        }
        public Boolean PropostaTemContrato(Int32 numeroDocumento)
        {
            return new DocumentoDAO().PropostaTemContrato(numeroDocumento);
        }
        public void AtualizarProposta(Documento documento, Int32 idUsuario, Int32 idPerfil)
        {
            Int32 idWorkflowEtapa = 1;
            if (documento.Modelo.Segmento.IdSegmento == 3)
                idWorkflowEtapa = 7;

            Workflow oWorkflow = new Workflow
            {
                WorkflowEtapa = new WorkflowEtapa(idWorkflowEtapa),
                IdPerfil = idPerfil,
                IdUsuario = idUsuario
            };

            WorkflowAcaoExecutada oWorkflowAcaoExecutada = new WorkflowAcaoExecutada
            {
                IdUsuario = idUsuario,
                IdPerfil = idPerfil,
                NumeroDocumento = documento.NumeroDocumento,
                TempoTotalAcao = 1
            };

            new DocumentoDAO().AtualizarDocumento(documento, oWorkflow, oWorkflowAcaoExecutada);
        }
        public void AtualizarPercentualLimpeza(Documento documento)
        {
            new DocumentoDAO().AtualizarPercentualLimpeza(documento.IdDocumento, documento.PercentualLimpeza);
        }

        public void FinalizarDocumento(Int32 perfilAtivo, Documento documento, Workflow workflow, WorkflowAcaoExecutada workflowAcaoExecutada)
        {
            documento.Eminuta = true;

            if (documento.EProposta)
            {
                if (documento.Modelo.Segmento.IdSegmento != 3 && new WorkflowAlcadaBusiness().DocumentoDentroAlcada(perfilAtivo, documento))
                    documento.Eminuta = false;
                else
                    documento.Eminuta = true;
            }

            documento.DocumentoStatus.IdDocumentoStatus = 1;
            workflowAcaoExecutada.TempoTotalAcao = new WorkflowAcaoExecutadaBusiness().ObterTempoDesdeUltimaAcao(documento.IdDocumento);

            new DocumentoDAO().AtualizarDocumento(documento, workflow, workflowAcaoExecutada);
        }

        public DataTable ObterTodos(List<Filial> filial, Int32 idPerfil, Int32 idWorkFlowAcao)
        {
            StringBuilder filiais = new StringBuilder();
            String segmentos;

            for (int i = 0; i < filial.Count; i++)
            {
                filiais.Append(filial[i].IdFilial);
                if (filial.Count - 1 != i)
                    filiais.Append(",");
            }

            switch (idPerfil)
            {
                case 7:
                case 6:
                    segmentos = "3,4";
                    break;
                case 3:
                case 2:
                    segmentos = "1,2,3,4,5,6,7";
                    break;
                default:
                    segmentos = "1,2,4,5,6,7";
                    break;
            }

            if (idWorkFlowAcao == 0)
                return new DocumentoDAO().ObterTodos(filiais.ToString(), segmentos);
            else
                return new DocumentoDAO().ObterTodosPorIdWorkflowAcao(filiais.ToString(), segmentos, idWorkFlowAcao);
        }
        public DataTable ObterTodasVersoes(Int32 idDocumento)
        {
            return new DocumentoDAO().ObterTodasVersoes(idDocumento);
        }
        public void AtualizarStatusDocumento(Int32 idDocumento)
        {
            new DocumentoDAO().AtualizarStatusDocumento(idDocumento);
        }
        public Boolean VerificarPropostaCancelada(Int32 numeroDocumento)
        {
            return new DocumentoDAO().PropostaCancelada(numeroDocumento);
        }
        public Documento ObterPropostaPorIdDocumentoContrato(Int32 idDocumentoContrato)
        {
            Documento oDocumento = new DocumentoDAO().ObterPorIdContrato(idDocumentoContrato);
            return oDocumento;
        }
        public Int32 ObterIdDocumentoProposta(Int32 idDocumentoContrato)
        {
            return new DocumentoDAO().ObterIdDocumentoProposta(idDocumentoContrato);
        }
        public Int32 ObterMaiorRevisaoProposta(Int32 idDocumentoContrato)
        {
            return new DocumentoDAO().ObterMaiorRevisaoProposta(idDocumentoContrato);
        }

        public DataTable ObterTodosDocumentosEmCirculacao(List<Filial> listFilial)
        {
            return new DocumentoDAO().ObterDocumentosEmCirculacao(listFilial);
        }

        public DataTable ObterTodosDocumentosCancelados(List<Filial> listFilial)
        {
            return new DocumentoDAO().ObterDocumentosCancelados(listFilial);
        }

        public DataTable ObterTodosDocumento(String valor, Int32 idPerfil, Usuario usuario)
        {
            StringBuilder filiais = new StringBuilder();
            for (int i = 0; i < usuario.Filiais.Count; i++)
            {
                filiais.Append(usuario.Filiais[i].IdFilial);
                if (usuario.Filiais.Count - 1 != i)
                    filiais.Append(",");
            }

            String segmentos;
            if (String.IsNullOrWhiteSpace(valor))
                throw new MyException("Informe um parametro para pesquisa");

            if (valor.Length < 3)
                throw new MyException("Parametro muito curto, o minímo necessário é de 3 caracteres.");

            switch (idPerfil)
            {
                case 7:
                case 6:
                    segmentos = "3";
                    break;
                case 3:
                case 2:
                    segmentos = "1,2,3,4,5,6";
                    break;
                default:
                    segmentos = "1,2,4,5,6";
                    break;
            }

            Int32 resultado;
            String valorLimpo = valor.Replace(".", String.Empty).Replace(" ", "").Trim();
            return new DocumentoDAO().ObterTodosDocumento(Int32.TryParse(valorLimpo, out resultado) ? valorLimpo : valor, segmentos, filiais.ToString());
        }

        public void VerificarPropostaFechadaPMWeb(Documento documento)
        {
            if (!documento.EProposta) return;
            DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(documento.NumeroDocumento);
            Document_TodoListDetails oDocumentTodoListDetails = new Document_TodoListDetails().ObterDocumentTodoListDetails(oDocumentTodoList.IdDocumentoTodoList);
            if (oDocumentTodoListDetails.Description != null)
                throw new MyException(String.Format("A oportunidade está fechada no PMWeb - ({0}) :(", oDocumentTodoListDetails.Description));
        }
    }
}
