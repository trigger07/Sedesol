using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataAccess;
using System.Data;
using SEDESOL.DataEntities.IntegrationObjects;

namespace SEDESOL.BusinessLogic
{
    public class ReportDAL
    {
        public string GetCaptureReport(List<FilterDTO> filterDto)
        {
            ReportDAO reporting = new ReportDAO();
            //return reporting.GetCaptureReport(filterDto);
            return DTOSerializer.Serialize(reporting.GetCaptureReport(filterDto)).InnerXml;

        }

        public string GetAttendanceReport(int CaptureId)
        {
            ReportDAO reporting = new ReportDAO();
            //return reporting.GetAttendanceReport(CaptureId);
            return DTOSerializer.Serialize(reporting.GetAttendanceReport(CaptureId)).InnerXml;

        }
    }
}
