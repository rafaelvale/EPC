<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuRodape.ascx.cs" Inherits="Rohr.EPC.Mobile.MenuRodape" %>
<nav class="bar bar-tab">
    <a class="tab-item <% = MenuProposta %>" href="Lista.aspx?t=1">
        <span class="icon icon icon-compose"></span>
        <span class="tab-label">Propostas (<% = TotalOportunidades %>)</span>
    </a>
    <a class="tab-item <% = MenuContrato %>" href="Lista.aspx?t=2">
        <span class="icon icon-pages"></span>
        <span class="tab-label">Contratos (<% = TotalContratos %>)</span>
    </a>
    <a class="tab-item" href="Configuracoes.aspx">
        <span class="icon icon-gear"></span>
        <span class="tab-label"><% = DescricaoPerfil %></span>
    </a>
</nav>
