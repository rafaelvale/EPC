<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ajuda.aspx.cs" Inherits="Rohr.EPC.Web.Ajuda" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>EPC</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="msapplication-tooltip" content="Elaboração de Propostas e Contratos" />
    <meta name="msapplication-starturl" content="Login" />
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
                            <span style="display: inline-block;">&nbsp;Ajuda</span>
                        </a>
                    </div>
                </div>
            </div>
        </div>
        <div class="container" style="margin: 0 auto;">
            <div class="span12">
                <span style="font-weight: bold; padding-top: 15px;">Problemas para acessar a sua conta?</span><br />
                <span>Se você não conseguir fazer login, tente um dos procedimentos abaixo:</span>
                <br />
                <br />
                <ul>
                    <li><strong>Esqueci meu nome de usuário:</strong> O nome de usuário é o mesmo utilizado para ter acesso a máquina.</li>
                    <li><strong>Esqueci minha senha:</strong> envie um e-mail para o departamento de Sistemas (sistemas@rohr.com.br) informe seu usuário ou matricula e solicite uma nova senha.</li>
                    <li><strong>Sei meu nome de usuário e minha senha, mas mesmo assim não consigo acessar:</strong> entre em contato com o departamento de Sistemas (sistemas@rohr.com.br), e informe o problema.</li>
                    <li><strong>Estou vendo uma mensagem de que meu navegador não é compatível:</strong> envie um e-mail para o DSI (dsi@rohr.com.br) informando o problema.</li>
                    <li><strong>Estou vendo uma mensagem de que meu usuário está bloqueado devido as tentativas inválidas de acesso:</strong> Por motivos de segurança o seu usuário foi bloqueado, devido as tentativas inválidas de acesso. Envie uma e-mail para o departamento de Sistemas (sistemas@rohr.com.br) informando o problema.</li>
                </ul>
                <br />
                <br />
                Se ainda assim não conseguir acessar sua conta ou estiver com dificuldade entre em contato com o DSI ou envie um e-mail para dsi@rohr.com.br.
                <br />
                <br />
            </div>
            <footer>
                <div class="span12">
                    <hr />
                    Rohr S/A Estruturas Tubulares&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
                    <a href="http://portal:portal@portalrohr/Processos%20Administrativos/DSI/PADSI%20002%20%20Politica%20de%20Uso%20de%20Recursos%20de%20TI-%20Rev%2000.pdf"
                    target="_blank">Política de Uso de Recursos de TI</a>
                        &nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;Departamento de Sistemas e Informações - Divisão Corporativa
                </div>
            </footer>
        </div>
    </form>
</body>
</html>
