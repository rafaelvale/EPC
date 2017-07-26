using Rohr.EPC.DAL;
using Rohr.EPC.Entity;
using System;

namespace Rohr.EPC.Business
{
    public class WorkflowAcaoBusiness
    {
        public static WorkflowAcao ObterPorId(Int32 id)
        {
            return new WorkflowAcaoDAO().Obter(id);
        }
        public String ObterMetaAnaliseGerente()
        {
            return new Util().TratarTempoFracao(new WorkflowAcaoDAO().Obter(1).Meta);
        }
        public String ObterMetaAnaliseDiretoriaOperacional()
        {
            return new Util().TratarTempoFracao(new WorkflowAcaoDAO().Obter(2).Meta);
        }
        public String ObterMetaAnaliseJuridico()
        {
            return new Util().TratarTempoFracao(new WorkflowAcaoDAO().Obter(6).Meta);
        }
        public String ObterMetaAnaliseDiretoria()
        {
            return new Util().TratarTempoFracao(new WorkflowAcaoDAO().Obter(7).Meta);
        }
        public String ObterMetaReceberContratoCliente()
        {
            return new Util().TratarTempoFracao(new WorkflowAcaoDAO().Obter(11).Meta);
        }
        public String ObterMetaReceberPropostaCliente()
        {
            return new Util().TratarTempoFracao(new WorkflowAcaoDAO().Obter(4).Meta);
        }
        public String ObterMetaArquivarContrato()
        {
            return new Util().TratarTempoFracao(new WorkflowAcaoDAO().Obter(13).Meta);
        }
    }
}
