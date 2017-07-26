using System;

namespace Rohr.EPC.Entity
{
    public class WorkflowAlcada
    {
        //Numerado conforme hierarquia de alçada
        public enum Alcada
        {
            Comercial = 1,
            Gerente = 2,
            Superintendencia = 3,
            DiretoriaOperacional = 4,
            VicePresidencia = 5
        }

        public Int32 IdWorkflowAlcada { get; set; }
        public Double AlcadaPercentualDescontoMedio { get; set; }
        public Double AlcadaValorFaturamentoMensalMinimo { get; set; }
        public Double AlcadaValorFaturamentoMensalMaximo { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
