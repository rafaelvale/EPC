<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signout.aspx.cs" Inherits="Rohr.EPC.Web.Signout" %>

<!DOCTYPE html>
<html class="no-js">
<head id="head" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>EPC</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="msapplication-tooltip" content="Elaboração de Propostas e Contratos" />
    <meta name="msapplication-starturl" content="/Cockpit/" />
    <meta name="msapplication-navbutton-color" content="#ffe100" />
    <meta name="msapplication-window" content="width=1230;height=700" />
    <meta name="description" content="Elaboração de Propostas e Contratos" />
    <link rel="apple-touch-icon" href="favicon.ico">
    <link rel="shortcut icon" type="image/x-icon" href="/favicon.ico" />
    <link rel="shortcut icon" href="favicon.ico">
    <webopt:BundleReference runat="server" Path="~/bundles/style" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="navbar">
            <div class="navbar-inner">
                <div class="container">
                    <div class="span3">
                        <a class="brand">
                            <img src="Content/Image/logo_epc.png" class="ajuste-img" alt="" />EPC</a>
                    </div>
                </div>
            </div>
        </div>
        <div class="span12">
            <p class="lead">
                Você foi desconectado do EPC ;-)
            <a href="Login.aspx" class="btn">Entrar</a>
            </p>
        </div>
        <div class="span12">
            <%--<img src="Content/Image/banner_blackberry.jpg" />--%>
        </div>
    </form>

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="../Content/Script/vendor/jquery-1.10.2.min.js"><\/script>');</script>
    <script src="../Content/Script/rohr-1.0.0.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(function () {
                var updateTime = 4;
                window.setInterval(function () {
                    updateTime = eval(updateTime) - eval(1);
                    if (updateTime == 0) {
                        window.location = ("Login.aspx");
                    }
                }, 900);
            });
        });
    </script>
</body>
</html>
