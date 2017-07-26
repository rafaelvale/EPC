<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Acoes.aspx.cs" Inherits="Rohr.EPC.Web.Acoes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .affix {
            top: 48px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <fieldset>
                    <legend>Documento</legend>
                    <div class="muted" style="float: left; width: 200px;">
                        Ação Principal:
                        <asp:Label runat="server" ID="lblAcao"></asp:Label>
                    </div>
                    <div class="muted">
                        Total de documentos:
                        <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                    </div>
                    <asp:Panel runat="server" ID="panelAcoes">
                        <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false">
                            <HeaderTemplate>
                                <table class="table table-condensed-small table-hover">
                                    <tr>
                                        <th title="Número do Documento - Contrato ou Proposta" class="text-nowrap" style="width: 50px;">Nº Doc.</th>
                                        <th class="text-nowrap" style="width: 240px">Cliente</th>
                                        <th class="text-nowrap" style="width: 240px">Obra</th>
                                        <th class="text-nowrap" title="Desconto Médio" style="width: 100px; text-align: right;">Desc. Médio (%)</th>
                                        <th class="text-nowrap" title="Valor do Negócio" style="width: 100px; text-align: right; padding-right: 20px;">Vl. Negócio (R$)</th>
                                        <th class="text-nowrap" title="Maior/Menor desconto por (item)" style="width: 100px; text-align: right; padding-right: 20px;">Maior/Menor desc.</th>
                                        <th class="text-nowrap" title="Alçada" style="width: 100px">Alçada</th>
                                        <th class="text-nowrap" title="Reprovado por" style="width: 100px">Reprovado por</th>
                                        <th class="text-nowrap" title="Observação da última reprovação">Obs. Últ. Reprovação</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("numeroDocumento") %></td>
                                    <td title="<%#Eval("nomeCliente") %>"><%#Eval("nomeCliente").ToString() %></td>
                                    <td title="<%#Eval("nomeObra") %>"><%#Eval("nomeObra").ToString() %></td>
                                    <td class="text-right"><%#Eval("descontoMedio", "{0:N2}") %></td>
                                    <td class="text-right" style="padding-right: 20px;"><%#Eval("valorNegocio", "{0:N2}") %></td>
                                    <td><%#Eval("maiorMenorDesconto") %></td>
                                    <td><%#Eval("alcada") %></td>
                                    <td class="text-error"><%#Eval("nomeUsuarioUltimaReprovacao") %></td>
                                    <td class="text-error"><%#Eval("observacaoUltimaReprovacao") %></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div style="padding-top: 30px;">
                            <span>Observação </span>
                            <asp:TextBox runat="server" ID="txtObservacao" Width="40%" TextMode="MultiLine" Rows="4" /><br />
                            <span style="padding-right: 65px;"></span>
                            <span id="chars">500 caracteres restantes</span>
                        </div>
                    </asp:Panel>
                </fieldset>
            </div>
        </div>
        <asp:Label ID="lblMensagemErro" runat="server" />
        <div class="row-fluid">
            <div class="span12">
                <div class="form-actions">
                    <asp:Button ID="btnAcaoPrimaria" runat="server" Text="Continuar" CssClass="btn" OnClick="btnAcaoPrimaria_Click" UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                    <asp:Button ID="btnAcaoSecundaria" runat="server" Text="Desistir" CssClass="btn btn-danger" OnClick="btnAcaoSecundaria_Click" UseSubmitBehavior="false" OnClientClick="ClientClickDisable(this);" />
                    <img src="Content/Image/301.gif" id="imgLoad" style="display: none" />
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $("#contentBody_txtObservacao").each(function () {
                $(this).attr('maxlength', 500);
            });

            var maxLength = 500;
            $('#contentBody_txtObservacao').keyup(function () {
                var length = $(this).val().length;
                var length = maxLength - length;

                if (length <= 0)
                    $('#chars').text('Limite da justificativa atingido.');
                else if (length == 1)
                    $('#chars').text('1 caracter restante.');
                else
                    $('#chars').text(length + ' caracteres restantes.');
            });
        });
    </script>
</asp:Content>
