using System;

namespace Rohr.EPC.Entity
{
    public class DocumentoBloqueado
    {
        public Int32 IdDocumentoBloqueado { get; set; }
        public Int32 IdDocumento { get; set; }
        public Int32 IdUsuario { get; set; }
        public Int32 IdPerfil { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataDesbloqueio { get; set; }
        public Boolean Atual { get; set; }
    }
}
