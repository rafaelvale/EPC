﻿using CKEditor.NET;
using Lib.Classes;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    
    public partial class PortfolioObras : BasePage
    {
        Documento _documento;
        Documento _documentoProposta;

        TableCell MontarTextBox(DocumentoPortfolioObras DocImg)
        {
            TableCell td = new TableCell();

            TextBox textBox = new TextBox { ID = "00" + DocImg.Id.ToString() };
            textBox.Attributes.Add("placeholder", "");
            textBox.CssClass = "input input x-large";
            textBox.TextMode = TextBoxMode.MultiLine;
            textBox.Width = 684;
            textBox.Height = 100;
            ((TextBox)textBox).Text = DocImg.DescricaoImagem;
            td.Controls.Add(textBox);
            td.ColumnSpan = 2;

            return td;
        }
        

        void CarregarPainelDetalhes()
        {
            if (!_documento.EProposta)
            {
                divContrato.Visible = true;
                lblContrato.Visible = true;
            }
            List<Label> listControles = new List<Label> { lblOportunidade, lblCliente, lblObra, lblModelo, lblComercial, lblContrato };
            new Util().CarregarPainelDetalhes(listControles, _documento, _documentoProposta);
        }

        void MontarHtml(List<DocumentoPortfolioObras> DocImagens)
        {
            Table1.Controls.Clear();

            foreach (DocumentoPortfolioObras DocImg in DocImagens)
            {
                TableRow trTesto = new TableRow();
                trTesto.Cells.Add(MontarTextBox(DocImg));
                trTesto.CssClass = "textarea";
                Table1.Rows.Add(trTesto);

                TableRow trimagem = new TableRow();

                TableCell tdckb = new TableCell();

                ImageButton btn = new ImageButton { ID = DocImg.Id.ToString() };
                btn.ImageUrl = "~/Imagens/Icons/delete.gif";
                btn.Click += new ImageClickEventHandler(butDeleteImagem_Click);
                tdckb.Controls.Add(btn);
                trimagem.Cells.Add(tdckb);
                Table1.Rows.Add(trimagem);

                TableCell tdig = new TableCell();
                Image ig = new Image();
                ig.ImageUrl = ResolveUrl(DocImg.Url);
                ig.ID = "idimg" + DocImg.Id.ToString();
                ig.CssClass = "image";
                tdig.Controls.Add(ig);
                trimagem.Cells.Add(tdig);
                Table1.Rows.Add(trimagem);
            }
        }

        protected void linkButtonPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                var DocuImagens = new DocumentoPortfolioBusiness().GetDocumentoImagens(_documento.IdDocumento);
                if (PesquisaFoto.Text.IsNullOrEmpty())
                {
                    LoadImagens(DocuImagens);
                }else
                {
                    LoadBuscaImagem(DocuImagens);
                }
                

            }catch(Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }


        }

        protected void butDeleteImagem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton Btn = sender as ImageButton;
                if (Btn != null)
                {
                    Int32 IdDocImagem = Convert.ToInt32(Btn.ID);

                    new DocumentoPortfolioBusiness().DeleteDocumentoPortfolio(IdDocImagem);
                }

                var DocuImagens = new DocumentoPortfolioBusiness().GetDocumentoImagens(_documento.IdDocumento);
               LoadImagens(DocuImagens);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        void VerificarRevisao(Documento oDocumento)
        {
            if (!oDocumento.Edicao) return;

            Workflow oWorkflow = new WorkflowBusiness().ObterUltimaAcaoPorIdDocumento(_documento.IdDocumento);
            WorkflowAcaoExecutada oWorkflowAcaoExecutada = new WorkflowAcaoExecutadaBusiness().ObterUltimaAcaoPorIdDocumento(_documento.IdDocumento);


            if (oWorkflow.WorkflowAcao.IdWorkflowAcao == 17 &&
                    (oWorkflowAcaoExecutada.WorkflowAcao.IdWorkflowAcao != 47 && oWorkflowAcaoExecutada.WorkflowAcao.IdWorkflowAcao != 48)
                )
            {
                _documento.VersaoInterna += 1;

                if (new WorkflowBusiness().VerificarDocumentoEncaminhadoCliente(_documento.IdDocumento))
                    _documento.RevisaoCliente += 1;

                _documento.Usuario = new Util().GetSessaoUsuario();
                _documento.DocumentoStatus.IdDocumentoStatus = 4;
                _documento.Eminuta = true;
                DocumentoBusiness oDocumentoBusiness = new DocumentoBusiness();


                if (_documento.EProposta)
                    _documento.IdDocumento = oDocumentoBusiness.AdicionarRevisaoProposta(oDocumento, new Util().GetSessaoUsuario().IdUsuario, new Util().GetSessaoPerfilAtivo());
                else
                {
                    Documento documentoProposta = new DocumentoBusiness().ObterPropostaPorIdDocumentoContrato(_documento.IdDocumento);
                    _documento.IdDocumento = oDocumentoBusiness.AdicionarRevisaoContrato(_documento, documentoProposta, new Util().GetSessaoUsuario().IdUsuario,
                        new Util().GetSessaoPerfilAtivo());
                }
            }
            else if (oWorkflow.WorkflowAcao.IdWorkflowAcao == 15)
            {
                if (_documento.ListPartePreenchida == null)
                    _documento.ListPartePreenchida = new PartePreenchidaBusiness().ObterTodasPorIdDocumento(_documento.IdDocumento);
            }
        }
        DocumentTodoList ObterDocumentTodoListSession()
        {
            if (Session["pmwebDocumentTodoList"] == null)
            {
                DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(_documento.NumeroDocumento);
                Session["pmwebDocumentTodoList"] = oDocumentTodoList;
            }
            return (DocumentTodoList)Session["pmwebDocumentTodoList"];
        }
        void ObterOportunidadePmweb()
        {
            DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(_documento.NumeroDocumento);
            Contacts comercialResponsavel = new Contacts().ObterComercialResponsavelOportunidade(oDocumentTodoList.IdDocumentoTodoList);
            Contacts contatoComercialObra = new Contacts().ObterContatoComercialOportunidade(oDocumentTodoList.IdDocumentoTodoList);
            Company empresaCliente = new Company().ObterCompany(contatoComercialObra.CompanyId);

            _documento.CodigoSistemaOrigem = oDocumentTodoList.IdDocumentoTodoList;
            _documento.DocumentoCliente.Nome = empresaCliente.CompanyName;
            _documento.DocumentoCliente.CpfCnpj = empresaCliente.CompanyCode;
            _documento.DocumentoCliente.RgIe = empresaCliente.StateTaxId;
            _documento.DocumentoCliente.CodigoOrigem = empresaCliente.Id;

            _documento.DocumentoComercial = new DocumentoComercial
            {
                PrimeiroNome = comercialResponsavel.FirstName,
                Sobrenome = comercialResponsavel.LastName,
                CodigoSistemaOrigem = comercialResponsavel.SpecificationsId,
                Email = comercialResponsavel.Email,
                Telefone = comercialResponsavel.Phone,
                Departamento = comercialResponsavel.DepartmentName
            };

            _documento.DocumentoObra.Nome = oDocumentTodoList.Description;
            _documento.DocumentoObra.Endereco = new Specifications().ObterEnderecoObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
            _documento.DocumentoObra.Bairro = new Specifications().ObterBairroObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
            _documento.DocumentoObra.Cidade = new Specifications().ObterCidadeObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
            _documento.DocumentoObra.Estado = new Specifications().ObterEstadoObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
            _documento.DocumentoObra.CEP = new Specifications().ObterCepObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
        }
        void VerificarPropostaFechadaPMWeb(Documento documento)
        {
            if (!documento.EProposta) return;
            DocumentTodoList oDocumentTodoList =
                new DocumentTodoList().ObterDocumentTodoLists(_documento.NumeroDocumento);

            Document_TodoListDetails oDocumentTodoListDetails =
                new Document_TodoListDetails().ObterDocumentTodoListDetails(oDocumentTodoList.IdDocumentoTodoList);
            if (oDocumentTodoListDetails.Description != null)
                throw new MyException(String.Format("A oportunidade está fechada no PMWeb - ({0}) :(", oDocumentTodoListDetails.Description));
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                ResumoGeralFotos.config.toolbar = new object[] { new object[] { "Maximize", "-", "Undo", "Redo", "-", "Bold", "Italic", "RemoveFormat" } };
                ResumoGeralFotos.config.enterMode = EnterMode.BR;
                ResumoGeralFotos.config.removePlugins = "elementspath";
                ResumoGeralFotos.config.pasteFromWordPromptCleanup = true;
                ResumoGeralFotos.config.forcePasteAsPlainText = true;
                ResumoGeralFotos.config.fillEmptyBlocks = false;
                ResumoGeralFotos.config.ignoreEmptyParagraph = false;



                Session["pmwebDocumentTodoList"] = null;
                _documento = new Util().GetSessaoDocumento();

                if (_documento.Modelo.ModeloTipo.IdModeloTipo == 3)
                {
                    Session["documento"] = null;
                    Session["documento"] = _documento;
                    Session["pmwebDocumentTodoList"] = null;
                    Response.Redirect("PortfolioObrasParte2.aspx", false);
                }

                if (!_documento.Edicao)
                    _documento.ListChavePreenchida.Clear();

                if (_documento.EProposta)
                {
                    ObterOportunidadePmweb();
                    VerificarPropostaFechadaPMWeb(_documento);
                }
                else
                {
                    CostManagementCommitments oCostManagementCommitments = new CostManagementCommitments().ObterContrato(_documento.CodigoSistemaOrigem);
                    new Estimates().OrcamentoFinalizado(oCostManagementCommitments.ProjectId);

                    DocumentoBusiness oDocumentoBusiness = new DocumentoBusiness();
                    Int32 idDocumentoProposta = oDocumentoBusiness.ObterIdDocumentoProposta(_documento.IdDocumento);
                    _documentoProposta = oDocumentoBusiness.ObterPorId(idDocumentoProposta);
                }

                CarregarPainelDetalhes();

              

                var DocuImagens = new DocumentoPortfolioBusiness().GetDocumentoImagens(_documento.IdDocumento);

                LoadImagens(DocuImagens);
                ObterItensTela();
                CarregarResumoProposta(_documento);



            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                btnContinuar.Attributes.Add("disabled", "disabled");
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        private void buscaUltimaFotoPortfolioParte2(int IdDocumento)
        {

            new DocumentoPortfolioBusiness().obterUltimaFotoParte2(IdDocumento);

        }

        private void LoadBuscaImagem(List<DocumentoPortfolioObras> DocuImagens)
        {

            if(DocuImagens != null)
                MontarHtml(DocuImagens);

                var Imagens = new ImagemBusiness().GetBuscaImagens(PesquisaFoto.Text);

                if(Imagens != null)
                {
                    var qrImagens = Imagens.AsQueryable();

                    if(DocuImagens != null)
                    {
                        var IdImagem = DocuImagens.Select(DI => DI.IdImagem).ToList();
                        qrImagens = qrImagens
                            .Where(I => !IdImagem.Contains(I.IdImagem));
                    }

                    var ListImagens = qrImagens
                  .Select(I => new Imagens
                  {
                      IdImagem = I.IdImagem,
                      DescrArquivo = I.DescrArquivo,
                      FlagAtivo = I.FlagAtivo,
                      Tag = I.Tag,
                      Url = ResolveUrl("~/Imagens/UploadFotos/" + I.DirArquivo + "/" + I.NomeInterno),
                  }).ToList();

                    livImagens.DataSource = ListImagens;
                    livImagens.DataBind();
                }
        }
        private void LoadImagens(List<DocumentoPortfolioObras> DocuImagens)
        {

            if (DocuImagens != null)
                MontarHtml(DocuImagens);

            var Imagens = new ImagemBusiness().GetImagens();

            if (Imagens != null)
            {
                var qrImagens = Imagens.AsQueryable();

                if (DocuImagens != null)
                {
                    var IdImagem = DocuImagens.Select(DI => DI.IdImagem).ToList();
                    qrImagens = qrImagens
                        .Where(I => !IdImagem.Contains(I.IdImagem));
                }

                var ListImagens = qrImagens
                    .Select(I => new Imagens
                    {
                        IdImagem = I.IdImagem,
                        DescrArquivo = I.DescrArquivo,
                        FlagAtivo = I.FlagAtivo,
                        Tag = I.Tag,
                        Url = ResolveUrl("~/Imagens/UploadFotos/" + I.DirArquivo + "/" + I.NomeInterno),
                    }).ToList();

                livImagens.DataSource = ListImagens;
                livImagens.DataBind();
            }

        }

        protected void butEscolherReg_Click(object sender, CommandEventArgs e)
        {
            try
            {
                var butEsc = new ButtonCommand(sender);
                var idImagem = Convert.ToInt32(butEsc.Argument);
                var Imagens = new ImagemBusiness().GetImagens();
                var imagem = Imagens.FirstOrDefault(I => I.IdImagem == idImagem);

                DocumentoPortfolioObras DocImg = new DocumentoPortfolioObras();

                DocImg.IdDocumento = _documento.IdDocumento;
                DocImg.IdImagem = imagem.IdImagem;
                DocImg.Url = "~/Imagens/UploadFotos/" + imagem.DirArquivo + "/" + imagem.NomeInterno;
                DocImg.DescricaoImagem = imagem.DescrArquivo;

                new DocumentoPortfolioBusiness().AdicionarDocumentoPortfolio(DocImg);

                var DocuImagens = new DocumentoPortfolioBusiness().GetDocumentoImagens(_documento.IdDocumento);
                if (DocuImagens != null)
                  LoadImagens(DocuImagens);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < Table1.Rows.Count; i++)
                {
                    foreach (Object controle in Table1.Rows[i].Cells[0].Controls)
                    {
                        DocumentoPortfolioObras DocImg = new DocumentoPortfolioObras();

                        DocImg.IdDocumento = _documento.IdDocumento;
                        if (controle is TextBox)
                        {
                            TextBox textBox = ((TextBox)controle);
                            DocImg.Id = Int32.Parse(textBox.ID);
                            DocImg.DescricaoImagem = textBox.Text;
                            new DocumentoPortfolioBusiness().UpdateDocumentoPortfolio(DocImg);
                        }
                    }
                }

                new DocumentoDescricaoGeralPortfolioBusiness().AdicionarDescricaoGeral(_documento);



                Session["documento"] = null;
                Session["documento"] = _documento;
                Session["pmwebDocumentTodoList"] = null;
                Response.Redirect("PortfolioObrasParte2.aspx", false);

                buscaUltimaFotoPortfolioParte2(_documento.IdDocumento);


            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnDesistir_Click(object sender, EventArgs e)
        {
            try
            {
                Session["pmwebDocumentTodoList"] = null;
                Response.Redirect("Redirecionar.aspx?p=" + CriptografiaBusiness.Criptografar("99"), false);
            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }


        void ObterItensTela()
        {
            _documento.DocumentoDescricaoGeralPortfolio.Descricao = ResumoGeralFotos.Text;
            _documento.DocumentoDescricaoGeralPortfolio.IdDocumento = _documento.IdDocumento;
        }
        void CarregarResumoProposta(Documento documento)
        {
            if (!IsPostBack)
            {
                ResumoGeralFotos.Text = new DocumentoPortfolioBusiness().ObterUltimoDescricaoGeral(documento.IdDocumento);
            }
            

        }

    }
}