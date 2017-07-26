using Rohr.Data;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class File
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public String FileName { get; set; }
        public String GUID { get; set; }
        public byte[] FileContent { get; set; }
        public String Extension { get; set; }
        public string ContentType { get; set; }

        public File GetImage(Int32 documentId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT * ");
            stringBuilder.Append("FROM PMWebFilesContent ");
            stringBuilder.Append(String.Format("WHERE [GUID] = (SELECT FileGuid FROM FileManager_Files WHERE Id = (SELECT FileId FROM DocumentAttachments WHERE DocumentId = {0} AND FileOption = 'UPLOAD' AND DocumentTypeId = 3 AND Description = 'proposta-contrato')) ", documentId));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(stringBuilder.ToString())))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    GUID = dataReader["GUID"].ToString();
                    FileContent = (byte[])dataReader["FileContent"];
                    Extension = dataReader["Extension"].ToString();
                }

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public File GetFile(string fileGuid)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@fileGuid", fileGuid));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(
                "SELECT b.Id, b.FileName, b.FileGuid, b.Extension, b.ContentType, a.FileContent FROM PMWebFilesContent a INNER JOIN FileManager_Files b ON a.GUID = b.FileGuid WHERE a.GUID = @fileGuid",
                parametros,
                System.Data.CommandType.Text)))
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    FileName = dataReader["FileName"].ToString();
                    Id = Int32.Parse(dataReader["Id"].ToString());
                    GUID = dataReader["FileGuid"].ToString();
                    FileContent = (byte[])dataReader["FileContent"];
                    Extension = dataReader["Extension"].ToString();
                    ContentType = dataReader["ContentType"].ToString();
                }

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return this;
        }
        public Boolean ExisteImagem(Int32 documentId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(String.Format("SELECT 1 FROM DocumentAttachments WHERE DocumentId = {0} AND FileOption = 'UPLOAD' AND DocumentTypeId = 3  AND Description = 'proposta-contrato'; ", documentId));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(stringBuilder.ToString())))
            {
                if (!dataReader.HasRows) return false;
                dataReader.Close();
                dataReader.Dispose();
                _dbHelperPmweb.CloseConnection();
                return true;
            }
        }
    }
}
