<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Precos.aspx.cs" Inherits="Rohr.EPC.Web.Precos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .affix {
            top: 48px;
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
                    <div class="control-title">
                        Etapas
                        <div class="progress">
                            <div class="bar" style="width: 60%">
                                3 de 5
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span9">
                <span class="reload">
                    <img src="Content/Image/Reload.png" onclick="reloadPage()" />
                </span>
                <div style="clear: left">
                    <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span9">
                        <fieldset>
                            <legend>Preço
                            </legend>
                            <div style="padding-bottom:10px;">
                                <asp:Label runat="server" ID="lblDescricao"></asp:Label>
                            </div>
                            <asp:Repeater runat="server" ID="repeaterItens" OnItemDataBound="repeaterItens_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="table table-condensed-small table-hover table-bordered">
                                        <thead>
                                            <tr>
                                                <th class='text-nowrap'>Tipo</th>
                                                <th class='text-nowrap'>Descrição</th>
                                                <th class='text-nowrap' style="width: 50px;" title="Valor Unitário de Locação">V.U.L(R$)</th>
                                                <th class='text-nowrap' style="width: 50px;" title="Valor Unitário de Indenização">V.U.I(R$)</th>
                                                <th class='text-nowrap' style="width: 50px;" title="Valor Unitário de Locação">Exibir V.U.L(R$)</th>
                                                <th class='text-nowrap' style="width: 50px;" title="Valor Unitário de Indenização">Exibir V.U.I(R$)</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="text-nowrap hidden-tablet">
                                            <%# DataBinder.Eval(Container.DataItem, "DescricaoResumida") %>
                                        </td>
                                        <td class="hidden-tablet">
                                            <asp:Label runat="server" ID="lblDescricaoCliente"></asp:Label>
                                        </td>
                                        <td class="text-nowrap hidden-tablet">
                                            <%# DataBinder.Eval(Container.DataItem, "ValorPraticadoLocacao") %>
                                        </td>
                                        <td class="text-nowrap hidden-tablet">
                                            <%# DataBinder.Eval(Container.DataItem, "ValorPraticadoIndenizacao") %>
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="checkboxLinhaVUL" Checked="true" />
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="checkboxLinhaVUI" Checked="true" Enabled="false" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </div>
                </div>
                <div class="form-actions">
                    <asp:Button ID="btnContinuar" runat="server" Text="Continuar" CssClass="btn" OnClick="btnContinuar_Click"
                        UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                    <asp:Button ID="btnDesistir" runat="server" Text="Desistir" CssClass="btn btn-danger"
                        OnClick="btnDesistir_Click" UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                    <img src="Content/Image/301.gif" id="imgLoad" height="28" width="28" style="display: none" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
