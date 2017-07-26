using System;

namespace Rohr.EPC.Entity
{
    public class WorkflowEtapa
    {
        public Int32 IdWorkflowEtapa { get; set; }
        public String Etapa { get; set; }
        public Decimal Meta { get; set; }

        public WorkflowEtapa()
        {
        }
        public WorkflowEtapa(Int32 idWorkflowEtapa)
        {
            IdWorkflowEtapa = idWorkflowEtapa;
        }
    }
}
