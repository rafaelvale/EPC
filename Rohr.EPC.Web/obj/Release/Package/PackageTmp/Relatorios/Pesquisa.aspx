<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Pesquisa.aspx.cs" Inherits="Rohr.EPC.Web.Relatorios.Pesquisa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="input-append">
                <asp:TextBox ID="txtPesquisar" runat="server" class="input-xxlarge" placeholder="Pesquisar por número do documento, cliente ou obra" onkeypress="submitPesquisa();" autofocus/>
                <asp:LinkButton ID="linkButtonPesquisar" runat="server" CssClass="btn" OnClick="linkButtonPesquisar_Click"><i class="icon-search"></i></asp:LinkButton>
            </div>
        </div>
        <div class="row-fluid">
            <div class="span12">
                <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-condensed-small table-hover" id="tableDocumentos">
                            <thead>
                                <tr>
                                    <th style="width: 12px;"></th>
                                    <th style="width: 1px;"></th>
                                    <th title="Número do Documento - Contrato ou Proposta" class="text-nowrap" style="width: 50px;">Nº Doc.</th>
                                    <th class="text-nowrap" title="P=Proposta / C=Contrato" style="width: 30px;">Tipo</th>
                                    <th title="Revisão do Cliente" class="text-nowrap hidden-tablet" style="width: 32px;">Rev.</th>
                                    <th title="Versão da Rohr" class="text-nowrap hidden-tablet" style="width: 32px;">Ver.</th>
                                    <th class="text-nowrap" style="width: 180px;">Cliente</th>
                                    <th class="text-nowrap" style="width: 170px;">Obra</th>
                                    <th class="text-nowrap hidden-tablet" style="width: 55px;">Filial</th>
                                    <th title="Desconto(-)/Acréscimo(+) Médio" class="text-nowrap" style="width: 55px;">Desc./Acr. M.(%)</th>
                                    <th title="Valor do Negócio" class="text-nowrap" style="width: 55px;">Vl. Negócio (R$)</th>
                                    <th title="Colaborador responsável pela criação do documento" class="text-nowrap hidden-tablet" style="width: 65px;">Criado por</th>
                                    <th class="text-nowrap">Próxima ação</th>
                                </tr>
                            </thead>
                            <tbody id="fbody">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ckDocumento" runat="server" />
                                <asp:HiddenField ID="hdIdDocumento" runat="server" />
                            </td>
                            <td></td>
                            <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "numeroDocumento", "{0:N0}") %></td>
                            <td class="text-nowrap"><%# Boolean.Parse(DataBinder.Eval(Container.DataItem, "eProposta").ToString()) ? "Proposta" : "Contrato" %></td>
                            <td class="text-nowrap hidden-tablet"><%# DataBinder.Eval(Container.DataItem, "revisaoCliente") %></td>
                            <td class="text-nowrap hidden-tablet"><%# DataBinder.Eval(Container.DataItem, "versaoInterna") %></td>
                            <td class="text-nowrap">
                                <asp:Label runat="server" ID="lblNomeCliente" /></td>
                            <td class="text-nowrap">
                                <asp:Label runat="server" ID="lblNomeObra" /></td>
                            <td class="text-nowrap hidden-tablet">
                                <asp:Label runat="server" ID="lblFilial" /></td>
                            <td class="text-nowrap text-right"><%# DataBinder.Eval(Container.DataItem, "percentualDesconto", "{0:N2}") %></td>
                            <td class="text-nowrap text-right"><%# DataBinder.Eval(Container.DataItem, "valorNegocio", "{0:N2}") %></td>
                            <td class="text-nowrap hidden-tablet"><%# DataBinder.Eval(Container.DataItem, "primeiroNome") %></td>
                            <td class="text-nowrap hidden-tablet">
                                <asp:Label runat="server" ID="lblDescricaoAcao"></asp:Label></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <br />
                <span id="totalDocumentos"></span>
            </div>
        </div>
    </div>
    <style>
        tbody tr {
            cursor: default;
        }
    </style>
</asp:Content>
