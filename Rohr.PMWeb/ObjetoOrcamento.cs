using Rohr.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Rohr.PMWeb
{
    public class ObjetoOrcamento
    {
        readonly DbHelper _dbHelperPmweb = new DbHelper("pmweb");

        public Int32 Id { get; set; }
        public Int32 EstimatedId { get; set; }
        public Int32 NumeroLinha { get; set; }
        public String Objeto { get; set; }
        public Double Volume { get; set; }
        public Double Area { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime PrevisaoInicio { get; set; }
        public DateTime PrevisaoTermino { get; set; }
        public List<ItemObjetoOrcamento> ItemObjetoOrcamento { get; set; }

        public List<ObjetoOrcamento> ObterObjetoOrcamento(Int32 estimateId)
        {
            List<ObjetoOrcamento> listObjetosOrcamentoBr = new List<ObjetoOrcamento>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.Id, a.EstimateId, a.NumeroLinha, a.Objeto, a.volume, a.area, a.CreatedDate, a.PrevisaoInicioLocacao, a.PrevisaoTerminoLocacao ");
            sql.Append("FROM ObjetosOrcamento_BR as a ");
            sql.Append("WHERE ");
            sql.Append(String.Format("a.EstimateId = {0} ORDER BY a.NumeroLinha; ", estimateId));

            using (SqlDataReader dataReader = ((SqlDataReader)_dbHelperPmweb.ExecutarDataReader(sql.ToString())))
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader["Objeto"].ToString() == String.Empty)
                            throw new Exception("Foi identificado um objeto sem descrição no PMWeb :(");

                        Double area, volume;
                        Double.TryParse(dataReader["area"].ToString(), out area);
                        Double.TryParse(dataReader["volume"].ToString(), out volume);

                        DateTime previsaoInicio, previsaoTermino;
                        DateTime.TryParse(dataReader["PrevisaoInicioLocacao"].ToString(), out previsaoInicio);
                        DateTime.TryParse(dataReader["PrevisaoTerminoLocacao"].ToString(), out previsaoTermino);

                        listObjetosOrcamentoBr.Add(new ObjetoOrcamento
                        {
                            Id = Int32.Parse(dataReader["Id"].ToString()),
                            EstimatedId = Int32.Parse(dataReader["EstimateId"].ToString()),
                            NumeroLinha = Int32.Parse(dataReader["NumeroLinha"].ToString()),
                            Objeto = dataReader["Objeto"].ToString(),
                            Volume = volume,
                            Area = area,
                            DataCriacao = DateTime.Parse(dataReader["CreatedDate"].ToString()),
                            ItemObjetoOrcamento = new ItemObjetoOrcamento().ObterItemObjetoOrcamento(Int32.Parse(dataReader["Id"].ToString())),
                            PrevisaoInicio = previsaoInicio,
                            PrevisaoTermino = previsaoTermino
                        });
                    }
                }
                else
                    throw new Exception("Não foi possível identificar o objeto no PMWeb :(");

                dataReader.Close();
                dataReader.Dispose();
            }
            _dbHelperPmweb.CloseConnection();

            return listObjetosOrcamentoBr;
        }
    }
}
