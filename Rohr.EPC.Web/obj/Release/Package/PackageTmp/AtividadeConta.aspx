<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterAuxiliar.Master" AutoEventWireup="true" CodeBehind="AtividadeConta.aspx.cs" Inherits="Rohr.EPC.Web.AtividadeConta1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="span3 box-left hidden-tablet">
            </div>
            <div class="span9">
                <fieldset>
                    <legend class="legend-small">Suas atividades recentes</legend>
                    <div style="padding-left: 10px;">
                        Notou alguma atividade estranha? Altere sua senha.
                    <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false">
                        <HeaderTemplate>
                            <table class="table table-condensed-small table-hover">
                                <tr>
                                    <th style="white-space: nowrap">Data</th>
                                    <th style="white-space: nowrap">Evento</th>
                                    <th style="white-space: nowrap">IP</th>
                                    <th style="white-space: nowrap">Máquina</th>
                                    <th style="white-space: nowrap">Tipo Acesso (Navegador, Celular)</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="row" runat="server">
                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "dataCadastro") %></td>
                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "descricao") %></td>
                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "ip") %></td>
                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "nomeMaquina") %></td>
                                <td class="text-nowrap"><%# Boolean.Parse(DataBinder.Eval(Container.DataItem, "acessoMobile").ToString()) ? "Celular" : "Navegador" %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    </div>
                </fieldset>
                <br />
                <asp:Label runat="server" ID="lblMensagem" CssClass="alert alert-error" Visible="false" />
                <br />
                <div class="form-actions">
                    <a href="ConfiguracoesConta.aspx" class="btn">Voltar</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
