using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SEDESOL.DataEntities.IntegrationObjects
{
    public class CodeParam
    {
        public List<MonthDTO> Months { get; set; }
        public List<YearDTO> Years { get; set; }

        public List<InspectionCodeDTO> ListCode { get; set; }
        public string InspectionCode { get; set; }

        public int Id_User { get; set; }
        public int Id_Month { get; set; }
        public int Id_Year { get; set; }
    }
}
