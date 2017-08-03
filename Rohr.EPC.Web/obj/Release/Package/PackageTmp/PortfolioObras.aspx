﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PortfolioObras.aspx.cs" Inherits="Rohr.EPC.Web.PortfolioObras" %>


<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .affix {
            top: 48px;
        }

        .image {
            margin: 2px 5px 20px 5px;
            border: 1px, solid;
        }

        .textArea {
            margin: 5px;
            border: 1px, solid;
        }
    </style>
    <link href="Admin/Css/Site.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="Content/Script/DataTables/jquery.datatables.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container">
        <div class="row">
            <div class="span3 box-left hidden-tablet">
                <div data-spy="affix" class="box-left space-left-small">
                    <div runat="server" id="divContrato" class="control-title" visible="false">
                        Contrato
                    </div>
                    <asp:Label ID="lblContrato" runat="server" Visible="false"></asp:Label>
                    <div class="control-title">
                        Oportunidade
                    </div>
                    <asp:Label ID="lblOportunidade" runat="server"></asp:Label>
                    <div class="control-title">
                        Comercial
                    </div>
                    <asp:Label ID="lblComercial" runat="server"></asp:Label>
                    <div class="control-title">
                        Cliente
                    </div>
                    <asp:Label ID="lblCliente" runat="server"></asp:Label>
                    <div class="control-title">
                        Obra
                    </div>
                    <asp:Label ID="lblObra" runat="server"></asp:Label>
                    <div class="control-title">
                        Modelo
                    </div>
                    <asp:Label ID="lblModelo" runat="server"></asp:Label>
                </div>
            </div>
            <div class="span9">
                <p class="muted" style="float: left">
                    Selecione as fotos que irão aparecer no Portfólio de Obras
                </p>
                <span class="reload">
                    <img src="Content/Image/Reload.png" onclick="reloadPage()" />
                </span>
                <div style="clear: left">
                    <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="row">
                    <div class="span9">
                        <fieldset>
                            <legend>Portfólio de Obras - Parte 1</legend>
                            <div id="fixedToHeader" style="background-color: #ffffff;">
                            <fieldset>
                                <p>Informe abaixo uma descrição geral das fotos selecionadas</p>
                            </fieldset>
                        </div>
                            <asp:Panel runat="server" ID="panelImagens">
                            <div id="panelResumoFotos">
                            
                            <CKEditor:CKEditorControl ID="ResumoGeralFotos" runat ="server" Height="100px" Width="684px" TextMode="MultiLine"></CKEditor:CKEditorControl>
                                        <br /><br />
                            </div>


                                <asp:Table ID="Table1" Width="100%" runat="server"></asp:Table>
                            </asp:Panel>
                        </fieldset>
                    </div>
                    <div class="span9">
                        <fieldset>
                            <legend></legend>




                               <div class="row-fluid">
                                <div class="span12">
                                       <div class="input-append">
                                            <asp:TextBox ID="PesquisaFoto" runat="server" class="input-xxlarge" placeholder="Pesquisa por Tag(separar palavras por vírgula)" onkeypress="submitPesquisa();" autofocus/>
                                            <asp:LinkButton ID="linkButtonPesquisar" runat="server" CssClass="btn" onclick="linkButtonPesquisar_Click"><i class="icon-search"></i></asp:LinkButton>
                                       </div>
                                </div>
                            </div>

                            <asp:ListView ID="livImagens" runat="server" DataKeyNames="IdImagem">
                                
                                <LayoutTemplate >
                                    <table id="table_id" class="display">
                                       
                                            <tr>
                                                <th>
                                                    <asp:Label ID="lblDescricao" runat="server">Descrição</asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblTag" runat="server">Tag</asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblImagens" runat="server">Imagens</asp:Label>
                                                </th>
                                                <th>
                                                    <asp:Label ID="lblAnexa" runat="server">Anexa</asp:Label>
                                                </th>
                                                <tr id="itemplaceholder" runat="server">
                                                </tr>
                                            </tr>
                                    </table>

                                </LayoutTemplate>
                                <ItemTemplate> 
                                    <tbody>
                                        <tr style="border: solid 1px" runat="server">
                                            <td>
                                                <asp:Label ID="lblDescrArquivo" Width="200px" runat="server" Text='<%# Eval("DescrArquivo")%>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="tag" Width="250px" runat="server" Text='<%# Eval( "Tag") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Image Style="margin: 2px" ID="Image1" runat="server" Height="100px" Width="200px" ImageUrl='<%# Eval("Url") %>' />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="butEscolherReg" runat="server" CommandArgument='<%# Eval("IdImagem")%>'
                                                    ImageUrl="~/Imagens/Icons/rt.gif" OnCommand="butEscolherReg_Click" />
                                            </td>
                                        </tr> 
                                        </tbody>

                                </ItemTemplate>
                               
                                   
                            </asp:ListView>
                        </fieldset>
                    </div>
                </div>
                <br />
                <asp:Label ID="lblMensagemErro" runat="server"></asp:Label>
                <div class="form-actions">
                    <asp:Button ID="btnContinuar" runat="server" Text="Continuar" CssClass="btn" OnClick="btnContinuar_Click"
                        UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                    <asp:Button ID="btnDesistir" runat="server" Text="Desistir" CssClass="btn btn-Danger"
                        OnClick="btnDesistir_Click" UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                    <img src="Content/Image/301.gif" id="imgLoad" height="28" width="28" style="display: none" />
                </div>

            </div>
        </div>
    </div>


    <script type="text/javascript" charset="utf8" src="Content/Script/DataTables/jquery.datatables.js"></script>

    <script type="text/javascript" charset="utf-8">$(document).ready(function () {
    $('#table_id').DataTable();
});</script>

</asp:Content>