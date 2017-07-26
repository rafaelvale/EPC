<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Finalizar.aspx.cs" Inherits="Rohr.EPC.Web.Finalizar" %>

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
                    <div runat="server" id="divContrato" class="control-title" visible="false">Contrato</div>
                    <asp:Label ID="lblContrato" runat="server" Visible="false"></asp:Label>
                    <div class="control-title">Oportunidade</div>
                    <asp:Label ID="lblOportunidade" runat="server" Text=""></asp:Label>
                    <div class="control-title">Comercial</div>
                    <asp:Label ID="lblComercial" runat="server" Text=""></asp:Label>
                    <div class="control-title">Cliente</div>
                    <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label>
                    <div class="control-title">Obra</div>
                    <asp:Label ID="lblObra" runat="server" Text=""></asp:Label>
                    <div class="control-title">Modelo</div>
                    <asp:Label ID="lblModelo" runat="server" Text=""></asp:Label>
                    <div class="control-title">
                        Etapas
                    <div class="progress">
                        <div class="bar" style="width: 100%">6 de 6</div>
                    </div>
                    </div>
                </div>
            </div>
            <div class="span9">
                <p class="muted">
                    Valida as informações e conclua a confecção do seu documento.
                </p>
                <div>
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span9">
                        <fieldset>
                            <legend>Informações Complementares</legend>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Desc.(-) / Acrésc.(+) médio concedido</td>
                                        <td>
                                            <div class="input-append">
                                                <asp:TextBox runat="server" ID="txtDescontoConcedido" CssClass="input-small"></asp:TextBox>
                                                <span class="add-on">%</span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Maior desconto concedido (item)</td>
                                        <td>
                                            <div class="input-append">
                                                <asp:TextBox runat="server" ID="txtMaiorDesconto" CssClass="input-small"></asp:TextBox>
                                                <span class="add-on">%</span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Menor desconto concedido (item)</td>
                                        <td>
                                            <div class="input-append">
                                                <asp:TextBox runat="server" ID="txtMenorDesconto" CssClass="input-small"></asp:TextBox>
                                                <span class="add-on">%</span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Valor do Faturamento Mensal</td>
                                        <td>
                                            <div class="input-prepend">
                                                <span class="add-on">R$</span>
                                                <asp:TextBox runat="server" ID="txtValorFaturamentoMensal" CssClass="input-small"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Valor do Negócio</td>
                                        <td>
                                            <div class="input-prepend">
                                                <span class="add-on">R$</span>
                                                <asp:TextBox runat="server" ID="txtValorNegocio" CssClass="input-small"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Data para exibição no documento</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDataExibicao" CssClass="input-small"></asp:TextBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </fieldset>
                        <br />
                        <table style="width: 65%;">

                            <tr>
                                <td style="font-weight: bold; font-size: 22px;">* Incluir História da Rohr
                                </td>
                                <td>
                                    <label class="radio inline">
                                        <asp:RadioButton ID="rbHistoriaSim" Text="Sim" runat="server" Checked="true" GroupName="historiaRohr" />
                                    </label>
                                    <label class="radio inline">
                                        <asp:RadioButton ID="rbHistoriaNao" Text="Não" runat="server" Checked="false" GroupName="historiaRohr" />
                                    </label>
                                </td>

                            </tr>
                            <tr>
                                <td colspan="2">* A História da Rohr só será inclusa nas propostas comerciais.
                                </td>
                            </tr>

                        </table>

                        <fieldset>
                            <legend>Planilha Orçamentaria
                            </legend>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lkPlanilhaOrcamentaria" runat="server" OnClick="LinkButton1_Click">Visualizar Planilha Orçamentaria</asp:LinkButton>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </fieldset>
                        <br />
                        <fieldset>
                            <legend>Notificações
                            </legend>
                            <table style="width: 100%">
                                <tr>
                                    <td>Enviar e-mail com a minuta para o comercial responsável : 
                                    </td>
                                    <td>
                                        <label class="radio inline">
                                            <asp:RadioButton ID="rbEmailSim1" Text="Sim" runat="server" Checked="true" GroupName="emailComercial" />
                                        </label>
                                        <label class="radio inline">
                                            <asp:RadioButton ID="rbEmailNao" Text="Não" runat="server" Checked="false" GroupName="emailComercial" />
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEmailComercial" Text="" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <CKEditor:CKEditorControl ID="CKEditorMensagemEmail" runat="server" Height="320px"></CKEditor:CKEditorControl>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br />
                        <fieldset>
                            <legend>Observações
                            </legend>
                            <table style="width: 100%">
                                <tr>
                                    <td>Se necessário informe uma justificativa</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtJustificativa" runat="server" CssClass="input-xxlarge" MaxLength="140"></asp:TextBox></td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>
                <br />
                <asp:Label ID="lblMensagemAlcada" runat="server"></asp:Label>
                <div class="form-actions">
                    <asp:Button ID="btnContinuar" runat="server" Text="Gerar Proposta" CssClass="btn" OnClick="btnContinuar_Click" UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                    <asp:Button ID="btnDesistir" runat="server" Text="Desistir" CssClass="btn btn-danger" OnClick="btnDesistir_Click" UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                    <img src="Content/Image/301.gif" id="imgLoad" height="28" width="28" style="display: none" alt="Aguarda" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
