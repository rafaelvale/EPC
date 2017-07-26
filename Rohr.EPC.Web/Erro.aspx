<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterAuxiliar.Master" AutoEventWireup="true" CodeBehind="Erro.aspx.cs" Inherits="Rohr.EPC.Web.Erro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="span12">
        <p class="lead">
            Oops! Parece que algo deu errado.
        </p>
    </div>
    <div class="span12">
        <% = erro %>
    </div>
</asp:Content>