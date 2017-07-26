<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="ContratoDocumento.aspx.cs" Inherits="Rohr.EPC.Web.ContratoDocumento" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>EPC - Escolha do Modelo</title>
    <style type="text/css">
        .affix {
            top: 48px;
        }
    </style>
    <script src="Content/Script/vendor/jquery-1.9.1.min.js"></script>
    <script>
        $(document).ready(function () {
            $(document.body).scrollspy({
                target: "#navparent"
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container">
        <div class="row">
            <div class="span3 box-left">
                <div data-spy="affix" class="box-left space-left-small">
                    <div class="control-title">
                        Contrato
                    </div>
                    <asp:Label ID="lblContrato" runat="server" Text=""></asp:Label>
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
                            <div class="bar" style="width: 20%">
                                1 de 6
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span9">
                <div>
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span9">
                        <br />
                        <asp:Panel ID="panelModelo1" runat="server">
                            <p class="muted">Escolha o modelo do contrato</p>
                            <div class="input-append">
                                <asp:DropDownList ID="ddlModelo" runat="server">
                                </asp:DropDownList>
                                <asp:LinkButton ID="linkButtonPesquisar" runat="server" CssClass="btn" OnClick="linkButtonPesquisar_Click"><i class="icon-search"></i></asp:LinkButton>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="panelModelo" runat="server" Visible="false">
                            <fieldset>
                                <legend>Modelo</legend>
                                <table class="table table-bordered">
                                    <tr>
                                        <td>
                                            <asp:Panel runat="server" ID="panelContratoCliente">
                                                Faça o upload do contrato do cliente no formato PDF.<br />
                                                Apenas um arquivo pode ser carregado.<br />
                                                O orçamento feito no PMWeb deve ficar semelhante ao orçamento do cliente.<br />
                                                Ao escolher utilizar o contrato do cliente não será possível alterar para um modelo Rohr.
                                                <div style="padding-top: 25px;">
                                                    <label class="btn" for="contentBody_flContrato">
                                                        <asp:FileUpload runat="server" ID="flContrato" Style="display: none;" onchange='$("#uploadFile").html($(this).val());' />
                                                        Upload do contrato
                                                    </label>
                                                </div>
                                                <div style="padding-top: 25px;">
                                                    Arquivo selecionado:
                                                <span class='' id="uploadFile"></span>
                                                </div>
                                            </asp:Panel>
                                            <CKEditor:CKEditorControl ID="CKEditor" runat="server"></CKEditor:CKEditorControl>
                                        </td>
                                        <td style="width: 150px;">
                                            <strong>Modelo</strong><br />
                                            <asp:Label ID="lblNomeModelo" runat="server" Text=""></asp:Label><br />
                                            <strong>Versão do modelo</strong><br>
                                            <asp:Label ID="lblVersao" runat="server" Text=""></asp:Label><br />
                                            <strong>Modelo criado em</strong><br />
                                            <asp:Label ID="lblCriado" runat="server" Text=""></asp:Label><br />
                                            <br />
                                            <asp:Label ID="lblMensagemDataCriacao" runat="server" ForeColor="Red"></asp:Label><br />
                                            <br />
                                            <br />
                                            <asp:Button ID="btnUtilizarModelo" runat="server" Text="Utilizar modelo" CssClass="btn btn-success"
                                                OnClick="btnUtilizarModelo_Click" />
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
