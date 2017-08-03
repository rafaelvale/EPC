<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="AnaliseJuridico.aspx.cs" Inherits="Rohr.EPC.Web.AnaliseJuridico" %>

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
                    <div runat="server" id="divContrato" class="control-title" visible="false">Contrato</div>
                    <asp:Label ID="lblContrato" runat="server" Visible="false"></asp:Label>
                    <div class="control-title">Oportunidade</div>
                    <asp:Label ID="lblOportunidade" runat="server" Text=""></asp:Label>
                    <div class="control-title">Comercial</div>
                    <asp:Label ID="lblComercial" runat="server" Text=""></asp:Label>
                    <div class="control-title">Cliente</div>
                    <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label>
                    <div class="control-title">Obra</div>
                    <asp:Label ID="lblObra" runat="server" Text=""></asp:Label>
                    <div class="control-title">Modelo</div>
                    <asp:Label ID="lblModelo" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Etapas
                    <div class="progress">
                        <div class="bar" style="width: 60%">3 de 5</div>
                    </div>
                    </div>
                </div>
            </div>
            <div class="span9">
                <div>
                    <asp:Label ID="lblMensagemErro" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span9">
                        <fieldset>
                            <legend>
                                <asp:Label ID="Label1" Text="Contrato" runat="server"></asp:Label>
                            </legend>
                            <div class="row">
                                <div class="span2" id="navparent">
                                    <ul class="nav nav-list span2 navex" data-spy="affix" data-offset-top="120">
                                        <% Response.Write(PartesDocumento); %>
                                    </ul>
                                </div>
                                <div class="span7">
                                    <div style="padding-bottom: 10px;">
                                        <p class="muted" runat="server" id="lblInstrucao">
                                            Analise todas as partes do documento.<br />
                                            As partes destacadas em vermelho foram alteradas pelo comercial responsável.
                                        </p>
                                        <asp:Label runat="server" ID="lblDescricao"></asp:Label>
                                    </div>
                                    <asp:Panel runat="server" ID="panelPartes">
                                    </asp:Panel>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="row">
                    <div class="span2"></div>
                    <div class="span4">
                        <h6>Observações</h6>
                    </div>
                </div>
                <div class="row">
                    <div class="span2"></div>
                    <div class="span7">
                        <asp:TextBox runat="server" ID="txtObservacao" AutoCompleteType="None" Width="97%" MaxLength="140" />
                    </div>
                </div>
                <div class="row">
                    <div class="span2"></div>
                    <div class="span7">
                        <div class="form-actions">
                            <asp:Button ID="btnContinuar" runat="server" Text="Aprovar Contrato" CssClass="btn" OnClick="btnContinuar_Click" UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                            <asp:Button ID="btnDesistir" runat="server" Text="Reprovar Contrato" CssClass="btn btn-danger" OnClick="btnDesistir_Click" UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                            <img src="Content/Image/301.gif" width="28" height="18" id="imgLoad" style="display: none" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
