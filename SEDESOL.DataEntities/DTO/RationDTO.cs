using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class RationDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int Id_SoupKitchen { get; set; }
        public int Id_Month { get; set; }
        public int Id_Year { get; set; }
        public Nullable<int> Id_Capture { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal RationQuantity { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string EditUser { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }

        public Nullable<bool> HasCoupon { get; set; }

        public MonthDTO MONTHDTO { get; set; }
        public SoupKitchenDTO SOUP_KITCHENDTO { get; set; }
        public YearDTO YEARDTO { get; set; }

        public string Sequential { get; set; }
    }
}
