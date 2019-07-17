using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SEDESOL.DataEntities.DTO
{
    public class RegionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<RegionProductDTO> RegionProductList { get; set; }
        public List<SoupKitchenDTO> SoupKitchenList { get; set; }

        public bool IsAdded { get; set; }
    }
}
