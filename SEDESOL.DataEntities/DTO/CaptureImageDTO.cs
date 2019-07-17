using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDESOL.DataEntities.DTO
{
    public class CaptureImageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public byte[] ImageFile { get; set; }
        public int Id_Capture { get; set; }

        public bool FromCam { get; set; }
        public CaptureDTO CaptureDTO { get; set; }

        public string ImageFileB64 { get; set; }
    }
}
