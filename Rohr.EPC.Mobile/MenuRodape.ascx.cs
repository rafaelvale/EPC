using Rohr.EPC.Business;
using System;

namespace Rohr.EPC.Mobile
{
    public partial class MenuRodape : System.Web.UI.UserControl
    {
        Int16 _menu;
        Int32 _totalOportunidades;
        Int32 _totalContratos;
        String _menuProposta;
        String _menuContrato;
        String _descricaoPerfil;

        protected Int32 TotalOportunidades
        {
            get
            {
                return _totalOportunidades;
            }
        }
        protected Int32 TotalContratos
        {
            get
            {
                return _totalContratos;
            }
        }
        protected String MenuProposta
        {
            get
            {
                return _menuProposta;
            }
        }
        protected String MenuContrato
        {
            get
            {
                return _menuContrato;
            }
        }
        protected String DescricaoPerfil
        {
            get
            {
                return _descricaoPerfil;
            }
        }
        public Int16 Menu
        {
            set
            {
                _menu = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _descricaoPerfil = new PerfilBusiness().ObterPorId(new Util().GetSessaoPerfilAtivo()).Descricao;
                _totalContratos = Convert.ToInt32(Session["totalContrato"].ToString());
                _totalOportunidades = Convert.ToInt32(Session["totalOportunidade"].ToString());

                if (_menu == 1 || _menu == 0)
                {
                    _menuContrato = String.Empty;
                    _menuProposta = "active";
                }
                else if (_menu == 2)
                {
                    _menuContrato = "active";
                    _menuProposta = String.Empty;
                }
                else if (_menu == 3)
                {
                    _menuContrato = String.Empty;
                    _menuProposta = String.Empty;
                }
            }
            catch (Exception) {
                Response.Redirect("Login.aspx");
            }
        }
    }
}