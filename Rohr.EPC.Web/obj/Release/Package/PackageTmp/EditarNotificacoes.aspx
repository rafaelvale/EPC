<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterAuxiliar.Master" AutoEventWireup="true" CodeBehind="EditarNotificacoes.aspx.cs" Inherits="Rohr.EPC.Web.EditarNotificacoes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="span3 box-left hidden-tablet">
            </div>
            <div class="span9">
                <asp:Label runat="server" ID="lblAlerta" Visible="False" CssClass="alert alert-error"></asp:Label>

                <fieldset>
                    <legend class="legend-small">Recebendo Notificações</legend>
                    <div style="padding: 10px;">
                        E-mail
                        <div class="input-prepend input-append">
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="span2"></asp:TextBox>
                            <span class="add-on">@rohr.com.br</span>
                        </div>
                    </div>

                    <div style="padding-left: 10px;">Escolha em qual ação receberá e-mail. É aconselhavel que deixe selecionado todas as opções.</div>
                    <div style="padding-left: 30px; padding-top: 5px;">
                        <asp:CheckBoxList runat="server" ID="ckListNotificacoes" CssClass="checkbox">
                        </asp:CheckBoxList>
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
