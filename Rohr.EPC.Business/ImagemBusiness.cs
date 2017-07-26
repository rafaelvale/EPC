using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.Business
{
    public class ImagemBusiness
    {
        /// <summary>
        /// Adiciona uma Imagem..
        /// </summary>
        /// <param name = "imagem"></param>
        public void AdicionarImagem(Imagens imagem)
        {
            new ImagensDAO().Adicionar(imagem);
        }

        /// <summary>
        /// Retorna uma lista de Imagens de acordo com o modelo passado
        /// </summary>
        /// <param name = "IdModeloDocumento"></param>
        /// <returns></returns>
        public List<Imagens> GetImagens()
        {
            return new ImagensDAO().GetImagens();
        }

        public List<Imagens> GetBuscaImagens(String valor)
        {
            if (String.IsNullOrWhiteSpace(valor))
                throw new MyException("Informe um parametro para pesquisa");

            if (valor.Length < 3)
                throw new MyException("Parametro muito curto, o minímo necessário é de 3 caracteres.");

            Int32 resultado;

            String valorLimpo = valor.Replace(".", String.Empty).Replace(" ", "").Trim();


            return new ImagensDAO().GetBuscaImagens(Int32.TryParse(valorLimpo, out resultado) ? valorLimpo : valor);
        }
    }
}
