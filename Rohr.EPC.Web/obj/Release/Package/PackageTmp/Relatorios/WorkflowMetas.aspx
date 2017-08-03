<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="WorkflowMetas.aspx.cs" Inherits="Rohr.EPC.Web.Relatorios.WorkflowMetas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .affix {
            top: 48px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
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
            width: 1500px;
        }

        .progtrckr-contrato {
            width: 4200px;
        }

        ol.progtrckr li {
            display: inline-block;
            text-align: center;
            line-height: 4em;
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

        .scrollsNoAnimation {
            overflow-x: scroll;
            overflow-y: hidden;
            height: 100px;
        }

        .workflow {
            width: 850px;
        }
    </style>
    <div class="container">
        <div class="row">
            <div class="span1 hidden-tablet">
            </div>
            <div class="span9">
                <div>
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span11">
                        <p class="muted">
                            Aqui você pode visualizar o fluxo e alçadas das propostas e contratos
                        </p>
                        <div class="tabbable">
                            <ul class="nav nav-tabs">
                                <li class="active"><a href="#tab1" data-toggle="tab">Ações Proposta</a></li>
                                <li><a href="#tab2" data-toggle="tab">Ações Contrato</a></li>
                                <li><a href="#tab3" data-toggle="tab">Ações Proposta PAT</a></li>
                                <li><a href="#tab4" data-toggle="tab">Ações Contrato PAT</a></li>
                                <li><a href="#tab5" data-toggle="tab">Modelos</a></li>
                                <li><a href="#tab6" data-toggle="tab">Modelos Condições Gerais</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="tab1">
                                    <div class="workflow">
                                        <div class="scrollsNoAnimation">
                                            <ol class='progtrckr progtrckr-proposta'>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>1 - Finalizar minuta proposta</li>
                                                <li class='progtrckr-done' title='Responsável: Gerente da filial'>2 - Análise do gerente da filial</li>
                                                <li class='progtrckr-done' title='Responsável: Superintendência'>3 - Superintendência</li>
                                                <li class='progtrckr-done' title='Responsável: Diretoria Operacional'>4 - Diretoria Operacional</li>
                                                <li class='progtrckr-done' title='Responsável: Vice-Presidência'>5 - Vice-Presidência</li>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>6 - Enviar para análise / assinatura do cliente</li>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>7 - Receber proposta do cliente</li>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>8 - Elaborar contrato</li>
                                            </ol>
                                        </div>
                                    </div>
                                    <table class="table table-condensed-small table-hover">
                                        <tr>
                                            <th>Ação
                                            </th>
                                            <th>Meta
                                            </th>
                                            <th>Responsável</th>
                                            <th style="width: 400px;">Descrição
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>0 - Inicio da elaboração da proposta</td>
                                            <td>-</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>1 - Finalizar minuta proposta</td>
                                            <td>1 hora</td>
                                            <td>Comercial</td>
                                            <td>Itens com desconto de até 9,99%, estão dentro da alçada do Comercial.</td>
                                        </tr>
                                        <tr>
                                            <td>2 - Análise do gerente da filial</td>
                                            <td>1 hora</td>
                                            <td>Gerente da filial</td>
                                            <td>A análise do gerente da filial é necessária, caso algum item do orçamento esteja com desconto acima 9,99%.<br />
                                                Ou Faturamento mensal menor que R$ 1.000,00.<br />
                                                Ou Contrato com duração menor que 30 dias.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>3 - Análise da Superintendência</td>
                                            <td>1 hora
                                            </td>
                                            <td>Superintendência</td>
                                            <td>A análise da Superintendência é necessária, caso algum item do orçamento esteja com desconto acima 19,99%<br />
                                                Ou Faturamento mensal menor que R$ 1.000,00.<br />
                                                Ou Contrato com duração menor que 30 dias.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>4 - Análise da Diretoria Operacional</td>
                                            <td>1 hora
                                            </td>
                                            <td>Diretoria Operacional</td>
                                            <td>A análise da Diretoria Operacional é necessária, caso algum item do orçamento esteja com desconto acima 29,99%<br />
                                                Ou Faturamento mensal menor que R$ 1.000,00.<br />
                                                Ou Contrato com duração menor que 30 dias.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>5 - Análise da Vice-Presidência</td>
                                            <td>1 hora
                                            </td>
                                            <td>Vice-Presidência</td>
                                            <td>A análise da Vice-Presidência é necessária, caso algum item do orçamento esteja com desconto acima 29,99%<br />
                                                Ou Faturamento mensal menor que R$ 1.000,00.<br />
                                                Ou Contrato com duração menor que 30 dias.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>6 - Enviar para Análise / assinatura do cliente</td>
                                            <td>1 hora</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>7 - Receber proposta do cliente</td>
                                            <td>7 dias</td>
                                            <td>Comercial</td>
                                            <td>Somente após o recebimento da proposta com o aceite do cliente é liberado a confecção do contrato.</td>
                                        </tr>
                                        <tr>
                                            <td>6 - Elaborar contrato</td>
                                            <td>1 hora</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="tab-pane" id="tab2">
                                    <div class="workflow">
                                        <div class="scrollsNoAnimation">
                                            <ol class='progtrckr progtrckr-contrato'>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>1 - Finalizar minuta contratual</li>
                                                <li class='progtrckr-done' title='Responsável: Gerente da filial'>2 - Análise do gerenteda filial</li>
                                                <li class='progtrckr-done' title='Responsável: Superintendência'>3 - Análise da Superintendência</li>
                                                <li class='progtrckr-done' title='Responsável: Diretoria Operacional'>4 - Análise da Diretoria Operacional</li>
                                                <li class='progtrckr-done' title='Responsável: Vice-Presidência'>5 - Análise da Vice-Presidência</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>6 - Análise do Jurídico</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>7 - Enviar contrato para a filial</li>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>8 - Receber contrato do Jurídico</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>9 - Enviar contrato para assinatura do cliente</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>10 - Receber contrato assinado do cliente</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>11 - Enviar contrato assinado para o Jurídico</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>12 - Receber contrato assinado da filial</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>13 - Legitimar contrato recebido da filial</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>14 - Assinatura da Diretoria</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>15 - Arquivar contrato (via Rohr)</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>16 - Enviar contrato para a filial (via Cliente)</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>17 - Receber contrato do Jurídico (via Cliente)</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>18 - Enviar contrato para cliente (via Cliente)</li>
                                                <li class='progtrckr-done'>19 - Concluído</li>
                                            </ol>
                                        </div>
                                    </div>
                                    <table class="table table-condensed-small table-hover">
                                        <tr>
                                            <th>Ação
                                            </th>
                                            <th>Meta
                                            </th>
                                            <th>Responsável</th>
                                            <th style="width: 400px;">Descrição
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>0 - Inicio da elaboração do contrato</td>
                                            <td>-</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>1 - Finalizar minuta contrato</td>
                                            <td>1 dia</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>2 - Análise do gerente da filial</td>
                                            <td>1 hora</td>
                                            <td>Gerente da filial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>3 - Análise da Superintendência</td>
                                            <td>1 hora</td>
                                            <td>Superintendência</td>
                                            <td>A análise da superintendência é necessária, caso algum item do
                                                orçamento esteja com desconto acima 19,99%<br />
                                                Ou Faturamento mensal menor que R$ 1.000,00.<br />
                                                Ou Contrato com duração menor que 30 dias.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>4 - Análise da Diretoria Operacional</td>
                                            <td>1 hora</td>
                                            <td>Diretoria Operacional</td>
                                            <td>A análise da Diretoria Operacional é necessária, caso algum item do
                                                orçamento esteja com desconto acima 29,99%<br />
                                                Ou Faturamento mensal menor que R$ 1.000,00.<br />
                                                Ou Contrato com duração menor que 30 dias.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>5 - Análise da Vice-Presidência</td>
                                            <td>1 hora</td>
                                            <td>Vice-Presidência</td>
                                            <td>A análise da Vice-Presidência é necessária, caso algum item do
                                                orçamento esteja com desconto acima 29,99%<br />
                                                Ou Faturamento mensal menor que R$ 1.000,00.<br />
                                                Ou Contrato com duração menor que 30 dias.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>6 - Análise do jurídico</td>
                                            <td>1 dia</td>
                                            <td>Jurídico</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>7 - Enviar contrato para a filial</td>
                                            <td>1 dia</td>
                                            <td>Jurídico</td>
                                            <td>Enviar contrato para a filial, para assinatura do cliente (2 vias)</td>
                                        </tr>
                                        <tr>
                                            <td>8 - Receber contrato do jurídico</td>
                                            <td>2 dias</td>
                                            <td>Comercial</td>
                                            <td>Receber contrato do jurídico, para assinatura do cliente (2 vias)</td>
                                        </tr>
                                        <tr>
                                            <td>9 - Enviar contrato para assinatura do cliente</td>
                                            <td>2 dias</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>10 - Receber contrato assinado do cliente</td>
                                            <td>10 dia</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>11 - Enviar contrato assinado para o jurídico</td>
                                            <td>1 dias</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>12 - Receber contrato assinado da filial</td>
                                            <td>1 dias</td>
                                            <td>Jurídico</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>13 - Legitimar contrato recebido da filial</td>
                                            <td>1 dias</td>
                                            <td>Jurídico</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>14 - Assinatura da Diretoria</td>
                                            <td>1 dia</td>
                                            <td>Diretoria</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>15 - Arquivar contrato (via Rohr)</td>
                                            <td>1 dia</td>
                                            <td>Jurídico</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>16 - Enviar contrato para a filial (via Cliente)</td>
                                            <td>1 dia</td>
                                            <td>Jurídico</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>17 - Receber contrato do Jurídico (via Cliente)</td>
                                            <td>1 dia</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>18 - Enviar contrato para cliente (via Cliente)</td>
                                            <td>1 dia</td>
                                            <td>Comercial</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>19 - Concluído</td>
                                            <td>-</td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="tab-pane" id="tab3">
                                    <div class="workflow">
                                        <div class="scrollsNoAnimation">
                                            <ol class='progtrckr progtrckr-proposta'>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>1 - Finalizar minuta proposta</li>
                                                <li class='progtrckr-done' title='Responsável: Supervisor PTA'>2 - Análise do Supervisor PTA</li>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>3 - Enviar para análise / assinatura
                                                    do cliente</li>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>4 - Receber proposta do cliente</li>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>5 - Elaborar contrato</li>
                                            </ol>
                                        </div>
                                    </div>
                                    <table class="table table-condensed-small table-hover">
                                        <tr>
                                            <th>Ação
                                            </th>
                                            <th>Meta
                                            </th>
                                            <th>Responsável</th>
                                            <th style="width: 450px;">Descrição
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>0 - Inicio da elaboração da proposta
                                            </td>
                                            <td>-
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>1 - Finalizar minuta proposta
                                            </td>
                                            <td>1 hora
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>2 - Análise do Supervisor PTA
                                            </td>
                                            <td>1 hora
                                            </td>
                                            <td>Supervisor PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>4 - Enviar para Análise / assinatura do cliente
                                            </td>
                                            <td>1 hora
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>5 - Receber proposta do cliente
                                            </td>
                                            <td>7 dias
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>6 - Elaborar contrato
                                            </td>
                                            <td>1 hora
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="tab-pane" id="tab4">
                                    <div class="workflow">
                                        <div class="scrollsNoAnimation">
                                            <ol class='progtrckr progtrckr-contrato'>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>1 - Finalizar minuta contratual</li>
                                                <li class='progtrckr-done' title='Responsável: Supervisor PTA'>2 - Análise do Supervisor PTA</li>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>3 - Enviar contrato para assinatura
                                                    do cliente</li>
                                                <li class='progtrckr-done' title='Responsável: Comercial'>4 - Receber contrato assinado
                                                    do cliente</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>5 - Enviar contrato assinado
                                                    para o jurídico</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>6 - Receber contrato assinado
                                                    da filial</li>
                                                <li class='progtrckr-done' title='Responsável: Jurídico'>7 - Arquivar contrato</li>
                                                <li class='progtrckr-done'>8 - Concluído</li>
                                            </ol>
                                        </div>
                                    </div>
                                    <table class="table table-condensed-small table-hover">
                                        <tr>
                                            <th>Ação
                                            </th>
                                            <th>Meta
                                            </th>
                                            <th>Responsável</th>
                                            <th style="width: 450px;">Descrição
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>0 - Inicio da elaboração do contrato
                                            </td>
                                            <td>-
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>1 - Finalizar minuta contrato
                                            </td>
                                            <td>1 dia
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>2 - Análise do Supervisor PTA
                                            </td>
                                            <td>1 hora
                                            </td>
                                            <td>Supervisor PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>3 - Enviar contrato para assinatura do cliente
                                            </td>
                                            <td>1 dia
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>4 - Receber contrato assinado do cliente
                                            </td>
                                            <td>10 dias
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>5 - Enviar contrato assinado para o jurídico
                                            </td>
                                            <td>2 dias
                                            </td>
                                            <td>Comercial PTA</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>6 - Receber contrato assinado da filial
                                            </td>
                                            <td>2 dias
                                            </td>
                                            <td>Jurídico</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>7 - Arquivar contrato
                                            </td>
                                            <td>1 dia
                                            </td>
                                            <td>Jurídico</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>8 - Concluído
                                            </td>
                                            <td>-
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="tab-pane" id="tab5">
                                    <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false" OnItemDataBound="Repeater1_ItemDataBound">
                                        <HeaderTemplate>
                                            <table class="table table-condensed-small table-striped">
                                                <tr>
                                                    <th>Documento</th>
                                                    <th>Criado em</th>
                                                    <th>Versão</th>
                                                    <th>Meta</th>
                                                    <th>Condições Gerais Obrigatório</th>
                                                    <th>Modelo</th>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="row" runat="server">
                                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "titulo") %></td>
                                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "dataCadastro") %></td>
                                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "versao") %></td>
                                                <td class="text-nowrap">
                                                    <asp:Label runat="server" ID="lblMeta" /></td>
                                                <td class="text-nowrap"><%# Boolean.Parse(DataBinder.Eval(Container.DataItem, "condicoesGeraisObrigatorio").ToString()) ? "Sim" : "Não"%></td>
                                                <td class="text-nowrap">
                                                    <asp:LinkButton ID="LinkButtonModelo" runat="server" OnClick="LinkButton1_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "idModelo") %>'>Visualizar</asp:LinkButton></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="tab-pane" id="tab6">
                                    <asp:Repeater ID="Repeater2" runat="server" EnableViewState="false">
                                        <HeaderTemplate>
                                            <table class="table table-condensed-small table-striped">
                                                <tr>
                                                    <th>Documento</th>
                                                    <th>Versão</th>
                                                    <th>Data</th>
                                                    <th>Modelo</th>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="row" runat="server">
                                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "nome") %></td>
                                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "versao") %></td>
                                                <td class="text-nowrap"><%# DataBinder.Eval(Container.DataItem, "dataCadastro") %></td>
                                                <td class="text-nowrap">
                                                    <asp:LinkButton ID="LinkButtonModeloCondicoesGerais" runat="server" OnClick="LinkButtonModeloCondicoesGerais_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdModeloCondicoesGerais") %>'>Visualizar</asp:LinkButton></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
