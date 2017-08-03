<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterAuxiliar.Master" AutoEventWireup="true" CodeBehind="Administracao.aspx.cs" Inherits="Rohr.EPC.Web.Administracao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            function Abrir(codigo) {
                window.open('PermissoesUsuario.aspx?codigo=' + codigo, 'Impressão', 'width=880, height=600, scrollbars=yes, left=80, top=15');
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">

            <div class="span6">
                <div class="input-append">
                    <input type="text" class="input-xxlarge" placeholder=""
                        id="txtPesquisar" autofocus>
                </div>
            </div>

            <div class="span6 text-right">
                <asp:Button Text="Adicionar Usuário" ID="btnAdicionarUsuario" runat="server" CssClass="btn" />
            </div>
        </div>
        <div class="row">
            <div class="span12">
                <asp:Repeater ID="repeaterUsuarios" runat="server">
                    <HeaderTemplate>
                        <table class="table table-condensed-small table-hover" id="tableCockpit">
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Matricula</th>
                                    <th>Primeiro Nome</th>
                                    <th>Sobrenome</th>
                                    <th>Login</th>
                                    <th>IP Ultimo Acesso</th>
                                    <th>Ativo</th>
                                    <th>Bloqueado</th>
                                    <th>E-mail</th>
                                </tr>
                            </thead>
                            <tbody id="fbody">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" Text='<%# Eval("idUsuario") %>' OnClientClick='<%# String.Format("Abrir(\"{0}\"); return false;", Eval("idUsuario")) %>'></asp:LinkButton></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "Matricula") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "PrimeiroNome") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "Sobrenome") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "Login") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "IP") %></td>
                            <td><%# Boolean.Parse(DataBinder.Eval(Container.DataItem, "Ativo").ToString()) ? "Ativo" : "Inativo" %></td>
                            <td><%# Boolean.Parse(DataBinder.Eval(Container.DataItem, "Bloqueado").ToString()) ? "Bloqueado" : "Desbloqueado" %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "Email") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>
