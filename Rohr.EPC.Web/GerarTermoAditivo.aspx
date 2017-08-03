<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="GerarTermoAditivo.aspx.cs" Inherits="Rohr.EPC.Web.TermoAditivo" %>

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
                    <asp:Panel runat="server" ID="panelDescricaoOportunidade" Visible="false" ViewStateMode="Disabled" CssClass="menu-left">
                        <div class="control-title">Oportunidade</div>
                        <asp:Label ID="lblOportunidade" runat="server" Text=""></asp:Label>
                        <div class="control-title">Comercial</div>
                        <asp:Label ID="lblComercial" runat="server" Text=""></asp:Label>
                        <div class="control-title">Cliente</div>
                        <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label>
                        <div class="control-title">Obra</div>
                        <asp:Label ID="lblObra" runat="server" Text=""></asp:Label>
                        <div class="control-title">Modelo</div>
                        <asp:Label ID="lblModelo" runat="server" Text="Termo Aditivo"></asp:Label>
                        
                    </asp:Panel>
                    <div class="control-title">
                        Etapas
                    <div class="progress">
                        <div class="bar" style="width: 20%">1 de 5</div>
                    </div>
                    </div>
                </div>
            </div>
            <div class="span9">
                <asp:Panel ID="panelPesquisa" runat="server" Visible="true">
                <p class="muted">
                    Informe o número do Termo Aditivo cadastrado no
                    <img src="Content/Image/logopmweb.jpg" alt="PMWeb" width="80" height="31" />                    
                </p>
                <div>
                    <div class="input-append">
                        <asp:TextBox class="input-small" ID="txtNumeroTA" MaxLength="5" placeholder="Termo Aditivo" runat="server" ViewStateMode="Enabled"></asp:TextBox>
                    </div>
                        
                        <asp:LinkButton ID="linkButtonPesquisar" runat="server" CssClass="btn btn-pesquisa" OnClick="linkButtonPesquisar_Click"><i class="icon-search"></i></asp:LinkButton>
                </div>
                </asp:Panel>
                <div>
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                </div>
                <div class="row">
                    <div class="span9">
                        <asp:Panel ID="panelTermos" runat="server" Visible="false" ViewStateMode="Disabled">
                            <fieldset>
                                <legend>Versões</legend>

                                <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table class="display" style="border-spacing: 20px; border-collapse: separate;">
                                            <thead>                                       
                                                
                                                <tr>
                                                    <th></th>                                                    
                                                    <th style="white-space: nowrap;display:none;">ID
                                                </th>
                                                <th style="white-space: nowrap;">Versão
                                                </th>                                                
                                                <th style="white-space: nowrap;">Descrição
                                                </th>
                                                    <th style="white-space: nowrap;">Tipo
                                                </th>
                                               <th style="white-space: nowrap;">Status
                                                </th>
                                            </tr>
                                                </thead>
                                           
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        
                                        <tr id="row" runat="server">
                               <td><asp:CheckBox ID="chkMarcado" runat="server">
                                   </asp:CheckBox>
                               </td>
                                            <td Style="display:none">
                                                <asp:Label ID="lblIDPesquisa" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>'></asp:Label>
                                                 
                                            </td>

                                            <td id="Cliente">
                                                <asp:Label ID="lblRevisionNumber"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RevisionNumber") %>'></asp:Label>
                                            
                                                 
                                            </td>
                                            <td id="Obra">
                                                <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>                                          
                                            </td>
                                            <td id="Tipo">
                                                <asp:Label ID="LblTipo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tipo") %>'></asp:Label>                                          
                                            </td>                                                    
                                             <td id="Status">
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'></asp:Label>
                                            </td> 
                                        </tr>
                                        
                                    </ItemTemplate>
                                    <FooterTemplate>                                                                            
                                        </table>
                                          <br />
                                            <br />
                               <asp:Button ID="btnCkSalvar" runat="server" Text="Criar Termo" CssClass="btn btn-success" OnClick="ckSalvar_Click" />
                                    </FooterTemplate>
                                </asp:Repeater>
                                
                                
                            </fieldset>
                        </asp:Panel>
                        <asp:Panel ID="panelModelo1" Visible="false" runat="server">
                            <p class="muted">Escolha o modelo do contrato</p>
                            <div class="input-append">
                                <asp:DropDownList ID="ddlModelo" runat="server">
                                </asp:DropDownList>
                                <asp:LinkButton ID="linkButton1" runat="server" CssClass="btn" OnClick="PesquisarModelo_Click"><i class="icon-search"></i></asp:LinkButton>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="panelModelo" runat="server" Visible="false">
                            <fieldset>
                                <legend>Modelo</legend>
                                <table class="table table-bordered">
                                    <tr>
                                        <td>
                                            <asp:Panel runat="server" ID="panelContratoCliente">
                                                Faça o upload do contrato do cliente no formato PDF.<br />
                                                Apenas um arquivo pode ser carregado.<br />
                                                O orçamento feito no PMWeb deve ficar semelhante ao orçamento do cliente.<br />
                                                Ao escolher utilizar o contrato do cliente não será possível alterar para um modelo Rohr.
                                                <div style="padding-top: 25px;">
                                                    <label class="btn" for="contentBody_flContrato">
                                                        <asp:FileUpload runat="server" ID="flContrato" Style="display: none;" onchange='$("#uploadFile").html($(this).val());' />
                                                        Upload do contrato
                                                    </label>
                                                </div>
                                                <div style="padding-top: 25px;">
                                                    Arquivo selecionado:
                                                <span class='' id="uploadFile"></span>
                                                </div>
                                            </asp:Panel>
                                            <CKEditor:CKEditorControl ID="CKEditor" runat="server"></CKEditor:CKEditorControl>
                                        </td>
                                        <td style="width: 150px;">
                                            <strong>Modelo</strong><br />
                                            <asp:Label ID="lblNomeModelo" runat="server" Text=""></asp:Label><br />
                                            <strong>Versão do modelo</strong><br>
                                            <asp:Label ID="lblVersao" runat="server" Text=""></asp:Label><br />
                                            <strong>Modelo criado em</strong><br />
                                            <asp:Label ID="lblCriado" runat="server" Text=""></asp:Label><br />
                                            <br />
                                            <asp:Label ID="lblMensagemDataCriacao" runat="server" ForeColor="Red"></asp:Label><br />
                                            <br />
                                            <br />
                                          <asp:Button ID="btnUtilizarModelo" runat="server" Text="Utilizar modelo" CssClass="btn btn-success" OnClick="btnUtilizarModelo_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
