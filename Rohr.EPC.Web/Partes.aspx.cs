using System.Drawing;
using System.Web.UI;
using CKEditor.NET;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Rohr.PMWeb;
using System.Web.UI.HtmlControls;
using Rohr.EPC.DAL;

namespace Rohr.EPC.Web
{
    public partial class Partes : BasePage
    {
        Documento _documento;
        String PartesDocumento;

        static void AdicionarPartesPreenchidas(Documento documento)
        {
            new PartePreenchidaBusiness().AdicionarPartePreenchida(documento);
        }
        void MontarCkEditor(Documento documento)
        {
            foreach (Parte parte in documento.Modelo.ListParte)
            {
                if (!parte.Exibir) continue;
                CKEditorControl ck = new CKEditorControl
                {
                    EnterMode = EnterMode.BR,
                    config =
                    {
                        pasteFromWordPromptCleanup = true,
                        forcePasteAsPlainText = true,
                        removeDialogTabs = "image:Link",
                        fillEmptyBlocks = false,
                        toolbar =
                            new object[]
                            {
                                new object[]
                                {
                                    "Maximize", "-", "Table", "-", "Cut", "Copy", "-", "Undo", "Redo", "-", "Bold",
                                    "Italic", "Underline", "-", "RemoveFormat", "Link"
                                }
                            }
                    },
                    IgnoreEmptyParagraph = false
                };


                if (!parte.PermiteEdicao)
                {
                    ck.ReadOnly = true;
                    ck.UIColor = Color.White;
                }

                String texto = String.Empty;

                if (documento.ListPartePreenchida != null && documento.ListPartePreenchida.Count > 0)
                {
                    Parte parte1 = parte;
                    var res = documento.ListPartePreenchida.Where(x => x.IdParte == parte1.IdParte);

                    if (res.Any())
                    {
                        if (!res.First().Texto.Equals(Server.HtmlDecode(parte.TextoParte)))
                            ck.UIColor = Color.Red;

                        texto = new Util().SubstituirChaves(res.Single().Texto, res.Single().IdParte, documento, true);
                    }

                }
                else
                {
                    texto = new Util().SubstituirChaves(parte.TextoParte, parte.IdParte, documento, true);
                }
                ck.ID = parte.IdParte.ToString();
                ck.Text = texto;

                panelPartes.Controls.Add(ck);

                PartesDocumento += String.Format("<li><a href=\"#cke_contentBody_{0}\">{1}</a>", ck.ID, parte.Nome);
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
            List<Label> listControles = new List<Label> { lblOportunidade, lblCliente, lblObra, lblModelo, lblComercial, lblContrato };
            new Util().CarregarPainelDetalhes(listControles, _documento, documentoProposta);
        }
        String SubstituirValorPorChave(String partePreenchida, Int32 idParte)
        {
            String[] partesTexto = partePreenchida.Split(new[] { "<a href=\"{" }, StringSplitOptions.None);

            var res = _documento.Modelo.ListParte.Single(x => x.IdParte == idParte);

            if (res.ListChave == null) return Server.HtmlDecode(partePreenchida);

            List<String> listChaveUnica = new List<String>();
            for (int i = 1; i < partesTexto.Length; i++)
            {
                if (!listChaveUnica.Contains(partesTexto[i].Substring(0, partesTexto[i].IndexOf("}", StringComparison.Ordinal))))
                    listChaveUnica.Add(partesTexto[i].Substring(0, partesTexto[i].IndexOf("}", StringComparison.Ordinal)));
            }

            if (listChaveUnica.Count != res.ListChave.Count)
                throw new MyException("Não foi possível identificar as chaves no modelo. Verifique se você não apagou nenhuma chave (em vermelho). As partes serão substituidas pelo texto original.");
            foreach (String chaveNoTexto in partesTexto)
            {
                String[] chaves = chaveNoTexto.Split(new[] { "}:" }, StringSplitOptions.None);

                if (chaves.Length != 2) continue;
                String chaveOriginal = "{" + chaves[0] + "}";   
                String valorChaveOriginal = Server.HtmlDecode(chaves[1].Substring(0, chaves[1].IndexOf("\">", StringComparison.OrdinalIgnoreCase)));
                String valorTela = Server.HtmlDecode(chaves[1]).Substring(Server.HtmlDecode(chaves[1]).IndexOf("\">", StringComparison.OrdinalIgnoreCase) + 2, valorChaveOriginal.Length);

                if (String.Compare(valorChaveOriginal, valorTela,   StringComparison.OrdinalIgnoreCase) != 0)
                    throw new MyException("Você alterou um valor de uma variável (em vermelho). Se for necessário, altere na tela de variáveis. As partes serão substituidas pelo texto original.");

                String chaveCompleta = "<a href=\"" + chaveOriginal + ":" + valorChaveOriginal + "\">" + valorTela ;
                partePreenchida = Server.HtmlDecode(partePreenchida).Replace(chaveCompleta, chaveOriginal);
            }
            return partePreenchida;
        }
        void CarregarPartesModelo()
        {
            panelPartes.Controls.Clear();
            _documento = new Util().GetSessaoDocumento();

            MontarCkEditor(_documento);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                panelPartes.Controls.Clear();
                _documento = new Util().GetSessaoDocumento();

                if (_documento.EProposta)
                {
                    lblTipoDocumento.Text = "Proposta";
                    PartesDocumento += "<li><strong>Proposta</strong></li>";
                    new DocumentoBusiness().VerificarPropostaFechadaPMWeb(_documento);
                }
                else
                {
                    lblTipoDocumento.Text = "Contrato";
                    PartesDocumento += "<li><strong>Contrato</strong></li>";
                }

                if (!_documento.Edicao)
                    _documento.ListPartePreenchida.Clear();

                MontarCkEditor(_documento);
                CarregarPainelDetalhes();

                menuApoio.Text = "<ul class=\"nav nav-list span2 navex\" data-spy=\"affix\" data-offset-top=\"120\" >" + PartesDocumento + "</ul>";
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                

                _documento = new Util().GetSessaoDocumento();
                List<PartePreenchida> listPartePreenchidaTela = panelPartes.Controls.OfType<CKEditorControl>().Select(item => (item)).Select(oCKEditorContro => new PartePreenchida
                {
                    IdDocumento = _documento.IdDocumento,
                    IdParte = Int32.Parse(oCKEditorContro.ID),
                    Texto = SubstituirValorPorChave(oCKEditorContro.Text, Int32.Parse(oCKEditorContro.ID)),
                    DataCadastro = DateTime.Now
                }).ToList();

                if (_documento.ListPartePreenchida == null)
                    _documento.ListPartePreenchida = new List<PartePreenchida>();

                _documento.ListPartePreenchida.Clear();
                _documento.ListPartePreenchida = listPartePreenchidaTela;


                AdicionarPartesPreenchidas(_documento);
                Session["documento"] = null;
                Session["documento"] = _documento;
                new AuditoriaLogBusiness().AdicionarLogDocumentoPartes(_documento, Request.Browser);
                Response.Redirect("Finalizar.aspx", false);
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "\'").Replace(Environment.NewLine, string.Empty) + "');</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                lblMensagem.Text = ex.Message;
                CarregarPartesModelo();
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
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnReload_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}