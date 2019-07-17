using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class CaptureApprovalDTO
    {
        public int Id { get; set; }
        public int Id_Capture { get; set; }
        public int Id_User { get; set; }
        public int Id_Status { get; set; }
        public System.DateTime CreateDate { get; set; }

        public CaptureDTO CaptureDto { get; set; }
        public StatusDTO StatusDto { get; set; }
        public UserDTO UserDto { get; set; }
    }
}
