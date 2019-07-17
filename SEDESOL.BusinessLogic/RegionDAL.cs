using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;

namespace SEDESOL.BusinessLogic
{
    public class RegionDAL
    {
        public List<RegionDTO> GetActiveRegions()
        {
            RegionDAO dao = new RegionDAO();
            return dao.GetActiveRegions();
        }
    }
}
