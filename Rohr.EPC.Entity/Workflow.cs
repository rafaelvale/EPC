using System;

namespace Rohr.EPC.Entity
{
    public class Workflow
    {
        public Int32 IdWorkflow { get; set; }
        public DateTime DataCadastro { get; set; }
        public String Justificativa { get; set; }
        public Int32 IdUsuario { get; set; }
        public Int32 IdPerfil { get; set; }
        public WorkflowAcao WorkflowAcao { get; set; }
        public WorkflowEtapa WorkflowEtapa { get; set; }
        public Int32 IdDocumento { get; set; }
        public Decimal Meta { get; set; }
    }
}
