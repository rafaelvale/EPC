using System;
using System.Collections.Generic;

namespace Rohr.EPC.Entity
{
    public class Modelo
    {
        public Int32 IdModelo { get; set; }
        public String Titulo { get; set; }
        public Boolean OrcamentoObrigatorio { get; set; }
        public Boolean CondicoesGeraisObrigatorio { get; set; }
        public DateTime DataCadastro { get; set; }
        public Boolean Ativo { get; set; }
        public Int32 Versao { get; set; }

        public Segmento Segmento { get; set; }
        public ModeloTipo ModeloTipo { get; set; }
        public ModeloMeta ModeloMeta { get; set; }
        public Boolean CapaObrigatorio { get; set; }
        public List<Parte> ListParte { get; set; }


        public Modelo()
        {
        }
        public Modelo(Int32 idModelo)
        {
            IdModelo = idModelo;
        }
    }
}
