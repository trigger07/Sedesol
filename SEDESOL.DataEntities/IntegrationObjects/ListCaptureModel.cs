using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.IntegrationObjects
{
    public class ListCaptureModel
    {
        public List<CaptureDTO> ListCapture { get; set; }

        public List<StateDTO> ListState { get; set; }

        public List<StatusDTO> ListStatus { get; set; }
    }
}
