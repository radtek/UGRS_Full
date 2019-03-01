using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.CyC.Tables;
using UGRS.Core.SDK.DI.DAO;

namespace UGRS.Core.SDK.DI.CyC.Services
{
    public class ComentsService
    {
        private TableDAO<Coments> mObjComentsDAO;

        public ComentsService()
        {
            mObjComentsDAO = new TableDAO<Coments>();
        }

        public int Add(Coments pObjComent)
        {
            return mObjComentsDAO.Add(pObjComent);
        }

        public int Update(Coments pObjComent)
        {
            return mObjComentsDAO.Update(pObjComent);
        }

        public int Remove(string lStrComentCode)
        {
            return mObjComentsDAO.Remove(lStrComentCode);
        }
    }
}
