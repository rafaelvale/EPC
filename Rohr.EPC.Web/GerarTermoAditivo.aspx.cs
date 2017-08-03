using Rohr.Data;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class TermoAditivo : BasePage
    {
        readonly Documento _documentoContrato = new Documento();
        readonly DbHelper _dbHelperPmweb = new DbHelper("PMWeb");
        readonly DbHelper _dbHelperEPC = new DbHelper("EPC");
        readonly Documento _documento = new Documento();
        Int32 numeroOportunidade = 0;
        Int32 _RevisionId = 0;

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

        private Boolean TAFinalizado(Int32 idEstimate)
        {
            DbHelper _dbHelperPmweb = new DbHelper("PMWeb");
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@idEstimate", idEstimate));
            string Query = "SELECT dbo.BuscaParametro(2, @idEstimate, 'Orçamento Finalizado', 'Dados Complementares')";
            string TAFin = _dbHelperPmweb.ExecutarScalar(Query, parametros).ToString();

            try
            {
                if (TAFin == "True")
                    return true;
                else
                    return false;
            }
            finally
            {
               //_dbHelperPmweb.CloseConnection();
            }

        }

        void ObterOportunidadePmweb(Int32 numeroOportunidade)
        {
            DbParametros parametros = new DbParametros();    
            DataTable DadosContrato = new DataTable();            
            DbHelper _dbHelperEPC = new DbHelper("EPC");
            

            try
            {
                DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(numeroOportunidade);

                Document_TodoListDetails oDocumentTodoListDetails = new Document_TodoListDetails().ObterDocumentTodoListDetails(oDocumentTodoList.IdDocumentoTodoList);
                if (oDocumentTodoListDetails.Description != null)
                    throw new MyException(String.Format("A oportunidade está fechada no PMWeb - ({0}) :(", oDocumentTodoListDetails.Description));

                Contacts comercialResponsavel = new Contacts().ObterComercialResponsavelOportunidade(oDocumentTodoList.IdDocumentoTodoList);
                Contacts contatoComercialCliente = new Contacts().ObterContatoComercialOportunidade(oDocumentTodoList.IdDocumentoTodoList);

                if (!new FilialBusiness().VerificarPropostaFilialUsuario(comercialResponsavel.CompanyId, new Util().GetSessaoUsuario()))
                    throw new MyException(String.Format("Você não tem permissão para criar o Termo Aditivo {0}. Pois não pertence a sua filial :(", numeroOportunidade));


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
                _documento.DataParaExibicao = DateTime.Now;
                parametros.Adicionar(new DbParametro("@numeroOportunidade", numeroOportunidade));
                string Query = "SELECT IDDocumento, IDModelo, CodigoOrigem FROM documentos WHERE NumeroDocumento = @numeroOportunidade";                
                DadosContrato = _dbHelperEPC.ExecutarDataTable(Query, parametros);

                

                if (DadosContrato.Rows.Count < 1)
                {
                    Util.ExibirMensagem(lblMensagem, "Modelo não encontrado.", Util.TipoMensagem.Alerta);
                }                
                else
                {           

                    ObterModeloDocumento(Int32.Parse(DadosContrato.Rows[0]["IDModelo"].ToString()));
                    _documento.IdDocumento = (Int32.Parse(DadosContrato.Rows[0]["IDDocumento"].ToString()));

                    _documento.EhTermoAditivo = true;
                    _documento.EProposta = true;
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
        }


        void CarregarPainelDetalhes()
        {
            List<Label> listControles = new List<Label>
                {
                    lblOportunidade,
                    lblCliente,
                    lblObra,
                    lblComercial

                };
            new Util().CarregarPainelDetalhes(listControles, _documento);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                ValidarTermo();
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
                _documentoContrato.EhTermoAditivo = true;

                Session["documentoProposta"] = null;
                Session["documento"] = null;
                Session["documento"] = _documentoContrato;

                if (oArquivo != null)
                {
                    oArquivo.IdDocumento = _documentoContrato.IdDocumento;
                    new ArquivoBusiness().AdicionarArquivo(oArquivo);                    
                    Response.Redirect("Variaveis.aspx", false);
                }
                else
                {                    
                    Response.Redirect("Variaveis.aspx", false);
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

        protected void PesquisarModelo_Click(object sender, EventArgs e)
        {
            try
            {
                Documento documentoProposta = new Util().GetSessaoDocumento();
                Int32 idModelo = Int32.Parse(ddlModelo.SelectedValue);

                CarregarDocumento(idModelo, documentoProposta);
                ObterDadosPmweb(documentoProposta);
                MontarHtmlModelo();
                CarregarPainelDetalhes();
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

        public void ValidarTermo()
        {
            DbHelper _dbHelperPmweb = new DbHelper("PMWeb");
            Int32 Termo = 0;

            if (String.IsNullOrEmpty(txtNumeroTA.Text) || (txtNumeroTA.Text.Length != 5))
            {
                Util.ExibirMensagem(lblMensagem, "Informe o número do Termo Aditivo com 5 Digitos", Util.TipoMensagem.Erro);
            }
            else if (new DocumentoBusiness().ObterTermoPMweb(Int32.Parse(txtNumeroTA.Text.ToString())).Rows.Count < 1)
            {
                Util.ExibirMensagem(lblMensagem, "Termo Aditivo não encontrado no PMWeb", Util.TipoMensagem.Alerta);
            }
            else
            {
                Termo = new DocumentoBusiness().ObterTermoID(Int32.Parse(txtNumeroTA.Text.ToString()));

                if (Termo < 1)
                {
                    Util.ExibirMensagem(lblMensagem, "Termo Aditivo não encontrado no EPC", Util.TipoMensagem.Alerta);
                }
                else
                {
                    if (!TAFinalizado(Termo))
                    {
                        Util.ExibirMensagem(lblMensagem, "TA não finalizado no PMWeb ;(", Util.TipoMensagem.Erro);

                    }
                    else
                    {
                        DbParametros parametros = new DbParametros();

                        parametros.Adicionar(new DbParametro("@Termo", Termo));
                        string Query1 = "SELECT SUBSTRING(Measure,4, 6) as M FROM Specifications WHERE SpecificationTemplateId = 171 AND EntityId = @Termo";
                        numeroOportunidade = Int32.Parse(_dbHelperPmweb.ExecutarScalar(Query1, parametros).ToString());

                        Session["documento"] = _documento;
                        _documentoContrato.EhTermoAditivo = true;
                        _documento.Usuario = new Util().GetSessaoUsuario();
                        _documento.NumeroDocumento = numeroOportunidade;
                        new AuditoriaLogBusiness().AdicionarLogDocumentoPesquisadoPMWeb(_documento, Request.Browser);
                        new DocumentoBusiness().VerificarPropostaFechadaPMWeb(_documento);
                        ObterTAPmweb(Int32.Parse(txtNumeroTA.Text));
                        //_documento.EClausula = Int32.Parse(DadosTermo.Rows[0]["EClausula"].ToString());
                        // _documento.EOrcamento = Int32.Parse(DadosTermo.Rows[0]["EOrcamento"].ToString());
                        // _documento.IDStatus = Int32.Parse(DadosTermo.Rows[0]["IDStatus"].ToString());
                        ObterOportunidadePmweb(numeroOportunidade);
                        panelDescricaoOportunidade.Visible = true;
                        
                        if (panelModelo1.Visible == true) { panelPesquisa.Visible = false; panelTermos.Visible = false; }
                            else { panelTermos.Visible = true; }

                        CarregarPainelDetalhes();

                        Documento documentoProposta = new Util().GetSessaoDocumento();
                        ObterDadosPmweb(documentoProposta);
                        CarregarDropDownModelos(documentoProposta);



                    }
                }
            }

        }



        protected void linkButtonPesquisar_Click(object sender, EventArgs e)
        {            
            ValidarTermo();
            
        }



        protected void ckSalvar_Click(object sender, EventArgs e)
        {                     
            foreach (RepeaterItem item in Repeater1.Items)
            {
                Label lblIDPesquisa = (Label)item.FindControl("lblIDPesquisa");
                Label lblRevisionNumber = (Label)item.FindControl("lblRevisionNumber");
                CheckBox cb = (CheckBox)item.FindControl("chkMarcado");              

                if (cb.Checked == true)
                {
                    _documento.EhTermoAditivo = true;
                    panelModelo1.Visible = true;
                    panelTermos.Visible = false;
                    panelPesquisa.Visible = false;                    
                    break;                                      
                }
                              
            }            
        }

        void ObterTAPmweb(Int32 RevisionId)
        {
            DbHelper _dbHelperPmweb = new DbHelper("PMWeb");
            DbParametros parametros = new DbParametros();
            _RevisionId = RevisionId;
            parametros.Adicionar(new DbParametro("@RevisionId", RevisionId));
            string Query = "SELECT Id, RevisionNumber, Description FROM Estimates WHERE SUBSTRING(RevisionId, 0, CHARINDEX('.', RevisionId, 0)) = @RevisionId";
            
            try
            {
                DataTable TA = _dbHelperPmweb.ExecutarDataTable(Query, parametros);

                DataTable TermoAditivo = new DataTable("TermoAditivo");
                TermoAditivo.Columns.Add("Id", typeof(string));
                TermoAditivo.Columns.Add("RevisionNumber", typeof(string));
                TermoAditivo.Columns.Add("Description", typeof(string));
                TermoAditivo.Columns.Add("Tipo", typeof(string));
                TermoAditivo.Columns.Add("Status", typeof(string));

                if (TA.Rows.Count < 1) { Util.ExibirMensagem(lblMensagem, "Termo Aditivo não encontrado no PMWeb :(", Util.TipoMensagem.Erro); }
                else
                {
                    for (int i = 0; i <= TA.Rows.Count - 1; i++)
                    {
                        TermoAditivo.Rows.Add(TA.Rows[i]["Id"],
                                         TA.Rows[i]["RevisionNumber"].ToString(),
                                         TA.Rows[i]["Description"].ToString().Substring(1,50),
                                         "EClausula",
                                         "Em elaboração"
                                         );

                    }

                    Repeater1.DataSource = TermoAditivo;
                    Repeater1.DataBind();

                    _dbHelperPmweb.CloseConnection();
                }
            }
            catch (SqlException sqlEx)
            {
                linkButtonPesquisar.Enabled = false;
                linkButtonPesquisar.Attributes.Add("disabled", "disabled");
                NLog.Log().Fatal(sqlEx);
                ExcecaoBusiness.Adicionar(sqlEx, HttpContext.Current.Request.Url.AbsolutePath);

                throw new Exception("O PMWeb está indisponível, tente novamente mais tarde :(");

            }
        }

       
    }


}