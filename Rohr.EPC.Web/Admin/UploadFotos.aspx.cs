using Rohr.EPC.Business;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Rohr.EPC.Web.Classes;
using Rohr.EPC.Entity;

namespace Rohr.EPC.Web.Admin
{
    public partial class UploadFotos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            //List<Modelo> listModelo = new ModeloBusiness().ObterTodos(new Util().GetSessaoPerfilAtivo());
            //ddlModelo.DataSource = listModelo;
            //ddlModelo.DataValueField = "IdModelo";
            //ddlModelo.DataTextField = "Titulo";
            //ddlModelo.DataBind();

            LoadImagens();
        }

        private void LoadImagens()
        {
            try
            {
                List<Modelo> listModelo = new ModeloBusiness().ObterTodos(new Util().GetSessaoPerfilAtivo());
                var Imagens = new ImagemBusiness().GetImagens();



                var ListImagens = Imagens
                    .Select(I => new
                    {
                        I.TipoConteudo,
                        I.Conteudo,
                        I.NomeOriginal,
                        I.NomeInterno,
                        I.DirArquivo,
                        I.DescrArquivo,
                        I.LarguraArquivo,
                        I.AlturaArquivo,
                        FlagAtivo = I.FlagAtivo ? "S" : "N",
                        UrlImagem = ResolveUrl("~/Imagens/UploadFotos/" + I.DirArquivo+ "/"+ I.NomeInterno),
                    }).ToList();

                gdvImagens.DataSource = ListImagens;
                gdvImagens.DataBind();

            }
            catch (Exception Exc)
            {
                //lblErro.Visible = true;
                //lblErro.Text = Exc.Message;
            }
        }

        protected void btnEnviarImagem_Click(object sender, EventArgs e)
        {
            try
            {
                //Int32? idModelo = Convert.ToInt32(ddlModelo.SelectedValue);
                //if (!idModelo.HasValue)
                //    throw new Exception("Favor escolher um modelo");

                String Descricao = txtDescricao.Text;

                if (Descricao.Length > 100)
                    throw new Exception("Descrição acima do tamanho permitido");

                String tag = BuscaFoto.Text;

                if(tag.IsNullOrEmpty())
                    throw new Exception("Campo Tag Obrigatório");






                lblErro.Visible = false;
                if (fupImagem.PostedFile != null)
                {


                    string nomeArquivo = Path.GetFileName(fupImagem.PostedFile.FileName);

                    var SubDir = SubDirUpload();
                    var NomeOriginal = fupImagem.FileName;

                    byte[] imageBytes = fupImagem.FileBytes;

                    var ImgFoto = new Bitmap(fupImagem.FileContent);

                    var Extensao = Path.GetExtension(NomeOriginal);
                    var NomeInterno = Guid.NewGuid().ToString() + Extensao;

                    if (!Extensao.EqualsCI(".jpg") && !Extensao.EqualsCI(".jpeg") && !Extensao.EqualsCI(".png"))
                    {
                        throw new Exception("Favor escolher um arquivo .jpg , .jpeg ou .png");
                    }


                    if (Extensao.EqualsCI(".jpg") || Extensao.EqualsCI(".jpeg") || Extensao.EqualsCI(".png"))
                    {
                        // Resize & Save
                        var Img = new Bitmap(fupImagem.FileContent);
                        var ImgsTn = Img.ResizeImage(290, 200, false);
                        ImgsTn.SaveAsJpeg(UploadDirectory + @"\" + SubDir + @"\" + NomeInterno, 80);
                    }

                    Imagens aImagem = new Imagens()
                    {
                        TipoConteudo = "Arquivo",
                        Conteudo = imageBytes,
                        NomeOriginal = NomeOriginal,
                        NomeInterno = NomeInterno,
                        DirArquivo = SubDir,
                        DescrArquivo = txtDescricao.Text,
                        LarguraArquivo = Convert.ToInt32(txtLargura.Text),
                        AlturaArquivo = Convert.ToInt32(txtAltura.Text),
                        FlagAtivo = ckbFlagAtivo.Checked,
                        IdUsuarioUpload = new Util().GetSessaoUsuario().IdUsuario,
                        Tag = BuscaFoto.Text,
                    };

                    new ImagemBusiness().AdicionarImagem(aImagem);

                    LoadImagens();
                    txtDescricao.Text = String.Empty;
                    BuscaFoto.Text = String.Empty;
                    lblErro.Visible = true;
                    lblErro.Text = "Imagem salva com sucesso!";
                }
                else
                    throw new Exception("Nenhum arquivo selecionado :(");

            }
            catch (Exception Exc)
            {
                lblErro.Visible = true;
                lblErro.Text = Exc.Message;
            }
        }


        /* --------------- Upload Arquivos --------------- */

        private String m_UploadDirectory = null;
        public String UploadDirectory
        {
            get
            {
                if (m_UploadDirectory == null)
                {
                    var Res = new StringBuilder(ConfigurationManager.AppSettings["UploadDirectory"]);
                    Res.Append(@"\");
                    Res.Replace(@"\\", @"\");
                    m_UploadDirectory = Res.ToString();
                }
                return m_UploadDirectory;
            }
        }

        // Retorna um sub diretório para guardar um arquivo
        // Guarda até 100 arquivos por sub-diretório
        public String SubDirUpload()
        {
            foreach (var SDir in Directory.GetDirectories(UploadDirectory))
            {
                var Files = Directory.EnumerateFiles(SDir);
                if (Files.Count() < 100)
                    return SDir.ReplaceStringCI(UploadDirectory, String.Empty);
            }
            var NDir = Guid.NewGuid().ToString();
            Directory.CreateDirectory(UploadDirectory + NDir);
            return NDir;
        }
    }
}