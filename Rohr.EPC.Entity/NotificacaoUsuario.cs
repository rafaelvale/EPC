using System;

namespace Rohr.EPC.Entity
{
    public class NotificacaoUsuario
    {
        public Int32 IdNotificaoUsuario { get; set; }
        public Boolean AnaliseGerente { get; set; }
        public Boolean AnaliseSuperintendencia { get; set; }
        public Boolean AnaliseDiretoriaOperacional { get; set; }
        public Boolean AnaliseVicePresidencia { get; set; }
        public Boolean AnaliseDiretoria { get; set; }
        public Boolean AnaliseJuridico { get; set; }
        public Boolean ContratoAssinadoCliente { get; set; }
        public Boolean ContratoArquivado { get; set; }
        public Boolean ResumoDiario { get; set; }
        public Boolean ResumoSemanal { get; set; }
        public Boolean ResumoMensal { get; set; }
    }
}
