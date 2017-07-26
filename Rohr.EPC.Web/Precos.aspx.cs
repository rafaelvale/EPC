using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class Precos : System.Web.UI.Page
    {
        Documento _documento;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _documento = new Util().GetSessaoDocumento();
                new DocumentoBusiness().VerificarPropostaFechadaPMWeb(_documento);
                CarregarPainelDetalhes();
                VerificarModeloCliente(_documento);

                if (IsPostBack) return;
                CarregarItens();
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
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
            List<Label> listControles = new List<Label> { lblOportunidade, lblCliente, lblObra, lblModelo, lblComercial, lblContrato };
            new Util().CarregarPainelDetalhes(listControles, _documento, documentoProposta);
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                Session["documento"] = null;
                Session["documento"] = _documento;

                List<ItemPreco> listItemPreco = new List<ItemPreco>();
                foreach (RepeaterItem item in repeaterItens.Controls)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        listItemPreco.Add(new ItemPreco()
                        {

                            idItemSistemaOrigem = Int32.Parse(((CheckBox)item.Controls[0].FindControl("checkboxLinhaVUI")).Attributes["CodigoItemSistemaOrigem"]),
                            exibirVUI = ((CheckBox)item.Controls[0].FindControl("checkboxLinhaVUI")).Checked,
                            exibirVUL = ((CheckBox)item.Controls[0].FindControl("checkboxLinhaVUL")).Checked
                        });
                    }
                }

                new AuditoriaLogBusiness().AdicionarLogDocumentoPreco(_documento, Request.Browser);
                new DocumentoObjetoDetalheContratoBusiness().AdicionarDocumentoObjetoDetalheContrato(_documento, listItemPreco);

                if (Util.VerificarModeloCliente(_documento.Modelo.ModeloTipo))
                    Response.Redirect("Finalizar.aspx", false);
                else
                    Response.Redirect("Partes.aspx", false);
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        void CarregarItens()
        {
            List<DocumentoObjetoItem> listSubgrupos = new List<DocumentoObjetoItem>();
            List<DocumentoObjetoItem> listItens = new List<DocumentoObjetoItem>();

            foreach (DocumentoObjeto objeto in _documento.ListDocumentoObjeto)
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

            repeaterItens.DataSource = listSubgrupos.Concat(listItens);
            repeaterItens.DataBind();
        }
        void VerificarModeloCliente(Documento documento)
        {
            if (Util.VerificarModeloCliente(_documento.Modelo.ModeloTipo))
            {
                lblDescricao.Text = "Contrato do cliente. Não é necessário formatar.";
                lblDescricao.CssClass = "label label-info";
            }
            else
                lblDescricao.Text = "Selecione as colunas a serem exibidas no documento";
        }

        protected void repeaterItens_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            ((CheckBox)e.Item.FindControl("checkboxLinhaVUI")).Attributes.Add("CodigoItemSistemaOrigem", ((Rohr.EPC.Entity.DocumentoObjetoItem)(e.Item.DataItem)).CodigoItemSistemaOrigem.ToString());
            ((CheckBox)e.Item.FindControl("checkboxLinhaVUL")).Attributes.Add("CodigoItemSistemaOrigem", ((Rohr.EPC.Entity.DocumentoObjetoItem)(e.Item.DataItem)).CodigoItemSistemaOrigem.ToString());

            if (Util.VerificarModeloCliente(_documento.Modelo.ModeloTipo))
                ((CheckBox)e.Item.FindControl("checkboxLinhaVUL")).Enabled = false;

            ((Label)e.Item.FindControl("lblDescricaoCliente")).Text = EPC.Business.Util.LimparNomeResumidoDescricao(((Rohr.EPC.Entity.DocumentoObjetoItem)(e.Item.DataItem)).DescricaoResumida, ((Rohr.EPC.Entity.DocumentoObjetoItem)(e.Item.DataItem)).DescricaoCliente);
        }
    }
}