using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class Variaveis : BasePage
    {
        Documento _documento;
        Documento _documentoProposta;

        TableCell MontarControles(Chave chave)
        {
            if (!chave.PermiteEdicao)
            {
                return MontarLabelValor(chave);
            }
            switch (chave.ChaveDescricao)
            {
                case "{rohr.documento.dataBase}": return MontarTextBox(chave, "", 2);
                case "{rohr.documento.dataFaturamento}": return MontarDropDownList(chave);
                case "{rohr.documento.vencimentoFaturamento}": return MontarDropDownList(chave);
                case "{rohr.unidade.enderecoRetirada}": return MontarTextBox(chave, "", 5, TextBoxMode.MultiLine);
                case "{rohr.unidade.enderecoDevolucao}": return MontarTextBox(chave, "", 5, TextBoxMode.MultiLine);
                case "{rohr.unidade.horarioFuncionamento}": return MontarTextBox(chave, "", 5, TextBoxMode.MultiLine);
                case "{rohr.documento.antecedenciaCargaDescarga}": return MontarDropDownList(chave);
                case "{rohr.documento.validadeFornecimento}": return MontarDropDownList(chave);
                case "{rohr.documento.antecedenciaSolicitacaoMO}": return MontarDropDownList(chave);
                case "{rohr.documento.periodoApuracao}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.vencimentoFatIndenizacao}": return MontarDropDownList(chave);
                case "{rohr.documento.prazoParaRetiradoDoBem}": return MontarDropDownList(chave);
                case "{rohr.documento.prazoMinimoLocacao}": return MontarDropDownList(chave);
                case "{rohr.documento.precoParaMobilizacao}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.precoParaDesmobilizacao}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.precoCorretivaTecnico}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.precoCorretivaMestre}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.precoCorretivaMontador}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.valorProporcao}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.pagamentoAposFaturamento}": return MontarDropDownList(chave);
                case "{rohr.documento.valorAjudante}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.quantidadeHoraCargaDescarga}": return MontarDropDownList(chave);
                case "{rohr.documento.horarioLimiteCargaDescarga}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.custoExtraordinario}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.comarca}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.percentualHoraExtraSeg_Sab}": return MontarTextBox(chave, "", 3);
                case "{rohr.documento.percentualHoraExtraDom_Fer}": return MontarTextBox(chave, "", 3);
                case "{rohr.documento.quantidadeMaterial}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.adicionalNoturno}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.periculosidade}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.insalubridade}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.jornadaTrabMO}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.antecedenciaManutencao}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.horarioAtendimentoMan}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.fatServicosExtraordinarios}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.fatIndenizacao}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.periodoUtilizacao}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.locacaoPorPeriodo}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.dataVencimentoFatIndenizacao}": return MontarDropDownList(chave);
                case "{rohr.documento.rgTestemunha}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.nomeTestemunha}": return MontarTextBox(chave, "", 4);
                case "{rohr.documento.percentualLimpeza}": return MontarTextBox(chave, "", 2);


                default: return MontarLabelTitulo(chave);
            }
        }

        TableCell MontarLabelTitulo(Chave chave)
        {
            Label label = new Label { Text = chave.Descricao };

            TableCell td = new TableCell();
            td.Controls.Add(label);

            return td;
        }
        TableCell MontarLabelValor(Chave chave)
        {
            Company empresaCliente = null;
            CompanyAddress clienteEndereco = null;
            Contacts comercialResponsavel = null;
            Company filial = null;
            Contacts contatoComercialCliente = null;
            if (!_documento.EProposta)
            {
                DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(_documentoProposta.NumeroDocumento);
                CostManagementCommitments oCostManagementCommitments = new CostManagementCommitments().ObterContratoPorOportunidade(oDocumentTodoList.IdDocumentoTodoList);
                Projects oProjects = new Projects().ObterProjects(oCostManagementCommitments.ProjectId);
                comercialResponsavel = new Contacts().ObterComercialResponsavelContrato(oProjects.Id);

                DocumentTodoList oDocumentTodoListProposta = new DocumentTodoList().ObterDocumentTodoLists(_documentoProposta.NumeroDocumento);
                contatoComercialCliente = new Contacts().ObterContatoComercialOportunidade(oDocumentTodoListProposta.IdDocumentoTodoList);

                empresaCliente = new Company().ObterCompany(oCostManagementCommitments.CompanyId);
                clienteEndereco = new CompanyAddress().ObterCompanyAddresses(oCostManagementCommitments.CompanyId);

                filial = new Company().ObterFilial(comercialResponsavel.CompanyId);

                new CompanyAddress().ObterCompanyAddresses(filial.Id);
            }

            Label label = new Label { ID = chave.IdChave.ToString() };

            if (chave.IdChave == 1 && _documento.EProposta)
                label.Text = _documento.DocumentoCliente.Nome;
            else if (empresaCliente != null) label.Text = empresaCliente.CompanyName;

            switch (chave.IdChave)
            {
                case 3:
                    if (_documento.EProposta)
                    {
                        Contacts contatoComercialClienteProposta = new Contacts().ObterContatoComercialOportunidade(ObterDocumentTodoListSession().IdDocumentoTodoList);
                        label.Text = String.Format("{0} {1}", contatoComercialClienteProposta.FirstName, contatoComercialClienteProposta.LastName);
                    }
                    else if (contatoComercialCliente != null)
                        label.Text = String.Format("{0} {1}", contatoComercialCliente.FirstName, contatoComercialCliente.LastName);
                    break;
                case 73:
                    if (_documento.EProposta)
                    {
                        Contacts contatoComercialClienteProposta = new Contacts().ObterContatoComercialOportunidade(ObterDocumentTodoListSession().IdDocumentoTodoList);
                        label.Text = contatoComercialClienteProposta.DepartmentName;
                    }
                    else if (contatoComercialCliente != null)
                        label.Text = contatoComercialCliente.DepartmentName;
                    break;
                case 19:
                    if (_documento.EProposta)
                    {
                        Contacts contatoComercialProposta = new Contacts().ObterContatoComercialOportunidade(ObterDocumentTodoListSession().IdDocumentoTodoList);
                        label.Text = contatoComercialProposta.Phone;
                    }
                    else if (contatoComercialCliente != null) label.Text = contatoComercialCliente.Phone;
                    break;
                case 18:
                    if (_documento.EProposta)
                    {
                        Contacts contatoComercialProposta = new Contacts().ObterContatoComercialOportunidade(ObterDocumentTodoListSession().IdDocumentoTodoList);
                        label.Text = contatoComercialProposta.Email;
                    }
                    else if (contatoComercialCliente != null) label.Text = contatoComercialCliente.Email;
                    break;
                case 7:
                    label.Text = "10% (dez por cento)";
                    break;
                case 8:
                    label.Text = "2% (dois por cento)";
                    break;
                case 16:
                    if (_documento.EProposta)
                    {
                        Contacts comercialResponsavelProposta = new Contacts().ObterComercialResponsavelOportunidade(ObterDocumentTodoListSession().IdDocumentoTodoList);
                        label.Text = String.Format("{0} {1}", comercialResponsavelProposta.FirstName, comercialResponsavelProposta.LastName);
                    }
                    else if (comercialResponsavel != null)
                        label.Text = String.Format("{0} {1}", comercialResponsavel.FirstName, comercialResponsavel.LastName);
                    break;
                case 17:
                    if (_documento.EProposta)
                    {
                        Contacts comercialResponsavelProposta = new Contacts().ObterComercialResponsavelOportunidade(ObterDocumentTodoListSession().IdDocumentoTodoList);
                        label.Text = comercialResponsavelProposta.DepartmentName;
                    }
                    else if (comercialResponsavel != null) label.Text = comercialResponsavel.DepartmentName;
                    break;
                case 21:
                    if (_documento.EProposta)
                    {
                        Contacts comercialResponsavelProposta = new Contacts().ObterComercialResponsavelOportunidade(ObterDocumentTodoListSession().IdDocumentoTodoList);
                        label.Text = comercialResponsavelProposta.Cell;
                    }
                    else if (comercialResponsavel != null) label.Text = comercialResponsavel.Cell;
                    break;
                case 20:
                    if (_documento.EProposta)
                    {
                        Contacts comercialResponsavelProposta = new Contacts().ObterComercialResponsavelOportunidade(ObterDocumentTodoListSession().IdDocumentoTodoList);
                        Company company = new Company().ObterCompany(comercialResponsavelProposta.CompanyId);
                        label.Text = company.CompanyName;
                    }
                    else if (filial != null) label.Text = filial.CompanyName;
                    break;
                case 22:
                    if (_documento.EProposta)
                    {
                        Contacts comercialResponsavelProposta = new Contacts().ObterComercialResponsavelOportunidade(ObterDocumentTodoListSession().IdDocumentoTodoList);
                        label.Text = comercialResponsavelProposta.Email;
                    }
                    else if (comercialResponsavel != null) label.Text = comercialResponsavel.Email;
                    break;
                case 2:
                    if (_documento.EProposta)
                    {
                        DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(_documento.NumeroDocumento);
                        Contacts contatoComercialProposta = new Contacts().ObterContatoComercialOportunidade(oDocumentTodoList.IdDocumentoTodoList);
                        Company company = new Company().ObterCompany(contatoComercialProposta.CompanyId);
                        CompanyAddress companyAddress = new CompanyAddress().ObterCompanyAddresses(company.Id);
                        string enderecoCompleto = String.Format("{0} - {1} - {2} - {3} - {4} - {5}", companyAddress.Address1, companyAddress.AltPhone, companyAddress.Address2, companyAddress.City.Description, companyAddress.States.StateKey, companyAddress.Zip);
                        label.Text = enderecoCompleto;
                    }
                    else
                    {
                        if (clienteEndereco != null)
                        {
                            string enderecoCompleto = String.Format("{0} - {1} - {2} - {3} - {4} - {5}", clienteEndereco.Address1, clienteEndereco.AltPhone, clienteEndereco.Address2, clienteEndereco.City.Description, clienteEndereco.States.StateKey, clienteEndereco.Zip);
                            label.Text = enderecoCompleto;
                        }
                    }
                    break;
                case 44:
                    label.Text = _documento.EProposta ? new Company().ObterCompanyProposta(_documento.CodigoSistemaOrigem).CompanyCode : empresaCliente.CompanyCode;
                    break;
                case 45:
                    label.Text = _documento.EProposta ? new Company().ObterCompanyProposta(_documento.CodigoSistemaOrigem).StateTaxId : empresaCliente.StateTaxId;
                    break;
                case 42:
                    {
                        CompanyAddress companyAddress = new CompanyAddress().ObterCompanyAddresses(_documento.Filial.CodigoOrigem);
                        string enderecoCompleto = String.Format("{0} - {1} - {2} - {3} - {4} - {5}", companyAddress.Address1, companyAddress.AltPhone, companyAddress.Address2, companyAddress.City.Description, companyAddress.States.StateKey, companyAddress.Zip);
                        label.Text = enderecoCompleto;
                    }
                    break;
                case 43:
                    {
                        Company company = new Company().ObterCompany(_documento.Filial.CodigoOrigem);
                        label.Text = company.CompanyCode;
                    }
                    break;
                case 67:
                    {
                        Company company = new Company().ObterCompany(_documento.Filial.CodigoOrigem);
                        label.Text = company.StateTaxId;
                    }
                    break;
                case 48:
                    {
                        if (_documento.EProposta)
                        {
                            string enderecoCompleto = String.Format("{0} - {1} - {2} - {3} - {4}",
                                _documento.DocumentoObra.Endereco,
                                _documento.DocumentoObra.Bairro,
                                _documento.DocumentoObra.Cidade,
                                _documento.DocumentoObra.Estado,
                                _documento.DocumentoObra.CEP);
                            label.Text = enderecoCompleto;
                        }
                        else
                        {
                            Int32 projectId = new CostManagementCommitments().ObterContrato(_documento.CodigoSistemaOrigem).ProjectId;
                            Projects oProjects = new Projects().ObterProjects(projectId);
                            string enderecoCompleto = String.Format("{0} {1} {2} {3} {4}", oProjects.Address1, oProjects.Address2, oProjects.City, oProjects.States.StateKey, oProjects.Zip);
                            label.Text = enderecoCompleto;
                        }
                    }
                    break;
                case 49:
                    {
                        Int32 projectId = new CostManagementCommitments().ObterContrato(_documento.CodigoSistemaOrigem).ProjectId;
                        Projects oProjects = new Projects().ObterProjects(projectId);

                        string enderecoCompleto = String.Format("{0},{1} - {2} - {3} - {4} - {5}",
                            new Specifications().ObterEnderecoEntregaFaturaEndereco(oProjects.Id).Measure,
                            new Specifications().ObterEnderecoEntregaFaturaNumero(oProjects.Id).Measure,
                            new Specifications().ObterEnderecoEntregaFaturaBairro(oProjects.Id).Measure,
                            new Specifications().ObterEnderecoEntregaFaturaCidade(oProjects.Id).Measure,
                            new States(Int32.Parse(new Specifications().ObterEnderecoEntregaFaturaEstado(oProjects.Id).ListItemId.ToString())).StateKey,
                            new Specifications().ObterEnderecoEntregaFaturaCep(oProjects.Id).Measure);

                        label.Text = enderecoCompleto;
                    }
                    break;
                case 46:
                    label.Text = _documentoProposta.NumeroDocumento.ToString();
                    break;
                case 57:
                    {
                        Int32 projectId = new CostManagementCommitments().ObterContrato(_documento.CodigoSistemaOrigem).ProjectId;
                        Projects oProjects = new Projects().ObterProjects(projectId);

                        string enderecoCompleto = String.Format("{0},{1} - {2} - {3} - {4} - {5}",
                            new Specifications().ObterEnderecoEntregaFaturaEndereco(oProjects.Id).Measure,
                            new Specifications().ObterEnderecoEntregaFaturaNumero(oProjects.Id).Measure,
                            new Specifications().ObterEnderecoEntregaFaturaBairro(oProjects.Id).Measure,
                            new Specifications().ObterEnderecoEntregaFaturaCidade(oProjects.Id).Measure,
                            new States(Int32.Parse(new Specifications().ObterEnderecoEntregaFaturaEstado(oProjects.Id).ListItemId.ToString())).StateKey,
                            new Specifications().ObterEnderecoEntregaFaturaCep(oProjects.Id).Measure);

                        label.Text = enderecoCompleto;
                    }
                    break;
                case 24:
                    if (_documento.EProposta)
                    {
                        DocumentTodoList oDocumentTodoListProposta = new DocumentTodoList().ObterDocumentTodoLists(_documento.NumeroDocumento);
                        label.Text = new Specifications().ObterDescricaoDoServico(oDocumentTodoListProposta.IdDocumentoTodoList).Measure;
                    }
                    break;
                case 69:
                    if (!_documento.EProposta)
                    {
                        label.Text = new DocumentoBusiness().ObterMaiorRevisaoProposta(_documento.IdDocumento).ToString();
                    }
                    break;
                case 75:
                    if (_documento.EProposta)
                    {
                        DocumentTodoList oDocumentTodoList = new DocumentTodoList().ObterDocumentTodoLists(_documento.NumeroDocumento);
                        Estimates oEstimates = new Estimates().ObterEstimate(new Estimates().ObterEstimatePorDocumentoId(oDocumentTodoList.IdDocumentoTodoList).Id);

                        if (oEstimates != null)
                            label.Text = new Specifications().ObterPercentualLimpezaProposta(oEstimates.Id).Measure + "%";
                        else
                            label.Text = "Aguardando o orçamento";
                    }
                    else
                    {
                        label.Text = new Specifications().ObterPercentualLimpezaContrato(_documento.CodigoSistemaOrigem).Measure + "%";
                    }
                    break;
            }

            TableCell td = new TableCell();
            td.Controls.Add(label);

            return td;
        }

        TableCell MontarTextBox(Chave chave, String placeHolder, Int32 tamanho, TextBoxMode textBoxMode = TextBoxMode.SingleLine)
        {
            TextBox textBox = new TextBox { ID = chave.IdChave.ToString() };
            textBox.Attributes.Add("placeHolder", placeHolder);

            switch (tamanho)
            {
                case 1:
                    textBox.CssClass = "input input-mini";
                    break;
                case 2:
                    textBox.CssClass = "input input-small";
                    break;
                case 3:
                    textBox.CssClass = "input input-medium";
                    break;
                case 4:
                    textBox.CssClass = "input input-large";
                    break;
                case 5:
                    textBox.CssClass = "input input-xlarge";
                    break;
            }

            if (!chave.PermiteEdicao)
                textBox.Attributes.Add("disabled", "true");

            if (textBoxMode == TextBoxMode.MultiLine)
            {
                textBox.TextMode = textBoxMode;
                textBox.Width = 280;
                textBox.Height = 50;
            }

            CarregarControle(chave, textBox);

            TableCell td = new TableCell();
            td.Controls.Add(textBox);

            return td;
        }
        TableCell MontarDropDownList(Chave chave)
        {
            DropDownList dropDownList = new DropDownList { ID = chave.IdChave.ToString() };
            CarregarControle(chave, dropDownList);

            TableCell td = new TableCell();
            td.Controls.Add(dropDownList);

            dropDownList.Dispose();

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
        void CarregarControle(Chave chave, Object controle)
        {
            if (controle is DropDownList)
                Util.CarregarValoresPadrao((DropDownList)controle, chave);

            if (_documento.ListChavePreenchida == null || _documento.ListChavePreenchida.Count == 0)
            {
                if (_documento.EProposta)
                {
                    switch (chave.IdChave)
                    {
                        case 6:
                            Util.CarregarDataBaseReajuste((TextBox)controle);
                            break;
                        case 12:
                            Util.CarregarEnderecoRetirada((TextBox)controle, _documento);
                            break;
                        case 13:
                            Util.CarregarEnderecoDevolucao((TextBox)controle, _documento);
                            break;
                        case 11:
                            Util.CarregarHorarioFuncionamentoUnidade((TextBox)controle);
                            break;
                    }
                }
                else
                {
                    if (_documentoProposta == null) return;
                    var chaveProposta = _documentoProposta.ListChavePreenchida.Where(x => chave.IdChave == x.IdChave);

                    if (!chaveProposta.Any()) return;

                    if (chave.IdChave == 15 || chave.IdChave == 14 || chave.IdChave == 9 || chave.IdChave == 10 || chave.IdChave == 25
                        || chave.IdChave == 40 || chave.IdChave == 28 || chave.IdChave == 29 || chave.IdChave == 41 || chave.IdChave == 51)
                        ((DropDownList)controle).SelectedValue = chaveProposta.First().Texto;
                    else
                        ((TextBox)controle).Text = chaveProposta.First().Texto;
                }
            }
            else
            {
                var res = _documento.ListChavePreenchida.Where(x => x.IdChave == chave.IdChave);

                if (res.Any())
                {
                    ChavePreenchida result = res.Single();

                    if (chave.IdChave == 14 && (result.Texto == "0" || result.Texto == "1" || result.Texto == "2"))
                        result.Texto = "3";


                    if (chave.IdChave == 15 || chave.IdChave == 14 || chave.IdChave == 9 || chave.IdChave == 10 || chave.IdChave == 25
                        || chave.IdChave == 40 || chave.IdChave == 28 || chave.IdChave == 29 || chave.IdChave == 41 || chave.IdChave == 51 || chave.IdChave == 68)
                        ((DropDownList)controle).SelectedValue = result.Texto;
                    else
                        ((TextBox)controle).Text = result.Texto;
                }
                else
                {
                    if (controle is TextBox)
                        ((TextBox)controle).Text = "";
                }
            }
        }
        void AdicionarChavesPreenchidas(Documento documento)
        {
            new ChavePreenchidaBusiness().AdicionarChavePreenchida(documento);
        }
        void MontarHtml(IEnumerable<Parte> partes, Table table)
        {
            List<Chave> listChavesFull = new List<Chave>();

            foreach (Parte parte in partes)
            {
                if (parte.ListChave == null) continue;
                foreach (Chave chave in parte.ListChave)
                {
                    Chave chave1 = chave;
                    var res = listChavesFull.Where(x => chave1 != null && x.IdChave == chave1.IdChave);
                    if (!res.Any())
                        listChavesFull.Add(chave);
                }
            }

            listChavesFull = listChavesFull.OrderByDescending(x => x.PermiteEdicao).ToList();

            foreach (Chave chave in listChavesFull)
            {
                if (!chave.Exibir) continue;

                TableRow tr = new TableRow();
                tr.Cells.Add(MontarLabelTitulo(chave));
                tr.Cells.Add(MontarControles(chave));
                tr.Style.Add("line-height", "20px");

                tr.Cells[0].Style.Add("width", "45%");
                table.Rows.Add(tr);
            }
            panelVariaveis.Controls.Add(table);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            
            try
            {                   

                Session["pmwebDocumentTodoList"] = null;
                _documento = new Util().GetSessaoDocumento();

                if (_documento.EhTermoAditivo) { panelVariaveis.Enabled = false; Contrato_Termo.Text = "Termo Aditivo"; }

                if (_documento.Modelo.ModeloTipo.IdModeloTipo == 3)
                {

                    Session["documento"] = null;
                    Session["documento"] = _documento;
                    Session["pmwebDocumentTodoList"] = null;
                    Response.Redirect("Objeto.aspx", false);
                }


                if (!_documento.Edicao)
                    _documento.ListChavePreenchida.Clear();

                if (_documento.EProposta)
                {
                    ObterOportunidadePmweb();
                    new DocumentoBusiness().VerificarPropostaFechadaPMWeb(_documento);                   

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

                Table table = new Table();
                table.Style.Add("width", "100%;");

                MontarHtml(_documento.Modelo.ListParte, table);
            }
            catch (Exception ex)
            {
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                btnContinuar.Attributes.Add("disabled", "disabled");
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {            
                try
                {

                    if (_documento.ListChavePreenchida == null)
                        _documento.ListChavePreenchida = new List<ChavePreenchida>();

                    _documento.ListChavePreenchida.Clear();

                    foreach (Table table in panelVariaveis.Controls.OfType<Table>().Select(item => (item)))
                    {
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            foreach (Object controle in table.Rows[i].Cells[1].Controls)
                            {
                                ChavePreenchida oChavePreenchida = new ChavePreenchida();
                                if (controle is TextBox)
                                {
                                    TextBox textBox = ((TextBox)controle);

                                    oChavePreenchida.IdChave = Int32.Parse(textBox.ID);
                                    oChavePreenchida.Texto = textBox.Text;
                                    oChavePreenchida.DataCadastro = DateTime.Now;
                                }
                                else if (controle is DropDownList)
                                {
                                    DropDownList dropDownList = ((DropDownList)controle);

                                    oChavePreenchida.IdChave = Int32.Parse(dropDownList.ID);
                                    oChavePreenchida.Texto = dropDownList.SelectedItem.Value;
                                    oChavePreenchida.DataCadastro = DateTime.Now;
                                }
                                else if (controle is Label)
                                {
                                    Label label = ((Label)controle);

                                    oChavePreenchida.IdChave = Int32.Parse(label.ID);
                                    oChavePreenchida.Texto = label.Text;
                                    oChavePreenchida.DataCadastro = DateTime.Now;
                                }

                                _documento.ListChavePreenchida.Add(oChavePreenchida);
                            }
                        }
                    }

                    VerificarRevisao(_documento);
                    AdicionarChavesPreenchidas(_documento);

                    Estimates oEstimates = null;
                    if (_documento.EProposta)
                        oEstimates = new Estimates().ObterEstimate(new Estimates().ObterEstimatePorDocumentoId(new DocumentTodoList().ObterDocumentTodoLists(_documento.NumeroDocumento).IdDocumentoTodoList).Id);

                   else
                    {
                        CostManagementCommitments oCostManagementCommitments = new CostManagementCommitments().ObterContrato(_documento.CodigoSistemaOrigem);
                        oEstimates = new Estimates().ObterEstimateContrato(oCostManagementCommitments.EstimatedId);
                    }

                    _documento.PercentualLimpeza = Convert.ToDecimal(new Specifications().ObterPercentualLimpezaProposta(oEstimates.Id).Measure);
                    new DocumentoBusiness().AtualizarPercentualLimpeza(_documento);

                    new DocumentoComercialBusiness().Atualizar(_documento);
                    new DocumentoObraBusiness().Atualizar(_documento);
                    new DocumentoClienteBusiness().Atualizar(_documento);

                    new AuditoriaLogBusiness().AdicionarLogDocumentoVariaveis(_documento, Request.Browser);

                if (_documento.Modelo.ModeloTipo.IdModeloTipo == 2)
                {
                    Session["documento"] = null;
                    Session["documento"] = _documento;
                    Session["pmwebDocumentTodoList"] = null;
                    Response.Redirect("Objeto.aspx", false);
                }

                else if (_documento.Modelo.ModeloTipo.IdModeloTipo == 1)
                {
                    Session["documento"] = null;
                    Session["documento"] = _documento;
                    Session["pmwebDocumentTodoList"] = null;

                    //Response.Redirect("Objeto.aspx", false);

                    Response.Redirect("Fotos.aspx", false);
                    buscaUltimaFoto(_documento.IdDocumento);

                }

                if (_documento.EhTermoAditivo) { Response.Redirect("Partes.aspx", false); } else { Response.Redirect("Objeto.aspx", false); }                    



                }
                catch (Exception ex)
                {
                    Util.ExibirMensagem(lblMensagemErro, ex.Message, Util.TipoMensagem.Erro);
                    NLog.Log().Error(ex);
                    ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                }            
        }

        private void buscaUltimaFoto(int IdDocumento)
        {

            new DocumentoImagensBusiness().obterUltimaFoto(IdDocumento);

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
    }
}