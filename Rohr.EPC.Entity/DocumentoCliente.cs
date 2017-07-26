using System;

namespace Rohr.EPC.Entity
{
    public class DocumentoCliente
    {
        public Int32 IdDocumentoCliente { get; set; }
        public String Nome { get; set; }
        public String CpfCnpj { get; set; }
        public String RgIe { get; set; }
        public Int32 CodigoOrigem { get; set; }
        public String Endereco { get; set; }
        public String Numero { get; set; }
        public String Bairro { get; set; }
        public String Cidade { get; set; }
        public String Estado { get; set; }
        public String Cep { get; set; }
        public Int32 IdDocumento { get; set; }

        public DocumentoCliente()
        {

        }
        public DocumentoCliente(Int32 idDocumentoCliente)
        {
            IdDocumentoCliente = idDocumentoCliente;
        }
    }
}
