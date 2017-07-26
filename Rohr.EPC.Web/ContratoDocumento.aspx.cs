using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class ContratoDocumento : BasePage
    {
        readonly Documento _documentoContrato = new Documento();

        void CarregarPainelDetalhes(Documento documentoProposta)
        {
            List<Label> listControles = new List<Label> { lblOportunidade, lblCliente, lblObra, lblModelo, lblComercial, lblContrato };
            new Util().CarregarPainelDetalhes(listControles, _documentoContrato, documentoProposta);
        }

        void CarregarDocumento(Int32 idModelo, Documento documentoProposta)
        {
            _documentoContrato.DataParaExibicao = DateTime.Now;
            _documentoContrato.Edicao = false;
            _documentoContrato.VersaoInterna = 0;
            _documentoContrato.RevisaoCliente = 0;
            _documentoContrato.DataCadastro = DateTime.Now;
            _documentoContrato.PercentualDesconto = 0;
            _documentoContrato.ValorFaturamentoMensal = 0;
            _documentoContrato.DataParaExibicao = DateTime.Now;
            _documentoContrato.Eminuta = true;
            _documentoContrato.EProposta = false;
            _documentoContrato.Usuario.IdUsuario = new Util().GetSessaoUsuario().IdUsuario;
            _documentoContrato.DocumentoStatus.IdDocumentoStatus = 4;
            _documentoContrato.Modelo = new ModeloBusiness().ObterModeloPorId(idModelo);
            _documentoContrato.Filial = documentoProposta.Filial;
        }
        void MontarHtmlModelo()
        {
            lblNomeModelo.Text = _documentoContrato.Modelo.Titulo;
            lblVersao.Text = _documentoContrato.Modelo.Versao.ToString();
            lblCriado.Text = _documentoContrato.Modelo.DataCadastro.ToString("dd-MM-yyyy");
            VerificarDataCriacao(_documentoContrato.Modelo);

            StringBuilder textoParte = new StringBuilder();

            if (!Util.VerificarModeloCliente(_documentoContrato.Modelo.ModeloTipo))
            {
                foreach (Parte parte in _documentoContrato.Modelo.ListParte)
                {
                    textoParte.Append(parte.TextoParte);
                    textoParte.Append("<br /><br />");
                }
                CKEditor.Text = textoParte.ToString();
                CKEditor.Visible = true;
            }
            else
                CKEditor.Visible = false;
        }
        void VerificarDataCriacao(Modelo modelo)
        {
            lblMensagemDataCriacao.Text = String.Empty;
            if (new ModeloBusiness().CadastroRecente(modelo))
                lblMensagemDataCriacao.Text = "Modelo atualizado ou criado recentemente.";
        }
        Boolean VerificarContratoCliente()
        {
            return ddlModelo.SelectedItem.Text == "Contrato do Cliente";
        }
        Arquivo ValidarContratoCliente()
        {
            if (VerificarContratoCliente())
            {
                if (flContrato.HasFile)
                {
                    if (Path.GetExtension(flContrato.FileName).ToLower() == ".pdf")
                    {
                        Stream fs = flContrato.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        String type = flContrato.PostedFile.ContentType;
                        Int32 size = flContrato.PostedFile.ContentLength;

                        return new Arquivo()
                        {
                            Nome = flContrato.PostedFile.FileName,
                            Extensao = Path.GetExtension(flContrato.FileName),
                            Tamanho = size,
                            Tipo = type,
                            Conteudo = bytes
                        };
                    }
                    else
                        throw new Exception("O tipo do arquivo é inválido :(");
                }
                else
                    throw new Exception("Nenhum arquivo selecionado :(");
            }
            else
                return null;
        }
        void ObterDadosPmweb(Documento documentoProposta)
        {
            DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(documentoProposta.NumeroDocumento);
            CostManagementCommitments oCostManagementCommitments = new CostManagementCommitments().ObterContratoPorOportunidade(oDocumentTodoList.IdDocumentoTodoList);
            new Estimates().OrcamentoFinalizado(oCostManagementCommitments.ProjectId);
            Projects oProjects = new Projects().ObterProjects(oCostManagementCommitments.ProjectId);
            Contacts comercialResponsavel = new Contacts().ObterComercialResponsavelContrato(oProjects.Id);

            Company empresaCliente = new Company().ObterCompany(oCostManagementCommitments.CompanyId);
            CompanyAddress clienteEndereco = new CompanyAddress().ObterCompanyAddresses(oCostManagementCommitments.CompanyId);

            _documentoContrato.CodigoSistemaOrigem = oCostManagementCommitments.Id;
            _documentoContrato.DocumentoCliente.Nome = empresaCliente.CompanyName;
            _documentoContrato.DocumentoCliente.CpfCnpj = empresaCliente.CompanyCode;
            _documentoContrato.DocumentoCliente.RgIe = empresaCliente.StateTaxId;
            _documentoContrato.DocumentoCliente.CodigoOrigem = empresaCliente.Id;

            _documentoContrato.DocumentoCliente.Endereco = clienteEndereco.Address1;
            _documentoContrato.DocumentoCliente.Numero = clienteEndereco.AltPhone;
            _documentoContrato.DocumentoCliente.Bairro = clienteEndereco.Address2;
            _documentoContrato.DocumentoCliente.Cidade = clienteEndereco.City.Description;
            _documentoContrato.DocumentoCliente.Estado = clienteEndereco.States.StateKey;
            _documentoContrato.DocumentoCliente.Cep = clienteEndereco.Zip;

            _documentoContrato.NumeroDocumento = Int32.Parse(oCostManagementCommitments.CommitmentCode.Substring(3));

            _documentoContrato.DocumentoComercial = new DocumentoComercial
            {
                PrimeiroNome = comercialResponsavel.FirstName,
                Sobrenome = comercialResponsavel.LastName,
                CodigoSistemaOrigem = comercialResponsavel.SpecificationsId,
                Departamento = comercialResponsavel.DepartmentName,
                Telefone = comercialResponsavel.Phone,
                Email = comercialResponsavel.Email
            };

            Company filial = new Company().ObterFilial(comercialResponsavel.CompanyId);
            CompanyAddress filialEndereco = new CompanyAddress().ObterCompanyAddresses(filial.Id);
            _documentoContrato.DocumentoFilial = new DocumentoFilial
            {
                NomeUnidade = filial.ShortName,
                Cidade = filialEndereco.City.Description,
                Telefone = filialEndereco.Phone
            };

            _documentoContrato.DocumentoObra.Nome = oProjects.ProjectName;
            _documentoContrato.DocumentoObra.Endereco = oProjects.Address1;
            _documentoContrato.DocumentoObra.Bairro = oProjects.Address2;
            _documentoContrato.DocumentoObra.Cidade = oProjects.City;
            _documentoContrato.DocumentoObra.Estado = oProjects.States.StateKey;
            _documentoContrato.DocumentoObra.CEP = oProjects.Zip;

            //string enderecoCompleto = String.Format("{0} {1} {2} {3} {4}", , , , , );
            //_documentoContrato.DocumentoObra.Endereco = enderecoCompleto;
        }
        void CarregarDropDownModelos(Documento documentoProposta)
        {
            List<Modelo> listModelo = new ModeloBusiness().ObterModeloContratoPorIdModeloProposta(documentoProposta.Modelo.IdModelo);
            ddlModelo.DataSource = listModelo;
            ddlModelo.DataValueField = "IdModelo";
            ddlModelo.DataTextField = "Titulo";
            ddlModelo.DataBind();
        }

        void MontarCkEditor()
        {
            CKEditor.config.toolbar = new object[] { new object[] { "Table", "Maximize", "-", "Bold", "Italic", "Underline" } };
            CKEditor.ReadOnly = true;
            CKEditor.config.removePlugins = "elementspath";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MontarCkEditor();

                if (IsPostBack) return;
                Documento documentoProposta = new Util().GetSessaoDocumento();
                ObterDadosPmweb(documentoProposta);
                CarregarPainelDetalhes(documentoProposta);
                CarregarDropDownModelos(documentoProposta);
            }
            catch (Exception ex)
            {
                Session["documento"] = null;
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                panelModelo.Visible = false;
                panelModelo1.Visible = false;
            }
        }
        protected void btnUtilizarModelo_Click(object sender, EventArgs e)
        {
            try
            {
                Arquivo oArquivo = ValidarContratoCliente();
                Documento documentoProposta = new Util().GetSessaoDocumento();
                Int32 idModelo = Int32.Parse(ddlModelo.SelectedValue);
                ObterDadosPmweb(documentoProposta);
                CarregarDocumento(idModelo, documentoProposta);

                _documentoContrato.IdDocumento = new DocumentoBusiness().AdicionarContrato(_documentoContrato, documentoProposta, new Util().GetSessaoUsuario().IdUsuario, new Util().GetSessaoPerfilAtivo());

                //if (_documentoContrato.Modelo.Segmento.IdSegmento != 6)
                  //  new Estimates().AtualizarPercentualDesconto(_documentoContrato.CodigoSistemaOrigem);

                Session["documentoProposta"] = null;
                Session["documento"] = null;
                Session["documento"] = _documentoContrato;

                if (oArquivo != null)
                {
                    oArquivo.IdDocumento = _documentoContrato.IdDocumento;
                    new ArquivoBusiness().AdicionarArquivo(oArquivo);
                    Response.Redirect("Objeto.aspx", false);
                }
                else
                    Response.Redirect("Variaveis.aspx", false);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                panelModelo.Visible = false;
            }
        }
        protected void linkButtonPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                Documento documentoProposta = new Util().GetSessaoDocumento();
                Int32 idModelo = Int32.Parse(ddlModelo.SelectedValue);

                CarregarDocumento(idModelo, documentoProposta);
                ObterDadosPmweb(documentoProposta);
                MontarHtmlModelo();
                CarregarPainelDetalhes(documentoProposta);
                panelModelo.Visible = true;

                if (VerificarContratoCliente())
                {
                    panelContratoCliente.Visible = true;
                    CKEditor.Visible = false;
                }
                else
                {
                    panelContratoCliente.Visible = false;
                    CKEditor.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                panelModelo.Visible = false;
            }
        }
    }
}