﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Resumo.aspx.cs" Inherits="Rohr.EPC.Web.Resumo" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .affix{
            top: 48px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBody" runat="server">
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
                    <asp:Label ID="lblOportunidade" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Comercial
                    </div>
                    <asp:Label ID="lblComercial" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Cliente
                    </div>
                    <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Obra
                    </div>
                    <asp:Label ID="LblObra" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Modelo
                    </div>
                    <asp:Label ID="LblModelo" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Etapas
                        <div class="progress">
                            <div class="bar" style="width: 60%">
                                4 de 6
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span9">
                <p class="muted" style="float: left;">
                    Faça um resumo da proposta para ser exibida no documento
                </p>
                <span class="reload">
                    <img src="Content/Image/Reload.png" onclick="reloadPage()" />
                </span>
                <div style="clear: left">
                    <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="row">
                    <div class ="span12">
                        <div id="fixedToHeader" style="background-color: #ffffff;">
                            <fieldset>
                                <legend style="margin-bottom: 5px;">Resumo da Proposta</legend>
                                <p>Informe abaixo o resumo de sua proposta</p>
                            </fieldset>
                        </div>
                        <div id="panelResumo">
                            
                                <CKEditor:CKEditorControl ID="CKResumoProposta" runat="server" Height="150px" Width="466px"></CKEditor:CKEditorControl>
                                                                
                                        <br />
                          </div>
                        <div class="form-actions">
                            <asp:Button ID="btnContinuar" runat="server" Text="Continuar" CssClass="btn" OnClick="btnContinuar_Click"
                                UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                            <asp:Button ID="btnDesistir" runat="server" Text="Desistir" CssClass="btn btn-danger"
                                OnClick="btnDesistir_Click" UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                            <img src="Content/Image/301.gif" id="imgLoad" height="28" width="28" style="display: none" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
