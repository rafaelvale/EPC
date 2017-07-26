using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;

namespace Rohr.EPC.Business
{
    public class ModeloBusiness
    {
        public Modelo ObterModeloPorId(Int32 idModelo)
        {
            Modelo oModelo = new ModeloDAO().ObterPorId(idModelo);

            oModelo.Segmento = new SegmentoDAO().ObterPorId(oModelo.Segmento.IdSegmento);
            oModelo.ModeloMeta = new ModeloMetaDAO().ObterPorId(oModelo.ModeloMeta.IdModeloMeta);
            oModelo.ModeloTipo = new ModeloTipoDAO().ObterPorId(oModelo.ModeloTipo.IdModeloTipo);
            oModelo.ListParte = new ParteDAO().ObterPorIdModelo(oModelo.IdModelo);

            if (oModelo.Segmento == null)
                throw new NullReferenceException("Não foi possível recuperar o segmento do modelo.");
            if (oModelo.ModeloMeta == null)
                throw new NullReferenceException("Não foi possível recuperar a meta do modelo.");
            if (oModelo.ModeloTipo == null)
                throw new NullReferenceException("Não foi possível recuperar o tipo do modelo.");
            if (!Util.VerificarModeloCliente(oModelo.ModeloTipo))
            {
                if (oModelo.ListParte == null)
                    throw new NullReferenceException("Não foi possível recuperar as partes do modelo.");

                foreach (Parte parte in oModelo.ListParte)
                    parte.ListChave = new ChaveDAO().ObterChavePorIdParte(parte.IdParte);
            }

            return oModelo;
        }
        public List<Modelo> ObterTodos(Int32 idPerfil)
        {
            List<Modelo> listModelo = new ModeloDAO().ObterTodosModeloProposta(idPerfil == 6);
            return listModelo;
        }

        public List<Modelo> ObterModeloContratoPorIdModeloProposta(Int32 idModeloProposta)
        {
            return new ModeloDAO().ObterTodosModeloContrato(idModeloProposta);
        }
        public List<Modelo> ObterTodosModelosRelatorio()
        {
            return new ModeloDAO().ObterTodosModelosRelatorio();
        }
        public Boolean CadastroRecente(Modelo modelo)
        {
            return DateTime.Now.Subtract(modelo.DataCadastro).Days < 30;
        }
    }
}
