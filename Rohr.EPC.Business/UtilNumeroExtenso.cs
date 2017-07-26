using System;

namespace Rohr.EPC.Business
{
    public static class UtilNumeroExtenso
    {
        public static string ObterValorPorExtenso(decimal valor)
        {
            if (valor < 0 | valor >= 1000000000000000)
                return "";

            if (valor == 0)
                return "Zero";

            string strValor = valor.ToString("000000000000000.00");
            string valor_por_extenso = string.Empty;

            for (int i = 0; i <= 15; i += 3)
            {
                valor_por_extenso += escreva_parte(Convert.ToDecimal(strValor.Substring(i, 3)));
                if (i == 0 & valor_por_extenso != string.Empty)
                {
                    if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                        valor_por_extenso += " trilhão" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " e " : string.Empty);
                    else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                        valor_por_extenso += " trilhões" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " e " : string.Empty);
                }
                else if (i == 3 & valor_por_extenso != string.Empty)
                {
                    if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                        valor_por_extenso += " bilhão" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " e " : string.Empty);
                    else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                        valor_por_extenso += " bilhões" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " e " : string.Empty);
                }
                else if (i == 6 & valor_por_extenso != string.Empty)
                {
                    if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                        valor_por_extenso += " milhão" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " e " : string.Empty);
                    else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                        valor_por_extenso += " milhões" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " e " : string.Empty);
                }
                else if (i == 9 & valor_por_extenso != string.Empty)
                    if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                        valor_por_extenso += " mil" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " e " : string.Empty);

                if (i == 12)
                {
                    if (valor_por_extenso.Length > 8)
                        if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "bilhão" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "milhão")
                            valor_por_extenso += " de";
                        else
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "bilhões" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "milhões" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "trilhões")
                                valor_por_extenso += " de";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "trilhões")
                                    valor_por_extenso += " de";

                    if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                        valor_por_extenso += " real";
                    else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                        valor_por_extenso += " reais";

                    if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                        valor_por_extenso += " e ";
                }

                if (i == 15)
                    if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                        valor_por_extenso += " centavo";
                    else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                        valor_por_extenso += " centavos";
            }
            return valor_por_extenso;
        }
        static string escreva_parte(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            string montagem = string.Empty;
            if (valor > 0 & valor < 1)
            {
                valor *= 100;
            }
            string strValor = valor.ToString("000");
            int a = Convert.ToInt32(strValor.Substring(0, 1));
            int b = Convert.ToInt32(strValor.Substring(1, 1));
            int c = Convert.ToInt32(strValor.Substring(2, 1));

            if (a == 1) montagem += (b + c == 0) ? "cem" : "cento";
            else if (a == 2) montagem += "duzentos";
            else if (a == 3) montagem += "trezentos";
            else if (a == 4) montagem += "quatrocentos";
            else if (a == 5) montagem += "quinhetos";
            else if (a == 6) montagem += "seiscentos";
            else if (a == 7) montagem += "setecentos";
            else if (a == 8) montagem += "oitocentos";
            else if (a == 9) montagem += "novecentos";

            switch (b)
            {
                case 1:
                    switch (c)
                    {
                        case 0:
                            montagem += ((a > 0) ? " e " : string.Empty) + "dez";
                            break;
                        case 1:
                            montagem += ((a > 0) ? " e " : string.Empty) + "onze";
                            break;
                        case 2:
                            montagem += ((a > 0) ? " e " : string.Empty) + "doze";
                            break;
                        case 3:
                            montagem += ((a > 0) ? " e " : string.Empty) + "treze";
                            break;
                        case 4:
                            montagem += ((a > 0) ? " e " : string.Empty) + "quatorze";
                            break;
                        case 5:
                            montagem += ((a > 0) ? " e " : string.Empty) + "quinze";
                            break;
                        case 6:
                            montagem += ((a > 0) ? " e " : string.Empty) + "dezesseis";
                            break;
                        case 7:
                            montagem += ((a > 0) ? " e " : string.Empty) + "dezessete";
                            break;
                        case 8:
                            montagem += ((a > 0) ? " e " : string.Empty) + "dezoito";
                            break;
                        case 9:
                            montagem += ((a > 0) ? " e " : string.Empty) + "dezenove";
                            break;
                    }
                    break;
                case 2:
                    montagem += ((a > 0) ? " e " : string.Empty) + "vinte";
                    break;
                case 3:
                    montagem += ((a > 0) ? " e " : string.Empty) + "trinta";
                    break;
                case 4:
                    montagem += ((a > 0) ? " e " : string.Empty) + "quarente";
                    break;
                case 5:
                    montagem += ((a > 0) ? " e " : string.Empty) + "cinquenta";
                    break;
                case 6:
                    montagem += ((a > 0) ? " e " : string.Empty) + "sessenta";
                    break;
                case 7:
                    montagem += ((a > 0) ? " e " : string.Empty) + "setenta";
                    break;
                case 8:
                    montagem += ((a > 0) ? " e " : string.Empty) + "oitenta";
                    break;
                case 9:
                    montagem += ((a > 0) ? " e " : string.Empty) + "noventa";
                    break;
            }

            if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " e ";

            if (strValor.Substring(1, 1) == "1") return montagem;
            switch (c)
            {
                case 1:
                    montagem += "um";
                    break;
                case 2:
                    montagem += "dois";
                    break;
                case 3:
                    montagem += "três";
                    break;
                case 4:
                    montagem += "quatro";
                    break;
                case 5:
                    montagem += "cinco";
                    break;
                case 6:
                    montagem += "seis";
                    break;
                case 7:
                    montagem += "sete";
                    break;
                case 8:
                    montagem += "oito";
                    break;
                case 9:
                    montagem += "nove";
                    break;
            }

            return montagem;
        }
    }
}