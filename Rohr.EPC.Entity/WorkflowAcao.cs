using System;

namespace Rohr.EPC.Entity
{
    public class WorkflowAcao
    {
        public Int32 IdWorkflowAcao { get; set; }
        public String Descricao { get; set; }
        public Decimal Meta { get; set; }

        public WorkflowAcao()
        {

        }
        public WorkflowAcao(Int32 idWorkflowAcao)
        {
            IdWorkflowAcao = idWorkflowAcao;
        }
    }
}
