using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class InspectionCodeDTO
    {
        public int Id { get; set; }
        public string InspectionCode { get; set; }
        public int Id_Year { get; set; }
        public int Id_Month { get; set; }
        public int Id_User { get; set; }

        public MonthDTO MonthDto { get; set; }
        public UserDTO UserDto { get; set; }
        public YearDTO YearDto { get; set; }

        public string Message { get; set; }
    }
}
