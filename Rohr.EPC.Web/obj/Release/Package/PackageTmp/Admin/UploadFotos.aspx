<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterDefault.Master" AutoEventWireup="true"
  CodeBehind="UploadFotos.aspx.cs" Inherits="Rohr.EPC.Web.Admin.UploadFotos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    // Copia o nome do arquivo selecionado
    function CopiaNome(event) {
      var File = jQuery('#<% =fupImagem.ClientID %>').val();
      jQuery('#<% =txtNomeOriginal.ClientID %>').val(File);
    }

  </script>
  <link href="CSS/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            float: left;
            width: 500px;
            height: 358px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
    <div style="z-index: 1; overflow: scroll;">
    <div class="auto-style1">
      <table class="Formulario">
        <tr>
          <td class="Label" colspan="3">
            <asp:Label ID="Label1" runat="server" Text="Upload de Imagens"></asp:Label>
          </td>
        </tr>
        <tr>
          <td class="Label" colspan="3">
            <asp:Label ID="lblErro" runat="server" Visible="False" Font-Bold="True" ForeColor="#FF3300"></asp:Label>
          </td>
        </tr>
        <tr>
          <td class="Label">
            <asp:Label ID="lblArquivo" Text="Arquivo" runat="server" />
          </td>
          <td class="Field" colspan="3">
            <asp:FileUpload ID="fupImagem" runat="server" onchange='CopiaNome();' />
          </td>
        </tr>
        <tr>
          <td class="Label">
            <asp:Label ID="lblNomeOriginal" Text="Nome *" runat="server" />
          </td>
          <td class="Field" colspan="3">
            <asp:TextBox ID="txtNomeOriginal" runat="server" Style="width: 200px;" Enabled="false" />
          </td>
        </tr>
        <tr>
          <td class="Label">
            <asp:Label ID="lblDescricao" Text="Descrição, Max 100 caracteres" runat="server" />
          </td>
          <td class="Field" colspan="3">
            <asp:TextBox ID="txtDescricao" runat="server" TextMode="MultiLine" CssClass="Memo" />
          </td>
        </tr>
        <tr>
          <td>
            <asp:Label ID="lblModelo" Text="Tags *" runat="server" />
          </td>
          <td class="Field" colspan="3">
            <asp:TextBox ID="BuscaFoto" runat="server" TextMode="MultiLine" CssClass="Memo" ></asp:TextBox>
            <%--<asp:DropDownList ID="ddlModelo" runat="server"></asp:DropDownList>--%>
          </td>
        </tr>
        <tr>
          <td class="Label">
            <asp:Label ID="lblFlagAtivo" Text="Ativo" runat="server" />
          </td>
          <td class="Field" colspan="3">
            <asp:CheckBox ID="ckbFlagAtivo" runat="server" Checked="true" />
          </td>
        </tr>
        <tr>
          <td class="Label">
            <asp:Label ID="lblLargura" Text="Largura" runat="server" />
          </td>
          <td class="Field">
            <asp:TextBox ID="txtLargura" runat="server" CssClass="TextoPeq" Text="290" Enabled="false" />
          </td>
          <td class="Label">
            <asp:Label ID="lblAltura" Text="Altura" runat="server" />
          </td>
          <td class="Field">
            <asp:TextBox ID="txtAltura" runat="server" CssClass="TextoPeq" Text="200" Enabled="false" />
          </td>
        </tr>
        <tr>
          <td></td>
          <td class="Button">
            <asp:LinkButton ID="lnkAtualizar" runat="server" OnClick="btnEnviarImagem_Click" Text="Enviar Imagem" />
          </td>
        </tr>

        <tr>
          <td>
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
          </td>
        </tr>
      </table>
    </div>
    <div class="GradeDados" style="z-index: 1; overflow: scroll; float: right; width: 800px; height: 500px; margin-right: 5px;">
      <asp:GridView ID="gdvImagens" runat="server" AutoGenerateColumns="False" CellPadding="6"
        HeaderStyle-CssClass="Cabec">
        <Columns>
          <%-- <asp:BoundField DataField="TipoConteudo" HeaderText="Tipo Conteúdo" />
          <asp:BoundField DataField="ModeloDocumento" HeaderText="Modelo Documento" />--%>
          <asp:BoundField DataField="NomeOriginal" HeaderText="Nome" />
          <asp:BoundField DataField="DescrArquivo" ItemStyle-Width="200px" HeaderText="Descrição" />
          <asp:BoundField DataField="LarguraArquivo" HeaderText="Largura Arquivo" />
          <asp:BoundField DataField="AlturaArquivo" HeaderText="Altura Arquivo" />
          <asp:BoundField DataField="FlagAtivo" HeaderText="Ativo" />
          <asp:ImageField DataImageUrlField="UrlImagem" ItemStyle-Width="150px" ControlStyle-Width="150" ControlStyle-Height="150"
            HeaderText="Imagem" />
        </Columns>
      </asp:GridView>
    </div>
  </div>
</asp:Content>
