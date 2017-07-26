using System.Web;
using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class ExcecaoBusiness
    {
        public static void Adicionar(Exception excecao, String url)
        {
            try
            {
                Int32 idUsuario = 0;
                Int32 idPerfil = 0;

                Object sessao = HttpContext.Current.Session["usuario"];
                if (sessao != null)
                    idUsuario = ((Usuario)sessao).IdUsuario;

                sessao = HttpContext.Current.Session["perfil"];
                if (sessao != null)
                    idPerfil = Convert.ToInt32(sessao);

                new ExcecaoDAO().Adicionar(
                    excecao,
                    url,
                    idUsuario,
                    idPerfil,
                    excecao.GetType().Name == "MyException" ? 1 : 2);
            }
            catch (Exception){ }
        }
    }
}
