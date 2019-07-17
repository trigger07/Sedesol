using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.IntegrationObjects
{
    public class CaptureModel
    {
        public CaptureDTO Capture { get; set; }

        public AttendanceDTO Attendance { get; set; }

        public string StatusMessage { get; set; }

        public List<ConditionDTO> ConditionDtoList { get; set; }

        public List<StateDTO> StateDtoList { get; set; }

        public string MessageError { get; set; }
    }
}
