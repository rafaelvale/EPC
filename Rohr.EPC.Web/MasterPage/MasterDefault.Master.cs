using System.Linq;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;



namespace Rohr.EPC.Web.MasterPage
{
    public partial class MasterDefault : System.Web.UI.MasterPage
    {
        protected String menuUsuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Usuario sessaoUsuario = new Util().GetSessaoUsuario();
                Int32 idPerfilAtivo = new Util().GetSessaoPerfilAtivo();
                MontarMenu(sessaoUsuario, idPerfilAtivo);

                Page.Title = String.Format("EPC - {0}", new PerfilBusiness().ObterPorId(idPerfilAtivo).Descricao);
            }
            catch (Exception ex)
            {
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
                Response.Redirect(ResolveUrl("~/Login.aspx?error=true"), false);
            }
        }
        void MontarMenu(Usuario usuario, Int32 idPerfil)
        {
            menuUsuario = MontarMenuUsuario(usuario, idPerfil);


            switch (Path.GetFileName(Request.Path))
            {
                case "Cockpit":
                    menuPerfil.Controls.Add(MontarMenuPerfil(idPerfil));
                    menuPerfil.Controls.Add(MontarMenuUploadFotos(usuario.IdUsuario));
                    menuPerfil.Controls.Add(MontarMenuApoio());

                    break;
                case "WorkflowView":
                    menuPerfil.Controls.Add(MontarMenuWorkflow(idPerfil));
                    break;
                case "Acoes":
                    menuPerfil.Controls.Add(MontarMenuAcao(idPerfil));



                    break;
                default:
                    if (Request.Url.Segments.Length > 1 && Request.Url.Segments[1] == "Relatorios/" && Request.Url.Segments[2] != "Pesquisa")
                        menuPerfil.Controls.Add(MontarMenuRelatorio(idPerfil));
                    else if (Request.Url.Segments.Length > 1 && Request.Url.Segments[1] == "Relatorios/" && Request.Url.Segments[2] == "Pesquisa")
                    {
                        menuPerfil.Controls.Add(MontarMenuPesquisaDoc(idPerfil));
                        menuPerfil.Controls.Add(MontarMenuApoio());
                    }
                    else if (Path.GetFileName(Request.Path) == "PropostaDocumento")
                        menuPerfil.Controls.Add(MontarMenuPerfilPropostaDocumento(idPerfil));

                    else if (Path.GetFileName(Request.Path) == "GerarTermoAditivo")
                        menuPerfil.Controls.Add(MontarMenuTA(idPerfil));
                    
                    else
                        menuPerfil.Controls.Add(MontarMenuPerfilProposta(idPerfil));
                    
                    break;
            }
        }

        #region Menu
        private void btnMenu_Click(object sender, EventArgs e)
        {
            LinkButton oLinkButton = (LinkButton)sender;

            try
            {
                String atributo = oLinkButton.Attributes["destino"];
                if (String.Compare(atributo, "101", StringComparison.Ordinal) == 0)
                    Response.Redirect(ResolveUrl(String.Format("~/Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar(oLinkButton.Attributes["destino"]))), false);
                else if (String.Compare(atributo, "102", StringComparison.Ordinal) == 0 || String.Compare(atributo, "104", StringComparison.Ordinal) == 0)
                    ObterProximaAcao(true);

                else if (String.Compare(atributo, "205", StringComparison.Ordinal) == 0)
                    Response.Redirect(ResolveUrl(String.Format("~/Relatorios/TempoAprovacaoPropostas.aspx", CriptografiaBusiness.Criptografar(oLinkButton.Attributes["destino"]))), false);

                else if (String.Compare(atributo, "100", StringComparison.Ordinal) == 0)
                    Response.Redirect(ResolveUrl(String.Format("~/GerarTermoAditivo.aspx", CriptografiaBusiness.Criptografar(oLinkButton.Attributes["destino"]))), false);

                else if (String.Compare(atributo, "103", StringComparison.Ordinal) == 0)
                    ObterProximaAcao();
                else if (String.Compare(atributo, "106", StringComparison.Ordinal) == 0)
                    GerarPdf();
                else if (String.Compare(atributo, "107", StringComparison.Ordinal) == 0)
                    GerarPlanilhaOrcamentaria();
                else if (String.Compare(atributo, "208", StringComparison.Ordinal) == 0)
                    Detalhe();
                else if (String.Compare(atributo, "201", StringComparison.Ordinal) == 0)
                     Response.Redirect(ResolveUrl(String.Format("~/VisitasSemOportunidade.aspx", CriptografiaBusiness.Criptografar(oLinkButton.Attributes["destino"]))), false);
                else if (String.Compare(atributo, "202", StringComparison.Ordinal) == 0 || String.Compare(atributo, "203", StringComparison.Ordinal) == 0
                    || String.Compare(atributo, "204", StringComparison.Ordinal) == 0 
                    || String.Compare(atributo, "206", StringComparison.Ordinal) == 0 || String.Compare(atributo, "207", StringComparison.Ordinal) == 0)
                    Response.Redirect(ResolveUrl(String.Format("~/Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar(atributo))), false);
                else if (String.Compare(atributo, "308", StringComparison.Ordinal) == 0)
                    Response.Redirect(ResolveUrl(String.Format("~/Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar(atributo))), false);

            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);
            }
        }

        private String MontarMenuUsuario(Usuario usuario, Int32 idPerfil)
        {
            StringBuilder menu = new StringBuilder();
            menu.Append("<li class=\"dropdown\"><a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">");
            menu.Append(usuario.PrimeiroNome);
            menu.Append(" <img src='../../Content/Image/img1.png' width='8' height='6'/>");
            menu.Append("</a>");
            menu.Append("<ul class=\"dropdown-menu\">");
            menu.Append("<li class=\"nav-header\">Perfil</li>");

            foreach (Perfil perfil in usuario.Perfis)
            {
                menu.Append(perfil.IdPerfil == idPerfil
                    ? String.Format("<li class=\"active\"><a href=../../Redirecionar.aspx?p={0}>{1}</a></li>",
                        CriptografiaBusiness.Criptografar(perfil.IdPerfil.ToString()), perfil.Descricao)
                    : String.Format("<li><a href=../../Redirecionar.aspx?p={0}>{1}</a></li>",
                        CriptografiaBusiness.Criptografar(perfil.IdPerfil.ToString()), perfil.Descricao));
            }

            menu.Append("<li class=\"divider\"></li>");
            menu.Append(String.Format("<li><a target=\"_blank\" href=\"../../Redirecionar.aspx?p={0}\">Configurações da Conta</a></li>", CriptografiaBusiness.Criptografar("50")));
            menu.Append("<li class=\"divider\"></li>");
            menu.Append(String.Format("<li><a href=\"../../Redirecionar.aspx?p={0}\">Sair</a></li>", CriptografiaBusiness.Criptografar("0")));
            menu.Append("</ul>");
            menu.Append("</li>");

            return menu.ToString();
        }

        private HtmlGenericControl MontarMenuPerfil(Int32 idPerfil)
        {
            HtmlGenericControl ul;

            if (idPerfil == 1 || idPerfil == 6)
            {

                ul = new HtmlGenericControl("ul");
                ul.Attributes.Add("class", "nav");

                HtmlGenericControl li = new HtmlGenericControl("li");                


                LinkButton novaProposta = new LinkButton
                {
                    Text = @"<img src='../../Content/Image/add.png' class='ajuste-img' width='24' height='24'  /> Nova Proposta"
                };
                novaProposta.Attributes.Add("destino", "101");
                novaProposta.Click += btnMenu_Click;
                li.Controls.Add(novaProposta);
                ul.Controls.Add(li);                             

                Dictionary<String, Int16> menu = new Dictionary<String, Int16>
                    {
                     {"Executar Próxima Ação", 103},
                        {"TA", 100 },
                        { "Executar Próxima Ação", 103},
                        {"Workflow", 102}
                        
                    };

                LinkButton oLinkButton;
                foreach (var item in menu)
                {
                    HtmlGenericControl li1 = new HtmlGenericControl("li");
                    if (item.Key == "TA") { oLinkButton = new LinkButton { Text = @"<img src='../../Content/Image/add.png' class='ajuste-img' width='24' height='24'  /> Termo Aditivo" }; }
                    else { oLinkButton = new LinkButton { Text = item.Key }; }                    
                    oLinkButton.Attributes.Add("destino", item.Value.ToString());
                    oLinkButton.Click += btnMenu_Click;
                    li1.Controls.Add(oLinkButton);

                    ul.Controls.Add(li1);
                }
            }

            else
            {
                ul = new HtmlGenericControl("ul");
                ul.Attributes.Add("class", "nav");

                Dictionary<String, Int16> menu = new Dictionary<String, Int16>();
                if (idPerfil != 2)
                    menu.Add("Executar Próxima Ação", 103);

                menu.Add("Workflow", 104);
                

                foreach (var item in menu)
                {
                    HtmlGenericControl li1 = new HtmlGenericControl("li");
                    LinkButton oLinkButton = new LinkButton();

                    if (item.Key == "Executar Próxima Ação")
                        oLinkButton.Text = @"<img src='../../Content/Image/share.png' class='ajuste-img' width='24' height='24'/> Executar Próxima Ação";
                    else if (item.Key == "Workflow" && menu.Count <= 1)
                        oLinkButton.Text = @"<img src='../../Content/Image/timeline.png' class='ajuste-img' width='24' height='24'/> Workflow";                    
                    else
                        oLinkButton.Text = item.Key;

                    oLinkButton.Attributes.Add("destino", item.Value.ToString());
                    oLinkButton.Click += btnMenu_Click;
                    li1.Controls.Add(oLinkButton);
                    ul.Controls.Add(li1);
                }
            }

            return ul;
        }

        private HtmlGenericControl MontarMenuUploadFotos(Int32 idUsuario)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "nav");

            if (idUsuario == 9 || idUsuario == 110 || idUsuario == 111)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");

                LinkButton Upload = new LinkButton
                {
                    Text = @"Upload de Fotos"
                };
                Upload.Attributes.Add("destino", "308");
                Upload.Click += btnMenu_Click;
                li.Controls.Add(Upload);
                ul.Controls.Add(li);
            }

            return ul;
        }

        private HtmlGenericControl MontarMenuPesquisaDoc(Int32 idPerfil)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "nav");

            if (idPerfil == 1 || idPerfil == 6)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");

                LinkButton novaProposta = new LinkButton
                {
                    Text = @"<img src='../../Content/Image/add.png' class='ajuste-img' width='24' height='24'  /> Nova Proposta"
                };
                novaProposta.Attributes.Add("destino", "101");
                novaProposta.Click += btnMenu_Click;
                li.Controls.Add(novaProposta);
                ul.Controls.Add(li);
                
            }

            Dictionary<String, Int16> menu = new Dictionary<String, Int16>
                    {
                        {"Workflow", 102}
                    };

            foreach (var item in menu)
            {
                HtmlGenericControl li1 = new HtmlGenericControl("li");
                LinkButton oLinkButton = new LinkButton { Text = item.Key };
                oLinkButton.Attributes.Add("destino", item.Value.ToString());
                oLinkButton.Click += btnMenu_Click;
                li1.Controls.Add(oLinkButton);

                ul.Controls.Add(li1);
            }


            return ul;
        }
        private HtmlGenericControl MontarMenuPerfilProposta(Int32 idPerfil)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "nav");

            HtmlGenericControl li = new HtmlGenericControl("li");
            if (idPerfil == 1 || idPerfil == 6)
            {
                LinkButton novaProposta = new LinkButton
                {
                    Text =
                        @"<img src='../../Content/Image/add.png' class='ajuste-img' width='24' height='24'/> Nova Proposta"
                };
                novaProposta.Attributes.Add("destino", "101");
                novaProposta.Click += btnMenu_Click;
                li.Controls.Add(novaProposta);
                ul.Controls.Add(li);
            }

            HtmlGenericControl li1 = new HtmlGenericControl("li");
            LinkButton oLinkButton = new LinkButton
            {
                Text = "Baixar PDF",
                OnClientClick = @"window.open('" + ResolveUrl("~/BaixarDocumento.ashx?p=a1a1") + "', target='_blank');"
            };
            li1.Controls.Add(oLinkButton);
            ul.Controls.Add(li1);

            return ul;
        }


        private HtmlGenericControl MontarMenuTA(Int32 idPerfil)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "nav");

            HtmlGenericControl li = new HtmlGenericControl("li");
            if (idPerfil == 1 || idPerfil == 6)
            {
                LinkButton novaProposta = new LinkButton
                {
                    Text =
                        @"<img src='../../Content/Image/add.png' class='ajuste-img' width='24' height='24'/> Nova Proposta"
                };
                novaProposta.Attributes.Add("destino", "101");
                novaProposta.Click += btnMenu_Click;
                li.Controls.Add(novaProposta);
                ul.Controls.Add(li);
            }
            

            return ul;
        }

        private HtmlGenericControl MontarMenuPerfilPropostaDocumento(Int32 idPerfil)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "nav");

            HtmlGenericControl li = new HtmlGenericControl("li");
            if (idPerfil != 1 && idPerfil != 6) return ul;
            LinkButton novaProposta = new LinkButton
            {
                Text = @"<img src='../../Content/Image/add.png' class='ajuste-img' width='24' height='24'/> Nova Proposta"
            };
            novaProposta.Attributes.Add("destino", "101");
            novaProposta.Click += btnMenu_Click;
            li.Controls.Add(novaProposta);
            ul.Controls.Add(li);
            return ul;
        }
        private HtmlGenericControl MontarMenuRelatorio(Int32 idPerfil)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "nav");

            HtmlGenericControl li = new HtmlGenericControl("li");
            if (idPerfil != 1 && idPerfil != 6) return ul;
            LinkButton novaProposta = new LinkButton
            {
                Text = @"<img src='../../Content/Image/add.png' class='ajuste-img' width='24' height='24'/> Nova Proposta"
            };
            novaProposta.Attributes.Add("destino", "101");
            novaProposta.Click += btnMenu_Click;
            li.Controls.Add(novaProposta);
            ul.Controls.Add(li);

            return ul;
        }
        private HtmlGenericControl MontarMenuWorkflow(Int32 idPerfil)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "nav");

            HtmlGenericControl li = new HtmlGenericControl("li");
            if (idPerfil == 1 || idPerfil == 6)
            {
                LinkButton novaProposta = new LinkButton
                {
                    Text = @"<img src='../../Content/Image/add.png' class='ajuste-img' width='24' height='24'/> Nova Proposta"
                };
                novaProposta.Attributes.Add("destino", "101");
                novaProposta.Click += btnMenu_Click;
                li.Controls.Add(novaProposta);
                ul.Controls.Add(li);
                                
            }

            HtmlGenericControl li1 = new HtmlGenericControl("li");
            LinkButton executarProximaAcao = new LinkButton
            {
                Text = idPerfil == 1 || idPerfil == 6 ?
                "Executar Próxima Ação" :
                @"<img src='../../Content/Image/share.png' class='ajuste-img' width='24' height='24'/> Executar Próxima Ação"
            };

            executarProximaAcao.Attributes.Add("destino", "103");
            executarProximaAcao.Click += btnMenu_Click;
            li1.Controls.Add(executarProximaAcao);
            ul.Controls.Add(li1);

            HtmlGenericControl li2 = new HtmlGenericControl("li");
            LinkButton oLinkButton2 = new LinkButton
            {
                Text = "Baixar PDF",
                OnClientClick = @"window.open('" + ResolveUrl("~/BaixarDocumento.ashx?p=a1a1") + "', target='_blank');"
            };
            li2.Controls.Add(oLinkButton2);
            ul.Controls.Add(li2);

            HtmlGenericControl li3 = new HtmlGenericControl("li");
            LinkButton oLinkButton3 = new LinkButton
            {
                Text = "Baixar Orçamento",
                OnClientClick = @"window.open('" + ResolveUrl("~/BaixarDocumento.ashx?p=a2b2") + "', target='_blank');"
            };
            li3.Controls.Add(oLinkButton3);
            ul.Controls.Add(li3);

            return ul;
        }



        private HtmlGenericControl MontarMenuAcao(Int32 idPerfil)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "nav");

            HtmlGenericControl li = new HtmlGenericControl("li");
            if (idPerfil != 1 && idPerfil != 6) return ul;
            LinkButton novaProposta = new LinkButton
            {
                Text = @"<img src='../../Content/Image/add.png' class='ajuste-img' width='24' height='24'/> Nova Proposta"
            };
            novaProposta.Attributes.Add("destino", "101");
            novaProposta.Click += btnMenu_Click;
            li.Controls.Add(novaProposta);
            ul.Controls.Add(li);
            return ul;
        }
        private HtmlGenericControl MontarMenuApoio()
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "nav");

            HtmlGenericControl li = new HtmlGenericControl("li");
            li.Attributes.Add("class", "dropdown");

            LinkButton oLinkButton1 = new LinkButton
            {
                Text = @"Mais <img src='../../Content/Image/img1.png' width='8' height='6'/>",
                CssClass = "dropdown-toggle"
            };
            oLinkButton1.Attributes.Add("data-toggle", "dropdown");

            li.Controls.Add(oLinkButton1);

            HtmlGenericControl ul2 = new HtmlGenericControl("ul");
            ul2.Attributes.Add("class", "dropdown-menu");

            HtmlGenericControl li3 = new HtmlGenericControl("li");
            LinkButton oLinkButton2 = new LinkButton { Text = "Baixar PDF" };
            oLinkButton2.Attributes.Add("destino", "106");
            oLinkButton2.Click += btnMenu_Click;
            li3.Controls.Add(oLinkButton2);
            ul2.Controls.Add(li3);



            HtmlGenericControl li4 = new HtmlGenericControl("li");
            LinkButton oLinkButton3 = new LinkButton { Text = "Baixar Orçamento" };
            oLinkButton3.Attributes.Add("destino", "107");
            oLinkButton3.Click += btnMenu_Click;
            li4.Controls.Add(oLinkButton3);
            ul2.Controls.Add(li4);

            HtmlGenericControl li16 = new HtmlGenericControl("li");
            LinkButton oLinkButton16 = new LinkButton { Text = "Detalhes" };
            oLinkButton16.Attributes.Add("destino", "208");
            oLinkButton16.Click += btnMenu_Click;
            li16.Controls.Add(oLinkButton16);
            ul2.Controls.Add(li16);

            HtmlGenericControl ul3 = new HtmlGenericControl("ul");
            ul3.Attributes.Add("class", "dropdown-menu");

            HtmlGenericControl li7 = new HtmlGenericControl("li");
            LinkButton oLinkButton5 = new LinkButton();
            oLinkButton5.Text = "Visitas Sem Oportunidade";
            oLinkButton5.Attributes.Add("destino", "201");
            oLinkButton5.Click += btnMenu_Click;
            li7.Controls.Add(oLinkButton5);
            ul2.Controls.Add(li7);


            HtmlGenericControl li14 = new HtmlGenericControl("li");
            li14.Attributes.Add("class", "divider");
            ul2.Controls.Add(li14);

            HtmlGenericControl li15 = new HtmlGenericControl("li");
            LinkButton oLinkButton15 = new LinkButton { Text = "Pesquisa Avançada" };
            oLinkButton15.Attributes.Add("destino", "207");
            oLinkButton15.Click += btnMenu_Click;
            li15.Controls.Add(oLinkButton15);
            ul2.Controls.Add(li15);

            HtmlGenericControl li5 = new HtmlGenericControl("li");
            li5.Attributes.Add("class", "divider");
            ul2.Controls.Add(li5);

            HtmlGenericControl li6 = new HtmlGenericControl("li");
            li6.Attributes.Add("class", "dropdown-submenu");

            LinkButton oLinkButton4 = new LinkButton { Text = "Relatórios" };
            oLinkButton4.Attributes.Add("href", "#");
            li6.Controls.Add(oLinkButton4);

            HtmlGenericControl ul3 = new HtmlGenericControl("ul");
            ul3.Attributes.Add("class", "dropdown-menu");

            HtmlGenericControl li8 = new HtmlGenericControl("li");
            LinkButton oLinkButton6 = new LinkButton { Text = "Doc. em Circulação" };
            oLinkButton6.Attributes.Add("destino", "202");
            oLinkButton6.Click += btnMenu_Click;
            li8.Controls.Add(oLinkButton6);
            ul3.Controls.Add(li8);

            HtmlGenericControl li9 = new HtmlGenericControl("li");
            LinkButton oLinkButton7 = new LinkButton();
            oLinkButton7.Text = "Doc. Reprovados";
            oLinkButton7.Attributes.Add("destino", "203");
            oLinkButton7.Click += btnMenu_Click;
            li9.Controls.Add(oLinkButton7);
            ul3.Controls.Add(li9);

            //HtmlGenericControl li10 = new HtmlGenericControl("li");
            //LinkButton oLinkButton8 = new LinkButton();
            //oLinkButton8.Text = "Efetividade Comercial";
            //oLinkButton8.Attributes.Add("destino", "204");
            //oLinkButton8.Click += btnMenu_Click;
            //li10.Controls.Add(oLinkButton8);
            //ul3.Controls.Add(li10);
            

            HtmlGenericControl li12 = new HtmlGenericControl("li");
            LinkButton oLinkButton10 = new LinkButton { Text = "Workflow, Metas e Modelos" };
            oLinkButton10.Attributes.Add("destino", "206");
            oLinkButton10.Click += btnMenu_Click;
            li12.Controls.Add(oLinkButton10);
            ul3.Controls.Add(li12);


            Int32 idPerfilAtivo = new Util().GetSessaoPerfilAtivo();

            if (idPerfilAtivo == 3 ||
                   idPerfilAtivo == 4||
                   idPerfilAtivo == 5||
                   idPerfilAtivo == 9 ||
                idPerfilAtivo == 10) {

                HtmlGenericControl li11 = new HtmlGenericControl("li");
                LinkButton oLinkButton9 = new LinkButton();
                oLinkButton9.Text = "Tempo de Aprovação das Propostas / Contratos";
                oLinkButton9.Attributes.Add("destino", "205");
                oLinkButton9.Click += btnMenu_Click;
                li11.Controls.Add(oLinkButton9);
                ul3.Controls.Add(li11);
            }
            li6.Controls.Add(ul3);
            ul2.Controls.Add(li6);
            li.Controls.Add(ul2);
            ul.Controls.Add(li);

            return ul;
        }
        List<Workflow> ObterListWorkflow()
        {
            List<Workflow> listWorkflow = new List<Workflow>();

            if (Path.GetFileName(Request.Path) == "WorkflowView")
            {
                Workflow oWorkflow = new WorkflowBusiness().ObterUltimaAcaoPorIdDocumento(new Util().GetSessaoDocumento().IdDocumento);
                listWorkflow.Add(oWorkflow);
            }
            else
            {
                foreach (Repeater control in contentBody.Controls.OfType<Repeater>())
                {
                    listWorkflow = new WorkflowBusiness().ObterListaWorkflowCockpit(control);
                }
            }

            return listWorkflow;
        }

        void ObterProximaAcao(Boolean visualizarWorkflow = false)
        {
            Int32 idPerfilAtivo = new Util().GetSessaoPerfilAtivo();
            List<Workflow> listWorkflow = ObterListWorkflow();
            
            if (listWorkflow.Count <= 0)
                throw new MyException("Nenhum documento selecionado.");
          

            if (visualizarWorkflow)
            {
                if (listWorkflow.Count > 1)
                    throw new MyException("Selecione um documento por vez para visualizar o workflow.");

                Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento);
                Session["documento"] = null;
                Session["documento"] = oDocumento;
                Response.Redirect(ResolveUrl(String.Format("~/Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("455"))), false);
            }

            else
            {
                Int32 idWorkflowAcaoBase = 0;
                for (int i = 0; i < listWorkflow.Count; i++)
                {
                    if (i == 0)
                        idWorkflowAcaoBase = listWorkflow[i].WorkflowAcao.IdWorkflowAcao;

                    if (idWorkflowAcaoBase != listWorkflow[i].WorkflowAcao.IdWorkflowAcao)
                        throw new MyException("Os documentos estão aguardando ações diferentes.");
                }

                if (new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento).DocumentoStatus.IdDocumentoStatus == 7)
                    throw new MyException("O documento foi marcado como sem negócio. Não é possível realizar nenhuma ação nesse documento.");

                if (idWorkflowAcaoBase == 15 || idWorkflowAcaoBase == 16 || idWorkflowAcaoBase == 17)
                {
                    if (listWorkflow.Count > 1)
                        throw new MyException("Selecione um documento por vez para executar a próxima ação.");

                    if (idPerfilAtivo != 1 && idPerfilAtivo != 6)
                        throw new MyException("Você não tem permissão para executar essa ação.");

                    if (DocumentoBloqueado(listWorkflow[0].IdDocumento))
                        throw new MyException(String.Format("O documento nº: {0}, está bloqueado. Não é possível executar a ação :(",
                    new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento).NumeroDocumento));

                    Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento);

                    if (oDocumento.Modelo.Segmento.IdSegmento == 3 && idPerfilAtivo != 6)
                        throw new MyException("Você não tem permissão para executar essa ação.");
                    if (oDocumento.Modelo.Segmento.IdSegmento != 3 && idPerfilAtivo != 1)
                        throw new MyException("Você não tem permissão para executar essa ação.");

                    oDocumento.DataParaExibicao = DateTime.Now;
                    oDocumento.Edicao = true;
                    Session["documento"] = null;
                    Session["documento"] = oDocumento;
                    Response.Redirect(ResolveUrl(String.Format("~/Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("400"))), false);
                }
                else if (idWorkflowAcaoBase == 1 || idWorkflowAcaoBase == 2 || idWorkflowAcaoBase == 3 || idWorkflowAcaoBase == 4 || idWorkflowAcaoBase == 7 || idWorkflowAcaoBase == 9 ||
                    idWorkflowAcaoBase == 10 || idWorkflowAcaoBase == 11 || idWorkflowAcaoBase == 12 || idWorkflowAcaoBase == 13 || idWorkflowAcaoBase == 14 || idWorkflowAcaoBase == 8 || idWorkflowAcaoBase == 49 ||
                    idWorkflowAcaoBase == 55 || idWorkflowAcaoBase == 56 || idWorkflowAcaoBase == 65 || idWorkflowAcaoBase == 66 || idWorkflowAcaoBase == 68 ||
                    idWorkflowAcaoBase == 70 || idWorkflowAcaoBase == 74 || idWorkflowAcaoBase == 76 || idWorkflowAcaoBase == 77 || idWorkflowAcaoBase == 78 ||
                    idWorkflowAcaoBase == 79)
                {
                    if (idWorkflowAcaoBase == 1 && idPerfilAtivo != 8)
                        throw new MyException("Você não tem permissão para executar essa ação.");
                    if (idWorkflowAcaoBase == 2 && idPerfilAtivo != 5)
                        throw new MyException("Você não tem permissão para executar essa ação.");
                    if ((idWorkflowAcaoBase == 3 || idWorkflowAcaoBase == 4 || idWorkflowAcaoBase == 9 || idWorkflowAcaoBase == 10 ||
                        idWorkflowAcaoBase == 11 || idWorkflowAcaoBase == 14 || idWorkflowAcaoBase == 66 || idWorkflowAcaoBase == 68 ||
                        idWorkflowAcaoBase == 78 || idWorkflowAcaoBase == 79) && idPerfilAtivo != 1 && idPerfilAtivo != 6)
                        throw new MyException("Você não tem permissão para executar essa ação.");
                    if ((idWorkflowAcaoBase == 7 || idWorkflowAcaoBase == 74) && idPerfilAtivo != 4)
                        throw new MyException("Você não tem permissão para executar essa ação.");
                    if ((idWorkflowAcaoBase == 8 || idWorkflowAcaoBase == 12 || idWorkflowAcaoBase == 13 || idWorkflowAcaoBase == 65 ||
                        idWorkflowAcaoBase == 70 || idWorkflowAcaoBase == 73 || idWorkflowAcaoBase == 77) && idPerfilAtivo != 3)
                        throw new MyException("Você não tem permissão para executar essa ação.");
                    if ((idWorkflowAcaoBase == 49) && idPerfilAtivo != 7)
                        throw new MyException("Você não tem permissão para executar essa ação.");
                    if ((idWorkflowAcaoBase == 55) && idPerfilAtivo != 9)
                        throw new MyException("Você não tem permissão para executar essa ação.");
                    if ((idWorkflowAcaoBase == 56) && idPerfilAtivo != 10)
                        throw new MyException("Você não tem permissão para executar essa ação.");

                    Session["listWorkflow"] = null;
                    Session["listWorkflow"] = listWorkflow;
                    Response.Redirect(String.Format("Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("500")), false);
                }
                else if (idWorkflowAcaoBase == 5)
                {
                    if (listWorkflow.Count > 1)
                        throw new MyException("Selecione um documento por vez para executar a próxima ação.");

                    if (idPerfilAtivo != 1 && idPerfilAtivo != 6)
                        throw new MyException("Você não tem permissão para executar essa ação.");

                    Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento);

                    if (oDocumento.DocumentoStatus.IdDocumentoStatus == 5)
                        throw new MyException(String.Format("Já existe um contrato para a oportunidade nº: {0} em andamento no EPC :)", oDocumento.NumeroDocumento.ToString("N0")));

                    if (DocumentoBloqueado(listWorkflow[0].IdDocumento))
                        throw new MyException(String.Format("O documento nº: {0}, está bloqueado. Não é possível executar a ação :(", oDocumento.NumeroDocumento.ToString("N0")));

                    Session["documento"] = null;
                    Session["documento"] = oDocumento;
                    Response.Redirect(ResolveUrl(String.Format(ResolveUrl("~/Redirecionar.aspx?p={0}"), CriptografiaBusiness.Criptografar("102"))), false);
                }
                else if (idWorkflowAcaoBase == 6)
                {
                    if (listWorkflow.Count > 1)
                        throw new MyException("Selecione um documento por vez para executar a próxima ação.");

                    if (idPerfilAtivo != 3)
                        throw new MyException("Você não tem permissão para executar essa ação.");

                    if (DocumentoBloqueado(listWorkflow[0].IdDocumento))
                        throw new MyException(String.Format("O documento nº: {0}, está bloqueado. Não é possível executar a ação :(",
                    new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento).NumeroDocumento));

                    Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento);
                    Session["documento"] = null;
                    Session["documento"] = oDocumento;

                    Session["listWorkflow"] = null;
                    Session["listWorkflow"] = listWorkflow;
                    Response.Redirect(ResolveUrl(String.Format(ResolveUrl("~/Redirecionar.aspx?p={0}"), CriptografiaBusiness.Criptografar("103"))), false);
                }
                else if (idWorkflowAcaoBase == 40)
                {
                    throw new MyException("Não é possível executar essa ação. O documento já foi finalizado.");
                }
                else
                    throw new NotImplementedException();
            }
        }



        void ObterTempoAprovacao(Boolean visualizarWorkflow = false)
        {
            Int32 idPerfilAtivo = new Util().GetSessaoPerfilAtivo();
            List<Workflow> listWorkflow = ObterListWorkflow();
            
            if (visualizarWorkflow)
            {
                if (listWorkflow.Count > 1)
                    throw new MyException("Selecione um documento por vez para visualiar o workflow.");

                Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento);
                Session["documento"] = null;
                Session["documento"] = oDocumento;
                Response.Redirect(ResolveUrl(String.Format("~/Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("205"))), false);
            }            
                else
                    throw new NotImplementedException();
            
        }

        void ObterTempoMedioAprovacao()
        {
            //DataTable IDDocumento = 3020; //3047
            Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(0);
                Session["documento"] = null;
                Session["documento"] = oDocumento;
                Response.Redirect(ResolveUrl(String.Format("~/Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("205"))), false);
            
        }

        void GerarPdf()
        {
            List<Workflow> listWorkflow = ObterListWorkflow();

            if (listWorkflow.Count <= 0)
                throw new MyException("Nenhum documento selecionado.");

            if (listWorkflow.Count > 1)
                throw new MyException("Selecione um documento por vez para gerar o pdf.");

            Session["documentoBaixar"] = null;
            Session["documentoBaixar"] = new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento);
            Page.ClientScript.RegisterStartupScript(GetType(), "newOpen", Util.AbrirPopup(ResolveUrl("~/BaixarDocumento.ashx?p=a1a1"), 830, 650), false);
        }
        void GerarPlanilhaOrcamentaria()
        {
            Int32 totalItensSelecionados = 0;
            Int32 idDocumento = 0;

            if (Path.GetFileName(Request.Path) == "Pesquisa")
            {
                foreach (CheckBox checkbox in from RepeaterItem item in ((Repeater)contentBody.Controls[5]).Items
                                              where item.ItemType == ListItemType.Item
                                                  || item.ItemType == ListItemType.AlternatingItem
                                              select (CheckBox)item.FindControl("ckDocumento") into checkbox
                                              where checkbox.Checked
                                              select checkbox)
                {
                    totalItensSelecionados++;
                    idDocumento = Int32.Parse(checkbox.Attributes["idDocumento"]);
                    checkbox.Checked = false;
                }

                if (totalItensSelecionados > 1)
                    throw new MyException("Selecione um documento por vez para gerar a planilha orçamentaria.");
            }
            else
            {
                foreach (CheckBox checkbox in from RepeaterItem item in ((Repeater)contentBody.Controls[22]).Items
                                              where item.ItemType == ListItemType.Item
                                                  || item.ItemType == ListItemType.AlternatingItem
                                              select (CheckBox)item.FindControl("ckDocumento") into checkbox
                                              where checkbox.Checked
                                              select checkbox)
                {
                    totalItensSelecionados++;

                    idDocumento = Int32.Parse(checkbox.Attributes["idDocumento"]);

                    checkbox.Checked = false;
                }

                if (totalItensSelecionados > 1)
                    throw new MyException("Selecione um documento por vez para gerar a planilha orçamentaria.");
            }

            if (totalItensSelecionados <= 0)
                    throw new MyException("Nenhum documento selecionado.");



            Session["documentoBaixar"] = null;
            Session["documentoBaixar"] = new DocumentoBusiness().RecuperarDocumento(idDocumento);
            Page.ClientScript.RegisterStartupScript(GetType(), "newOpen", Util.AbrirPopup(ResolveUrl("~/BaixarDocumento.ashx?p=a2b2"), 830, 250), false);
        }
        void Detalhe()
        {
            List<Workflow> listWorkflow = ObterListWorkflow();

            if (listWorkflow.Count <= 0)
                throw new MyException("Nenhum documento selecionado.");

            if (listWorkflow.Count > 1)
                throw new MyException("Selecione um documento por vez para visualizar os detalhes.");


            Documento oDocumento = new DocumentoBusiness().RecuperarDocumento(listWorkflow[0].IdDocumento);
            Session["documento"] = null;
            Session["documento"] = oDocumento;
            Response.Redirect(ResolveUrl(String.Format("~/Redirecionar.aspx?p={0}", CriptografiaBusiness.Criptografar("208"))), false);
        }

        Boolean DocumentoBloqueado(Int32 idDocumento)
        {
            DocumentoBloqueado oDocumentoBloqueado = new DocumentoBloqueadoBusiness().ObterBloqueioAtivo(idDocumento);
            if (oDocumentoBloqueado != null)
            {
                Int32 idUsuarioBloqueio = oDocumentoBloqueado.IdUsuario;
                if (idUsuarioBloqueio != 0 && idUsuarioBloqueio != new Util().GetSessaoUsuario().IdUsuario)
                    return true;
                else if (idUsuarioBloqueio != 0 && idUsuarioBloqueio == new Util().GetSessaoUsuario().IdUsuario)
                    new DocumentoBloqueadoBusiness().RemoverBloqueio(idDocumento);
                return false;
            }
            else
                return false;
        }
        #endregion
    }
}