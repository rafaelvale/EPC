<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="TempoAprovacaoPropostas.aspx.cs" Inherits="Rohr.EPC.Web.Relatorios.TempoAprovacaoPropostas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .affix {
            top: 48px;
            left: 0;
        }

        .affix-box-left {
            width: 220px;
        }

        ol.progtrckr {
            margin: 0;
            list-style-type: none;
        }

        .progtrckr-proposta {
            width: 1400px;
        }

        .progtrckr-proposta-n {
            width: 1600px;
        }

        .progtrckr-contrato {
            width: 2500px;
        }

        .progtrckr-contrato-n {
            width: 3800px;
        }

        ol.progtrckr li {
            display: inline-block;
            text-align: center;
            line-height: 4em;
        }

            ol.progtrckr li.progtrckr-todo {
                color: silver;
                border-bottom: 8px solid silver;
            }

            ol.progtrckr li.progtrckr-do {
                color: #fd4d27;
                font-weight: bold;
                border-bottom: 8px solid #fd4d27;
            }

            ol.progtrckr li.progtrckr-done {
                color: black;
                border-bottom: 8px solid #fdb827;
            }

            ol.progtrckr li:after {
                content: "\00a0\00a0";
            }

            ol.progtrckr li:before {
                position: relative;
                bottom: -3.2em;
                float: left;
                left: 50%;
                line-height: 2.2em;
            }

            ol.progtrckr li.progtrckr-todo:before {
                height: 1.3em;
                width: 1.3em;
                line-height: 1.2em;
                border: none;
                border-radius: 2.2em;
                content: "\039f";
                color: white;
                background-color: silver;
                font-size: 1.5em;
                bottom: -2.1em;
            }

            ol.progtrckr li.progtrckr-do:before {
                height: 1.3em;
                width: 1.3em;
                line-height: 1.2em;
                border: none;
                border-radius: 2.2em;
                content: "\039f";
                color: white;
                background-color: #fd4d27;
                font-size: 1.5em;
                bottom: -2.1em;
            }

            ol.progtrckr li.progtrckr-done:before {
                content: "\2713";
                color: white;
                background-color: #fdb827;
                height: 2.2em;
                width: 2.2em;
                line-height: 2.2em;
                border: none;
                border-radius: 2.2em;
            }

        .scrolls {
            overflow-x: scroll;
            overflow-y: hidden;
            height: 95px;
        }

        .workflow {
            width: 690px;
        }

        .stats .stat {
            display: table-cell;
            width: 400px;
            vertical-align: top;
        }

        .valorDocumentoCriado {
            display: block;
            margin-bottom: .55em;
            font-size: 30px;
            font-weight: bold;
            letter-spacing: -1px;
            color: #F90;
        }

        .documentoCriado {
            text-align: center;
            padding-top: 5px;
            font-weight: bold;
        }

        .caixa-esquerda {
            width: 260px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container-fluid">
       
        <div class="row">
            <div class="offset3">
                <fieldset>
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                    <legend>Tempo Médio Total</legend>     
                                
                    <div class="tabbable">
                        <ul class="nav nav-tabs">
                            <li class="active"><a href="#tab1" data-toggle="tab">Propostas</a></li>                            
                            <li><a href="#tab2" data-toggle="tab">Contratos</a></li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane active" id="tab1">
                                <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table class="table table-condensed-small table-hover">
                                            <tr>
                                                <th style="white-space: nowrap;">Ação
                                                </th>                                                
                                                <th style="white-space: nowrap;">Perfil
                                                </th>
                                                <th style="white-space: nowrap;">Meta
                                                </th>
                                                <th style="white-space: nowrap;">Tempo Médio Total
                                                </th>                                                
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="row" runat="server">
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Acao") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Perfil") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Meta") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "TempoMedioTotal") %>
                                            </td>                                    
                                            
                                        </tr>
                                        
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="tab-pane" id="tab2">
                                <asp:Repeater ID="Repeater2" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table class="table table-condensed-small table-hover">
                                            <tr>
                                                <th style="white-space: nowrap;">Ação
                                                </th>                                                
                                                <th style="white-space: nowrap;">Perfil
                                                </th>
                                                <th style="white-space: nowrap;">Meta
                                                </th>
                                                <th style="white-space: nowrap;">Tempo Medio Total
                                                </th>                                                
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="row" runat="server">
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Acao") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Perfil") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Meta") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "TempoMedioTotal") %>
                                            </td>                                    
                                            
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                           
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
