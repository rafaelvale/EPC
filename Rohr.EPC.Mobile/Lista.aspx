<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Lista.aspx.cs" Inherits="Rohr.EPC.Mobile.Lista" %>
<%@ Register Src="~/MenuRodape.ascx" TagPrefix="uc1" TagName="MenuRodape" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8">
    <title>EPC Mobile</title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no">

    <link rel="stylesheet" href="Content/Style/main.css">

    <style>
        p {
            line-height: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <header class="bar bar-nav" style="background-color: #ffe100">
            <a href="Lista.aspx">
                <img src="Content/Image/logo_epc.png" alt="EPC" style="float: left; padding-top: 10px;" />
            </a>
            <h1 class="title">Documentos</h1>
            <img src="Content/Image/logo.png" width="40" height="40" alt="Rohr" style="float: right; padding-top: 10px;" />
        </header>
        <div class="content">
            <uc1:MenuRodape runat="server" id="MenuRodape" />
            <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false">
                <HeaderTemplate>
                    <ul class="table-view">
                </HeaderTemplate>
                <ItemTemplate>
                    <li class="table-view-cell media">
                        <a class="navigate-right" href="Acoes.aspx?t=<% = Request.QueryString["t"] ?? "1" %>&i=<%# DataBinder.Eval(Container.DataItem, "idDocumento") %>">
                            <div class="media-body">
                                <%# DataBinder.Eval(Container.DataItem, "nomeObra") %>
                                <p>Cliente: <%# DataBinder.Eval(Container.DataItem, "nomeCliente") %></p>
                                <p>Nº Doc.: <%# DataBinder.Eval(Container.DataItem, "numeroDocumento", "{0:N0}") %></p>
                                <p>Vl. Negócio (R$): <%# DataBinder.Eval(Container.DataItem, "valorNegocio", "{0:N2}") %></p>
                                <p>Desc.(-)/Acr.(+) M.(%): <%# DataBinder.Eval(Container.DataItem, "percentualDesconto", "{0:N2}") %></p>
                            </div>
                        </a>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <div style="padding-bottom: 50px;"></div>
        </div>
        <script src="Content/Script/vendor/jquery-1.10.2.min.js"></script>
        <script src="Content/Script/vendor/ratchet.min.js"></script>
    </form>
</body>
</html>
