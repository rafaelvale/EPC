using System.Net;
using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Web;

namespace Rohr.EPC.Business
{
    public class AuditoriaLogBusiness
    {
        public List<AuditoriaLog> ObterLog(Int32 idTipoLog, Usuario usuario)
        {
            return new AuditoriaLogDAO().Obter(usuario);
        }
        public void AdicionarLogPdf(Documento documento, HttpBrowserCapabilities browser)
        {
            String descricao = String.Format("Pdf gerado, documento nº: {0}", documento.NumeroDocumento);
            AdicionarLog(1, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
        }
        public void AdicionarLogPlanilhaOrcamentaria(Documento documento, HttpBrowserCapabilities browser)
        {
            String descricao = String.Format("Planilha orçamentaria gerada, documento nº: {0}", documento.NumeroDocumento);
            AdicionarLog(2, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
        }
        public void AdicionarLogLogin(HttpBrowserCapabilities browser)
        {
            const String descricao = "Login";
            AdicionarLog(3, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
        }

        public void AdicionarLogLogout(HttpBrowserCapabilities browser)
        {
            const String descricao = "Logout";
            AdicionarLog(3, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
        }
        public void AdicionarLogPrimeiroAcesso(HttpBrowserCapabilities browser)
        {
            const String descricao = "Login - Primeiro Acesso";
            AdicionarLog(8, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
        }
        public void AdicionarLogAlteracaoSenha(HttpBrowserCapabilities browser)
        {
            const String descricao = "Senha Alterada";
            AdicionarLog(4, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
        }
        public void AdicionarLogAlteracaoNotificacao(HttpBrowserCapabilities browser)
        {
            const String descricao = "Notificações de Email Alterada";
            AdicionarLog(5, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
        }
        public void AdicionarLogDocumento(Documento documento, HttpBrowserCapabilities browser)
        {
            String descricao;
            if (documento.Edicao)
            {
                descricao = String.Format("Documento nº: {0} editado", documento.NumeroDocumento);
                AdicionarLog(7, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
            }
            else
            {
                descricao = String.Format("Documento nº: {0} criado", documento.NumeroDocumento);
                AdicionarLog(6, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
            }
        }
        public void AdicionarLogDocumentoPesquisadoPMWeb(Documento documento, HttpBrowserCapabilities browser)
        {
            AdicionarLog(7, new Util().GetSessaoUsuario().IdUsuario, String.Format("Documento nº: {0} pesquisado no PMWeb", documento.NumeroDocumento), browser);
        }
        public void AdicionarLogDocumentoInicioElabarocao(Documento documento, HttpBrowserCapabilities browser)
        {
            String descricao;
            if (documento.EProposta)
            {
                descricao = String.Format("Inicio da elaboração da Proposta nº: {0}", documento.NumeroDocumento);
                AdicionarLog(8, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
            }
            else
            {
                descricao = String.Format("Inicio da elaboração do Contrato nº: {0} criado", documento.NumeroDocumento);
                AdicionarLog(9, new Util().GetSessaoUsuario().IdUsuario, descricao, browser);
            }
        }
        public void AdicionarLogDocumentoVariaveis(Documento documento, HttpBrowserCapabilities browser)
        {
            AdicionarLog(10, new Util().GetSessaoUsuario().IdUsuario, String.Format("Campos do documento nº: {0} preenchido", documento.NumeroDocumento), browser);
        }
        public void AdicionarLogDocumentoObjeto(Documento documento, HttpBrowserCapabilities browser)
        {
            AdicionarLog(11, new Util().GetSessaoUsuario().IdUsuario, String.Format("Objeto do documento nº: {0} personalizado", documento.NumeroDocumento), browser);
        }
        public void AdicionarLogDocumentoPreco(Documento documento, HttpBrowserCapabilities browser)
        {
            AdicionarLog(22, new Util().GetSessaoUsuario().IdUsuario, String.Format("Parte Preços do documento nº: {0} personalizado", documento.NumeroDocumento), browser);
        }
        public void AdicionarLogDocumentoPartes(Documento documento, HttpBrowserCapabilities browser)
        {
            AdicionarLog(12, new Util().GetSessaoUsuario().IdUsuario, String.Format("Partes do documento nº: {0} revisadas e preenchidas", documento.NumeroDocumento), browser);
        }
        public void AdicionarLogDocumentoAcaoPrimaria(Documento documento, HttpBrowserCapabilities browser)
        {
            AdicionarLog(13, new Util().GetSessaoUsuario().IdUsuario, String.Format("Ação primaria realizada, documento nº: {0}", documento.NumeroDocumento), browser);
        }
        public void AdicionarLogDocumentoAcaoSecundaria(Documento documento, HttpBrowserCapabilities browser)
        {
            AdicionarLog(14, new Util().GetSessaoUsuario().IdUsuario, String.Format("Ação secundaria realizada, documento nº: {0}", documento.NumeroDocumento), browser);
        }
        public void AdicionarLogModeloExibicao(HttpBrowserCapabilities browser)
        {
            AdicionarLog(15, new Util().GetSessaoUsuario().IdUsuario, String.Format("Modo de exibição alterado"), browser);
        }
        public void AdicionarLogDocumentoResumo(Documento documento, HttpBrowserCapabilities browser)
        {
            AdicionarLog(23, new Util().GetSessaoUsuario().IdUsuario, String.Format("Reusumo da Proposta nº: {0} personalizado", documento.NumeroDocumento), browser);
        }
        public void AdicionarLog(Int32 idTipoLog, Int32 idUsuario, String descricao, HttpBrowserCapabilities browser)
        {
            new AuditoriaLogDAO().Adicionar(idTipoLog, idUsuario, new Util().GetSessaoPerfilAtivo(), descricao,
                HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                Dns.GetHostName(),
                HttpContext.Current.Request.ServerVariables["LOGON_USER"],
                browser.Type,
                browser.Browser,
                browser.Version,
                browser.IsMobileDevice);
        }
    }
}
