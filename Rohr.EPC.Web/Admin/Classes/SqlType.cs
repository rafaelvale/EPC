using System;
using System.Web.UI;

namespace Lib.Classes
{
    public enum SqlType { Insert, Update, Delete, Select }

    public class SqlTypeParam
    {
        private StateBag m_ViewState = null;
        private String m_Name = null;

        public SqlTypeParam(StateBag viewState, String name)
        {
            m_ViewState = viewState;
            m_Name = name;
        }

        public SqlType Value
        {
            set
            {
                m_ViewState[m_Name] = value;
            }
            get
            {
                if (m_ViewState[m_Name] == null)
                    return default(SqlType);

                return (SqlType)m_ViewState[m_Name];
            }
        }

        /// <summary>
        /// True se Value == Insert
        /// </summary>
        public Boolean IsInsert
        {
            get { return Value == SqlType.Insert; }
        }

        /// <summary>
        /// True se Value == Update
        /// </summary>
        public Boolean IsUpdate
        {
            get { return Value == SqlType.Update; }
        }

        /// <summary>
        /// True se Value == Delete
        /// </summary>
        public Boolean IsDelete
        {
            get { return Value == SqlType.Delete; }
        }

        /// <summary>
        /// True se Value == Select
        /// </summary>
        public Boolean IsSelect
        {
            get { return Value == SqlType.Select; }
        }

        /// <summary>
        /// True se Value == Insert ou Value == Update
        /// </summary>
        public Boolean IsInsUpd
        {
            get { return Value == SqlType.Insert || Value == SqlType.Update; }
        }

        /// <summary>
        /// True se Value == Update ou Value == Delete
        /// </summary>
        public Boolean IsUpdDel
        {
            get { return Value == SqlType.Update || Value == SqlType.Delete; }
        }

        /* ----------- Mensagens ----------- */
        public String VerboTipo
        {
            get
            {
                if (Value == SqlType.Insert)
                    return "Cadastrar";
                if (Value == SqlType.Update)
                    return "Atualizar";
                if (Value == SqlType.Delete)
                    return "Excluir";
                return "Consultar";
            }
        }

        public String TituloM(String tabela, String registro)
        {
            if (Value == SqlType.Insert)
                return "Novo " + tabela;

            return tabela + " '" + registro + "'";
        }

        public String TituloF(String tabela, String registro)
        {
            if (Value == SqlType.Insert)
                return "Nova " + tabela;

            return tabela + " '" + registro + "'";
        }

        public String ConfirmacaoM
        {
            get
            {
                if (Value == SqlType.Insert)
                    return " cadastrado";
                if (Value == SqlType.Update)
                    return " atualizado";
                return " excluido";
            }
        }

        public String ConfirmacaoF
        {
            get
            {
                if (Value == SqlType.Insert)
                    return " cadastrada";
                if (Value == SqlType.Update)
                    return " atualizada";
                return " excluida";
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}