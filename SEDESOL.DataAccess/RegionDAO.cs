using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities;
using SEDESOL.DataModel;
using SEDESOL.DataEntities.DTO;

namespace SEDESOL.DataAccess
{
    public class RegionDAO
    {
        public List<RegionDTO> GetActiveRegions()
        {
            using (SEDESOLEntities entities = new SEDESOLEntities())
            {
                var query = from region in entities.REGIONs
                            where region.IsActive == true
                                //join month in entities.MONTHs on capture.Id_Month equals month.Id
                                //join year in entities.YEARs on capture.Id_Year equals year.Id
                                //join status in entities.STATUS on capture.Id_Status equals status.Id
                            select new RegionDTO
                            {
                                Id = region.Id,
                                Name = region.Name,
                                Description = region.Description,
                                IsActive = region.IsActive
                            };
                return query.ToList<RegionDTO>();
            }
        }
    }
}
