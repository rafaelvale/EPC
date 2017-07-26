<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Acoes.aspx.cs" Inherits="Rohr.EPC.Mobile.Acoes" %>

<%@ Register Src="~/MenuRodape.ascx" TagPrefix="uc1" TagName="MenuRodape" %>


<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8">
    <title>EPC Mobile</title>
    <meta name="HandheldFriendly" content="True">
    <meta name="MobileOptimized" content="320">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no">
    <meta http-equiv="cleartype" content="on">
    <meta name="msapplication-TileImage" content="Content/Image/favicon.ico">
    <meta name="msapplication-TileColor" content="#222222">

    <link rel="stylesheet" href="Content/Style/main.css">

    <style>
        .bar.bar-nav a {
            color: #bbb;
        }

        .content {
            margin: 6px;
        }

        .form-actions {
            padding: 14px 15px 15px;
            margin-top: 5px;
            margin-bottom: 20px;
            background-color: #f5f5f5;
            border-top: 1px solid #e5e5e5;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <header class="bar bar-nav" style="background-color: #ffe100">
            <a href="Lista.aspx">
                <img src="Content/Image/logo_epc.png" alt="EPC" style="float: left; padding-top: 10px;" />
            </a>
            <h1 class="title">Análise</h1>
            <img src="Content/Image/logo.png" width="40" height="40" alt="Rohr" style="float: right; padding-top: 10px;" />
        </header>
        <div class="content">
            <uc1:MenuRodape runat="server" ID="MenuRodape" />
            <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false" OnItemDataBound="Repeater1_ItemDataBound">
                <HeaderTemplate>
                    <ul class="table-view">
                </HeaderTemplate>
                <ItemTemplate>
                    <li class="table-view-cell media">
                        <div class="media-body">
                            <%# DataBinder.Eval(Container.DataItem, "nomeObra") %>
                            <p>Cliente: <%# DataBinder.Eval(Container.DataItem, "nomeCliente") %></p>
                            <p>Nº Doc.: <%# DataBinder.Eval(Container.DataItem, "numeroDocumento", "{0:N0}") %></p>
                            <p>Vl. Negócio (R$): <%# DataBinder.Eval(Container.DataItem, "valorNegocio", "{0:N2}") %></p>
                            <p>Desc.(-)/Acr.(+) M.(%): <%# DataBinder.Eval(Container.DataItem, "percentualDesconto", "{0:N2}") %></p>
                            <br />
                            <p>
                                <asp:Label runat="server" ID="lblComercial"></asp:Label><br />
                                <a runat="server" ID="lblEmail"></a><br />
                                <a runat="server" ID="lblCelular"></a><br />
                            </p>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div style="padding-top: 10px;">
                Observações
            <asp:TextBox runat="server" ID="txtObservacao" AutoCompleteType="None" MaxLength="140" />
                <asp:Label ID="lblMensagemErro" runat="server" CssClass="mensagem" />
                <div class="form-actions">
                    <div style="width: 80%;">
                        <asp:Button runat="server" ID="btnAcaoPrimaria" Text="Aprovar" CssClass="btn btn-positive" OnClick="btnAcaoPrimaria_Click" />
                    </div>
                    <div style="width: 80%; margin-top: 15px;">
                        <asp:Button runat="server" ID="btnAcaoSecundaria" Text="Reprovar" CssClass="btn btn-negative" OnClick="btnAcaoSecundaria_Click" />
                    </div>
                </div>
                <div style="padding-bottom: 50px;"></div>
            </div>
        </div>
        <script src="Content/Script/vendor/jquery-1.10.2.min.js"></script>
        <script src="Content/Script/vendor/ratchet.min.js"></script>
    </form>
</body>
</html>
