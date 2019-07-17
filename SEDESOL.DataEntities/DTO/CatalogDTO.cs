using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class CatalogDTO
    {
        [Display(Name = "Años")]
        public List<YearDTO> Years { get; set; }

        [Display(Name = "Meses")]
        public List<MonthDTO> Months { get; set; }

        [Display(Name = "Estados Mexicanos")]
        public List<StateDTO> States { get; set; }

        [Display(Name = "Estados")]
        public List<StatusDTO> Status { get; set; }
    }
}
