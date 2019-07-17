using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEDESOL.DataEntities;
using SEDESOL.UI.Models;
using SEDESOL.UI.SedesolService;
using SEDESOL.DataEntities.IntegrationObjects;
using SEDESOL.DataEntities.DTO;
using System.Configuration;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using SelectPdf;

namespace SEDESOL.UI.Controllers
{
    public class InspectionCodeController : Controller
    {
        [SessionExpireFilterAttribute]
        // GET: InspectionCode
        public ActionResult Index()
        {
            SedesolServiceClient proxy = new SedesolServiceClient();
            CodeParam model = proxy.GetParamCode(((UserDTO)Session["userData"]).Id);
            List<YearDTO> years = model.Years;

            years = years.Where(item => Convert.ToInt32(item.Description) <= DateTime.Now.Year).ToList();
            model.Years = years;

            return View(model);
        }

        [SessionExpireFilterAttribute]
        public JsonResult GenerateCode()
        {
            SedesolServiceClient proxy = new SedesolServiceClient();
            string data = proxy.GenerateCode(((UserDTO)Session["userData"]).Id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult SaveCode(InspectionCodeDTO att)
        {
            try
            {
                att.Id_User = ((UserDTO)Session["userData"]).Id;
                SedesolServiceClient proxy = new SedesolServiceClient();
                att = proxy.SaveInspectionCode(att);
                List<InspectionCodeDTO> listCode = new List<InspectionCodeDTO>();

                if (att.Message == "SUCCESS")
                {
                    listCode = proxy.GetCodesByUserId(((UserDTO)Session["userData"]).Id);
                }

                string viewContent = ConvertViewToString("CodeList", listCode);
                return Json(new { message = att.Message, PartialView = viewContent });
                //return PartialView("AttendanceList", captureModel.Capture.AttendanceList);
            }
            catch (Exception ex)
            {
                //return PartialView("AttendanceList", null);
                return Json(new { message = "ERROR" });
            }


        }

        private string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }

        [SessionExpireFilterAttribute]
        public ActionResult ExportToPdf(string pYear, string pMonth, string pCode)
        {
            // read parameters from the webpage
            string htmlString = "<html><body><div style=\" margin-top:30px; margin-left:20px;\"><p>" + pMonth + " | " + pYear + "</p><p><b><h2>Código de Inspección: " + pCode + "</h2></b></p><div></body></html>";
            string baseUrl = string.Empty;

            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(
                typeof(PdfPageOrientation), pdf_orientation, true);

            int webPageWidth = 1024;
            try
            {
                webPageWidth = System.Convert.ToInt32(900);
            }
            catch { }

            int webPageHeight = 0;
            try
            {
                webPageHeight = System.Convert.ToInt32(500);
            }
            catch { }

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);

            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Codigo_Inspeccion.pdf";
            return fileResult;
        }

        [SessionExpireFilterAttribute]
        public ActionResult GetDates()
        {
            List<GencodeDayDTO> model = new List<GencodeDayDTO>();

            SedesolServiceClient proxy = new SedesolServiceClient();
            model = proxy.GetGenerationCodeDayAll();

            return PartialView("AvaiDate", model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult SaveGencodeDay(GencodeDayDTO att)
        {
            try
            {
                SedesolServiceClient proxy = new SedesolServiceClient();
                att = proxy.SaveGencodeDay(att);
                List<GencodeDayDTO> listCode = new List<GencodeDayDTO>();

                if (att.Message == "SUCCESS")
                {
                    listCode = proxy.GetGenerationCodeDayAll();
                }

                string viewContent = ConvertViewToString("AvaiDateEdit", listCode);
                return Json(new { message = att.Message, PartialView = viewContent });
                //return PartialView("AttendanceList", captureModel.Capture.AttendanceList);
            }
            catch (Exception ex)
            {
                //return PartialView("AttendanceList", null);
                return Json(new { message = "ERROR" });
            }


        }

        [SessionExpireFilterAttribute]
        public JsonResult EditGenCodeDay(int id)
        {
            SedesolServiceClient proxy = new SedesolServiceClient();
            GencodeDayDTO data = proxy.GetGenerationCodeDayById(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilterAttribute]
        public ActionResult CreateGencodeDay()
        {
            GencodeDayParam model = new GencodeDayParam();

            SedesolServiceClient proxy = new SedesolServiceClient();
            model = proxy.GetParamGen();
            List<YearDTO> years = model.Years;

            years = years.Where(item => Convert.ToInt32(item.Description) <= DateTime.Now.Year).ToList();
            model.Years = years;

            return View(model);
        }
    }
}