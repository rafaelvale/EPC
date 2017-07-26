using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;

namespace Lib.Classes
{
    public interface ICustomControl
    {
        String DispString { get; set; }
        Boolean ReadOnly { set; get; }
    }

    public interface IListCustomControl : ICustomControl
    {
        ListControl ListControl { get; }
    }

    public static class Conv
    {
        /// <summary>
        /// Value or Default
        /// </summary>
        public static T ValueOD<T>(this Nullable<T> value) where T : struct
        {
            return value.GetValueOrDefault();
        }

        public static Boolean IsNullOrEmpty(this String str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static Boolean IsNullOrWhiteSpace(this String str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        public static String Left(this String str, Int32 n)
        {
            if (str == null)
                return String.Empty;

            return str.Length > n ? str.Substring(0, n) : str;
        }

        // http://stackoverflow.com/questions/271398/what-are-your-favorite-extension-methods-for-c-codeplex-com-extensionoverflow
        public static bool In<T>(this T source, params T[] list)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return list.Contains(source);
        }

        // Idem
        public static void ThrowIfNull<T>(this T obj, String parameterName = "object") where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName + " not allowed to be null");
        }

        // WJan ...
        public static T CastOrNew<T>(this Object obj) where T : class, new()
        {
            return obj as T ?? new T();
        }

        // WJan 2 ...
        public static T Cast<T>(this Object obj) where T : class
        {
            var Res = obj as T;
            if (Res == null)
                throw new Exception("Invalid Cast");
            return Res;
        }

        /// <summary>
        /// Case Insensitive Equal
        /// </summary>
        public static Boolean EqualsCI(this String str, String compare)
        {
            str = str ?? String.Empty;
            return str.Equals(compare, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Case Insensitive Equal
        /// </summary>
        public static Boolean ContainsCI(this String str, String compare)
        {
            str = str ?? String.Empty;
            return str.IndexOf(compare, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        /// <summary>
        /// Case Insensitive StartsWith
        /// </summary>
        public static Boolean StartsWithCI(this String str, String compare)
        {
            str = str ?? String.Empty;
            return str.StartsWith(compare, true, new CultureInfo(1046, true));
        }

        /// <summary>
        /// True if str contains ,id,
        /// </summary>
        public static Boolean ContainsDelimID(this String str, Int32? id)
        {
            str = str ?? String.Empty;
            var strId = "," + id.ToString() + ",";
            return str.IndexOf(strId) >= 0;
        }

        public static String MaxLen(this String value, Int32 maxLen)
        {
            value = value ?? String.Empty;

            return value.Length > maxLen ?
                value.Substring(0, maxLen) : value;
        }

        // http://stackoverflow.com/questions/244531/is-there-an-alternative-to-string-replace-that-is-case-insensitive
        public static String ReplaceStringCI(this String str, String oldValue, String newValue)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, StringComparison.CurrentCultureIgnoreCase);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, StringComparison.CurrentCultureIgnoreCase);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        public static String ToSqlString(this String str)
        {
            return str != null ? "'" + str.Replace("'", "''") + "'" : "null";
        }

        public static String ToSqlLike(this String str)
        {
            str = str ?? string.Empty;
            return "'%" + str.Replace("'", "''") + "%'";
        }

        public static int MonthDifference(this DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }

        // Converte uma data para String
        public static String DateToString(this DateTime Dt)
        {
            return Dt.ToString("dd/MM/yyyy");
        }

        public static String DateToString(this DateTime? Dt)
        {
            return Dt.HasValue ? Dt.Value.DateToString() : String.Empty;
        }

        // Converte uma data para String
        public static String DateTimeToString(this DateTime Dt)
        {
            return Dt.ToString("dd/MM/yyyy HH:mm");
        }

        public static String DateTimeToString(this DateTime? Dt)
        {
            return Dt.HasValue ? Dt.Value.DateTimeToString() : String.Empty;
        }

        // Converte uma Data para um String SQL
        public static String DateToSqlString(this DateTime Dt)
        {
            return "'" + Dt.ToString("yyyy-MM-dd") + "'";
        }

        public static String DateToSqlString(this DateTime? Dt)
        {
            return Dt.HasValue ? Dt.Value.DateToSqlString() : "null";
        }

        // Converte String para Data
        public static DateTime StringToDate(this String Str)
        {
            return DateTime.Parse(Str, new CultureInfo(1046, true),
                DateTimeStyles.NoCurrentDateDefault);
        }

        public static String IntToSqlString(this Int32? value)
        {
            return value.HasValue ? value.ToString() : "null";
        }

        public static String SegundosToString(this Int32? segundos)
        {
            var Res = new StringBuilder();
            Res.Append((segundos.GetValueOrDefault() / 60).ToString());
            Res.Append(":");
            Res.Append((segundos.GetValueOrDefault() % 60).ToString("00"));
            return Res.ToString();
        }

        static public DateTime DataInicioDefault(this DateTime? data)
        {
            return data ?? new DateTime(1900, 1, 1);
        }

        static public DateTime DataTerminoDefault(this DateTime? data)
        {
            return data ?? new DateTime(2099, 12, 31);
        }

        /// <summary>
        /// Retorna verdadeiro se os dois periodos se sobrepoem
        /// http://stackoverflow.com/questions/325933/determine-whether-two-date-ranges-overlap
        /// </summary>
        public static Boolean DateOverlap(DateTime? inicioA,
            DateTime? terminoA, DateTime? inicioB, DateTime? terminoB)
        {
            return (inicioA.DataInicioDefault() <= terminoB.DataTerminoDefault())
                && (terminoA.DataTerminoDefault() >= inicioB.DataInicioDefault());
        }

        // Converte um String para um Decimal
        public static decimal StringToDecimal(this String Str)
        {
            return decimal.Parse(Str.Replace(".", String.Empty),
                NumberStyles.Float, new CultureInfo(1046, true));
        }

        // Converte um Decimal para um String com 2 casos decimais
        public static String DecimalToString(this Decimal dec, Int32 scale = 2)
        {
            return dec.ToString("#,##0.00000000".Substring(0, 6 + scale),
                        new CultureInfo(1046, true));
        }

        public static String DecimalToString(this Decimal? dec, Int32 scale = 2)
        {
            return dec.HasValue ? dec.Value.DecimalToString(scale) : String.Empty;
        }

        public static String DecimalToSqlString(this Decimal dec, Int32 scale = 2)
        {
            return dec.ToString("0.000000".Substring(0, 2 + scale),
                        new CultureInfo("en-US", true));
        }

        public static String DecimalToSqlString(this Decimal? dec, Int32 scale = 2)
        {
            return dec.HasValue ? dec.Value.DecimalToSqlString(scale) : "null";
        }

        public static String BoolToString(this Boolean? value)
        {
            return value.GetValueOrDefault() ? "S" : "N";
        }

        // retorna um DateTime sem mili segundos
        public static DateTime DateSemMSec(this DateTime Dt)
        {
            return new DateTime(Dt.Year, Dt.Month, Dt.Day, Dt.Hour, Dt.Minute, Dt.Second);
        }

        public static List<String> SepString(this String str)
        {
            Char SepChar = Convert.ToChar(System.Configuration.ConfigurationManager.AppSettings["SepCharCSV"]);

            List<String> Result = new List<String>();

            Int32 Quotes = 0;
            StringBuilder Field = new StringBuilder();
            foreach (Char C in str)
            {
                if (C == '\"')
                {
                    Quotes++;
                    if (Quotes == 3)
                    {
                        // 2 quotes viram 1 !
                        Field.Append(C);
                        Quotes = 1;
                    }
                }
                // Se for separador e não tiver em quote 
                // (Quotes == 1), fecha o campo
                else if (C == SepChar && Quotes != 1)
                {
                    Result.Add(Field.ToString());
                    Field = new StringBuilder();
                    Quotes = 0;
                }
                else
                    Field.Append(C);   // adiciona !
            }
            Result.Add(Field.ToString());

            return Result;
        }

        /// <summary>
        /// Transforma o codigo num Int64, descarta tudo que nao e digito
        /// </summary>
        /// <param name="Codigo"></param>
        /// <returns>O Codigo transformado em numero</returns>
        public static Int64 StringToInt64(this String Codigo)
        {
            Int32 Digs = 0, Valor0 = Convert.ToInt32('0');
            Int64 Result = 0;

            foreach (char C in Codigo)
            {
                if (Char.IsDigit(C) && Digs <= 18)
                {
                    Result = Result * 10 + Convert.ToInt32(C) - Valor0;

                    // Soma a qtde de digitos, discartar os zeros na frente
                    if (Result > 0)
                        Digs++;
                }
            }
            return Result;
        }

        public static Int64 Bytes2Int64(this byte[] Bytes)
        {
            Int32 J = Bytes.Length;
            Int64 Result = 0;

            for (int I = 0; I < J; I++)
            {
                Int32 D = Bytes[I];

                Result *= 0x100;
                Result += D;
            }
            return Result;
        }

        public static String StrRight(this String original, Int32 numberCharacters)
        {
            return original.Substring(numberCharacters > original.Length ? 0 : original.Length - numberCharacters);
        }
    }

    public class ControlInit
    {
        private Boolean m_CopyValue = false;

        public ControlInit(Boolean copyValue)
        {
            m_CopyValue = copyValue;
        }
        public static void SetListControl(ListControl listControl, String value)
        {
            try
            {
                listControl.SelectedValue = value;
                return;
            }
            catch { }

            if (listControl.Items.Count > 0)
                listControl.SelectedIndex = 0;
        }

        // Transfere formato ,1,2,3, para os itens
        public static void SetCheckBoxList(ListItemCollection items, String value)
        {
            // Primeiro apaga todos
            foreach (ListItem Item in items)
                Item.Selected = false;

            // Começa no 1 pq este tipo de string começa com ,
            Int32 Inic = 1, Len = value.Length;
            while (Inic < Len)
            {
                // Separa um elemento da lista
                Int32 Pos = value.IndexOf(",", Inic);
                Pos = Pos > -1 ? Pos : Len;
                String ItemStr = value.Substring(Inic, Pos - Inic);
                Inic = Pos + 1;

                // Procura o item e seleciona o mesmo
                foreach (ListItem Item in items)
                    if (Item.Value == ItemStr)
                        Item.Selected = true;
            }
        }
    }

    public class UserErrors
    {
        private StringBuilder m_UserErrors = new StringBuilder();

        private Boolean m_GotError = false;
        public Boolean GotError
        {
            get { return m_GotError; }
        }

        public void AddError(String fieldName, String error)
        {
            m_UserErrors.Append("'");
            m_UserErrors.Append(fieldName);
            m_UserErrors.Append("' ");
            m_UserErrors.Append(error);
            m_UserErrors.AppendLine("<br />");

            m_GotError = true;
        }

        public override String ToString()
        {
            return m_UserErrors.ToString();
        }
    }

    public delegate Boolean ValidateFieldEvent(
        FieldValidation fval, String fieldName, ref String dispString);

    // Validate fields and add user errors to StringBuilder
    public class FieldValidation
    {
        public const String msgCampoObrig = "Favor informar o campo";
        public const String msgFormatoInvalido = "Favor corrigir o formato do campo";

        // Concatena os items selecionados, separando com ,
        public static String GetCheckBoxList(ListItemCollection items)
        {
            bool TemSel = false;
            StringBuilder Result = new StringBuilder(",");

            foreach (ListItem Item in items)
                if (Item.Selected)
                {
                    Result.Append(Item.Value);
                    Result.Append(",");
                    TemSel = true;
                }
            return TemSel ? Result.ToString() : String.Empty;
        }

        private Boolean m_Validate = false;
        public UserErrors m_UserErrors = null;
        public UserErrors UserErrors
        {
            get { return m_UserErrors; }
        }

        public FieldValidation(Boolean validate)
        {
            m_UserErrors = new UserErrors();
            m_Validate = validate;
        }

        public Boolean GotError
        {
            get { return m_UserErrors.GotError; }
        }

        public String ErrorsToString()
        {
            return m_UserErrors.ToString();
        }

        public void ThrowIfError()
        {
            if (m_UserErrors.GotError)
                throw new UserException(m_UserErrors.ToString());
        }
    }

    public class UserException : Exception
    {
        public UserException(string p)
            : base(p)
        {
        }
    }

    public class ButtonCommand
    {
        private IButtonControl m_Btn = null;

        public ButtonCommand(object sender)
        {
            m_Btn = sender as IButtonControl;
            if (m_Btn == null)
                throw new Exception("sender não é IButtonControl");
        }

        public String Command
        {
            get { return m_Btn.CommandName; }
        }

        public SqlType SqlType
        {
            get
            {
                try
                {
                    return (SqlType)Enum.Parse(typeof(SqlType), m_Btn.CommandName, true);
                }
                catch { }
                return SqlType.Select;
            }
        }

        public String Argument
        {
            get { return m_Btn.CommandArgument; }
        }

        public Int32 ArgumentAsInt
        {
            get
            {
                Int32 Result = 0;
                Int32.TryParse(m_Btn.CommandArgument, out Result);
                return Result;
            }
        }
    }

    // Para ajudar a construção de Links
    // Com a possibilidade de posteriormente encryptar os argumentos...
    public class UrlBuilder
    {
        private String m_ArgPref;
        private StringBuilder m_URL;

        public UrlBuilder(String pageName)
        {
            m_URL = new StringBuilder();
            m_ArgPref = "?";
            m_URL.Append(pageName);
        }

        public void AppendArgument(String name, String value)
        {
            m_URL.Append(m_ArgPref);
            m_URL.Append(name);
            m_URL.Append("=");
            m_URL.Append(HttpUtility.UrlEncode(value));

            m_ArgPref = "&";
        }

        public void AppendArgument(String name, Int32? value)
        {
            AppendArgument(name, value.ValueOD().ToString());
        }

        public void AppendArgument(String name, DateTime? value)
        {
            AppendArgument(name, value.ValueOD().DateToString());
        }

        public void AppendArgument(String name, Guid value)
        {
            AppendArgument(name, value.ToString());
        }

        public void AppendSqlType(SqlType sqlType)
        {
            AppendSqlType(sqlType.ToString());
        }

        public void AppendSqlType(String sqlType)
        {
            AppendArgument("SqlType", sqlType);
        }

        public void AppendPrevHID(Int32 histryID)
        {
            AppendArgument("PrevHID", histryID.ToString());
        }

        public override String ToString()
        {
            return m_URL.ToString();
        }

    }

    /// <summary>
    /// Para ajudar na construção de Listas
    /// </summary>
    public class ListBuilder
    {
        private String m_ListSep;
        private String m_CurSep;
        private StringBuilder m_List;

        public ListBuilder(String separator)
        {
            m_List = new StringBuilder();
            m_CurSep = String.Empty;
            m_ListSep = separator;
        }

        public ListBuilder()
            : this(",")
        {
        }

        public void Append(String item)
        {
            m_List.Append(m_CurSep);
            m_List.Append(item);

            m_CurSep = m_ListSep;
        }

        public override String ToString()
        {
            return m_List.ToString();
        }
    }
}
