<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterAuxiliar.Master" AutoEventWireup="true" CodeBehind="PermissoesUsuario.aspx.cs" Inherits="Rohr.EPC.Web.PermissoesUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="span12 text-right">
                <asp:Button Text="Resetar Senha" ID="btnResetarSenha" runat="server" CssClass="btn" />
                <asp:Button Text="Salvar" ID="btnSalvar" runat="server" CssClass="btn" />
            </div>
        </div>
        <div class="row">

            <div class="span4"></div>

            <div class="span2">
                <fieldset>
                    <legend>Perfis</legend>
                </fieldset>
                <label class='checkbox'>
                    <asp:CheckBox ID="CheckBox1" runat="server" />Administrativo</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="CheckBox2" runat="server" />Comercial</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="CheckBox3" runat="server" />Comercial PTA</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="CheckBox4" runat="server" />D. Operacional/Sup.</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="CheckBox5" runat="server" />Diretoria</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="CheckBox6" runat="server" />Gerente</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="CheckBox7" runat="server" />Jurídico</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="CheckBox8" runat="server" />Supervisor PTA</label>
            </div>
            <div class="span2">
                <fieldset>
                    <legend>Filiais</legend>
                </fieldset>
                <label class='checkbox'>
                    <asp:CheckBox ID="ckBh" runat="server" />Belo Horizonte</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="ckDf" runat="server" />Brasília</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="ckCr" runat="server" />Curitiba</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="ckMc" runat="server" />Macaé</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="ckRs" runat="server" />Porto Alegre</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="ckRj" runat="server" />Rio de Janeiro</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="ckSp" runat="server" />São Paulo</label>
                <label class='checkbox'>
                    <asp:CheckBox ID="ckBa" runat="server" />Simões Filho - BA</label>
            </div>
        </div>
    </div>
</asp:Content>
