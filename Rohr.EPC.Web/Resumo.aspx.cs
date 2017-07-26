using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using CKEditor.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class Resumo : BasePage
    {
        Documento _documento;

        //void ValidarCampo()
        //{


        //    if (String.IsNullOrWhiteSpace(ResumoProposta.Text.Trim()))
        //        throw new MyException("Informe uma observação :(");


        //    if (ResumoProposta.Text.Trim().Length > 1000)
        //        throw new MyException("Resumo da Proposta muito longa. :(");


        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CKResumoProposta.config.toolbar = new object[] { new object[] { "Maximize", "-", "Undo", "Redo", "-", "Bold", "Italic", "RemoveFormat" } };
                CKResumoProposta.config.enterMode = EnterMode.BR;
                CKResumoProposta.config.removePlugins = "elementspath";
                CKResumoProposta.config.pasteFromWordPromptCleanup = true;
                CKResumoProposta.config.forcePasteAsPlainText = true;
                CKResumoProposta.config.fillEmptyBlocks = false;
                CKResumoProposta.config.ignoreEmptyParagraph = false;



                _documento = new Util().GetSessaoDocumento();
                new DocumentoBusiness().VerificarPropostaFechadaPMWeb(_documento);
                CarregarPainelDetalhes();

                if (IsPostBack) return;

                ObterItensTela();
                CarregarResumoProposta(_documento);
                
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                btnContinuar.Enabled = false;
                btnContinuar.Attributes.Add("disabled", "disabled");
                //panelResumo.Visible = false;
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);

            }

        }
        
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                ObterItensTela();

                Session["documento"] = null;
                Session["documento"] = _documento;
                new DocumentoResumoPropostaBusiness().AdicionarResumoProposta(_documento);

                new AuditoriaLogBusiness().AdicionarLogDocumentoResumo(_documento, Request.Browser);

                Response.Redirect("Partes.aspx", false);
            }
            catch(Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        protected void btnDesistir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Redirecionar.aspx?p=" + CriptografiaBusiness.Criptografar("99"), false);
            }
            catch(Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        void CarregarPainelDetalhes()
        {
            Documento documentoProposta = null;
            if (!_documento.EProposta)
            {
                DocumentoBusiness oDocumentoBusiness = new DocumentoBusiness();
                Int32 idDocumentoProposta = oDocumentoBusiness.ObterIdDocumentoProposta(_documento.IdDocumento);
                documentoProposta = oDocumentoBusiness.ObterPorId(idDocumentoProposta);

                divContrato.Visible = true;
                lblContrato.Visible = true;
            }
            List<Label> listControles = new List<Label> { lblOportunidade, lblCliente, LblObra, LblModelo, lblComercial, lblContrato };
            new Util().CarregarPainelDetalhes(listControles, _documento, documentoProposta);
        }

        void ObterItensTela()
        {
            _documento.DocumentoResumoProposta.Resumo = CKResumoProposta.Text;
            _documento.DocumentoResumoProposta.IdDocumento = _documento.IdDocumento;
        }
        void CarregarResumoProposta(Documento documento)
        {
            CKResumoProposta.Text = new DocumentoResumoBusiness().ObterUltimoResumo(documento.IdDocumento);
        }
       
    }
}