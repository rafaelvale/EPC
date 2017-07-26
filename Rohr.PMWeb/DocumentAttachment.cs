using Rohr.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Rohr.PMWeb
{
    public class DocumentAttachment
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public int FileId { get; set; }
        public string Description { get; set; }
        public string FileGuid { get; set; }

        public List<DocumentAttachment> GetAllFiles(int documentId)
        {
            DbParametros parametros = new DbParametros();
            parametros.Adicionar(new DbParametro("@documentId", documentId));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(
                "select FileId, Description, FileGuid from DocumentAttachments inner join FileManager_Files on FileManager_Files.Id = FileId where DocumentId = @documentId and DocumentTypeId = 106 and FileOption = 'UPLOAD'",
                parametros,
                System.Data.CommandType.Text)))
            {
                List<DocumentAttachment> listDocumentAttachment = new List<DocumentAttachment>();
                if (dataReader.HasRows)
                {
                    listDocumentAttachment = new List<DocumentAttachment>();
                    while (dataReader.Read())
                    {
                        listDocumentAttachment.Add(new DocumentAttachment
                        {
                            FileId = Int32.Parse(dataReader["FileId"].ToString()),
                            Description = dataReader["Description"].ToString(),
                            FileGuid = dataReader["FileGuid"].ToString()
                        });
                    }
                }

                dataReader.Close();
                dataReader.Dispose();
                _dbHelperPmweb.CloseConnection();

                return listDocumentAttachment;
            }
        }
    }
}
