using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.IntegrationObjects
{
    public class GencodeDayParam
    {
        public int Id { get; set; }
        public int Id_Month { get; set; }
        public int Id_Year { get; set; }

        public int? Day { get; set; }

        public MonthDTO MonthDto { get; set; }
        public YearDTO YearDto { get; set; }

        public List<GencodeDayDTO> listGencodeDayDTO { get; set; }

        public List<MonthDTO> Months { get; set; }
        public List<YearDTO> Years { get; set; }
    }
}
