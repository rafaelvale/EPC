<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Objeto.aspx.cs" Inherits="Rohr.EPC.Web.Objeto" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .affix {
            top: 48px;
        }
    </style>
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
                    <div class="control-title">
                        Etapas
                        <div class="progress">
                            <div class="bar" style="width: 60%">
                                3 de 6
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span9">
                <p class="muted" style="float: left;">
                    Selecione os itens e as colunas a serem exibidas no documento
                </p>
                <span class="reload">
                    <img src="Content/Image/Reload.png" onclick="reloadPage()" />
                </span>
                <div style="clear: left">
                    <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span12">
                        <div id="fixedTopHeader" style="background-color: #ffffff;">
                            <fieldset>
                                <legend style="margin-bottom: 5px;">Objeto
                                <asp:LinkButton ID="lkPlanilhaOrcamentaria" runat="server" OnClick="lkPlanilhaOrcamentaria_Click"
                                    Style="font-size: 13px;">Visualizar Planilha Orçamentaria</asp:LinkButton></legend>
                            </fieldset>
                            <div runat="server" id="tableColunas">
                                <div style="padding-bottom: 2px;">
                                    <asp:Label runat="server" ID="lblDescricao"></asp:Label>
                                    <div style="float: right;">
                                        PMWeb: Orçamento
                                    <asp:Label ID="lblOrcamento" runat="server" />
                                        - Revisão
                                    <asp:Label ID="lblRevisao" runat="server" />
                                        , gerado em
                                    <asp:Label ID="lblDataRevisao" runat="server" />
                                        <asp:Label ID="lblIdOrcamento" runat="server" Visible="False" />
                                    </div>
                                </div>
                                <table class="table table-condensed-small table-hover table-bordered" runat="server"
                                    id="tabelaColunas" style="margin-bottom: 8px;">
                                    <tr>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c02"/>Resumido</label>
                                        </th>
                                        <th class="text-nowrap" style="width: 178px;">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c03" checked disabled />Descrição</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c04" checked />Quant.</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c05" checked />UM</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c06" />Peso</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c07"/>V.U.L</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c08" checked />V.U.L Prat</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c09"/>V.U.I</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c10" />V.U.I Prat</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c11" />Desc. (%)</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c12" checked />Total</label>
                                        </th>
                                        <th class="text-nowrap">
                                            <label class="checkbox">
                                                <input type="checkbox" runat="server" id="c13"/>Peso Total</label>
                                        </th>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="panelObjeto">
                        </asp:Panel>
                        <br />
                        <div runat="server" id="demaisOpcoes">
                            <span>Selecione os itens a serem exibidos por objeto</span>
                            <table class="table table-condensed-small table-hover">
                                <tr>
                                    <td class="text-nowrap" style="width: 180px;">
                                        <label class="checkbox">
                                            <asp:CheckBox ID="ckSubtotalNegocio" runat="server" Text="Subtotal do negócio"/></label>
                                    </td>
                                    <td class="text-nowrap" style="width: 250px;">
                                        <label class="checkbox">
                                            <asp:CheckBox ID="ckSubtotalFaturamentoMensal" runat="server" Text="Subtotal da locação mensal"
                                                Checked="true" /></label>
                                    </td>
                                    <td class="text-nowrap" style="width: 200px;">
                                        <label class="checkbox">
                                            <asp:CheckBox ID="ckPrevisaoUtilizacao" runat="server" Text="Previsão de utilização"
                                                Checked="false" /></label>
                                    </td>
                                    <td class="text-nowrap" style="width: 200px;">
                                        <label class="checkbox">
                                            <asp:CheckBox ID="ckSubTotalPeso" runat="server" Text="Subtotal do peso"
                                                Checked="true" /></label>
                                    </td>
                                </tr>
                            </table>
                            <span>Selecione os totais a serem exibidos no final (total geral)</span>
                            <table class="table table-condensed-small table-hover">
                                <tr>
                                    <td class="text-nowrap" style="width: 300px;">
                                        <label class="checkbox">
                                            <asp:CheckBox ID="ckValorTotalNegocio" runat="server" Text="Valor total do Negócio (R$ 0,00)"
                                                Checked="false" /></label>
                                    </td>
                                    <td class="text-nowrap" style="width: 300px;">
                                        <label class="checkbox">
                                            <asp:CheckBox ID="ckValorTotalFaturamentoMensal" runat="server" Text="Valor total da Locação Mensal (R$ 0,00)"
                                                Checked="true" /></label>
                                    </td>
                                    <td class="text-nowrap" style="width: 300px;"></td>
                                </tr>
                            </table>
                            <span>Se necessário informe as observações para serem exibidas abaixo do objeto</span>
                            <table class="table">
                                <tr>
                                    <td class="text-nowrap">
                                        <CKEditor:CKEditorControl ID="CKEditorObservacoesObjeto" runat="server" Height="150px" Width="466px"></CKEditor:CKEditorControl>

                                   </td>
                                </tr>
                            </table>
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
