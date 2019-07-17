using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;


namespace SEDESOL.BusinessLogic
{
    public class CaptureImageDAL
    {
        public int Save(CaptureImageDTO dto)
        {
            CaptureImageDAO b = new CaptureImageDAO();
            int resp = b.Save(dto);
            return resp;
        }

        public string Delete(int id)
        {
            CaptureImageDAO d = new CaptureImageDAO();
            string resp = d.Delete(id);

            return resp;
        }

        public List<CaptureImageDTO> GetImageByCaptureId(int pCaptureId)
        {
            CaptureImageDAO d = new CaptureImageDAO();
            return d.GetImageByCaptureId(pCaptureId);
        }
    }
}
