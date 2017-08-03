<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Fim.aspx.cs" Inherits="Rohr.EPC.Web.Fim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container">
        <div class="row">
            <div class="span3"></div>
            <div class="span6">
                <div class="hero-unit text-center" style="background-color: #3B5998;">
                    <img src="Content/Image/Verify.png" />
                    <h3 style="color: #ffffff">Seu documento foi gerado.</h3>
                    <asp:Button ID="btnNovaProposta" Text="Nova Proposta" runat="server" CssClass="btn" OnClick="btnNovaProposta_Click" />
                    <br />
                    <asp:Button ID="btnPaginaPrincipal" Text="Pagina principal (3)" runat="server" CssClass="btn btn-danger btn-large" OnClick="btnPaginaPrincipal_Click" />
                </div>
            </div>
        </div>
    </div>
    <script src="../Content/Script/vendor/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var settimmer = 0;
            $(function () {
                var updateTime = 3;
                window.setInterval(function () {
                    updateTime = eval(updateTime) - eval(1);
                    if (updateTime < 0)
                        updateTime = 0;
                    $("#contentBody_btnPaginaPrincipal").val("Pagina principal (" + updateTime + ")");
                    if (updateTime <= 0) {
                        window.location = ("Redirecionar.aspx?p=5sRJ3o2ln+E=");
                    }
                }, 1000);
            });
        });
    </script>
</asp:Content>
