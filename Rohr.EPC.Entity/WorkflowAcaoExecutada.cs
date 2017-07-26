using System;

namespace Rohr.EPC.Entity
{
    public class WorkflowAcaoExecutada
    {
        public Int32 IdWorkflowAcaoExecutada { get; set; }
        public Int32 IdUsuario { get; set; }
        public Int32 IdDocumento { get; set; }
        public DateTime DataCadastro { get; set; }
        public String Justificativa { get; set; }
        public WorkflowAcao WorkflowAcao { get; set; }
        public Int32 NumeroDocumento { get; set; }
        public Int32 IdPerfil { get; set; }
        public Int32 MetaEmMinuto { get; set; }
        public Double TempoTotalAcao { get; set; }
        public Int32 IdWorkflow { get; set; }
    }
}
