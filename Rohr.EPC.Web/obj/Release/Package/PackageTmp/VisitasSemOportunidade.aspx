<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true" CodeBehind="VisitasSemOportunidade.aspx.cs" Inherits="Rohr.EPC.Web.VisitasSemOportunidade" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Style/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="Content/Script/jquery-1.12.4.js"></script>
    <script src="Content/Script/jquery.dataTables.min.js"></script>
    

    <style type="text/css">
        .affix {
            top: 48px;
        }
        
    </style>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('#visitas').DataTable();
        });      

    </script>

    
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div class="container">
        <div class="row">
            <div class="span3 box-left hidden-tablet">             

                 <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table id="visitas" class="display">
                                            <thead>                                       
                                                
                                                <tr>
                                                    <th></th>                                                    
                                                    <th style="white-space: nowrap;display:none">ID
                                                </th>
                                                <th style="white-space: nowrap;">Cliente
                                                </th>                                                
                                                <th style="white-space: nowrap;">Obra
                                                </th>
                                               
                                            </tr>
                                                </thead>
                                           
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        
                                        <tr id="row" runat="server">
                               <td><asp:CheckBox ID="chkMarcado" runat="server">
                                   </asp:CheckBox>
                               </td>
                                            <td style="display:none">
                                                <asp:Label ID="lblIDPesquisa" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>'></asp:Label>
                                                 
                                            </td>

                                            <td id="Cliente">
                                                <asp:Label ID="Label2"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cliente") %>'></asp:Label>
                                            
                                                 
                                            </td>
                                            <td id="Obra">
                                                <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Obra") %>'></asp:Label>
                                            
                                            </td>                                                                               
                                            
                                        </tr>
                                        
                                    </ItemTemplate>
                                    <FooterTemplate>                                                                            
                                        </table>
                                        <asp:Button ID="btnCarregar" runat="server" Text="Carregar" 
                                            onclick="ckSalvar_Click" />
                                    </FooterTemplate>
                                </asp:Repeater>
               
            </div>
            <div class="span3" style="margin-left:150px;">                
                <div>
                    <asp:Label ID="lblMensagem" runat="server" CssClass="" EnableViewState="False"></asp:Label>
                    <br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxEmail" ErrorMessage="Favor Digitar um E-Mail Válido" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </div>
                
                <div class="row">
                    <div class="span9">
                        <fieldset>
                            <legend>
                                <asp:Table runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                    Visitas Sem Oportunidade
                                        </asp:TableCell>
                                        <asp:TableCell style="font-size:15px;padding-left:120px;padding-bottom:20px;">
                                            Responsável(ROHR):
                                        </asp:TableCell>
                                        <asp:TableCell CSSClass="input-append">                                             
                                                <asp:TextBox runat="server" Enabled="false" ID="TextBoxComResp" CssClass="input-small" ></asp:TextBox>
                                        </asp:TableCell>


                                       </asp:TableRow>

                                </asp:Table>
                                
                               
                                
                            </legend>
                            <table>
                                <tbody>
                                    <tr>
                                        <td style="padding-bottom:10px;">
                                            <asp:Label ID="Label1" runat="server" Text="Nome do Cliente:"></asp:Label>
                                        </td>
                                        <td>                                            
                                            
                                            <div class="input-append">
                                                
                                                <asp:TextBox runat="server" ID="TextBoxNomeCli" CssClass="input-large"></asp:TextBox>                                            

                                                
                                            </div>
                                            
                                        </td>                                       

                                        <td style="padding-left:20px">Obra:</td>
                                        <td>
                                            
                                            
                                            <div class="input-append">                                              
                                                
                                                <asp:TextBox runat="server" ID="TextBoxObra" CssClass="input-large"></asp:TextBox>
                                                
                                                
                                            </div>
                                        
                                        </td> 
                                        
                                    </tr>       
                                        
                                    
                                    <tr>                                        
                                        
                                        <td>Mercado de Atuação:</td>
                                        <td>
                                            <div class="input-append">                                                
                                            <asp:DropDownList CssClass="input-large" ID="Mercado" runat="server" DataSourceID="SqlDataSource2" DataTextField="Nome" DataValueField="Nome">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:EPC %>" SelectCommand="SELECT Nome FROM Mercado"></asp:SqlDataSource>


                                            </div>
                                        </td>     
                                        
                                        
                                        
                                        <td style="padding-left:20px;">CEP:</td>
                                        <td>
                                            <div class="input-prepend">                                                
                                                <asp:TextBox runat="server" ID="TextBoxCEP" AutoPostBack="true" MaxLength="8" OnTextChanged="TextBoxCEP_TextChanged" CssClass="input-large"></asp:TextBox>
                                            </div>
                                        </td>

                                        

                                    </tr>
                                    <tr>
                                        <td>Endereço:</td>
                                        <td>
                                            <div class="input-prepend">                                                
                                                <asp:TextBox runat="server" ID="TextBoxEndereco" CssClass="input-large"></asp:TextBox>
                                            </div>
                                        </td>


                                        <td style="padding-left:20px">Bairro:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="TextBoxBairro" CssClass="input-large"></asp:TextBox></td>

                                    </tr>                                    

                                    <tr>
                                        <td>Cidade:</td>
                                        <td>
                                            <div class="input-prepend">                                                
                                                <asp:TextBox runat="server" ID="TextBoxCidade" CssClass="input-large"></asp:TextBox>
                                            </div>
                                        </td>

                                        <td style="padding-left:20px">Estado:</td>
                                        <td>
                                            <div class="input-prepend">                                                
                                            <asp:DropDownList CssClass="input-large" ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="UFE_SG" DataValueField="UFE_SG">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dneConnectionString %>" SelectCommand="SELECT DISTINCT [UFE_SG] FROM [Log_Faixa_UF]"></asp:SqlDataSource>
                                            </div>
                                        </td>

                                    </tr>                                    

                                    <tr>
                                        <td>Contato Cliente:</td>
                                        <td>
                                            <div class="input-prepend">                                                
                                                <asp:TextBox runat="server" ID="TextBoxContatoCliente" CssClass="input-large"></asp:TextBox>
                                            </div>
                                        </td>


                                        <td style="padding-left:20px;">Setor do Contato:</td>
                                        <td>
                                            <div class="input-prepend">                                                
                                            <asp:DropDownList CssClass="input-large" ID="SetorContato" runat="server" DataSourceID="SqlDataSource3" DataTextField="Nome" DataValueField="Nome">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:EPC %>" SelectCommand="SELECT Nome FROM SetorContato"></asp:SqlDataSource>


                                            </div>
                                        </td>                                                                            

                                    </tr>

                                    <tr>
                                        <td>E-Mail: </td>
                                        <td>                                        
                                            
                                            <div class="input-prepend">                                             
                                           
                                                <asp:TextBox ID="TextBoxEmail" CssClass="input-large" runat="server" ></asp:TextBox>
                                            
                                            </div>
                                        </td>


                                        <td style="padding-left:20px">Telefone:</td>
                                        <td >
                                            <div class="input-prepend">                                                
                                                <asp:TextBox runat="server" ID="TextBoxTelefone" TextMode="Phone" CssClass="input-large" MaxLength="11"></asp:TextBox>
                                            </div>
                                        </td> 


                                    </tr>
                                    <tr><td colspan="10">
                                        <hr style="width:700px;" />
                                        </td></tr>

                                    <tr>
                                        <td>Data Visita:</td>
                                        <td>
                                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                                                </asp:ScriptManager>
                                            <telerik:RadDatePicker id="Calendar1" runat="server"  />
                                            
                                        </td>


                                        <td style="padding-left:20px">Hora da Visita:</td>
                                        <td>
                                            <div class="input-prepend">                                                
                                                <asp:TextBox TextMode="Time" ID="TextBoxHoraVisita" CssClass="input-small" runat="server"></asp:TextBox>                                       
                                            </div>
                                        </td>

                                    </tr>
                                    
                                    <tr>
                                        <td>Acompanhamento:</td>
                                        <td>
                                            <div  class="input-prepend">                                                
                                                <asp:DropDownList CssClass="input-large" ID="DropDownListAcompanhamento" runat="server">                                                
                                                <asp:ListItem>Visita sem Oportunidade</asp:ListItem>
                                                <asp:ListItem>Gerou Oportunidade</asp:ListItem>
                                                <asp:ListItem>Sem Expectativa</asp:ListItem>
                                                <asp:ListItem>Prospecção</asp:ListItem>
                                            </asp:DropDownList>
                                            </div>
                                        </td>

                                                 <td>
                                                     Acompanhamento/Ação:
                                                 </td>
                                        <td>
                                          <asp:TextBox id="TextAreaAcom" TextMode="multiline" Columns="25" Rows="2" runat="server" />
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </fieldset>


                        <table>
                            <tr>
                                <td><fieldset><h4>Histórico</h4></fieldset>
                                    <asp:Repeater ID="Repeater2" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table style="border-spacing: 27px 7px;" class="table">
                                            <tr>
                                                <th style="white-space: nowrap;">Acompanhamento
                                                </th>
                                                <th style="white-space: nowrap;">Data Visita
                                                </th>
                                                <th style="white-space: nowrap;">Hora Visita
                                                </th>
                                                <th style="white-space: nowrap;">Acompanhamento/Ação
                                                </th>                                                
                                                
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr id="row" runat="server">
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "Acompanhamento") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "DataVisita") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "HoraVisita") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# DataBinder.Eval(Container.DataItem, "AcompanhamentoAcao") %>
                                            </td>                                   
                                            
                                        </tr>
                                        
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                </td>
                            </tr>
                        </table>

                       
                        
                    </div>
                </div>                
                <div class="form-actions">
                    <table><tr><td>
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" />
                               </td>
                        <td>
                            <asp:Button ID="btnUpdate" runat="server" Enabled="false" Text="Alterar" CssClass="btn btn-success" OnClick="btnUpdate_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnLimpar" runat="server" Text="Limpar" CssClass="btn btn-success" OnClick="btnLimpar_Click" />
                        </td>
                           </tr></table>
                    
                    
                    
                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>