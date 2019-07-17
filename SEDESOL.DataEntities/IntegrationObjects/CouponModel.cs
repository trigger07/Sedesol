using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.IntegrationObjects
{
    public class CouponModel
    {
        public string Month { get; set; }
        public string Year { get; set; }

        public string SoupKitchen { get; set; }

        public string Folio { get; set; }

        public int Id_Region { get; set; }
        public string Region { get; set; }

        public string Id_Coupon { get; set; }

        public List<ProductDTO> ListProduct { get; set; }

        public decimal RationQuantity { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string Contact { get; set; }

        public string Coupon_Folio { get; set; }
    }
}
