<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Partes.aspx.cs" Inherits="Rohr.EPC.Web.Partes"
    ViewStateMode="Disabled" %>
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
                            <div class="bar" style="width: 80%">
                                4 de 5
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span9">
                <p class="muted" style="float: left;">
                    Revise as partes do seu documento, e caso seja necessário, realize as alterações.<br />
                    Todas as alterações realizadas em cada parte do documento, serão analisadas pelo
                    departamento <strong>Jurídico</strong>.
                </p>
                <span class="reload">
                    <img src="Content/Image/Reload.png" onclick="reloadPage()" />
                </span>
                <div style="clear: left">
                    <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span9">
                        <fieldset>
                            <legend>
                                <asp:Label Text="Proposta" ID="lblTipoDocumento" runat="server"></asp:Label>
                            </legend>
                            <div class="row">
                                <div class="span2">
                                    <asp:Literal runat="server" ID="menuApoio"></asp:Literal>
                                </div>
                                <div class="span7">
                                    <asp:Panel runat="server" ID="panelPartes">
                                    </asp:Panel>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="form-actions">
                    <asp:Button ID="btnContinuar" runat="server" Text="Continuar" CssClass="btn" OnClick="btnContinuar_Click"
                        UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                    <asp:Button ID="btnDesistir" runat="server" Text="Desistir" CssClass="btn btn-danger"
                        UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" OnClick="btnDesistir_Click" />
                    <img src="Content/Image/301.gif" id="imgLoad" height="28" width="28" style="display: none" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
