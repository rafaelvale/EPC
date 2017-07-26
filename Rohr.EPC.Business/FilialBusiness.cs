using Rohr.EPC.Entity;
using Rohr.EPC.DAL;
using Rohr.PMWeb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rohr.EPC.Business
{
    public class FilialBusiness
    {
        public List<Filial> ObterFilialPorIdUsuario(Int32 idUsuario)
        {
            List<Filial> listFilial = new FilialDAO().ObterFilialPorIdUsuario(idUsuario);

            if (listFilial == null || listFilial.Count == 0)
                throw new MyException("Não foi possível recuperar a filial do usuário");

            return listFilial;
        }
        public Company ObterFilialPmweb(Int32 idCompany)
        {
            return new Company().ObterFilial(idCompany);
        }
        public Boolean VerificarPropostaFilialUsuario(Int32 idCompany, Usuario usuario)
        {
            Int32 result = usuario.Filiais.Count(i => i.IdFilial == idCompany);

            return result != 0;
        }
        public Filial ObterFilial(Int32 idFilial)
        {
            return new FilialDAO().ObterPorId(idFilial);
        }
    }
}