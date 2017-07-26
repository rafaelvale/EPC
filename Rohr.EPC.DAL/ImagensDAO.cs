using Rohr.Data;
using Rohr.EPC.Entity;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.EPC.DAL
{
    public class ImagensDAO
    {
        readonly DbHelper _dbHelper = new DbHelper("epc");
        public void Adicionar(Imagens imagem)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@TipoConteudo", imagem.TipoConteudo));
            parametros.Adicionar(new DbParametro("@Conteudo", imagem.Conteudo));
            parametros.Adicionar(new DbParametro("@nomeOriginal", imagem.NomeOriginal));
            parametros.Adicionar(new DbParametro("@NomeInterno", imagem.NomeInterno));
            parametros.Adicionar(new DbParametro("@DirArquivo", imagem.DirArquivo));
            parametros.Adicionar(new DbParametro("@DescrArquivo", imagem.DescrArquivo));
            parametros.Adicionar(new DbParametro("@LarguraArquivo", imagem.LarguraArquivo));
            parametros.Adicionar(new DbParametro("@AlturaArquivo", imagem.AlturaArquivo));
            parametros.Adicionar(new DbParametro("@FlagAtivo", imagem.FlagAtivo));
            parametros.Adicionar(new DbParametro("@IdUsuarioUpload", imagem.IdUsuarioUpload));
            parametros.Adicionar(new DbParametro("@Tag", imagem.Tag));

            _dbHelper.ExecutarNonQuery("AddImagens", parametros, CommandType.StoredProcedure);

        }

       
        public List<Imagens> GetBuscaImagens(string valor)
        {

            List<Imagens> ListImagens = null;

            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@valor", valor));

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetBuscaImagens", parametros, CommandType.StoredProcedure))
            {
                if (dataReader.HasRows)
                {



                    ListImagens = new List<Imagens>();
                    while (dataReader.Read())
                    {

                        ListImagens.Add(new Imagens
                        {
                            IdImagem = dataReader.GetInt32(0),
                            TipoConteudo = dataReader.GetString(1),
                            Conteudo = (byte[])dataReader.GetValue(2),
                            NomeOriginal = dataReader.GetString(3),
                            NomeInterno = dataReader.GetString(4),
                            DirArquivo = dataReader.GetString(5),
                            DescrArquivo = dataReader.GetString(6),
                            LarguraArquivo = dataReader.GetInt32(7),
                            AlturaArquivo = dataReader.GetInt32(8),
                            FlagAtivo = dataReader.GetBoolean(9),
                            IdUsuarioUpload = dataReader.GetInt32(10),
                            Tag = dataReader.GetString(11),

                        });

                    }
                }
                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return ListImagens;
            }

        }


        public List<Imagens> GetImagens()
        {
            List<Imagens> ListImagens = null;

            using (SqlDataReader dataReader = (SqlDataReader)_dbHelper.ExecutarDataReader("GetImagens", CommandType.StoredProcedure))
            {
                    if (dataReader.HasRows)
                {



                    ListImagens = new List<Imagens>();
                    while (dataReader.Read())
                    {

                        ListImagens.Add(new Imagens
                        {
                            IdImagem = dataReader.GetInt32(0),
                            TipoConteudo = dataReader.GetString(1),
                            Conteudo = (byte[])dataReader.GetValue(2),
                            NomeOriginal = dataReader.GetString(3),
                            NomeInterno = dataReader.GetString(4),
                            DirArquivo = dataReader.GetString(5),
                            DescrArquivo = dataReader.GetString(6),
                            LarguraArquivo = dataReader.GetInt32(7),
                            AlturaArquivo = dataReader.GetInt32(8),
                            FlagAtivo = dataReader.GetBoolean(9),
                            IdUsuarioUpload = dataReader.GetInt32(10),
                            Tag = dataReader.GetString(11),

                        });

                    }
                }
                dataReader.Close();
                dataReader.Dispose();
                _dbHelper.CloseConnection();

                return ListImagens;
            }
        }
    }
}
