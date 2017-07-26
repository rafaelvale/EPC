using System;

namespace Rohr.EPC.Entity
{
    public class AuditoriaLog
    {
        public Int32 IdLog { get; set; }
        public Int32 IdUsuario { get; set; }
        public Int32 IdTipoLog { get; set; }
        public DateTime DataCadastro { get; set; }
        public String Descricao { get; set; }
        public String Ip { get; set; }
        public String NomeMaquina { get; set; }
        public Boolean AcessoMobile { get; set; }
    }
}
