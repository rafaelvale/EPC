<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterAuxiliar.Master" AutoEventWireup="true" CodeBehind="EditarSenha.aspx.cs" Inherits="Rohr.EPC.Web.EditarSenha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="span3 box-left hidden-tablet">
            </div>
            <div class="span9">
                <fieldset>
                    <legend class="legend-small">Segurança</legend>
                    <div style="padding-left: 10px;">
                        É aconselhavel que você escolha uma senha forte. A senha é de uso pessoal e intransferível. <strong>Nunca informe sua senha ao DSI</strong>.
                                <table>
                                    <tr>
                                        <td>
                                            <label for="txtSenhaAtual">Senha atual</label></td>
                                        <td>
                                            <asp:TextBox runat="server" AutoCompleteType="None" TextMode="Password" MaxLength="12" CssClass="input-medium" ID="txtSenhaAtual" autofocus /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label for="txtNovaSenha">Nova senha</label></td>
                                        <td>
                                            <asp:TextBox runat="server" AutoCompleteType="None" TextMode="Password" MaxLength="12" ID="txtNovaSenha" CssClass="input-medium" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label for="txtConfirmarSenha">Confirmar nova senha</label></td>
                                        <td>
                                            <asp:TextBox runat="server" AutoCompleteType="None" TextMode="Password" MaxLength="12" ID="txtConfirmarSenha" CssClass="input-medium" /></td>
                                    </tr>
                                </table>
                        Selecione uma combinação de letras, números e símbolos para criar uma senha única que não tenha relação com suas informações pessoais.                            
                            Não deixe suas listas de senhas anotadas no computador ou na escrivaninha
                    </div>
                </fieldset>

                <br />
                <asp:Label runat="server" ID="lblMensagem" CssClass="alert alert-error" Visible="false" />
                <br />
                <div class="form-actions">
                    <asp:Button ID="btnContinuar" runat="server" Text="Confirmar" CssClass="btn" OnClick="btnContinuar_Click" />
                    <a href="ConfiguracoesConta.aspx" class="btn btn-danger">Cancelar</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
