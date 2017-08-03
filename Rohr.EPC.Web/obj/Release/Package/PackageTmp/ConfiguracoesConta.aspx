<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterAuxiliar.Master" AutoEventWireup="true" CodeBehind="ConfiguracoesConta.aspx.cs" Inherits="Rohr.EPC.Web.ConfiguracoesConta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Style/password_stregth.css" rel="stylesheet">
    <style type="text/css">
        .affix {
            top: 48px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="span3 box-left hidden-tablet">
                <div data-spy="affix" class="box-left space-left-small">
                    <h5>
                        <asp:Label runat="server" ID="lblNome"></asp:Label></h5>
                    <div class="control-title">
                        Matricula
                    </div>
                    <asp:Label ID="lblMatricula" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Criado em
                    </div>
                    <asp:Label ID="lblCadastroUsuario" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="span9">
                <asp:Label runat="server" ID="lblAlerta" Visible="False" CssClass="alert alert-error"></asp:Label>
                <fieldset>
                    <legend class="legend-small">Perfil</legend>
                    <div style="padding-left: 10px;">
                        <div>
                            <asp:Label runat="server" ID="lblFilial"></asp:Label>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblPerfil"></asp:Label>
                        </div>
                    </div>
                </fieldset>
                <fieldset>
                    <legend class="legend-small">Segurança <a href="editarSenha.aspx" runat="server"
                        id="linkEditarSenha" style="font-size: 13px;">Alterar senha</a></legend>
                    <div style="padding-left: 10px;">
                        <asp:Panel runat="server" ID="panelSessaoSenha">
                            Senha alterada em <strong>
                                <asp:Label ID="lblDataAlteracaoSenha" Text="" runat="server" /></strong>.<br />
                        </asp:Panel>
                        <p>
                            Nunca informe sua senha ao
                            <abbr title="Departamento de Sistemas e Informações">
                                DSI</abbr>. É aconselhavel que você escolha uma senha forte. A senha é de uso
                            pessoal e intransferível.<br />
                            Selecione uma combinação de letras, números e símbolos para criar uma senha única
                            que não tenha relação com suas informações pessoais. A sua senha é criptografada não sendo possível a visualização pelo
                            <abbr title="Departamento de Sistemas e Informações">DSI</abbr>
                        </p>
                        <asp:Panel runat="server" ID="panelSenha" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <label for="txtSenhaAtual">
                                            Senha atual</label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" AutoCompleteType="None" TextMode="Password" MaxLength="12"
                                            CssClass="input-medium" ID="txtSenhaAtual" autofocus />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtNovaSenha">
                                            Nova senha</label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" AutoCompleteType="None" TextMode="Password" MaxLength="12"
                                            ID="txtNovaSenha" CssClass="input-medium" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtConfirmarSenha">
                                            Confirmar nova senha</label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" AutoCompleteType="None" TextMode="Password" MaxLength="12"
                                            ID="txtConfirmarSenha" CssClass="input-medium" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </fieldset>
                <fieldset>
                    <legend class="legend-small">Recebendo Notificações <a href="editarNotificacoes.aspx"
                        runat="server" id="linkEditarNotificacoes" style="font-size: 13px;">Alterar notificações</a></legend>
                    <div style="padding-left: 10px;">
                        Você pode escolher em qual ação receberá e-mail. É aconselhavel que deixe selecionado
                        todas as opções.
                    </div>
                    <div style="padding-left: 10px; padding-top: 15px; font-weight: bold">
                        E-mail:
                        <asp:Label runat="server" ID="lblEmail"></asp:Label>
                    </div>
                    <div style="padding-left: 30px; padding-top: 5px;">
                        <asp:CheckBoxList runat="server" ID="ckListNotificacoes" CssClass="checkbox">
                        </asp:CheckBoxList>
                    </div>
                </fieldset>
                <asp:Panel runat="server" ID="panelAtividadeConta" Visible="false">
                    <fieldset>
                        <legend class="legend-small">Atividades da conta <a href="AtividadeConta.aspx" runat="server"
                            id="A2" style="font-size: 13px;">Visualizar</a></legend>
                    </fieldset>
                </asp:Panel>
                <asp:Panel runat="server" ID="panelAcao" Visible="false">
                    <div class="form-actions">
                        <asp:Button ID="btnContinuar" runat="server" Text="Confirmar" CssClass="btn" OnClick="btnContinuar_Click" />
                        <asp:Button ID="btnDesistir" runat="server" Text="Cancelar" CssClass="btn btn-danger"
                            OnClick="btnDesistir_Click" />
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
