using System.Collections.Generic;
using Rohr.Data;
using Rohr.EPC.Entity;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.EPC.DAL
{
    public class DocumentoDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        readonly DbHelper _dbHelperPMWeb = new DbHelper("PMWeb");


        public Int32 ObterTermoID(Int32 TermoNumero)
        {
            Int32 Termo = 0;
            string query = String.Format("SELECT ISNULL(MAX(IDTermo), 0) FROM documentos_ta WHERE NumeroTermo = {0}", TermoNumero);            
            Termo = Int32.Parse(_dbHelper.ExecutarScalar(query, CommandType.Text).ToString());
            _dbHelper.CloseConnection();
            return Termo;
        }

        public DataTable ObterTermoPMweb(Int32 TermoNumero)
        {
            string query = String.Format("SELECT Id, RevisionNumber, Description FROM Estimates WHERE SUBSTRING(RevisionId, 0, CHARINDEX('.', RevisionId, 0)) = {0}", TermoNumero);
            DataTable dataTable = _dbHelperPMWeb.ExecutarDataTable(query, CommandType.Text);
            _dbHelper.CloseConnection();
            return dataTable;
        }


        public DataTable ObterDadosTermo(Int32 IDOportunidade)
        {
            string query = String.Format("SELECT EClausula, EOrcamento, IDStatus FROM documentos_ta WHERE IDOportunidade = {0}", IDOportunidade);            
            DataTable dataTable = _dbHelper.ExecutarDataTable(query, CommandType.Text);
            _dbHelper.CloseConnection();
            return dataTable;
        }
        public Int32 AdicionarProposta(Documento documento, Workflow workflow)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@CodigoOrigemDocumento", documento.CodigoSistemaOrigem));
            parametros.Adicionar(new DbParametro("@NumeroDocumento", documento.NumeroDocumento));
            parametros.Adicionar(new DbParametro("@PercentualDesconto", documento.PercentualDesconto));
            parametros.Adicionar(new DbParametro("@ValorFaturamentoMensal", documento.ValorFaturamentoMensal));
            parametros.Adicionar(new DbParametro("@ValorNegocio", documento.ValorNegocio));
            parametros.Adicionar(new DbParametro("@IdUsuario", documento.Usuario.IdUsuario));
            parametros.Adicionar(new DbParametro("@IdDocumentoStatus", documento.DocumentoStatus.IdDocumentoStatus));
            parametros.Adicionar(new DbParametro("@IdFilial", documento.Filial.IdFilial));
            parametros.Adicionar(new DbParametro("@IdModelo", documento.Modelo.IdModelo));
            parametros.Adicionar(new DbParametro("@RevisaoCliente", documento.RevisaoCliente));
            parametros.Adicionar(new DbParametro("@VersaoInterna", documento.VersaoInterna));

            parametros.Adicionar(new DbParametro("@CodigoOrigemCliente", documento.DocumentoCliente.CodigoOrigem));
            parametros.Adicionar(new DbParametro("@CPF_CNPJ", documento.DocumentoCliente.CpfCnpj));
            parametros.Adicionar(new DbParametro("@RG_IE", documento.DocumentoCliente.RgIe));
            parametros.Adicionar(new DbParametro("@NomeCliente", documento.DocumentoCliente.Nome));
            parametros.Adicionar(new DbParametro("@EnderecoCliente", documento.DocumentoCliente.Endereco + " - " + documento.DocumentoCliente.Numero));
            parametros.Adicionar(new DbParametro("@BairroCliente", documento.DocumentoCliente.Bairro));
            parametros.Adicionar(new DbParametro("@CidadeCliente", documento.DocumentoCliente.Cidade));
            parametros.Adicionar(new DbParametro("@EstadoCliente", documento.DocumentoCliente.Estado));
            parametros.Adicionar(new DbParametro("@CEPCliente", documento.DocumentoCliente.Cep));

            parametros.Adicionar(new DbParametro("@CodigoOrigemObra", documento.DocumentoObra.CodigoOrigem));
            parametros.Adicionar(new DbParametro("@NomeObra", documento.DocumentoObra.Nome));
            parametros.Adicionar(new DbParametro("@EnderecoObra", documento.DocumentoObra.Endereco));
            parametros.Adicionar(new DbParametro("@BairroObra", documento.DocumentoObra.Bairro));
            parametros.Adicionar(new DbParametro("@CidadeObra", documento.DocumentoObra.Cidade));
            parametros.Adicionar(new DbParametro("@EstadoObra", documento.DocumentoObra.Estado));
            parametros.Adicionar(new DbParametro("@CEPObra", documento.DocumentoObra.CEP));

            parametros.Adicionar(new DbParametro("@IdWorkflowEtapa", workflow.WorkflowEtapa.IdWorkflowEtapa));
            parametros.Adicionar(new DbParametro("@IdPerfil", workflow.IdPerfil));

            parametros.Adicionar(new DbParametro("@CodigoComercialOrigem", documento.DocumentoComercial.CodigoSistemaOrigem));
            parametros.Adicionar(new DbParametro("@PrimeiroNome", documento.DocumentoComercial.PrimeiroNome));
            parametros.Adicionar(new DbParametro("@Sobrenome", documento.DocumentoComercial.Sobrenome));
            parametros.Adicionar(new DbParametro("@Email", documento.DocumentoComercial.Email));
            parametros.Adicionar(new DbParametro("@Departamento", documento.DocumentoComercial.Departamento));
            parametros.Adicionar(new DbParametro("@Telefone", documento.DocumentoComercial.Telefone));

            parametros.Adicionar(new DbParametro("@IdWorkflowAcao", workflow.WorkflowAcao.IdWorkflowAcao));

            if (documento.PercentualLimpeza == 0 || string.IsNullOrWhiteSpace(documento.PercentualLimpeza.ToString()))
                parametros.Adicionar(new DbParametro("@percentualLimpeza", DBNull.Value));
            else
                parametros.Adicionar(new DbParametro("@percentualLimpeza", documento.PercentualLimpeza));

            Int32 novoIdDocumento = (Int32)_dbHelper.ExecutarScalar("AddDocumentoProposta", parametros, CommandType.StoredProcedure);

            _dbHelper.CloseConnection();

            return novoIdDocumento;
        }
        public Int32 AdicionarContrato(Documento documentoContrato, Documento documentoProposta, Workflow workflow)
        {
            DbParametros parametros = new DbParametros();

            parametros.Adicionar(new DbParametro("@IdFilial", documentoContrato.Filial.IdFilial));
            parametros.Adicionar(new DbParametro("@IdUsuario", documentoContrato.Usuario.IdUsuario));
            parametros.Adicionar(new DbParametro("@IdPerfil", workflow.IdPerfil));

            parametros.Adicionar(new DbParametro("@CodigoOrigemDocumento", documentoContrato.CodigoSistemaOrigem));
            parametros.Adicionar(new DbParametro("@NumeroDocumento", documentoContrato.NumeroDocumento));
            parametros.Adicionar(new DbParametro("@PercentualDesconto", documentoContrato.PercentualDesconto));
            parametros.Adicionar(new DbParametro("@ValorFaturamentoMensal", documentoContrato.ValorFaturamentoMensal));
            parametros.Adicionar(new DbParametro("@ValorNegocio", documentoContrato.ValorNegocio));
            parametros.Adicionar(new DbParametro("@IdDocumentoStatus", documentoContrato.DocumentoStatus.IdDocumentoStatus));
            parametros.Adicionar(new DbParametro("@IdModelo", documentoContrato.Modelo.IdModelo));
            parametros.Adicionar(new DbParametro("@RevisaoCliente", documentoContrato.RevisaoCliente));
            parametros.Adicionar(new DbParametro("@VersaoInterna", documentoContrato.VersaoInterna));

            parametros.Adicionar(new DbParametro("@CodigoOrigemCliente", documentoContrato.DocumentoCliente.CodigoOrigem));
            parametros.Adicionar(new DbParametro("@CPF_CNPJ", documentoContrato.DocumentoCliente.CpfCnpj));
            parametros.Adicionar(new DbParametro("@RG_IE", documentoContrato.DocumentoCliente.RgIe));
            parametros.Adicionar(new DbParametro("@NomeCliente", documentoContrato.DocumentoCliente.Nome));
            parametros.Adicionar(new DbParametro("@EnderecoCliente", documentoContrato.DocumentoCliente.Endereco + " - " + documentoContrato.DocumentoCliente.Numero));
            parametros.Adicionar(new DbParametro("@BairroCliente", documentoContrato.DocumentoCliente.Bairro));
            parametros.Adicionar(new DbParametro("@CidadeCliente", documentoContrato.DocumentoCliente.Cidade));
            parametros.Adicionar(new DbParametro("@EstadoCliente", documentoContrato.DocumentoCliente.Estado));
            parametros.Adicionar(new DbParametro("@CEPCliente", documentoContrato.DocumentoCliente.Cep));

            parametros.Adicionar(new DbParametro("@CodigoOrigemObra", documentoContrato.DocumentoObra.CodigoOrigem));
            parametros.Adicionar(new DbParametro("@NomeObra", documentoContrato.DocumentoObra.Nome));
            parametros.Adicionar(new DbParametro("@EnderecoObra", documentoContrato.DocumentoObra.Endereco));
            parametros.Adicionar(new DbParametro("@BairroObra", documentoContrato.DocumentoObra.Bairro));
            parametros.Adicionar(new DbParametro("@CidadeObra", documentoContrato.DocumentoObra.Cidade));
            parametros.Adicionar(new DbParametro("@EstadoObra", documentoContrato.DocumentoObra.Estado));

            parametros.Adicionar(new DbParametro("@CodigoComercialOrigem", documentoContrato.DocumentoComercial.CodigoSistemaOrigem));
            parametros.Adicionar(new DbParametro("@PrimeiroNome", documentoContrato.DocumentoComercial.PrimeiroNome));
            parametros.Adicionar(new DbParametro("@Sobrenome", documentoContrato.DocumentoComercial.Sobrenome));
            parametros.Adicionar(new DbParametro("@Email", documentoContrato.DocumentoComercial.Email));
            parametros.Adicionar(new DbParametro("@Departamento", documentoContrato.DocumentoComercial.Departamento));
            parametros.Adicionar(new DbParametro("@Telefone", documentoContrato.DocumentoComercial.Telefone));


            parametros.Adicionar(new DbParametro("@CEPObra", documentoContrato.DocumentoObra.CEP));

            parametros.Adicionar(new DbParametro("@IdWorkflowEtapa", workflow.WorkflowEtapa.IdWorkflowEtapa));

            parametros.Adicionar(new DbParametro("@IdDocumentoProposta", documentoProposta.IdDocumento));

            parametros.Adicionar(new DbParametro("@IdWorkflowAcao", workflow.WorkflowAcao.IdWorkflowAcao));

            if (documentoContrato.PercentualLimpeza == 0 || string.IsNullOrWhiteSpace(documentoContrato.PercentualLimpeza.ToString()))
                parametros.Adicionar(new DbParametro("@percentualLimpeza", DBNull.Value));
            else
                parametros.Adicionar(new DbParametro("@percentualLimpeza", documentoContrato.PercentualLimpeza));

            int novoIdDocumento = Convert.ToInt32(_dbHelper.ExecutarScalar("AddDocumentoContrato", parametros, CommandType.StoredProcedure));

            _dbHelper.CloseConnection();

            return novoIdDocumento;
        }

        public void AtualizarDocumento(Documento documento, Workflow workflow, WorkflowAcaoExecutada workflowAcaoExecutada)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@PercentualDesconto", documento.PercentualDesconto));
            parametros.Adicionar(new DbParametro("@ValorFaturamentoMensal", documento.ValorFaturamentoMensal));
            parametros.Adicionar(new DbParametro("@ValorNegocio", documento.ValorNegocio));
            parametros.Adicionar(new DbParametro("@DataParaExibicao", documento.DataParaExibicao.ToString("yyyy-MM-dd")));
            parametros.Adicionar(new DbParametro("@Justificativa", workflowAcaoExecutada.Justificativa));
            parametros.Adicionar(new DbParametro("@IdWorkflowAcao", workflow.WorkflowAcao.IdWorkflowAcao));
            parametros.Adicionar(new DbParametro("@IdWorkflowAcaoExecutada", workflowAcaoExecutada.WorkflowAcao.IdWorkflowAcao));
            parametros.Adicionar(new DbParametro("@IdWorkflowEtapa", workflow.WorkflowEtapa.IdWorkflowEtapa));
            parametros.Adicionar(new DbParametro("@IdPerfil", workflow.IdPerfil));
            parametros.Adicionar(new DbParametro("@IdUsuario", workflow.IdUsuario));
            parametros.Adicionar(new DbParametro("@TempoTotalAcao", workflowAcaoExecutada.TempoTotalAcao));
            parametros.Adicionar(new DbParametro("@IdDocumento", documento.IdDocumento));
            parametros.Adicionar(new DbParametro("@IdDocumentoStatus", documento.DocumentoStatus.IdDocumentoStatus));
            parametros.Adicionar(new DbParametro("@EMinuta", documento.Eminuta));

            if (documento.PercentualLimpeza == 0 || string.IsNullOrWhiteSpace(documento.PercentualLimpeza.ToString()))
                parametros.Adicionar(new DbParametro("@percentualLimpeza", DBNull.Value));
            else
                parametros.Adicionar(new DbParametro("@percentualLimpeza", documento.PercentualLimpeza));

            _dbHelper.ExecutarNonQuery("UpdateDocumento", parametros, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();
        }
        public void AtualizarStatusDocumento(Int32 idDocumento)
        {
            StringBuilder query = new StringBuilder();
            query.Append(String.Format("UPDATE documentos SET idDocumentoStatus = 2 WHERE idDocumento = {0}", idDocumento));
            _dbHelper.ExecutarNonQuery(query.ToString(), CommandType.Text);
        }
        public void AtualizarPercentualLimpeza(Int32 idDocumento, decimal percentualLimpeza)
        {
            StringBuilder query = new StringBuilder();
            query.Append(String.Format("UPDATE documentos SET percentualLimpeza = {1} WHERE idDocumento = {0}", idDocumento, percentualLimpeza));
            _dbHelper.ExecutarNonQuery(query.ToString(), CommandType.Text);
        }

        public Boolean PropostaExistente(Int32 numeroOportunidade)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@numeroDocumento", numeroOportunidade));

            Object proposta = _dbHelper.ExecutarScalar("CheckPropostaExiste", parametros, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return proposta != null;
        }
        public Boolean PropostaTemContrato(Int32 numeroDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@numeroDocumento", numeroDocumento));

            Object proposta = _dbHelper.ExecutarScalar("CheckPropostaTemContrato", parametros, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return proposta != null;
        }
        public Boolean PropostaCancelada(Int32 numeroDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@NumeroDocumento", numeroDocumento));

            Object idStatusDocumento = _dbHelper.ExecutarScalar("CheckPropostaCancelada", parametros, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return (Int32)idStatusDocumento == 3;
        }


        public Documento Obter(String proc, int id)
        {
            Documento oDocumento = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", id));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader(proc, parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                {
                    oDocumento = new Documento
                    {
                        IdDocumento = Int32.Parse(dataReader["idDocumento"].ToString()),
                        CodigoSistemaOrigem = Int32.Parse(dataReader["codigoOrigem"].ToString()),
                        VersaoInterna = Int32.Parse(dataReader["versaoInterna"].ToString()),
                        RevisaoCliente = Int32.Parse(dataReader["revisaoCliente"].ToString()),
                        DataCadastro = DateTime.Parse(dataReader["dataCadastro"].ToString()),
                        DataParaExibicao = DateTime.Parse(dataReader["dataParaExibicao"].ToString()),
                        NumeroDocumento = Int32.Parse(dataReader["numeroDocumento"].ToString()),
                        PercentualDesconto = Decimal.Parse(dataReader["percentualDesconto"].ToString()),
                        ValorFaturamentoMensal = Decimal.Parse(dataReader["valorFaturamentoMensal"].ToString()),
                        ValorNegocio = Decimal.Parse(dataReader["valorNegocio"].ToString()),
                        Eminuta = Boolean.Parse(dataReader["eMinuta"].ToString()),
                        EProposta = Boolean.Parse(dataReader["eProposta"].ToString()),
                        Modelo = new Modelo(Int32.Parse(dataReader["idModelo"].ToString())),
                        Filial = new Filial(Int32.Parse(dataReader["idFilial"].ToString())),
                        Usuario = new Usuario(Int32.Parse(dataReader["idUsuario"].ToString())),
                        DocumentoStatus = new DocumentoStatus(Int32.Parse(dataReader["IdDocumentoStatus"].ToString())),
                        PercentualLimpeza = Decimal.Parse(dataReader["percentualLimpeza"].ToString()),
                    };
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return oDocumento;
            }
        }
        public Documento ObterPorId(Int32 idDocumento)
        {
            return Obter("GetDocumento", idDocumento);
        }
        public Documento ObterPorIdContrato(Int32 idDocumentoContrato)
        {
            return Obter("GetDocumentoContrato", idDocumentoContrato);
        }
        public DataTable ObterTodos(String filiais, String segmentos)
        {
            string query = String.Format("SELECT * FROM view_cockpit WHERE idFilial IN ({0}) AND idSegmento IN ({1}) order by dataCadastro desc", filiais, segmentos);
            DataTable dataTable = _dbHelper.ExecutarDataTable(query, CommandType.Text);
            _dbHelper.CloseConnection();

            return dataTable;
        }
        public DataTable ObterTodosPorIdWorkflowAcao(String filiais, String segmentos, Int32 idWorkflowAcao)
        {
            string query = String.Format("SELECT * FROM view_cockpit WHERE idFilial IN ({0}) AND idSegmento IN ({1}) AND idWorkflowAcao IN ({2}) order by dataCadastro desc", filiais, segmentos, idWorkflowAcao);
            DataTable dataTable = _dbHelper.ExecutarDataTable(query, CommandType.Text);
            _dbHelper.CloseConnection();

            return dataTable;
        }
        public DataTable ObterDocumentosEmCirculacao(List<Filial> listFilial)
        {
            DbParametros parametros = new DbParametros();

            String listaIdFiilial = String.Empty;
            for (int i = 0; i < listFilial.Count; i++)
            {
                if (i != listFilial.Count - 1)
                    listaIdFiilial += listFilial[i].IdFilial + ",";
                else
                    listaIdFiilial += listFilial[i].IdFilial;
            }

            parametros.Adicionar(new DbParametro("@IdFilial", listaIdFiilial));

            using (DataTable dataTable = _dbHelper.ExecutarDataTable("GetDocumentoEmCirculacao", parametros, CommandType.StoredProcedure))
            {
                _dbHelper.CloseConnection();
                return dataTable;
            }
        }

        public DataTable ObterDocumentosCancelados(List<Filial> listFilial)
        {
            DbParametros parametros = new DbParametros();

            String listaIdFiilial = String.Empty;
            for (int i = 0; i < listFilial.Count; i++)
            {
                if (i != listFilial.Count - 1)
                    listaIdFiilial += listFilial[i].IdFilial + ",";
                else
                    listaIdFiilial += listFilial[i].IdFilial;
            }

            parametros.Adicionar(new DbParametro("@IdFilial", listaIdFiilial));

            using (DataTable dataTable = _dbHelper.ExecutarDataTable("GetDocumentoCancelados", parametros, CommandType.StoredProcedure))
            {
                _dbHelper.CloseConnection();
                return dataTable;
            }
        }

        public DataTable ObterTodosDocumento(String valor, String segmentos, String filiais)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@valor", valor));
            parametros.Adicionar(new DbParametro("@segmento", segmentos));
            parametros.Adicionar(new DbParametro("@filiais", filiais));

            using (DataTable dataTable = _dbHelper.ExecutarDataTable("PesquisaDocumentoCliente", parametros, CommandType.StoredProcedure))
            {
                _dbHelper.CloseConnection();
                return dataTable;
            }
        }
        public DataTable ObterTodasVersoes(Int32 numeroDocumento)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@NumeroDocumento", numeroDocumento));
            DataTable dataTable = _dbHelper.ExecutarDataTable("GetLisDocumento", parametros, CommandType.StoredProcedure);
            _dbHelper.CloseConnection();

            return dataTable;
        }
        public Int32 ObterIdDocumentoProposta(Int32 idDocumentoContrato)
        {
            Int32 idDocumentoProposta = 0;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumentoContrato", idDocumentoContrato));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetIdDocumentoProposta", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                    Int32.TryParse(dataReader["IdDocumentoProposta"].ToString(), out idDocumentoProposta);

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return idDocumentoProposta;
            }
        }
        public Int32 ObterMaiorRevisaoProposta(Int32 idDocumentoContrato)
        {
            Int16 revisao = 0;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@IdDocumento", idDocumentoContrato));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetMaiorRevisaoProposta", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.Read())
                    revisao = Convert.ToInt16(dataReader["Revisao"].ToString());

                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return revisao;
            }
        }
    }
}
