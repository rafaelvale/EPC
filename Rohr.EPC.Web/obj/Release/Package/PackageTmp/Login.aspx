<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Rohr.EPC.Web.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>EPC</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="msapplication-tooltip" content="Elaboração de Propostas e Contratos" />
    <meta name="msapplication-starturl" content="Login.aspx" />
    <meta name="msapplication-navbutton-color" content="#ffe100" />
    <meta name="msapplication-window" content="width=1230;height=700" />
    <meta name="description" content="Elaboração de Propostas e Contratos" />
    <link rel="apple-touch-icon" href="~/Content/Image/favicon.ico">
    <link rel="shortcut icon" type="image/x-icon" href="~/Content/Image/favicon.ico" />
    <link rel="shortcut icon" href="~/Content/Image/favicon.ico">
    <webopt:BundleReference runat="server" Path="~/bundles/style" />
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/scripts") %>
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <div class="navbar">
        <div class="navbar-inner">
            <div class="container">
                <div class="span3">
                    <a href="#" class="brand">
                        <img src="~/Content/Image/logo_epc.png" width="24" height="24" class="ajuste-img"
                            alt="EPC" style="display: inline-block;" runat="server" id="logoEpc" />
                        <span style="display: inline-block;">&nbsp;EPC</span> </a>
                </div>
            </div>
        </div>
    </div>
    <div class="container" style="font-size: 14px; margin: 0 auto;">
        <asp:Label runat="server" ID="lblNavegador" Visible="False" CssClass="alert alert-error"></asp:Label>
        <div class="span5">
            <ul class="infoEPC">
                <li>
                    <img src="Content/Image/Agreement-02.png" width="32" height="32" />Elaboração de
                    Propostas e Contratos</li>
                <li>
                    <img src="Content/Image/Play-Once.png" width="32" height="32" />Fluxo de Aprovações</li>
                <li>
                    <img src="Content/Image/Check.png" width="32" height="32" />Fluxo de Alçadas</li>
                <li>
                    <img src="Content/Image/Repeat-All.png" width="32" height="32" />Relatórios</li>
                <li>
                    <img src="Content/Image/Tab-History.png" width="32" height="32" />Histórico de Ações</li>
            </ul>
            <br />
            <span style="font-weight: bold; padding-top: 15px;">Evoluir é simplificar</span><br />
            O EPC ficou moderno, integrado com o PMWeb e mais fácil de usar.<br />
            <span style="font-weight: bold; padding-top: 15px;">Linha do Tempo</span><br />
            <span>Agora é possível acompanhar o andamento dos documentos em todo o seu ciclo.</span><br />
            <span style="font-weight: bold; padding-top: 15px;">Objeto</span><br />
            <span>Personalize a forma de exibir o objeto das propostas e contratos, podendo escolher
                as colunas e itens a serem exibidos para o cliente.</span>
            <br />
            <br />
            <%--<span style="color: #dd4b39">EPC 2.0.3</span>--%>
        </div>
        <div class="span5">
            <div class="well">
                <h3 style="display: inline-block; color: #565656">
                    :) Login</h3>
                <img alt="Rohr" height="60" src="Content/Image/Logo03.png" style="float: right;" width="60" />
                <div class="control-group">
                    <label for="txtUsuario">
                        Acesse com a sua conta Rohr</label>
                    <asp:TextBox ID="txtUsuario" CssClass="input-xlarge" MaxLength="20" autocomplete="true"
                        runat="server" autofocus></asp:TextBox>
                </div>
                <div class="control-group">
                    <label for="txtSenha">
                        Senha</label>
                    <asp:TextBox ID="txtSenha" TextMode="Password" CssClass="input-xlarge" MaxLength="20"
                        autocomplete="off" runat="server" ViewStateMode="Enabled"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblMensagem" runat="server" Text="" ForeColor="#dd4b39"></asp:Label>
                </div>
                <div class="control-group">
                    <asp:Button ID="btnAcessar" runat="server" Text="Login" CssClass="btn" OnClick="btnAcessar_Click" />
                </div>
                <a href="Ajuda.aspx">Não consegue acessar sua conta?</a>
            </div>
            <div style="text-align:center;" class="text-error">
                <asp:Label runat="server" ID="lblServidor" Visible="false"></asp:Label>
            </div>
        </div>
        <footer>
            <div class="span12">
                <hr />
                Rohr S/A Estruturas Tubulares&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp; <a href="http://portal:portal@portalrohr/Processos%20Administrativos/DSI/PADSI%20002%20%20Politica%20de%20Uso%20de%20Recursos%20de%20TI-%20Rev%2000.pdf"
                    target="_blank">Política de Uso de Recursos de TI</a> &nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;Departamento
                de Sistemas - Divisão Corporativa
            </div>
        </footer>
    </div>
    </form>
</body>
</html>