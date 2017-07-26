using System.Web;
using Rohr.EPC.Entity;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;

namespace Rohr.EPC.Business
{
    public class Util
    {
        public enum TipoMensagem
        {
            Informacao,
            Erro,
            Alerta
        }

        public Usuario GetSessaoUsuario()
        {
            Object sessaoUsuario = HttpContext.Current.Session["usuario"];
            if (sessaoUsuario == null)
                throw new MyException("Não foi possível recuperar a sessão do usuário");

            return (Usuario)sessaoUsuario;
        }
        public Int32 GetSessaoPerfilAtivo()
        {
            Object sessaoPerfilAtivo = HttpContext.Current.Session["perfil"];
            if (sessaoPerfilAtivo == null)
                throw new MyException("Não foi possível recuperar a sessão do perfil do usuário");

            return Int32.Parse(sessaoPerfilAtivo.ToString());
        }
        public Documento GetSessaoDocumento()
        {
            Object sessaoDocumento = HttpContext.Current.Session["documento"];
            if (sessaoDocumento == null)
                throw new MyException("Não foi possível recuperar a sessão documento");

            return (Documento)sessaoDocumento;
        }

        public static void ExibirMensagem(Label label, string mensagem, TipoMensagem tipoMensagem)
        {
            label.Text = mensagem;
            label.Visible = true;

            switch (tipoMensagem)
            {
                case TipoMensagem.Informacao:
                    label.CssClass = "alert alert-info fade.in";
                    break;
                case TipoMensagem.Erro:
                    label.CssClass = "alert alert-error fade.in";
                    break;
                case TipoMensagem.Alerta:
                    label.CssClass = "alert alert-danger fade.in";
                    break;
                default:
                    label.CssClass = "alert";
                    break;
            }
        }

        public static void CarregarValoresPadrao(DropDownList dropDownList, Chave chave)
        {
            if (chave.IdChave != 51)
            {
                dropDownList.CssClass = "input-mini";

                List<AplicacaoLista> listAplicacaoLista = new AplicacaoListaBusiness().ObterPorIdChave(chave.IdChave);

                foreach (AplicacaoLista aplicacaoLista in listAplicacaoLista)
                    dropDownList.Items.Add(new ListItem(aplicacaoLista.Descricao, aplicacaoLista.Descricao));
            }
            else
                CarregarQuantidadeHorasParaCarga(dropDownList);


            switch (chave.IdChave)
            {
                case 15:
                case 28:
                    dropDownList.SelectedIndex = 5;
                    break;
                case 10:
                    dropDownList.CssClass = "input-xlarge";
                    break;
                case 14:
                    dropDownList.SelectedIndex = 3;
                    break;
                case 9:
                    dropDownList.SelectedIndex = 14;
                    dropDownList.CssClass = "input-small";
                    break;
                case 40:
                case 25:
                    dropDownList.SelectedIndex = 1;
                    break;
                case 41:
                case 29:
                    dropDownList.SelectedIndex = 7;
                    break;
            }
        }
        private static void CarregarQuantidadeHorasParaCarga(DropDownList dropDownList)
        {
            dropDownList.Items.Add(new ListItem("1 Hora", "1"));
            for (int i = 2; i <= 12; i++)
                dropDownList.Items.Add(new ListItem(i + " Horas", i.ToString()));

            dropDownList.SelectedIndex = 4;
            dropDownList.CssClass = "input-small";
        }
        public static void CarregarDataBaseReajuste(TextBox textBox)
        {
            String mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            String mesPorExtenso = Char.ToUpper(mes[0]) + mes.Substring(1);
            textBox.Text = mesPorExtenso + "/" + DateTime.Now.Year;
        }
        public static void CarregarHorarioFuncionamentoUnidade(TextBox textBox)
        {
            textBox.Text = "Segunda a Quinta-feira das 7:30 as 17:30hs / Sexta-feira das 7:30 as 16:30hs";
        }
        public static void CarregarEnderecoRetirada(TextBox textBox, Documento documento)
        {
            CompanyAddress companyAddress = new CompanyAddress().ObterCompanyAddresses(documento.Filial.CodigoOrigem);
            string enderecoCompleto = String.Format("{0} - {1} - {2} - {3} - {4} - {5}", companyAddress.Address1, companyAddress.AltPhone, companyAddress.Address2, companyAddress.City.Description, companyAddress.States.StateKey, companyAddress.Zip);
            textBox.Text = enderecoCompleto;
        }
        public static void CarregarEnderecoDevolucao(TextBox textBox, Documento documento)
        {
            CompanyAddress companyAddress = new CompanyAddress().ObterCompanyAddresses(documento.Filial.CodigoOrigem);
            string enderecoCompleto = String.Format("{0} - {1} - {2} - {3} - {4} - {5}", companyAddress.Address1, companyAddress.AltPhone, companyAddress.Address2, companyAddress.City.Description, companyAddress.States.StateKey, companyAddress.Zip);
            textBox.Text = enderecoCompleto;
        }

        public void CarregarPainelDetalhes(List<Label> listaControles, Documento documento)
        {
            CarregarPainelDetalhes(listaControles, documento, null);
        }
        public void CarregarPainelDetalhes(List<Label> listaControles, Documento documento, Documento documentoProposta)
        {
            foreach (Label label in listaControles)
            {
                switch (label.ID)
                {
                    case "lblOportunidade":
                        if (documentoProposta != null && documentoProposta.NumeroDocumento != 0)
                            label.Text = documentoProposta.NumeroDocumento.ToString("N0");
                        else if (documento != null && documento.NumeroDocumento != 0)
                            label.Text = documento.NumeroDocumento.ToString("N0");
                        else
                            label.Text = "não foi possível recuperar";
                        break;
                    case "lblContrato":
                        label.Text = documento.EProposta == false && documento.NumeroDocumento != 0 ? documento.NumeroDocumento.ToString("N0") : "não foi possível recuperar";
                        break;
                    case "lblCliente":
                        label.Text = documento.DocumentoCliente != null && String.IsNullOrEmpty(documento.DocumentoCliente.Nome) == false ? documento.DocumentoCliente.Nome : "não foi possível recuperar";
                        break;
                    case "lblObra":
                        label.Text = documento.DocumentoObra != null && String.IsNullOrEmpty(documento.DocumentoObra.Nome) == false ? documento.DocumentoObra.Nome : "não foi possível recuperar";
                        break;
                    case "lblModelo":
                        label.Text = documento.Modelo != null && String.IsNullOrEmpty(documento.Modelo.Titulo) == false ? documento.Modelo.Titulo : "não foi possível recuperar";
                        break;
                    case "lblComercial":
                        label.Text = documento.DocumentoComercial != null && String.IsNullOrEmpty(documento.DocumentoComercial.PrimeiroNome) == false ? documento.DocumentoComercial.PrimeiroNome + " " + documento.DocumentoComercial.Sobrenome : "não foi possível recuperar";
                        break;
                }
            }
        }

        private static String TratarTempo(DateTime dataInicio, DateTime dataFim)
        {
            TimeSpan diferenca = dataFim.Subtract(dataInicio);
            String saida = String.Empty;

            if (diferenca.Days < 1)
            {
                if (diferenca.TotalMinutes <= 60)
                    saida = Math.Round(diferenca.TotalMinutes, 0) < 1 ? "menos de 1 min" : Math.Round(diferenca.TotalMinutes, 0) + " min";
                else
                {
                    saida = Math.Round(diferenca.TotalHours, 0) <= 1 ? "1 hora" : Math.Truncate(diferenca.TotalHours) + " horas";
                    saida += String.Format(" e {0}", diferenca.Minutes < 1 ? "menos de 1 min" : diferenca.Minutes + " min");
                }
            }
            else
            {
                if (diferenca.Days <= 30)
                {
                    saida = diferenca.Days == 1 ? "1 dia" : diferenca.Days + " dias";
                    saida += String.Format(" e {0}", (diferenca.Hours) <= 1 ? "1 hora" : diferenca.Hours + " horas");
                }
                else if (diferenca.Days / 30 >= 1 && diferenca.Days / 30 < 12)
                {
                    saida = (diferenca.Days / 30) == 1 ? "1 mês" : diferenca.Days / 30 + " meses";
                    saida += String.Format(" e {0}", (diferenca.Days % 30 <= 30) ? "1 dia" : (diferenca.Days % 30) + " dias");
                }
                else if (diferenca.Days / 360 >= 1)
                {
                    Int32 anos = (diferenca.Days / 360);
                    saida = anos == 1 ? String.Format("mais de {0} ano", anos) : String.Format("mais de {0} anos", anos);
                }
            }

            return saida;
        }
        public String TratarTempo(DateTime dataBase)
        {
            return TratarTempo(dataBase, DateTime.Now);
        }

        public String TratarTempoMinuto(Decimal minuto)
        {
            return TratarTempo(minuto);
        }
        public String TratarTempoFracao(Decimal fracaoDia)
        {
            return TratarTempo(((fracaoDia * 24) * 60));
        }
        static String TratarTempo(Decimal tempo)
        {
            String saida;
            Int32 minutosAux = Int32.Parse(Math.Round(tempo, 0).ToString());

            if ((minutosAux / 1440) > 0)
            {
                saida = (minutosAux / 1440) == 1 ? "1 dia" : (minutosAux / 1440) + " dias";

                if ((minutosAux % 1440) > 0)
                    saida += String.Format(" e {0} h", ((minutosAux % 1440) / 60));
            }
            else if ((minutosAux / 60) > 0)
            {
                saida = (minutosAux / 60) == 1 ? "1 hora" : (minutosAux / 60) + " horas";

                if ((minutosAux % 60) > 0)
                    saida += String.Format(" e {0} min", (minutosAux % 60));
            }
            else
                saida = (minutosAux <= 1) ? "menos de 1 min" : minutosAux + " min";

            return saida;
        }

        public String SubstituirChaves(String textoParte, Int32 idParte, Documento documento, Boolean editor)
        {
            String htmlFormatado = textoParte;
            var partes = documento.Modelo.ListParte.Where(x => x.IdParte == idParte);

            if (partes.Single().ListChave == null) return htmlFormatado;
            foreach (Chave chave in partes.Single().ListChave)
            {
                if (documento.ListChavePreenchida == null) continue;
                Chave chave1 = chave;
                var chavePreenchida = documento.ListChavePreenchida.Where(x => chave1 != null && x.IdChave == chave1.IdChave);

                if (chavePreenchida.Any())
                {
                    if (editor)
                    {
                        String saida;

                        String[] chavePreenchidaOriginal = chavePreenchida.Single().Texto.Split(' ');
                        String chavePreenchidaFormatada = chavePreenchidaOriginal.Where(t => !String.IsNullOrWhiteSpace(t.Trim())).Aggregate(String.Empty, (current, t) => current + (t + " "));

                        chavePreenchidaFormatada = chavePreenchidaFormatada.Trim();

                        if (String.IsNullOrWhiteSpace(chavePreenchida.Single().Texto))
                            saida = String.Format("<a href=\"{1}:{2}\">{0}</a>", "_", chave.ChaveDescricao, "_");
                        else
                            saida = String.Format("<a href=\"{1}:{2}\">{0}</a>", chavePreenchidaFormatada,
                                chave.ChaveDescricao, chavePreenchidaFormatada);
                        htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, saida);
                    }
                    else
                        htmlFormatado = htmlFormatado.Replace(chave.ChaveDescricao, chavePreenchida.Single().Texto);
                }
            }

            return htmlFormatado;
        }

        public static String GetNomeFilial(Int32 idFilial)
        {
            return new FilialBusiness().ObterFilial(idFilial).Descricao;
        }
        public static void ObterServidorDados(Label label)
        {
            label.Visible = System.Configuration.ConfigurationManager.ConnectionStrings["EPC"].ConnectionString.Contains("10.11.5.26");

            label.Text = "DATABASE - HOMOLOGAÇÃO";
        }
        public static Boolean VerificarModeloCliente(ModeloTipo modeloTipo)
        {
            return modeloTipo.IdModeloTipo == 3;
        }
        public static String AbrirPopup(String url, Int16 width, Int16 height)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<script type='text/javascript'> ");
            sb.Append(String.Format("var left = (screen.width / 2) - ({0}/2); ", width));
            sb.Append(String.Format("var top = (screen.height / 2) - ({0}/2); ", height));
            sb.Append(String.Format("popupWindow = window.open(\"" + url + "\",\"\",'width=' + {0} + ',height=' + {1} + ',top=10,left=' + left); ", width, height));
            sb.Append(@"</script> ");
            return sb.ToString();
        }
        public static void TratarTextoBotaoAcao(Button botaoPrimario, Button botaoSecundario, Int32 idWorkflowAcao)
        {
            switch (idWorkflowAcao)
            {
                case 79:
                    botaoPrimario.Text = "Contrato enviado para o cliente (via Cliente)";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 78:
                    botaoPrimario.Text = "Receber contrato do Jurídico (via Cliente)";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 77:
                    botaoPrimario.Text = "Enviar contrato para a filial (via Cliente)";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 76:
                    botaoPrimario.Text = "Arquivar contrato (via Rohr)";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 74:
                    botaoPrimario.Text = "Assinado pela Diretoria";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 70:
                    botaoPrimario.Text = "Aceitar contrato assinado";
                    botaoSecundario.Text = "Reprovar contrato assinado";
                    break;
                case 3:
                    botaoPrimario.Text = "Enviar para análise / assinatura do Cliente";
                    botaoSecundario.Text = "Reprovar para revisar";
                    break;
                case 49:
                case 55:
                case 56:
                case 2:
                case 1:
                case 7:
                    botaoPrimario.Text = "Aprovar";
                    botaoSecundario.Text = "Reprovar";
                    break;
                case 8:
                    botaoPrimario.Text = "Enviar contrato assinado para a Filial";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 9:
                    botaoPrimario.Text = "Receber contrato assinado da diretoria";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 10:
                    botaoPrimario.Text = "Enviar contrato para assinatura do cliente";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 11:
                    botaoPrimario.Text = "Receber contrato assinado do cliente";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 12:
                    botaoPrimario.Text = "Receber contrato assinado da filial";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 13:
                    botaoPrimario.Text = "Arquivar contrato";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 14:
                    botaoPrimario.Text = "Enviar contrato para arquivamento no Jurídico";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 65:
                    botaoPrimario.Text = "Enviar contrato para a filial";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 66:
                    botaoPrimario.Text = "Receber contrato do Jurídico";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                case 68:
                    botaoPrimario.Text = "Enviar contrato assinado para o Jurídico";
                    botaoSecundario.Text = "";
                    botaoSecundario.Visible = false;
                    break;
                default:
                    botaoPrimario.Text = "Aprovado / Assinado pelo cliente";
                    botaoSecundario.Text = "Cliente recusou";
                    break;
            }

        }
        public static String LimparDominioEmail(String email)
        {
            return email.Contains("@rohr") ? email.Substring(0, email.IndexOf("@", StringComparison.Ordinal)) : email;
        }

        public static String LimparNomeResumidoDescricao(String nomeResumido, String descricaoCompleta)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in descricaoCompleta.Trim())
            {
                if (!char.IsWhiteSpace(c))
                    sb.Append(c);
                else
                    sb.Append(" ");
            }

            if (String.Compare(sb.ToString().Substring(0, 2), "$$", StringComparison.Ordinal) == 0)
            {
                Int32 res;
                return (Int32.TryParse(sb.ToString().Substring(2, 4), out res)
                    ? sb.ToString().Substring(8, sb.Length - 8)
                    : sb.ToString().Substring(2, sb.Length - 2));
            }
            else if (String.CompareOrdinal(nomeResumido, "MO") == 0)
                return sb.ToString();
            else
            {
                Int32 tamanhoResumido = nomeResumido.Length;

                if (sb.ToString().Length > tamanhoResumido)
                {
                    return String.CompareOrdinal(sb.ToString().Substring(0, tamanhoResumido), nomeResumido) == 0
                            ? sb.ToString().Substring(tamanhoResumido + 3, sb.ToString().Length - (tamanhoResumido + 3))
                            : sb.ToString();
                }
                return sb.ToString();
            }
        }

        public enum TipoAcesso
        {
            Celular,
            Navegador
        }
        public static TipoAcesso ObterTipoAcesso(String http_user_agent)
        {
            String u = http_user_agent;
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                return TipoAcesso.Celular;
            else
                return TipoAcesso.Navegador;
        }
    }
}
