using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class RegionProductDTO
    {
        public int Id { get; set; }
        public int Id_Region { get; set; }
        public int Id_Product { get; set; }
        public Nullable<bool> IsActive { get; set; }

        //public virtual PRODUCT PRODUCT { get; set; }
        //public virtual REGION REGION { get; set; }
    }
}
