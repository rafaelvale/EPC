﻿using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rohr.EPC.DAL
{
    public class DocumentoResumoPropostaDAO
    {
        readonly DbHelper _dbhelper = new DbHelper("epc");

        public void AdicionarResumoProposta(Documento documento)
        {
            IDbTransaction transacao = _dbhelper.BeginTransacao();

            try
            {
                _dbhelper.CommitTransacao(transacao);

                new DocumentoResumoDAO().Adicionar(documento.DocumentoResumoProposta);
            }
            catch (Exception)
            {
                _dbhelper.RollbackTransacao(transacao);
                throw;
            }
        }
    }
}
