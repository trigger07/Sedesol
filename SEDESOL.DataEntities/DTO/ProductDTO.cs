using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public decimal Measure { get; set; }
        public string UnitMeasure { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public decimal Momio { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string EditUser { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        //public virtual ICollection<COUPON_DETAIL> COUPON_DETAIL { get; set; }
        //public virtual ICollection<REGION_PRODUCT> REGION_PRODUCT { get; set; }

        public List<RegionDTO> ListRegion { get; set; }

        public string Message { get; set; }

        public string MomioString { get; set; }

        public string MeasureString { get; set; }

        public decimal QuantityCoupon { get; set; }
    }
}
