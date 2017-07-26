<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true"
    CodeBehind="DocumentosCancelados.aspx.cs" Inherits="Rohr.EPC.Web.Relatorios.DocumentosCancelados" %>

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
                    <legend>Documentos Cancelados</legend>
                </fieldset>
            </div>
            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound" EnableViewState="false">
                <HeaderTemplate>
                    <table class="table table-condensed-small table-hover" id="tableDocumentos">
                        <thead>
                            <tr>
                                <th style="width: 14px;"></th>
                                <th title="Número do Documento - Contrato ou Proposta" class="text-nowrap" style="width: 50px;">Nº Doc.</th>
                                <th class="text-nowrap" title="P=Proposta / C=Contrato" style="width: 30px;">Tipo</th>
                                <th title="Revisão do Cliente" class="text-nowrap hidden-tablet" style="width: 32px;">Rev.</th>
                                <th title="Versão da Rohr" class="text-nowrap hidden-tablet" style="width: 32px;">Ver.</th>
                                <th class="text-nowrap" style="width: 200px;">Cliente</th>
                                <th class="text-nowrap" style="width: 140px;">Obra</th>
                                <th class="text-nowrap hidden-tablet" style="width: 55px;">Filial</th>
                                <th title="Desconto(-)/Acréscimo(+) Médio" class="text-nowrap" style="width: 55px;">Desc./Acr. M.(%)</th>
                                <th title="Valor do Faturamento Mensal" class="text-nowrap" style="width: 55px;">Vl. F. Mensal (R$)</th>
                                <th title="Motivo da Reprovação" class="text-nowrap hidden-tablet" style="width: 65px;">Motivo Reprovação</th>
                            </tr>
                        </thead>
                        <tbody id="fbody">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="text-nowrap">
                            <asp:Label runat="server" ID="lblNumeroDocumento" />
                        </td>
                        <td class="text-nowrap">
                            <%# DataBinder.Eval(Container.DataItem, "numeroDocumento", "{0:N0}") %>
                        </td>
                        <td class="text-nowrap">
                            <%# Boolean.Parse(DataBinder.Eval(Container.DataItem, "eProposta").ToString()) ? "P" : "C" %>
                        </td>
                        <td class="text-nowrap hidden-tablet">
                            <%# DataBinder.Eval(Container.DataItem, "revisaoCliente") %>
                        </td>
                        <td class="text-nowrap hidden-tablet">
                            <%# DataBinder.Eval(Container.DataItem, "versaoInterna") %>
                        </td>
                        <td class="text-nowrap">
                            <asp:Label runat="server" ID="lblNomeCliente" />
                        </td>
                        <td class="text-nowrap">
                            <asp:Label runat="server" ID="lblNomeObra" />
                        </td>
                        <td class="text-nowrap hidden-tablet">
                            <%# DataBinder.Eval(Container.DataItem, "filial") %>
                        </td>
                        <td class="text-nowrap text-right">
                            <%# DataBinder.Eval(Container.DataItem, "percentualDesconto", "{0:N2}") %>
                        </td>
                        <td class="text-nowrap text-right">
                            <%# DataBinder.Eval(Container.DataItem, "valorFaturamentoMensal", "{0:N2}") %>
                        </td>
                        <td class="text-nowrap hidden-tablet">
                            <%# DataBinder.Eval(Container.DataItem, "Descricao") %>
                        </td>
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
