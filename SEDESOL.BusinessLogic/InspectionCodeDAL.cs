using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;

namespace SEDESOL.BusinessLogic
{
    public class InspectionCodeDAL
    {
        public List<InspectionCodeDTO> GetCodesByUserId(int pUserId)
        {
            InspectionCodeDAO d = new InspectionCodeDAO();
            return d.GetCodesByUserId(pUserId);
        }

        public InspectionCodeDTO Save(InspectionCodeDTO dto)
        {
            InspectionCodeDAO b = new InspectionCodeDAO();
            return b.Save(dto);
        }

        public string GenerateCode(int pUserId)
        {
            InspectionCodeDAO b = new InspectionCodeDAO();
            string resp = b.GenerateCode(pUserId);
            return resp;
        }
    }
}
