<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="Cockpit.aspx.cs" Inherits="Rohr.EPC.Web.Cockpit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container-fluid" style="display: none;">
        <div class="row-fluid">
            <div class="span12">
                <div class="input-append">
                    <asp:TextBox ID="txtPesquisar" runat="server" class="input-xxlarge" placeholder="Pesquisar por número do documento, cliente ou obra" onkeypress="submitPesquisa();" autofocus/>
                    <asp:LinkButton ID="linkButtonPesquisar" runat="server" CssClass="btn" onclick="linkButtonPesquisar_Click"><i class="icon-search"></i></asp:LinkButton>
                </div>
            </div>
            
               
        </div>
        <DIV style="font-weight:bold;">ESTA PÁGINA IRÁ ATUALIZAR EM: <a id="sessao"></a>
        <IMG src="Content/Image/clock_16x16.png" /></DIV>
                     
        <div class="row-fluid">
            <div class="span12">
                <div style="display: block; padding: 0 0 2px 0">
                    <div style="float: left; display: inline-block; vertical-align: bottom; padding: 5px 8px 0 0;">
                        
                        <span id="totalDocSelecionados"></span>
                    </div>
                    <div style="text-align: right;">
                        <div style="text-align: left; display: inline-block;">
                            <span id="totalDocumentos" style="padding: 5px 8px 0 0"></span>
                            <asp:Label Text="" runat="server" ID="lblTotalDocumentos" Style="padding: 5px 8px 0 0; font-weight: bold;" />
                        </div>
                        <div style="text-align: left; display: inline-block; padding: 5px 8px 0 0">
                            <div class="btn-group">
                                <asp:Button Text="<" ToolTip="Próximos" runat="server" ID="btnAnterior" CssClass="btn" OnClick="btnAnterior_Click" />
                                <asp:Button Text=">" ToolTip="Anteriores" runat="server" ID="btnProximo" CssClass="btn" OnClick="btnProximo_Click" />
                            </div>
                        </div>

                        <div style="text-align: left; padding-left: 5px; display: inline-block;">
                            <div class="input-append" style="margin-bottom: 0;">
                                <div class="btn-group">
                                    <button class="btn dropdown-toggle" data-toggle="dropdown">Marcadores <span class="caret"></span></button>
                                    <ul class="dropdown-menu">
                                        <li>
                                            <asp:LinkButton runat="server" ID="lbMarcarBloqueio" OnClick="lbMarcarBloqueio_Click">Marcar como bloqueado</asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton runat="server" ID="lblRemoverBloqueio" OnClick="lblRemoverBloqueio_Click">Remover bloqueio</asp:LinkButton></li>
                                    </ul>
                                </div>
                            </div>
                        </div>

                        <div style="text-align: left; padding-left: 5px; display: inline-block;">
                            <div class="input-append" style="margin-bottom: 0;">
                                <div class="btn-group">
                                    <button class="btn dropdown-toggle" data-toggle="dropdown" runat="server" id="btnAcaoSerExecutada">Ação a ser executada<span class="caret"></span></button>
                                    <ul class="dropdown-menu pull-right">
                                        <li><asp:LinkButton runat="server" ID="lkb1" OnClick="lkb_Click" CommandArgument="0">Todas</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lkb2" OnClick="lkb_Click" CommandArgument="1">Análise do Gerente da Filial</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lkb3" OnClick="lkb_Click" CommandArgument="2">Análise da Superintendência</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lkb4" OnClick="lkb_Click" CommandArgument="55">Análise da Diretoria Operacional</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lkb5" OnClick="lkb_Click" CommandArgument="56">Análise da Vice-Presidência</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lkb6" OnClick="lkb_Click" CommandArgument="6">Análise do Jurídico</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lkb11" OnClick="lkb_Click" CommandArgument="10">Enviar contrato para assinatura do Cliente</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lkb7" OnClick="lkb_Click" CommandArgument="3">Enviar para análise / assinatura do Cliente</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="LinkButton1" OnClick="lkb_Click" CommandArgument="68">Enviar contrato assinado para o Jurídico</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lkb9" OnClick="lkb_Click" CommandArgument="78">Receber contrato do Jurídico (via Cliente)</asp:LinkButton></
                                        <li><asp:LinkButton runat="server" ID="lbk12" OnClick="lkb_Click" CommandArgument="11">Receber contrato assinado do Cliente</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lkb8" OnClick="lkb_Click" CommandArgument="4">Receber proposta do Cliente</asp:LinkButton></li>
                                        <li><asp:LinkButton runat="server" ID="lbk13" OnClick="lkb_Click" CommandArgument="35">Contrato enviado para assinatura do Cliente</asp:LinkButton></li>
                                    </ul>
                                </div>
                            </div>
                        </div>

                        <div style="text-align: left; display: inline-block;">
                            <asp:Button ID="btnExibicao" runat="server" Text="Exibir todos" CssClass="btn" OnClick="btnExibicao_Click" />
                        </div>
                    </div>
                </div>
                <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound"
                    EnableViewState="false">
                    <HeaderTemplate>
                        <table class="table table-condensed-small table-hover" id="tableDocumentos">
                            <thead>
                                <tr>
                                    <th style="width: 12px;">
                                        <input type="checkbox" id="checkBoxAll"></th>
                                    <th style="width: 1px;"></th>
                                    <th title="Número do Documento - Contrato ou Proposta" class="text-nowrap" style="width: 50px;">Nº Doc.</th>
                                    <th class="text-nowrap" title="P=Proposta / C=Contrato" style="width: 30px;">Tipo</th>
                                    <th title="Revisão do Cliente" class="text-nowrap hidden-tablet" style="width: 32px;">Rev.</th>
                                    <th title="Versão da Rohr" class="text-nowrap hidden-tablet" style="width: 32px;">Ver.</th>
                                    <th class="text-nowrap" style="width: 150px;">Cliente</th>
                                    <th class="text-nowrap" style="width: 140px;">Obra</th>
                                    <th class="text-nowrap hidden-tablet" style="width: 55px;">Filial</th>
                                    <th title="Desconto(-)/Acréscimo(+) Médio" class="text-nowrap" style="width: 55px;">Desc./Acr. M.(%)</th>
                                    <th title="Valor do Negócio" class="text-nowrap" style="width: 55px;">Vl. Negócio (R$)</th>
                                    <th title="Colaborador responsável pela criação do documento" class="text-nowrap hidden-tablet" style="width: 65px;">Criado por</th>
                                    <th class="text-nowrap hidden-tablet" style="width: 160px;"><i class=" icon-arrow-down"></i>Pendente há</th>
                                    <th class="text-nowrap">Ação a ser executada</th>
                                </tr>
                            </thead>
                            <tbody id="fbody">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ckDocumento" runat="server" />
                                <asp:HiddenField ID="hdIdDocumento" runat="server" />
                            </td>
                            <td class="text-nowrap">
                                <asp:Label runat="server" ID="lblNumeroDocumento" />
                            </td>
                            <td class="text-nowrap">
                                <%# DataBinder.Eval(Container.DataItem, "numeroDocumento", "{0:N0}") %>
                            </td>
                            <td class="text-nowrap" title='<%# Boolean.Parse(DataBinder.Eval(Container.DataItem, "eProposta").ToString()) ? "Proposta" : "Contrato" %>'>
                                <%# Boolean.Parse(DataBinder.Eval(Container.DataItem, "eProposta").ToString()) ? "P" : "C" %>
                            </td>
                            <td class="text-nowrap hidden-tablet">
                                <%# DataBinder.Eval(Container.DataItem, "revisaoCliente") %>
                            </td>
                            <td class="text-nowrap hidden-tablet">
                                <%# DataBinder.Eval(Container.DataItem, "versaoInterna") %>
                            </td>
                            <td class="text-nowrap">
                                <asp:Label runat="server" ID="lblNomeCliente" />
                            </td>
                            <td class="text-nowrap">
                                <asp:Label runat="server" ID="lblNomeObra" />
                            </td>
                            <td class="text-nowrap hidden-tablet">
                                <%# DataBinder.Eval(Container.DataItem, "filial") %>
                            </td>
                            <td class="text-nowrap text-right">
                                <%# DataBinder.Eval(Container.DataItem, "percentualDesconto", "{0:N2}") %>
                            </td>
                            <td class="text-nowrap text-right">
                                <%# DataBinder.Eval(Container.DataItem, "ValorNegocio", "{0:N2}") %>
                            </td>
                            <td class="text-nowrap hidden-tablet">
                                <%# DataBinder.Eval(Container.DataItem, "primeiroNome") %>
                            </td>
                            <td class="text-nowrap hidden-tablet">
                                <asp:Image runat="server" ID="imgPendente" Height="16" Width="16" /><asp:Label runat="server"
                                    ID="lblPendente" />
                            </td>
                            <td class="text-nowrap">
                                <%# DataBinder.Eval(Container.DataItem, "descricaoAcao") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <div id="modal-content" class="modal fade" data-backdrop="static" style="display: none;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Alterações</h4>
                </div>
                <div class="modal-body">
                    <p>
                        Olá <b><% Response.Write(new Rohr.EPC.Business.Util().GetSessaoUsuario().PrimeiroNome.ToString()); %></b>,<br />
                        Algumas alterações foram feitas no EPC!
                        <br /><br />
                    </p>
                    <p>
                        1º Layout de Propostas e Contratos alterados para novo modelo que forem confeccionados a partir de 26/05/17.<br /><br />
                        2º Foi inserida uma nova tela na geração de propostas, onde deverá informado todo o resumo da proposta. Este Resumo aparecerá no documento gerado para a proposta do cliente.<br /><br />
                        3º Na ultima parte da geração de propostas/contratos, existe a opção de enviar junto ao documento a História da Rohr, onde somente aparecerá em propostas feitas a partir da data informada acima.<br /><br />
                        <%--1º Em todos os documentos o prazo mínimo que a carga/descarga deverá ser programa passa a ser de <b>3 dias úteis</b>.<br /><br />--%>
                        <%--2º Propostas finalizadas como sem negócio no PMWeb, serão encerradas automaticamente no EPC independente da situação no fluxo de aprovações. Esses documentos ficaram disponíveis apenas para consulta.<br /><br />--%>
                    </p>
                    <div class="modal-footer">
                        <a class="btn" data-dismiss="modal" aria-hidden="true">Entendido</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>

    <script type="text/javascript">

var tempo = new Number();
// Tempo em segundos
tempo = 120;

function startCountdown(){

	// Se o tempo não for zerado
	if((tempo - 1) >= 0){

		// Pega a parte inteira dos minutos
		var min = parseInt(tempo/60);
		// Calcula os segundos restantes
		var seg = tempo%60;

		// Formata o número menor que dez, ex: 08, 07, ...
		if(min < 10){
			min = "0"+min;
			min = min.substr(0, 2);
		}
		if(seg <=9){
			seg = "0"+seg;
		}

		// Cria a variável para formatar no estilo hora/cronômetro
		horaImprimivel = '00:' + min + ':' + seg;
		//JQuery pra setar o valor
		$("#sessao").html(horaImprimivel);

		// Define que a função será executada novamente em 1000ms = 1 segundo
		setTimeout('startCountdown()',1000);

		// diminui o tempo
		tempo--;

	// Quando o contador chegar a zero faz esta ação
	} else {
        window.location='<%= ResolveUrl("~/Cockpit.aspx") %>'
	}

}

// Chama a função ao carregar a tela
startCountdown();

</script>

    <script >
        $(document).ready(function () {
            $('#editarNotificacao').click(function (e) {
                window.open("EditarNotificacoes.aspx");
            });
        });

    </script>
    
    <style>
        tbody tr {
            cursor: default;
        }
    </style>
</asp:Content>
