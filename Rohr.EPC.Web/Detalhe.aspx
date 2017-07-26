<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Detalhe.aspx.cs" Inherits="Rohr.EPC.Web.Detalhe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .affix {
            top: 48px;
        }

        .stats .stat {
            display: table-cell;
            width: 100px;
            vertical-align: top;
            font-weight: bold;
            color: #999;
        }

        .stat-value {
            display: block;
            margin-bottom: .55em;
            font-size: 25px;
            font-weight: bold;
            letter-spacing: -2px;
            color: #444;
        }

        .stat-time {
            text-align: left;
            padding-top: 1.5em;
        }

            .stat-time .stat-value {
                color: #F90;
                font-size: 40px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container">
        <div class="row">
            <div class="span3 box-left hidden-tablet">
                <div data-spy="affix" class="box-left space-left-small">
                    <div runat="server" id="divContrato" class="control-title" visible="false">
                        Contrato
                    </div>
                    <asp:Label ID="lblContrato" runat="server" Visible="false"></asp:Label>
                    <div class="control-title">
                        Oportunidade
                    </div>
                    <asp:Label ID="lblOportunidade" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Comercial
                    </div>
                    <asp:Label ID="lblComercial" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Cliente
                    </div>
                    <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Obra
                    </div>
                    <asp:Label ID="lblObra" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Modelo
                    </div>
                    <asp:Label ID="lblModelo" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="span9">
                <div>
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span9">
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#tab1" data-toggle="tab">Revisões</a></li>
                                <li runat="server" id="liAcaoJuridico" visible="false"><a href="#tab2" data-toggle="tab">Ações Jurídico</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="tab1">
                                    <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false">
                                        <HeaderTemplate>
                                            <table class="table table-condensed-small table-hover" id="table">
                                                <thead>
                                                    <tr>
                                                        <th title="Revisão do Cliente" class="text-nowrap hidden-tablet" style="width: 32px;">Rev.
                                                        </th>
                                                        <th title="Versão da Rohr" class="text-nowrap hidden-tablet" style="width: 32px;">Ver.
                                                        </th>
                                                        <th title="Desconto Médio" class="text-nowrap" style="width: 55px;">Desc. M. (%)
                                                        </th>
                                                        <th title="Valor do Negócio" class="text-nowrap" style="width: 55px;">Vl. Negócio (R$)</th>
                                                        <th title="Colaborador responsável pela criação do documento" class="text-nowrap hidden-tablet"
                                                            style="width: 65px;">Criado por
                                                        </th>
                                                        <th class="text-nowrap hidden-tablet">Criado em</th>
                                                        <th class="text-nowrap">Tipo documento</th>
                                                        <th class="text-nowrap">PDF</th>
                                                        <th class="text-nowrap">Orçamento</th>
                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="text-nowrap hidden-tablet">
                                                    <%# DataBinder.Eval(Container.DataItem, "revisaoCliente") %>
                                                </td>
                                                <td class="text-nowrap hidden-tablet">
                                                    <%# DataBinder.Eval(Container.DataItem, "versaoInterna") %>
                                                </td>
                                                <td class="text-nowrap text-right">
                                                    <%# DataBinder.Eval(Container.DataItem, "percentualDesconto", "{0:N2}") %>
                                                </td>
                                                <td class="text-nowrap text-right">
                                                    <%# DataBinder.Eval(Container.DataItem, "valorNegocio", "{0:N2}") %>
                                                </td>
                                                <td class="text-nowrap hidden-tablet">
                                                    <%# DataBinder.Eval(Container.DataItem, "primeiroNome") %>
                                                </td>
                                                <td class="text-nowrap hidden-tablet">
                                                    <%# DataBinder.Eval(Container.DataItem, "dataCadastro", "{0:dd-MM-yyy HH:mm}") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# DataBinder.Eval(Container.DataItem, "tipoDocumento") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <asp:LinkButton runat="server" ID="lkVisualizar" OnClick="lkVisualizar_Click" CommandName="idDocumento"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "idDocumento") %>'>Visualizar</asp:LinkButton>
                                                </td>
                                                <td class="text-nowrap">
                                                    <asp:LinkButton runat="server" ID="lkOrcamento" OnClick="lkOrcamento_Click" CommandName="idDocumento"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "idDocumento") %>'>Visualizar</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody> </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="tab-pane" id="tab2">
                                    <div class="alert alert-error">
                                        <strong>Atenção! Essa ação libera o contrato para edição, mesmo que já esteja
                                            assinado pela direção. O documento estará disponível para edição do comercial.</strong><br />
                                        <br />
                                        <span>Observações</span>
                                        <asp:TextBox runat="server" ID="txtObservacao" AutoCompleteType="None" Width="98%" />
                                        <asp:Button ID="btnLiberarDocumento" runat="server" Text="Liberar para edição" CssClass="btn btn-danger"
                                            Enabled="false" OnClick="btnLiberarDocumento_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
