<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Rohr.EPC.Mobile.Login" %>

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

    <style>
        .content {
            margin: 6px;
        }

        .well {
            min-height: 200px;
            padding: 10px;
            background-color: #f7f7f7;
            border: 1px solid #e3e3e3;
            -ms-border-radius: 2px;
            border-radius: 2px;
            -moz-box-shadow: 0 2px 2px #000000;
            -moz-box-shadow: 0 2px 2px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0 2px 2px #000000;
            -webkit-box-shadow: 0 2px 2px rgba(0, 0, 0, 0.3);
            box-shadow: 0 2px 2px #000000;
            box-shadow: 0 2px 2px rgba(0, 0, 0, 0.3);
        }

        .banner h1 {
            text-align: center;
            direction: ltr;
            color: #555;
            font-size: 20px;
            font-weight: normal;
            margin-top: 10px;
            margin-bottom: 20px;
        }

        .bloco-login {
            width: 100%;
            padding: 15px 15px;
            color: #555;
        }
    </style>

    <link rel="stylesheet" href="Content/Style/main.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="content">
            <div style="text-align: center; padding-bottom: 10px;">
                <img src="Content/Image/logo.png" alt="Rohr" style="width: 55px; height: 55px;" />
            </div>
            <div class="banner">
                <h1>Acesse com a sua conta Rohr</h1>
            </div>
            <div class="well bloco-login">
                <asp:Label runat="server" ID="lblNavegador" Visible="False" CssClass="alert alert-error"></asp:Label>
                <h4 style="text-align: center; padding-bottom: 10px; font-weight: 400;">Login</h4>
                <label for="txtUsuario">Conta Rohr</label>
                <asp:TextBox ID="txtUsuario" runat="server" autofocus></asp:TextBox>
                <label for="txtSenha">Senha</label>
                <asp:TextBox ID="txtSenha" TextMode="Password" CssClass="input-xlarge" MaxLength="20" runat="server"></asp:TextBox>
                <asp:Label ID="lblMensagem" runat="server" Text="" CssClass="mensagem text-error"></asp:Label>
                <asp:Button ID="btnAcessar" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnAcessar_Click" />
                <div style="text-align: center;" class="text-error">
                    <asp:Label runat="server" ID="lblServidor" Visible="false" CssClass="mensagem"></asp:Label>
                </div>
                <a style="display: inline-block; margin-top: 10px;">Não consegue acessar sua conta?</a>
            </div>

            <footer style="text-align: center; padding-top: 30px;">
                <div>
                    <p>
                        Departamento de Sistemas<br />
                        Divisão Corporativa
                    </p>
                </div>
            </footer>
        </div>
        <script src="Content/Script/vendor/jquery-1.10.2.min.js"></script>
        <script src="Content/Script/vendor/ratchet.min.js"></script>
    </form>
</body>
</html>
