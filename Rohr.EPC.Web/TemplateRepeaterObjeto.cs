using Rohr.EPC.Business;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rohr.EPC.Web
{
    public class TemplateRepeaterObjeto : ITemplate
    {
        readonly ListItemType _templateType;
        public TemplateRepeaterObjeto(ListItemType type)
        {
            _templateType = type;
        }

        public void InstantiateIn(Control container)
        {
            PlaceHolder ph = new PlaceHolder();
            CheckBox checkBox = new CheckBox { ID = "ckExibir" };

            Label item1 = new Label();
            Label item2 = new Label();
            Label item3 = new Label();
            Label item4 = new Label();
            Label item5 = new Label();
            Label item6 = new Label();
            Label item7 = new Label();
            Label item8 = new Label();
            Label item9 = new Label();
            Label item10 = new Label();
            Label item11 = new Label();
            Label item12 = new Label();

            item1.ID = "resumida";
            item2.ID = "Descricao";
            item3.ID = "Quantidade";
            item4.ID = "UnidadeMedida";
            item5.ID = "Peso";
            item6.ID = "ValorTabelaLocacao";
            item7.ID = "ValorPraticadoLocacao";
            item8.ID = "ValorTabelaIndenizacao";
            item9.ID = "ValorPraticadoIndenizacao";

            //item7.ID = "ValorTabelaLocacao";
            //item8.ID = "ValorPraticadoLocacao";
            //item6.ID = "ValorTabelaIndenizacao";
            //item9.ID = "ValorPraticadoIndenizacao";
            item10.ID = "Desconto";
            item11.ID = "TotalLocacao";
            item12.ID = "PesoTotal";

            switch (_templateType)
            {
                case ListItemType.AlternatingItem:
                case ListItemType.Item:
                    ph.Controls.Add(new LiteralControl("<tr><td>"));
                    ph.Controls.Add(checkBox);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-nowrap'>"));
                    ph.Controls.Add(item1);
                    ph.Controls.Add(new LiteralControl("</td><td>"));
                    ph.Controls.Add(item2);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-right'>"));
                    ph.Controls.Add(item3);
                    ph.Controls.Add(new LiteralControl("</td><td>"));
                    ph.Controls.Add(item4);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-right'>"));
                    ph.Controls.Add(item5);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-right'>"));
                    ph.Controls.Add(item6);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-right' style='background-color: #ffffcc'>"));
                    ph.Controls.Add(item7);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-right'>"));
                    ph.Controls.Add(item8);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-right' style='background-color: #ffffcc'>"));
                    ph.Controls.Add(item9);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-right' style='background-color: #ffffcc'>"));
                    ph.Controls.Add(item10);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-right' style='background-color: #ffffcc'>"));
                    ph.Controls.Add(item11);
                    ph.Controls.Add(new LiteralControl("</td><td class='text-right'>"));
                    ph.Controls.Add(item12);
                    ph.Controls.Add(new LiteralControl("</td></tr>"));
                    ph.DataBinding += Item_DataBinding;
                    break;
                case ListItemType.Header:
                    ph.Controls.Add(new LiteralControl("<table class='table table-condensed-small table-hover table-bordered tabelaObjeto'>" +
                                                       "<thead>" +
                                                       "<tr>" +
                                                       "<th class='text-nowrap' style=\"width: 15px;\"><input type='checkbox' class='checkBoxAllObjeto' checked></th>" +
                                                       "<th class='text-nowrap' style=\"width: 65px;\">Resumido</th>" +
                                                       "<th class='text-nowrap' style=\"width: 260px;\">Descrição</th>" +
                                                       "<th class='text-nowrap' style=\"width: 50px;\">Quant.</th>" +
                                                       "<th class='text-nowrap' style=\"width: 20px;\" title=\"Unidade de Medida\">UM</th>" +
                                                       "<th class='text-nowrap' style=\"width: 50px;\">Peso</th>" +
                                                       "<th class='text-nowrap' style=\"width: 45px;\" title=\"Valor Unitário de Locação - TABELA\">V.U.L(R$)</th>" +
                                                       "<th class='text-nowrap' style=\"width: 45px;\" title=\"Valor Unitário de Locação - PRATICADO\">V.U.L Prat (R$)</th>" +
                                                       "<th class='text-nowrap' style=\"width: 45px;\" title=\"Valor Unitário de Indenização - TABELA\">V.U.I(R$)</th>" +
                                                       "<th class='text-nowrap' style=\"width: 45px;\" title=\"Valor Unitário de Indenização - PRATICADO\">V.U.I Prat (R$)</th>" +
                                                       "<th class='text-nowrap' style=\"width: 40px;\" title=\"Desconto (%)\">Desc. (%)</th>" +
                                                       "<th class='text-nowrap' style=\"width: 50px;\">Total(R$)</th>" +
                                                       "<th class='text-nowrap' style=\"width: 50px;\">Peso total</th>" +
                                                       "</tr>" +
                                                       "</thead>"));
                    break;
                case ListItemType.Footer:
                    ph.Controls.Add(new LiteralControl("</table>"));
                    break;
            }
            container.Controls.Add(ph);
        }

        static void Item_DataBinding(object sender, EventArgs e)
        {
            PlaceHolder ph = (PlaceHolder)sender;
            RepeaterItem ri = (RepeaterItem)ph.NamingContainer;

            Double quantidade = (Double)DataBinder.Eval(ri.DataItem, "Quantidade");
            Double peso = (Double)DataBinder.Eval(ri.DataItem, "Peso");
            Double valorTabelaLocacao = (Double)DataBinder.Eval(ri.DataItem, "ValorTabelaLocacao");
            Double valorPraticadoLocacao = (Double)DataBinder.Eval(ri.DataItem, "ValorPraticadoLocacao");
            Double valorTabelaIndenizacao = (Double)DataBinder.Eval(ri.DataItem, "ValorTabelaIndenizacao");
            Double valorPraticadoIndenizacao = (Double)DataBinder.Eval(ri.DataItem, "ValorPraticadoIndenizacao");
            Double desconto = (Double)DataBinder.Eval(ri.DataItem, "Desconto");
            Double totalLocacao = (Double)DataBinder.Eval(ri.DataItem, "TotalLocacao");
            Double pesoTotal = quantidade * peso;

            ((CheckBox) ph.FindControl("ckExibir")).Checked = Convert.ToBoolean(DataBinder.Eval(ri.DataItem, "Exibir"));
            ((CheckBox)ph.FindControl("ckExibir")).Attributes.Add("idItemObjetoOrcamento", DataBinder.Eval(ri.DataItem, "Id").ToString());
            ((CheckBox)ph.FindControl("ckExibir")).Attributes.Add("codigoItem", DataBinder.Eval(ri.DataItem, "Codigo").ToString());
            ((CheckBox)ph.FindControl("ckExibir")).Attributes.Add("idTabelaPreco", DataBinder.Eval(ri.DataItem, "TabelaItensID").ToString());
            ((CheckBox)ph.FindControl("ckExibir")).Attributes.Add("codigoSubGrupoItem", DataBinder.Eval(ri.DataItem, "ItemGroupId").ToString());

            ((Label)ph.FindControl("resumida")).Text = DataBinder.Eval(ri.DataItem, "DescricaoResumida").ToString();
            ((Label)ph.FindControl("descricao")).Text = Util.LimparNomeResumidoDescricao(DataBinder.Eval(ri.DataItem, "DescricaoResumida").ToString(), DataBinder.Eval(ri.DataItem, "Descricao").ToString());
            ((Label)ph.FindControl("UnidadeMedida")).Text = DataBinder.Eval(ri.DataItem, "UnidadeMedida").ToString();

            ((Label)ph.FindControl("Peso")).Text = String.Format("{0:N4}", peso);
            ((Label)ph.FindControl("Quantidade")).Text = String.Format("{0:N4}", quantidade);
            ((Label)ph.FindControl("ValorTabelaLocacao")).Text = String.Format("{0:N2}", valorTabelaLocacao);
            ((Label)ph.FindControl("ValorPraticadoLocacao")).Text = String.Format("{0:N2}", valorPraticadoLocacao);
            ((Label)ph.FindControl("ValorTabelaIndenizacao")).Text = String.Format("{0:N2}", valorTabelaIndenizacao);
            ((Label)ph.FindControl("ValorPraticadoIndenizacao")).Text = String.Format("{0:N2}", valorPraticadoIndenizacao);
            ((Label)ph.FindControl("Desconto")).Text = String.Format("{0:N2}", desconto);
            ((Label)ph.FindControl("TotalLocacao")).Text = String.Format("{0:N2}", totalLocacao);
            ((Label)ph.FindControl("PesoTotal")).Text = String.Format("{0:N2}", pesoTotal);
        }
    }
}