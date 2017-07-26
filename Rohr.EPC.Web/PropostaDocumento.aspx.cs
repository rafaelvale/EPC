using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class PropostaDocumento : BasePage
    {
        readonly Documento _documento = new Documento();

        void ValidarNumeroProposta()
        {
            Int32 numeroOportunidade;

            if (String.IsNullOrEmpty(txtNumeroOportunidade.Text) || (txtNumeroOportunidade.Text.Length != 6))
                throw new MyException("Informe o número da oportunidade corretamento ;(");

            if (!Int32.TryParse(txtNumeroOportunidade.Text, out numeroOportunidade))
                throw new MyException("Informe o número da oportunidade corretamento ;(");

            if (new DocumentoBusiness().PropostaExiste(numeroOportunidade))
                throw new MyException("A oportunidade pesquisada já se encontra no fluxo de aprovações ;(");
            if (new DocumentoBusiness().PropostaTemContrato(numeroOportunidade))
                throw new MyException("Já existe um contrato associado a oportunidade pesquisada ;(");

            _documento.NumeroDocumento = numeroOportunidade;
            _documento.Usuario = new Util().GetSessaoUsuario();
            _documento.EProposta = true;
            _documento.Eminuta = true;


            ObterOportunidadePmweb(numeroOportunidade);
            ObterModeloDocumento(Int32.Parse(ddlModelo.SelectedValue));

            panelDescricaoOportunidade.Visible = true;
            panelModelo.Visible = true;

            CarregarPainelDetalhes();
        }
        void ObterOportunidadePmweb(Int32 numeroOportunidade)
        {
            try
            {
                DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(numeroOportunidade);

                Document_TodoListDetails oDocumentTodoListDetails = new Document_TodoListDetails().ObterDocumentTodoListDetails(oDocumentTodoList.IdDocumentoTodoList);
                if (oDocumentTodoListDetails.Description != null)
                    throw new MyException(String.Format("A oportunidade está fechada no PMWeb - ({0}) :(", oDocumentTodoListDetails.Description));

                Contacts comercialResponsavel = new Contacts().ObterComercialResponsavelOportunidade(oDocumentTodoList.IdDocumentoTodoList);
                Contacts contatoComercialCliente = new Contacts().ObterContatoComercialOportunidade(oDocumentTodoList.IdDocumentoTodoList);

                if (!new FilialBusiness().VerificarPropostaFilialUsuario(comercialResponsavel.CompanyId, new Util().GetSessaoUsuario()))
                    throw new MyException(String.Format("Você não tem permissão para criar a oportunidade {0}. A oportunidade não pertence a sua filial :(", numeroOportunidade));


                Company filial = new Company().ObterFilial(comercialResponsavel.CompanyId);
                CompanyAddress filialEndereco = new CompanyAddress().ObterCompanyAddresses(filial.Id);
                _documento.DocumentoFilial = new DocumentoFilial
                {
                    NomeUnidade = filial.ShortName,
                    Cidade = filialEndereco.City.Description,
                    Telefone = filialEndereco.Phone
                };


                Company cliente = new Company().ObterCompany(contatoComercialCliente.CompanyId);
                CompanyAddress clienteEndereco = new CompanyAddress().ObterCompanyAddresses(cliente.Id);

                _documento.Filial = new FilialBusiness().ObterFilial(filial.Id);

                _documento.CodigoSistemaOrigem = oDocumentTodoList.IdDocumentoTodoList;
                _documento.DocumentoCliente.Nome = cliente.CompanyName;
                _documento.DocumentoCliente.CpfCnpj = cliente.CompanyCode;
                _documento.DocumentoCliente.RgIe = cliente.StateTaxId;
                _documento.DocumentoCliente.CodigoOrigem = cliente.Id;
                _documento.DocumentoCliente.Endereco = clienteEndereco.Address1;
                _documento.DocumentoCliente.Numero = clienteEndereco.AltPhone;
                _documento.DocumentoCliente.Bairro = clienteEndereco.Address2;
                _documento.DocumentoCliente.Cidade = clienteEndereco.City.Description;
                _documento.DocumentoCliente.Estado = clienteEndereco.States.StateKey;
                _documento.DocumentoCliente.Cep = clienteEndereco.Zip;

                _documento.DocumentoComercial = new DocumentoComercial
                {
                    PrimeiroNome = comercialResponsavel.FirstName,
                    Sobrenome = comercialResponsavel.LastName,
                    CodigoSistemaOrigem = comercialResponsavel.SpecificationsId,
                    Departamento = comercialResponsavel.DepartmentName,
                    Email = comercialResponsavel.Email,
                    Telefone = comercialResponsavel.Phone
                };

                _documento.DocumentoObra.Nome = oDocumentTodoList.Description;
                _documento.DocumentoObra.Endereco = new Specifications().ObterEnderecoObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
                _documento.DocumentoObra.Bairro = new Specifications().ObterBairroObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
                _documento.DocumentoObra.Cidade = new Specifications().ObterCidadeObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
                _documento.DocumentoObra.CEP = new Specifications().ObterCepObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
                _documento.DocumentoObra.Estado = new Specifications().ObterEstadoObraOportunidade(oDocumentTodoList.IdDocumentoTodoList, _documento.EProposta).Measure;
            }
            catch (SqlException sqlEx)
            {
                Session["documento"] = null;
                linkButtonPesquisar.Enabled = false;
                linkButtonPesquisar.Attributes.Add("disabled", "disabled");
                NLog.Log().Fatal(sqlEx);
                ExcecaoBusiness.Adicionar(sqlEx, HttpContext.Current.Request.Url.AbsolutePath);

                throw new Exception("O PMWeb está indisponível, tente novamente mais tarde :(");
            }
        }
        void ObterModeloDocumento(Int32 idModelo)
        {
            Modelo oModelo = new ModeloBusiness().ObterModeloPorId(idModelo);

            if (oModelo == null || oModelo.IdModelo == 0)
            {
                Util.ExibirMensagem(lblMensagem, "Não foi possível recuperar o modelo da proposta.", Util.TipoMensagem.Alerta);
                return;
            }

            _documento.Modelo = oModelo;
            MontarHtmlModelo(oModelo);
        }
        void MontarHtmlModelo(Modelo modelo)
        {
            lblModeloEscolhido.Text = modelo.Titulo;
            lblVersao.Text = modelo.Versao.ToString();
            lblCriado.Text = modelo.DataCadastro.ToString("dd-MM-yyyy");
            StringBuilder textoParte = new StringBuilder();
            foreach (Parte parte in modelo.ListParte)
            {
                textoParte.Append(parte.TextoParte);
                textoParte.Append("<br /><br />");
            }
            CKEditor.Text = textoParte.ToString();
            VerificarDataCriacao(modelo);
        }
        void CarregarDropDownModelos()
        {
            List<Modelo> listModelo = new ModeloBusiness().ObterTodos(new Util().GetSessaoPerfilAtivo());
            ddlModelo.DataSource = listModelo;
            ddlModelo.DataValueField = "IdModelo";
            ddlModelo.DataTextField = "Titulo";
            ddlModelo.DataBind();
        }
        void VerificarDataCriacao(Modelo modelo)
        {
            lblMensagemDataCriacao.Text = String.Empty;
            if (new ModeloBusiness().CadastroRecente(modelo))
                lblMensagemDataCriacao.Text = "Modelo criado ou atualizado recentemente.";
        }
        void CarregarPainelDetalhes()
        {
            List<Label> listControles = new List<Label>
                {
                    lblOportunidade,
                    lblCliente,
                    lblObra,
                    lblModelo,
                    lblComercial,
                    lblModelo
                };
            new Util().CarregarPainelDetalhes(listControles, _documento);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CKEditor.config.toolbar = new object[] { new object[] { "Table", "Maximize", "-", "Bold", "Italic", "Underline" } };
                CKEditor.ReadOnly = true;
                CKEditor.config.removePlugins = "elementspath";

                if (IsPostBack) return;

                CarregarDropDownModelos();
            }
            catch (Exception ex)
            {
                Session["documento"] = null;
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void linkButtonPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                Session["documento"] = null;
                ValidarNumeroProposta();
                new AuditoriaLogBusiness().AdicionarLogDocumentoPesquisadoPMWeb(_documento, Request.Browser);
                new DocumentoBusiness().VerificarPropostaFechadaPMWeb(_documento);
                Session["documento"] = _documento;
            }
            catch (Exception ex)
            {
                Session["documento"] = null;
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
        protected void btnUtilizarModelo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["documento"] == null)
                    throw new MyException("Não foi possível recuperar o documento");

                Documento oDocumento = ((Documento)Session["documento"]);
                oDocumento.Edicao = false;
                oDocumento.DataParaExibicao = DateTime.Now;

                ValidarNumeroProposta();
                DocumentoBusiness oDocumentoBusiness = new DocumentoBusiness();

                Estimates oEstimates = null;
                if (oDocumento.EProposta)
                    oEstimates = new Estimates().ObterEstimate(new Estimates().ObterEstimatePorDocumentoId(new DocumentTodoList().ObterDocumentTodoLists(_documento.NumeroDocumento).IdDocumentoTodoList).Id);
                else
                {
                    CostManagementCommitments oCostManagementCommitments = new CostManagementCommitments().ObterContrato(_documento.CodigoSistemaOrigem);
                    oEstimates = new Estimates().ObterEstimateContrato(oCostManagementCommitments.EstimatedId);
                }

                oDocumento.PercentualLimpeza = Convert.ToDecimal(new Specifications().ObterPercentualLimpezaProposta(oEstimates.Id).Measure);

                oDocumento.IdDocumento = oDocumentoBusiness.AdicionarProposta(oDocumento, new Util().GetSessaoUsuario().IdUsuario, new Util().GetSessaoPerfilAtivo());


                new AuditoriaLogBusiness().AdicionarLogDocumentoInicioElabarocao(_documento, Request.Browser);

                Response.Redirect("Variaveis.aspx", false);
            }
            catch (SqlException exSql)
            {
                Util.ExibirMensagem(lblMensagem, "Não foi possível validar o modelo escolhido :(", Util.TipoMensagem.Erro);
                NLog.Log().Error(exSql);
                ExcecaoBusiness.Adicionar(exSql, HttpContext.Current.Request.Url.AbsolutePath);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, "Não foi possível validar o modelo escolhido." + ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }
    }
}