using Rohr.Data;
using Rohr.EPC.Business;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public partial class Visitas : System.Web.UI.Page
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        static int IDUpdate = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario sessaoUsuario;

            try
            {
                sessaoUsuario = new Util().GetSessaoUsuario();
                TextBoxComResp.Text = sessaoUsuario.PrimeiroNome;
                PreencheVisitas(sessaoUsuario.PrimeiroNome);                

            }
            catch (Exception ex)
            {                
                Util.ExibirMensagem(lblMensagem, ex.Message, Util.TipoMensagem.Erro);
                NLog.Log().Error(ex);
                ExcecaoBusiness.Adicionar(ex, HttpContext.Current.Request.Url.AbsolutePath);                
            }  
           
           
        }


        protected void linkButtonPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtPesquisar.Text))
                    throw new MyException("Informe um parametro para pesquisa");
                else if (txtPesquisar.Text.Length < 3)
                    throw new MyException("Parâmetro muito curto, o minímo necessário é de 3 caracteres.");
                else CarregaGrid(txtPesquisar.Text);
            }
            catch (Exception ex)
            {
                var msg = "<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "").Replace(Environment.NewLine, string.Empty) + "');</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "alert", msg);
            }
        }

        protected void CarregaGrid(string txtPesquisar)
        {          

            DataTable GetVisitaSemOportunidade = new WorkflowBusiness().GetVisitaSemOportunidade(txtPesquisar, 4);

            GrdVisitas.DataSource = GetVisitaSemOportunidade;
            GrdVisitas.DataBind();        
            
            
            _dbHelper.CloseConnection();
        }
        
        protected void btnLimpar_Click(object sender, EventArgs e)
        {

            Calendar1.Enabled = true;
            TextBoxNomeCli.Text = ""; TextBoxNomeCli.Enabled = true;
            TextBoxObra.Text = ""; TextBoxObra.Enabled = true;
            TextBoxContatoCliente.Text = ""; TextBoxContatoCliente.Enabled = true;
            TextBoxEmail.Text = ""; TextBoxEmail.Enabled = true;
            TextBoxTelefone.Text = ""; TextBoxTelefone.Enabled = true;
            TextBoxHoraVisita.Text = ""; TextBoxHoraVisita.Enabled = true;
            DropDownListAcompanhamento.SelectedIndex = 0; DropDownListAcompanhamento.Enabled = true;
            SetorContato.Enabled = true;
            Mercado.Enabled = true;
            Mercado.SelectedIndex = 0;
            SetorContato.SelectedIndex = 0;
            DropDownList1.SelectedIndex = 0;



            TextAreaAcom.Text = ""; TextAreaAcom.Enabled = true;
            TextBoxEndereco.Text = ""; TextBoxEndereco.Enabled = true;
            TextBoxBairro.Text = ""; TextBoxBairro.Enabled = true;
            TextBoxCidade.Text = ""; TextBoxCidade.Enabled = true;
            TextBoxCEP.Text = ""; TextBoxCEP.Enabled = true;
            DropDownList1.Enabled = true;
            btnSalvar.Enabled = true;
            btnUpdate.Enabled = false;
            IDUpdate = 0;
            Response.Redirect(Request.RawUrl, true);


        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";

            if (TextBoxNomeCli.Text == "")
            {
                Util.ExibirMensagem(lblMensagem, "Favor digitar o nome do cliente", Util.TipoMensagem.Alerta);
            }

            else if (TextBoxObra.Text == "")
            {
                Util.ExibirMensagem(lblMensagem, "Favor digitar o nome da obra", Util.TipoMensagem.Alerta);
            }

            else if (Calendar1.SelectedDate.ToString() == "")
            {
                Util.ExibirMensagem(lblMensagem, "Favor escolher uma data", Util.TipoMensagem.Alerta);

            }

            else if (TextBoxHoraVisita.Text == "")
            {
                Util.ExibirMensagem(lblMensagem, "Favor digitar um horário", Util.TipoMensagem.Alerta);

            }

            else
            {
                DbParametros parametros = new DbParametros();

                parametros.Adicionar(new DbParametro("@ID", IDUpdate));
                parametros.Adicionar(new DbParametro("@DataVisita", Calendar1.SelectedDate));
                parametros.Adicionar(new DbParametro("@HoraVisita", TextBoxHoraVisita.Text));
                parametros.Adicionar(new DbParametro("@Acompanhamento", DropDownListAcompanhamento.SelectedValue));
                parametros.Adicionar(new DbParametro("@AcompanhamentoAcao", TextAreaAcom.Text));



                _dbHelper.ExecutarScalar("UpdateVisitaSemOportunidade", parametros, CommandType.StoredProcedure);

                _dbHelper.CloseConnection();

                
               Util.ExibirMensagem(lblMensagem, "Registro alterado com sucesso", Util.TipoMensagem.Informacao);

                SetorContato.Enabled = true;
                Mercado.Enabled = true;
                TextBoxNomeCli.Enabled = true;
                TextBoxNomeCli.Text = "";
                TextBoxNomeCli.Enabled = true;
                TextBoxObra.Text = "";
                TextBoxObra.Enabled = true;
                TextBoxContatoCliente.Text = "";
                TextBoxContatoCliente.Enabled = true;
                TextBoxEmail.Text = "";
                TextBoxEmail.Enabled = true;
                TextBoxTelefone.Text = "";
                TextBoxTelefone.Enabled = true;
                TextBoxHoraVisita.Text = "";
                TextBoxHoraVisita.Enabled = true;
                TextAreaAcom.Text = "";
                TextAreaAcom.Enabled = true;
                TextBoxEndereco.Text = "";
                TextBoxEndereco.Enabled = true;
                TextBoxBairro.Text = "";
                TextBoxBairro.Enabled = true;
                TextBoxCidade.Text = "";
                TextBoxCidade.Enabled = true;
                TextBoxCEP.Text = "";
                TextBoxCEP.Enabled = true;
                IDUpdate = 0;
                Response.Redirect(Request.RawUrl, true);


            }


        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";

            if (TextBoxNomeCli.Text == "")
            {
                Util.ExibirMensagem(lblMensagem, "Favor digitar o nome do cliente", Util.TipoMensagem.Alerta);
            }

            else if (TextBoxObra.Text == "")
            {
                Util.ExibirMensagem(lblMensagem, "Favor digitar o nome da obra", Util.TipoMensagem.Alerta);
            }

            else if (Calendar1.SelectedDate.ToString() == "")
            {
                Util.ExibirMensagem(lblMensagem, "Favor escolher uma data", Util.TipoMensagem.Alerta);

            }

            else if (TextBoxHoraVisita.Text == "")
            {
                Util.ExibirMensagem(lblMensagem, "Favor digitar um horário", Util.TipoMensagem.Alerta);

            }


            else
            {

                DbParametros parametros = new DbParametros();
                parametros.Adicionar(new DbParametro("@ComercialResponsavel", TextBoxComResp.Text));
                parametros.Adicionar(new DbParametro("@NomeCliente", TextBoxNomeCli.Text));
                parametros.Adicionar(new DbParametro("@Obra", TextBoxObra.Text));
                parametros.Adicionar(new DbParametro("@ContatoCliente", TextBoxContatoCliente.Text));
                parametros.Adicionar(new DbParametro("@EMail", TextBoxEmail.Text));
                parametros.Adicionar(new DbParametro("@Telefone", TextBoxTelefone.Text));
                parametros.Adicionar(new DbParametro("@DataVisita", Calendar1.SelectedDate));
                parametros.Adicionar(new DbParametro("@HoraVisita", TextBoxHoraVisita.Text));
                parametros.Adicionar(new DbParametro("@Acompanhamento", DropDownListAcompanhamento.SelectedValue));
                parametros.Adicionar(new DbParametro("@AcompanhamentoAcao", TextAreaAcom.Text));
                parametros.Adicionar(new DbParametro("@SetorContato", SetorContato.SelectedValue));
                parametros.Adicionar(new DbParametro("@Mercado", Mercado.SelectedValue));
                parametros.Adicionar(new DbParametro("@Endereco", TextBoxEndereco.Text));
                parametros.Adicionar(new DbParametro("@Bairro", TextBoxBairro.Text));
                parametros.Adicionar(new DbParametro("@Cidade", TextBoxCidade.Text));
                parametros.Adicionar(new DbParametro("@Estado", DropDownList1.SelectedValue));
                parametros.Adicionar(new DbParametro("@CEP", TextBoxCEP.Text));

                _dbHelper.ExecutarScalar("addVisitaSemOportunidade", parametros, CommandType.StoredProcedure);

                _dbHelper.CloseConnection();

                Util.ExibirMensagem(lblMensagem, "Registro inserido com sucesso", Util.TipoMensagem.Informacao);



                DropDownList1.Enabled = true;

                Calendar1.SelectedDate = DateTime.Today;
                SetorContato.Enabled = true;
                Mercado.Enabled = true;
                TextBoxNomeCli.Enabled = true;
                TextBoxNomeCli.Text = "";
                TextBoxNomeCli.Enabled = true;
                TextBoxObra.Text = "";
                TextBoxObra.Enabled = true;
                TextBoxContatoCliente.Text = "";
                TextBoxContatoCliente.Enabled = true;
                TextBoxEmail.Text = "";
                TextBoxEmail.Enabled = true;
                TextBoxTelefone.Text = "";
                TextBoxTelefone.Enabled = true;
                TextBoxHoraVisita.Text = "";
                TextBoxHoraVisita.Enabled = true;
                TextAreaAcom.Text = "";
                TextAreaAcom.Enabled = true;
                TextBoxEndereco.Text = "";
                TextBoxEndereco.Enabled = true;
                TextBoxBairro.Text = "";
                TextBoxBairro.Enabled = true;
                TextBoxCidade.Text = "";
                TextBoxCidade.Enabled = true;
                TextBoxCEP.Text = "";
                TextBoxCEP.Enabled = true;
                IDUpdate = 0;
                Response.Redirect(Request.RawUrl, true);

            }

        }

        protected void TextBoxCEP_TextChanged(object sender, EventArgs e)
        {

            DataTable CarregaCEP = new WorkflowBusiness().ObterEnderecoDne(TextBoxCEP.Text);

            if (CarregaCEP.Rows.Count == 0)
            {
                Util.ExibirMensagem(lblMensagem, "CEP Não Encontrado", Util.TipoMensagem.Alerta);
            }

            else
            {

                TextBoxEndereco.Text = CarregaCEP.Rows[0]["log_no"].ToString();
                TextBoxBairro.Text = CarregaCEP.Rows[0]["bai_no"].ToString();
                TextBoxCidade.Text = CarregaCEP.Rows[0]["loc_no"].ToString();
                DropDownList1.Text = CarregaCEP.Rows[0]["ufe_sg"].ToString();
            }

        }


        void PreencheVisitas(String ComercialResponsavel)
        {
            DataTable GetVisitaSemOportunidade = new WorkflowBusiness().GetVisitaSemOportunidade(ComercialResponsavel, 1);

            DataTable Visitas = new DataTable("Visitas");
            
            Visitas.Columns.Add("Cliente", typeof(string));
            Visitas.Columns.Add("Obra", typeof(string));


            for (int i = 0; i <= GetVisitaSemOportunidade.Rows.Count - 1; i++)
            {
                Visitas.Rows.Add(GetVisitaSemOportunidade.Rows[i]["Cliente"].ToString(),
                                 GetVisitaSemOportunidade.Rows[i]["Obra"].ToString());

            }

            GrdVisitas.DataSource = Visitas;
            GrdVisitas.DataBind();

        }       
       
        

        protected void GetVisitasSemOportunidade(string id)
        {
            DataTable GetVisitaSemOportunidade = new WorkflowBusiness().GetVisitaSemOportunidade(id, 2);

            TextBoxNomeCli.Text = GetVisitaSemOportunidade.Rows[0]["NomeCliente"].ToString();
            TextBoxNomeCli.Enabled = false;
            TextBoxObra.Text = GetVisitaSemOportunidade.Rows[0]["Obra"].ToString();
            TextBoxObra.Enabled = false;
            TextBoxCEP.Text = GetVisitaSemOportunidade.Rows[0]["CEP"].ToString();
            TextBoxCEP.Enabled = false;
            TextBoxEndereco.Text = GetVisitaSemOportunidade.Rows[0]["Endereco"].ToString();
            TextBoxEndereco.Enabled = false;
            TextBoxBairro.Text = GetVisitaSemOportunidade.Rows[0]["Bairro"].ToString();
            TextBoxBairro.Enabled = false;
            TextBoxCidade.Text = GetVisitaSemOportunidade.Rows[0]["Cidade"].ToString();
            TextBoxCidade.Enabled = false;
            DropDownList1.Text = GetVisitaSemOportunidade.Rows[0]["Estado"].ToString();
            DropDownList1.Enabled = false;
            TextBoxContatoCliente.Text = GetVisitaSemOportunidade.Rows[0]["ContatoCliente"].ToString();
            TextBoxContatoCliente.Enabled = false;
            TextBoxTelefone.Text = GetVisitaSemOportunidade.Rows[0]["Telefone"].ToString();
            TextBoxTelefone.Enabled = false;
            TextBoxEmail.Text = GetVisitaSemOportunidade.Rows[0]["EMail"].ToString().Trim();
            TextBoxEmail.Enabled = false;
            Calendar1.SelectedDate = DateTime.Parse(GetVisitaSemOportunidade.Rows[0]["DataVisita"].ToString());

            SetorContato.SelectedItem.Text = GetVisitaSemOportunidade.Rows[0]["SetorContato"].ToString();
            SetorContato.Enabled = false;
            Mercado.SelectedItem.Text = GetVisitaSemOportunidade.Rows[0]["Mercado"].ToString();
            Mercado.Enabled = false;



            TextBoxHoraVisita.Text = GetVisitaSemOportunidade.Rows[0]["HoraVisita"].ToString();


            DropDownListAcompanhamento.SelectedItem.Text = GetVisitaSemOportunidade.Rows[0]["Acompanhamento"].ToString();

            TextAreaAcom.Text = GetVisitaSemOportunidade.Rows[0]["AcompanhamentoAcao"].ToString();
            btnSalvar.Enabled = true;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblresult.Text = "Grato Pai";
        }
    }
}