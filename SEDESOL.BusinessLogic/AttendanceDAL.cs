using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;

namespace SEDESOL.BusinessLogic
{
    public class AttendanceDAL
    {
        public AttendanceDTO Save(AttendanceDTO dto)
        {
            AttendanceDAO b = new AttendanceDAO();
            dto = b.Save(dto);

            return dto;
        }

        public AttendanceDTO GetAttendanceById(int id)
        {
            AttendanceDTO dto = new AttendanceDTO();
            AttendanceDAO d = new AttendanceDAO();
            dto = d.GetAttendanceById(id);

            return dto;
        }

        public string Delete(int id)
        {
            AttendanceDAO dao = new AttendanceDAO();
            return dao.Delete(id);
        }

        public List<AttendanceDTO> GetDataForAc(string term)
        {
            AttendanceDAO dao = new AttendanceDAO();
            return dao.GetDataForAc(term);
        }
    }
}
