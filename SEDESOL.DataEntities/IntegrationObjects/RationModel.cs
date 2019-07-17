using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.IntegrationObjects
{
    public class RationModel
    {
        public List<RationDTO> ListRation { get; set; }

        public List<StateDTO> ListState { get; set; }

        public List<YearDTO> ListYear { get; set; }

        public List<MonthDTO> ListMonth { get; set; }

    }
}
