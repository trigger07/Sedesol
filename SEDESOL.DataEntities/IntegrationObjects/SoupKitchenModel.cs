using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.IntegrationObjects
{
    public class SoupKitchenModel
    {
        public SoupKitchenDTO SoupKitchen  { get; set; }

        public List<StateDTO> States { get; set; }

        public List<SkUserTypeDTOcs> ListUserTypeSKDto { get; set; }

        public List<UserTypeDTO> ListUserType { get; set; }

        public List<RegionDTO> ListRegion { get; set; }
    }
}
