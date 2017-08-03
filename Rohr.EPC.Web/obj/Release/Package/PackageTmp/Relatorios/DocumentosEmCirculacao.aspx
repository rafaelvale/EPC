<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="DocumentosEmCirculacao.aspx.cs" Inherits="Rohr.EPC.Web.Relatorios.DocumentosEmCirculacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <div>
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                </div>
                <fieldset>
                    <legend>Documentos em circulação</legend>
                </fieldset>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div class="input-append">
                        <input type="text" class="input-xxlarge" placeholder="Pesquisar documento, cliente, obra ..." id="txtPesquisar" autofocus>
                    </div>
                </div>
                <div>
                    <div style="float: left; display: inline-block; vertical-align: bottom; padding: 5px 8px 0 0;">
                        <span id="totalDocumentos" style="padding: 5px 8px 0 0"></span>
                    </div>
                </div>
            </div>
            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound" EnableViewState="false">
                <HeaderTemplate>
                    <table class="table table-condensed-small table-hover" id="table">
                        <thead>
                            <tr>
                                <th title="Número do Documento - Contrato ou Proposta" class="text-nowrap" style="width: 55px;">Nº Doc.</span></th>
                                <th class="text-nowrap">Tipo</th>
                                <th class="text-nowrap" style="width: 205px;">Cliente</th>
                                <th class="text-nowrap" style="width: 205px;">Obra</th>
                                <th class="text-nowrap hidden-tablet" style="width: 55px;">Filial</th>
                                <th title="Desconto Médio" class="text-nowrap" style="width: 55px;">Desc. M. (%)</th>
                                <th title="Valor do Faturamento Mensal" class="text-nowrap" style="width: 55px;">Vl. F. Mensal (R$)</th>
                                <th class="text-nowrap hidden-tablet">Executado em</th>
                                <th class="text-nowrap hidden-tablet">Em circulação há</th>
                                <th class="text-nowrap hidden-tablet">Destino</th>
                            </tr>
                        </thead>
                        <tbody id="fbody">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "numeroDocumento", "{0:N0}") %></td>
                        <td class="text-nowrap"><%# Boolean.Parse(DataBinder.Eval(Container.DataItem, "eProposta").ToString()) ? "Proposta" : "Contrato" %></td>
                        <td class="text-nowrap">
                            <asp:Label runat="server" ID="lblNomeCliente" /></td>
                        <td class="text-nowrap">
                            <asp:Label runat="server" ID="lblNomeObra" /></td>
                        <td class="text-nowrap">
                            <asp:Label runat="server" ID="lblFilial" /></td>
                        <td class="text-nowrap text-right"><%# DataBinder.Eval(Container.DataItem, "percentualDesconto", "{0:N2}") %></td>
                        <td class="text-nowrap text-right"><%# DataBinder.Eval(Container.DataItem, "valorFaturamentoMensal", "{0:N2}") %></td>
                        <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "dataCadastro") %></td>
                        <td class="text-nowrap">
                            <asp:Label runat="server" ID="lblCriado"></asp:Label></td>
                        <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "descricao") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                        </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
