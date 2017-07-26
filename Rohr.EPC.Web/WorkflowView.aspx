<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="WorkflowView.aspx.cs" Inherits="Rohr.EPC.Web.WorkflowView" %>

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
            <div class="span3 box-left">
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
                    <asp:Label ID="lblObra" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Modelo
                    </div>
                    <asp:Label ID="lblModelo" runat="server" Text=""></asp:Label>
                </div>
            </div>

            <div class="offset3" <%Response.Write(semNegocio); %>>
                <div style="min-width: 600px;">
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                    <fieldset>
                        <legend>Linha do Tempo</legend>
                    </fieldset>
                    <div class="stats">
                        <div class="stat">
                            <strong>Meta de conclusão para o documento:</strong>
                            <asp:Label runat="server" ID="lblMetaDocumento"></asp:Label>
                            <br>
                            <strong>Próximo passo:</strong>
                            <asp:Label runat="server" ID="lblProximoPasso"></asp:Label><br />
                            <strong>Meta do próximo passo:</strong>
                            <asp:Label runat="server" ID="lblMetaProximoPasso"></asp:Label>
                            <div class="workflow">
                                <div class="scrolls">
                                    <% Response.Write(Workflow);%>
                                </div>
                            </div>
                        </div>
                        <div class="stat">
                            <div class="caixa-esquerda">
                                <div class="stat documentoCriado">
                                    <asp:Label runat="server" ID="lblLabelDocumentoCriado">
                                        Documento criado há 
                                    </asp:Label>
                                    <span class="valorDocumentoCriado">
                                        <asp:Label runat="server" ID="lblDocumentoCriado"></asp:Label></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="offset3">
                <fieldset>
                    <legend>Histórico</legend>
                    <div id="divSemNegocio" runat="server" visible="false" class="cardSemNegocio cardSemNegocio-agendamento cardSemNegocio-red">
                        <span>
                            <asp:Label runat="server" ID="lblSemNegocioData"></asp:Label>
                        </span>
                        <div style="padding-top: .3rem;">
                            <asp:Label runat="server" ID="lblSemNegocioMotivo"></asp:Label>
                        </div>
                    </div>
                    <div class="tabbable" <%Response.Write(semNegocio); %>>
                        <ul class="nav nav-tabs">
                            <li class="active"><a href="#tab1" data-toggle="tab">Ações</a></li>
                            <li><a href="#tab3" data-toggle="tab">Anexos</a></li>
                            <li><a href="#tab2" data-toggle="tab">Bloqueios</a></li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane active" id="tab1">
                                <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false" OnItemDataBound="Repeater1_ItemDataBound">
                                    <HeaderTemplate>
                                        <table class="table table-condensed-small table-hover">
                                            <tr>
                                                <th style="white-space: nowrap;">Ação
                                                </th>
                                                <th style="white-space: nowrap;">Executada em
                                                </th>
                                                <th style="white-space: nowrap;">Responsável
                                                </th>
                                                <th style="white-space: nowrap;">Perfil
                                                </th>
                                                <th style="white-space: nowrap;">Meta
                                                </th>
                                                <th style="white-space: nowrap;">Realizado em
                                                </th>
                                                <th style="white-space: nowrap; width: 270px;">Observações
                                                </th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="row" runat="server">
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "DescricaoWorkflowAcao") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "DataCadastro") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "PrimeiroNome") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "DescricaoPerfil") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Meta") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Realizado") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Justificativa") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="tab-pane" id="tab2">
                                <asp:Repeater ID="RepeaterBloqueios" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table class="table table-condensed-small table-hover">
                                            <tr>
                                                <th style="white-space: nowrap; width: 130px;">Bloqueado em
                                                </th>
                                                <th style="white-space: nowrap; width: 130px;">Desbloqueado em
                                                </th>
                                                <th style="white-space: nowrap; width: 130px;">Tempo bloqueado
                                                </th>
                                                <th style="white-space: nowrap; width: 150px;">Bloqueado por
                                                </th>
                                                <th style="white-space: nowrap;">Perfil
                                                </th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="row" runat="server">
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "DataCadastro") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "DataDesbloqueio") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "tempo") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "PrimeiroNome") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Descricao") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="tab-pane" id="tab3">
                                <asp:Repeater ID="RepeaterAnexos" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table class="table table-condensed-small table-hover">
                                            <tr>
                                                <th style="white-space: nowrap; width: 300px;">Descrição</th>
                                                <th style="white-space: nowrap;">Arquivo</th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="row" runat="server">
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Description") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <asp:LinkButton runat="server" OnClick="lkVisualizar_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FileGuid") %>'>Visualizar</asp:LinkButton>
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
