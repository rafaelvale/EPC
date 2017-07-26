using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Data;

namespace Rohr.EPC.Business
{
    public class ModeloEmailBusiness
    {
        private ModeloEmail Obter(Int32 idModelo)
        {
            return new ModeloEmailDAO().ObterPorId(idModelo);
        }

        public void EnviarEmail(String mensagem, String tituloCorpoEmail, String destinatario)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(destinatario) && !String.IsNullOrWhiteSpace(mensagem))
                    new ModeloEmailDAO().EnviarEmail(destinatario, "EPC",
                        SubstituirChavesModelo(
                            Obter(2).Modelo,
                            "",
                            null,
                            mensagem,
                            tituloCorpoEmail,
                            "")
                            );
            }
            catch (Exception ex)
            {
                ExcecaoBusiness.Adicionar(ex, "");
            }
        }
        public void EnviarEmail(Documento documento, Int32 idPerfilDestino)
        {
            EnviarEmail(documento, idPerfilDestino, String.Format("EPC - Análise de documento nº: {0}", documento.NumeroDocumento.ToString("N0")));
        }
        public void EnviarEmail(Documento documento, Int32 idPerfilDestino, String titulo)
        {
            try
            {
                DataTable listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, idPerfilDestino);

                for (int i = 0; i < listaEmail.Rows.Count; i++)
                {
                    new ModeloEmailDAO().EnviarEmail(
                        listaEmail.Rows[i][1].ToString(),
                        titulo,
                        SubstituirChavesModelo(
                            Obter(4).Modelo,
                            listaEmail.Rows[i][0].ToString(),
                            documento,
                            "EPC - Análise de documento",
                            "Verifiquei que chegou no EPC um documento para sua análise.")
                        );
                }
            }
            catch (Exception ex)
            {
                ExcecaoBusiness.Adicionar(ex, "");
            }
        }
        public void EnviarEmail(Documento documento, Int32 idPerfilDestino, String titulo, String conteudo)
        {
            try
            {
                DataTable listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, idPerfilDestino);

                for (int i = 0; i < listaEmail.Rows.Count; i++)
                {
                    new ModeloEmailDAO().EnviarEmail(
                        listaEmail.Rows[i][1].ToString(),
                        titulo,
                        SubstituirChavesModelo(
                            Obter(4).Modelo,
                            listaEmail.Rows[i][0].ToString(),
                            documento,
                            titulo,
                            conteudo)
                        );
                }
            }
            catch (Exception ex)
            {
                ExcecaoBusiness.Adicionar(ex, "");
            }
        }
        public void EnviarEmail(Documento documento, Workflow workflow, WorkflowBusiness.Acao acao)
        {
            try
            {
                DataTable listaEmail = null;
                String subtitulo;
                String titulo;

                var aprovado = acao == WorkflowBusiness.Acao.Aprovar;

                if (acao == WorkflowBusiness.Acao.Reprovar && (workflow.WorkflowEtapa.IdWorkflowEtapa == 1 || workflow.WorkflowEtapa.IdWorkflowEtapa == 7))
                {

                    titulo = String.Format("EPC - Documento reprovado nº: {0}", documento.NumeroDocumento.ToString("N0"));
                    subtitulo = String.Format("Verifiquei que chegou no EPC um documento reprovado pelo(a) {0} no perfil {1}. <br /><br /><b>Justificativa: {2}</b>",
                        new Util().GetSessaoUsuario().PrimeiroNome,
                        new PerfilBusiness().ObterPorId(new Util().GetSessaoPerfilAtivo()).Descricao,
                        workflow.Justificativa);

                    try
                    {
                        String email = UsuarioBusiness.ObterPorId(documento.Usuario.IdUsuario).Email;
                        if (email != null)

                            new ModeloEmailDAO().EnviarEmail(email, titulo,
                                SubstituirChavesModelo(
                                   Obter(4).Modelo,
                                   UsuarioBusiness.ObterPorId(documento.Usuario.IdUsuario).PrimeiroNome,
                                   documento,
                                   titulo,
                                   subtitulo));
                    }
                    catch (Exception ex)
                    {
                        ExcecaoBusiness.Adicionar(ex, "");
                    }
                }
                else
                {
                    if (workflow.Justificativa != String.Empty)
                        EnviarEmail(documento, 1,
                            String.Format("EPC - Documento nº {0} ({1})", documento.NumeroDocumento.ToString("N0"), "Aprovado"),
                            String.Format("O(a) {0} no perfil {1} aprovou o documento nº {2}, mas deixou uma justificativa. <br /><br /><b>Justificativa: {3}</b>",
                            new Util().GetSessaoUsuario().PrimeiroNome,
                            new PerfilBusiness().ObterPorId(new Util().GetSessaoPerfilAtivo()).Descricao,
                            documento.NumeroDocumento.ToString("N0"),
                            workflow.Justificativa
                            ));


                    if (new PerfilBusiness().ObterPorId(new Util().GetSessaoPerfilAtivo()).IdPerfil != 10)
                    {
                        titulo = String.Format("EPC - Documento nº {0} ({1})", documento.NumeroDocumento.ToString("N0"), workflow.WorkflowAcao.Descricao);
                        subtitulo = "Verifiquei que chegou no EPC um documento para você.";

                        switch (workflow.WorkflowEtapa.IdWorkflowEtapa)
                        {
                            case 1:
                                listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, 1, aprovado);
                                break;
                            case 5:
                                listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, 5, aprovado);
                                break;
                            case 3:
                                listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, 3, aprovado);
                                break;
                            case 4:
                                listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, 4, aprovado);
                                break;
                            case 6:
                                listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, 5, aprovado);
                                break;
                            case 7:
                                listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, 6, aprovado);
                                break;
                            case 9:
                                listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, 8, aprovado);
                                break;
                            case 11:
                                listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, 9, aprovado);
                                break;
                            case 12:
                                listaEmail = ObterListaDestinatarioEmail(documento.Filial.IdFilial, 10, aprovado);
                                break;
                        }
                        for (int i = 0; i < listaEmail.Rows.Count; i++)
                        {
                            try
                            {
                                if (listaEmail.Rows[i][1] != null)
                                    new ModeloEmailDAO().EnviarEmail(listaEmail.Rows[i][1].ToString(), titulo,
                                        SubstituirChavesModelo(
                                           Obter(4).Modelo,
                                           listaEmail.Rows[i][0].ToString(),
                                           documento,
                                           titulo,
                                           subtitulo)
                                        );
                            }
                            catch (Exception ex)
                            {
                                ExcecaoBusiness.Adicionar(ex, "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExcecaoBusiness.Adicionar(ex, "");
            }
        }

        DataTable ObterListaDestinatarioEmail(Int32 idFilial, Int32 idPerfilDestino)
        {
            return new ModeloEmailDAO().ObterListaEmail(idFilial, idPerfilDestino);
        }
        DataTable ObterListaDestinatarioEmail(Int32 idFilial, Int32 idPerfilDestino, Boolean aprovado)
        {
            return new ModeloEmailDAO().ObterListaEmail(idFilial, idPerfilDestino, aprovado);
        }

        String SubstituirChavesModelo(String modelo, String nomeUsuarioDestino, Documento documento, String conteudo, String tituloCorpoEmail, String subTitulo)
        {
            String retorno = modelo;
            retorno = retorno.Replace("{nome.usuario}", nomeUsuarioDestino);

            if (documento != null)
            {
                retorno = retorno.Replace("{numero.documento}", documento.NumeroDocumento.ToString("N0"));
                retorno = retorno.Replace("{nome.cliente}", documento.DocumentoCliente.Nome);
                retorno = retorno.Replace("{nome.obra}", documento.DocumentoObra.Nome);
                retorno = retorno.Replace("{valorFaturamentoMensal}", "R$ " + documento.ValorFaturamentoMensal.ToString("N2"));
                retorno = retorno.Replace("{valorNegocio}", "R$ " + documento.ValorNegocio.ToString("N2"));
                retorno = retorno.Replace("{descontoMedio}", documento.PercentualDesconto.ToString("N2") + "%");
                retorno = retorno.Replace("{maiorDesconto}", new DocumentoObjetoBusiness().ObterMaiorDesconto(documento).ToString("N2") + "%");
                retorno = retorno.Replace("{menorDesconto}", new DocumentoObjetoBusiness().ObterMenorDesconto(documento).ToString("N2") + "%");
                retorno = retorno.Replace("{nome.comercial}", documento.DocumentoComercial.PrimeiroNome + " " + documento.DocumentoComercial.Sobrenome);
                retorno = retorno.Replace("{historicoAcoes}", new WorkflowBusiness().ObterHistoricoAcoesFormatadoEmail(documento));
            }
            retorno = retorno.Replace("{titulo}", tituloCorpoEmail);
            retorno = retorno.Replace("{conteudo}", conteudo);
            retorno = retorno.Replace("{subtitulo}", subTitulo);

            return retorno;
        }
        String SubstituirChavesModelo(String modelo, String nomeUsuarioDestino, Documento documento, String tituloCorpoEmail, String subTitulo)
        {
            return SubstituirChavesModelo(modelo, nomeUsuarioDestino, documento, "", tituloCorpoEmail, subTitulo);
        }
    }
}