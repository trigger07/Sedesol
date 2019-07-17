using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SEDESOL.DataEntities.IntegrationObjects
{
    public class CaptureParam
    {
        public List<MonthDTO> Months { get; set; }
        public List<YearDTO> Years { get; set; }
        public List<SoupKitchenDTO> SoapKitchens { get; set; }

        public string Folio { get; set; }
        public string InspectionCode { get; set; }
    }
}
