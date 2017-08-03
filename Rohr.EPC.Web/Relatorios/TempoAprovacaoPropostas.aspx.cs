using Rohr.EPC.Business;
using System;
using System.Data;
using System.Web.UI;


namespace Rohr.EPC.Web.Relatorios
{
    public partial class TempoAprovacaoPropostas : Page
    {

        public String Workflow;
        public DataTable DocsP = new DataTable("DocsP");
        public DataTable DocsC = new DataTable("DocsC");



        protected void Page_Load(object sender, EventArgs e)
        {
            CarregarWorkflowPropostas();
            CarregarWorkflowContratos();

        }    

        void CarregarWorkflowPropostas()
        {
            DocsP.Columns.Add("Acao", typeof(string));
            DocsP.Columns.Add("Perfil", typeof(string));
            DocsP.Columns.Add("Meta", typeof(string));
            DocsP.Columns.Add("TempoMedioTotal", typeof(string));

            DataTable CarregaDados = new WorkflowBusiness().ObterTempoAprovacaoProposta();

            string TempoMedio = null;
            decimal TempoMedioTotal = 0;

            for (int i= 0; i <= CarregaDados.Rows.Count - 1; i++)
            {
                TempoMedio = CarregaDados.Rows[i]["TempoMedioTotal"].ToString();
                TempoMedioTotal = Convert.ToDecimal(TempoMedio);
                                
                double min = (double)(TempoMedioTotal / 60);

                if (CarregaDados.Rows[i]["Perfil"].ToString() == "Diretoria Operacional")
                {
                   DocsP.Rows.Add(CarregaDados.Rows[i]["Acao"], CarregaDados.Rows[i]["Perfil"], "1 Dia", new Util().TratarTempoMinuto(Convert.ToDecimal(min)));
                }

                else
                {
                    DocsP.Rows.Add(CarregaDados.Rows[i]["Acao"], CarregaDados.Rows[i]["Perfil"], "1 Hora", new Util().TratarTempoMinuto(Convert.ToDecimal(min)));
                }
            }             

            Repeater1.DataSource = DocsP;
            Repeater1.DataBind();
        }


        void CarregarWorkflowContratos()
        {
            DocsC.Columns.Add("Acao", typeof(string));
            DocsC.Columns.Add("Perfil", typeof(string));
            DocsC.Columns.Add("Meta", typeof(string));
            DocsC.Columns.Add("TempoMedioTotal", typeof(string));

            DataTable CarregaDados = new WorkflowBusiness().ObterTempoAprovacaoContratos();

            string TempoMedio = null;
            decimal TempoMedioTotal = 0;

            for (int i = 0; i <= CarregaDados.Rows.Count - 1; i++)
            {
                TempoMedio = CarregaDados.Rows[i]["TempoMedioTotal"].ToString();
                TempoMedioTotal = Convert.ToDecimal(TempoMedio);

                double min = (double)(TempoMedioTotal / 60);

                if (CarregaDados.Rows[i]["Perfil"].ToString() == "Gerente"
                    || CarregaDados.Rows[i]["Perfil"].ToString() == "Superintendência"
                    || CarregaDados.Rows[i]["Perfil"].ToString() == "Vice-Presidência")
                {
                    DocsC.Rows.Add(CarregaDados.Rows[i]["Acao"], CarregaDados.Rows[i]["Perfil"], "1 Hora", new Util().TratarTempoMinuto(Convert.ToDecimal(min)));
                }

                else
                {
                    DocsC.Rows.Add(CarregaDados.Rows[i]["Acao"], CarregaDados.Rows[i]["Perfil"], "1 Dia", new Util().TratarTempoMinuto(Convert.ToDecimal(min)));

                }

            }

            Repeater2.DataSource = DocsC;
            Repeater2.DataBind();
        }


    }
}