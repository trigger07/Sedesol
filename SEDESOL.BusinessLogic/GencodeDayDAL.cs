using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;

namespace SEDESOL.BusinessLogic
{
    public class GencodeDayDAL
    {
        public List<GencodeDayDTO> GetGenerationCodeDayAll()
        {
            GencodeDayDAO dao = new GencodeDayDAO();
            return dao.GetGenerationCodeDayAll();
        }

        public GencodeDayDTO Save(GencodeDayDTO dto)
        {
            GencodeDayDAO dao = new GencodeDayDAO();
            return dao.Save(dto);
        }

        public GencodeDayDTO GetGenerationCodeDayById(int Id)
        {
            GencodeDayDAO dao = new GencodeDayDAO();
            return dao.GetGenerationCodeDayById(Id);
        }
    }
}
