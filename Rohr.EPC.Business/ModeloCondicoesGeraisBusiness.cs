using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;

namespace Rohr.EPC.Business
{
    public class ModeloCondicoesGeraisBusiness
    {
        public ModeloCondicoesGerais ObterPorId(Int32 idModeloCondicoesGerais)
        {
            return new ModeloCondicoesGeraisDAO().ObterPorId(idModeloCondicoesGerais);
        }
        public List<ModeloCondicoesGerais> ObterLista()
        {
            return new ModeloCondicoesGeraisDAO().ObterLista();
        }
        public ModeloCondicoesGerais ObterPorModeloDocumento(Documento documento)
        {
            DateTime dataBase;
            if (documento.EProposta)
                dataBase = documento.DataCadastro;
            else
                dataBase = new PMWeb.CostManagementCommitments().ObterContrato(documento.CodigoSistemaOrigem).CreateDate;


            if (dataBase <= new DateTime(2014, 10, 17))
                return new ModeloCondicoesGeraisDAO().ObterPorId(2); //Condições gerais 
            else if (dataBase >= new DateTime(2016, 06, 14) && documento.Modelo.Segmento.IdSegmento == 6)
                return documento.PercentualLimpeza == 3 ? new ModeloCondicoesGeraisDAO().ObterPorId(15) : new ModeloCondicoesGeraisDAO().ObterPorId(16);
            else if (dataBase >= new DateTime(2014, 12, 16))
                return new ModeloCondicoesGeraisDAO().ObterPorId(15); //Condições gerais com reajuste claúsula 4.2
            else
                return new ModeloCondicoesGeraisDAO().ObterPorId(4); //Condições gerais com reajuste do valor da limpeza.
        }
    }
}
