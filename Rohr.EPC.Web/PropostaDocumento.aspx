<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="PropostaDocumento.aspx.cs" Inherits="Rohr.EPC.Web.PropostaDocumento" ValidateRequest="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >
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
                    <asp:Panel runat="server" ID="panelDescricaoOportunidade" Visible="false" ViewStateMode="Disabled" CssClass="menu-left">
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
                    </asp:Panel>
                    <div class="control-title">
                        Etapas
                    <div class="progress">
                        <div class="bar" style="width: 20%">1 de 6</div>
                    </div>
                    </div>
                </div>
            </div>
            <div class="span9">
                <p class="muted">
                    Informe o número da oportunidade cadastrada no
                    <img src="Content/Image/logopmweb.jpg" alt="PMWeb" width="80" height="31" />
                    e selecione o modelo a ser utilizado
                </p>
                <div>
                    <div class="input-append">
                        <asp:TextBox autofocus class="input-small" ID="txtNumeroOportunidade" MaxLength="6" placeholder="Oportunidade" AutoCompleteType="Disabled"
                            required runat="server" ViewStateMode="Enabled"></asp:TextBox>
                    </div>
                        <asp:DropDownList ID="ddlModelo" runat="server"></asp:DropDownList>
                        <asp:LinkButton ID="linkButtonPesquisar" runat="server" CssClass="btn btn-pesquisa" OnClick="linkButtonPesquisar_Click"><i class="icon-search"></i></asp:LinkButton>
                </div>
                <div>
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span9">
                        <asp:Panel ID="panelModelo" runat="server" Visible="false" ViewStateMode="Disabled">
                            <fieldset>
                                <legend>Modelo</legend>

                                <table class="table table-bordered">
                                    <tr>
                                        <td style="width: 150px;">
                                            <strong>Modelo</strong><br />
                                            <asp:Label ID="lblModeloEscolhido" runat="server" Text=""></asp:Label><br />
                                            <strong>Versão do modelo</strong><br>
                                            <asp:Label ID="lblVersao" runat="server" Text=""></asp:Label><br />
                                            <strong>Modelo criado em</strong><br />
                                            <asp:Label ID="lblCriado" runat="server" Text=""></asp:Label><br />
                                            <br />
                                            <asp:Label ID="lblMensagemDataCriacao" runat="server" ForeColor="Red"></asp:Label><br />
                                            <br />
                                            <br />
                                            <asp:Button ID="btnUtilizarModelo" runat="server" Text="Utilizar modelo" CssClass="btn btn-success" OnClick="btnUtilizarModelo_Click" />
                                        </td>
                                        <td>
                                            <CKEditor:CKEditorControl ID="CKEditor" runat="server" Height="358px" Width="451px"></CKEditor:CKEditorControl>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
