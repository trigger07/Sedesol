using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SEDESOL.DataEntities.DTO;
using SEDESOL.DataEntities.IntegrationObjects;
using SEDESOL.UI.SedesolService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SEDESOL.UI.Controllers
{
    public class RationController : Controller
    {
        [SessionExpireFilterAttribute]
        public ActionResult Create()
        {
            RationModel model = new RationModel();
            List<RationDTO> listRation = new List<RationDTO>();
            SedesolServiceClient proxy = new SedesolServiceClient();
            model.ListYear = proxy.GetYears();
            model.ListMonth = proxy.GetMonths();
            model.ListRation = listRation;
            var states = proxy.GetActiveStates();

            var itemToRemove = states.Single(r => r.Id == Convert.ToInt32(ConfigurationManager.AppSettings["nacExtranjero"].ToString()));
            states.Remove(itemToRemove);

            model.ListState = states;

            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult Search(int pYear=0, int pMonth=0)
        {
            try
            {
                RationModel model = new RationModel();
                SedesolServiceClient proxy = new SedesolServiceClient();

                model = proxy.GetRationSearch(pYear, pMonth);
                proxy.Close();

                return PartialView("RationList", model.ListRation);
            }
            catch (Exception ex)
            {
                return PartialView("RationList", null);
            }

        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        public ActionResult UploadFile(int pYear, int pMonth)
        {
            string result = string.Empty;

            SedesolServiceClient client = new SedesolServiceClient();

            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];

                if (file != null && file.ContentLength > 0)
                {
                    DataTable dt = new DataTable();
                    dt = GetDataTableFromSpreadsheet(file.InputStream, false, pYear, pMonth);
                    dt.TableName = "dtRations";
                    if (dt.Rows.Count > 1)
                        result = client.SaveRations(DTOSerializer.Serialize(dt).InnerXml);
                    else
                        result = "No se pudo obtener la informaciòn del archivo";
                }
            }
            RationModel model = new RationModel();
            model = client.GetRationSearch(pYear, pMonth);
            client.Close();

            string viewContent = ConvertViewToString("RationList", model.ListRation);
            return Json(new { message = result, PartialView = viewContent });
        }

        public static DataTable GetDataTableFromSpreadsheet(Stream MyExcelStream, bool ReadOnly, int pYear, int pMonth)
        {
            DataTable dt = new DataTable();
            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream, ReadOnly))
            {
                WorkbookPart workbookPart = sDoc.WorkbookPart;
                IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dt.Columns.Add(GetCellValue(sDoc, cell));
                }

                dt.Columns.Add("Year");
                dt.Columns.Add("Month");

                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();

                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        tempRow[i] = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
                    }
                    tempRow[2] = pYear;
                    tempRow[3] = pMonth;

                    dt.Rows.Add(tempRow);
                }
            }
            dt.Rows.RemoveAt(0);
            return dt;
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }

        [SessionExpireFilterAttribute]
        public ActionResult DownloadFile()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\" + "Raciones_Plantilla.xlsx";

            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.ContentType = "application/text";
            Response.Charset = "UTF-8";
            Response.WriteFile(filePath);
            Response.Flush();
            Response.End();

            return View();
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
        public ActionResult GetCouponFile(int pIdRation)
        {
            SedesolServiceClient client = new SedesolServiceClient();
            var couponModel = client.GetCouponData(pIdRation);
            byte[] bytes;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 10, 10);

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                GetPdfSheet(couponModel, pdfDoc);

                pdfDoc.Close();

                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }

            //FileResult fileResult = new FileContentResult(bytes, "application/pdf");
            //fileResult.FileDownloadName = "Vale_" + couponModel.Folio + "_" + couponModel.Month + "" + couponModel.Year + ".pdf";
            //return fileResult;

            Response.Clear();
            string pdfName = "Vale_" + couponModel.Folio + "_" + couponModel.Month + "" + couponModel.Year;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfName + ".pdf");
            Response.ContentType = "application/pdf";
            Response.Buffer = true;
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.BinaryWrite(bytes);
            Response.End();
            Response.Close();

            return RedirectToAction("Create");
        }

        [SessionExpireFilterAttribute]
        public ActionResult GetCouponFileByState(int pIdState = 0, int pYear = 0, int pMonth = 0)
        {
            SedesolServiceClient client = new SedesolServiceClient();
            List<CouponModel> couponModelList = client.GetCouponDataByState(pIdState, pYear, pMonth);
            byte[] bytes;
            int count = couponModelList.Count();

           using (MemoryStream memoryStream = new MemoryStream())
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 10, 10);

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                foreach (var item in couponModelList)
                {
                    GetPdfSheet(item, pdfDoc);
                    count--;
                    if (count != 0)
                        pdfDoc.NewPage();
                }

                pdfDoc.Close();

                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }

            Response.Clear();
            string pdfName = "Consolidado-" + pIdState.ToString() + "_" + pMonth.ToString() + "" + pYear.ToString();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfName + ".pdf");
            Response.ContentType = "application/pdf";
            Response.Buffer = true;
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.BinaryWrite(bytes);
            Response.End();
            Response.Close();

            return RedirectToAction("Create");
        }

        private void GetPdfSheet(CouponModel couponModel, iTextSharp.text.Document pdfDoc)
        {
            #region HeaderTable
            // tabla header
            PdfPTable tableHeader = new PdfPTable(4);
            tableHeader.WidthPercentage = 97;
            tableHeader.SetWidths(new[] { 20, 62, 15, 12 });

            BaseFont bfHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(bfHeader, 7, iTextSharp.text.Font.NORMAL);

            iTextSharp.text.Font fontRed = new iTextSharp.text.Font(bfHeader, 7, iTextSharp.text.Font.NORMAL);
            fontRed.SetColor(255, 0, 0);

            PdfPCell cellHeader = new PdfPCell(new Phrase("", fontHeader));
            cellHeader.Rowspan = 3;
            cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            cellHeader = new PdfPCell(new Phrase("Secretaría de Desarrollo Social", fontHeader));
            cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            cellHeader = new PdfPCell(new Phrase("Folio Comedor:", fontHeader));
            //cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            cellHeader = new PdfPCell(new Phrase(couponModel.Folio, fontHeader));
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            //linea 2
            cellHeader = new PdfPCell(new Phrase("Programa de Comedores Comunitarios", fontHeader));
            cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            cellHeader = new PdfPCell(new Phrase("Folio Vale:", fontHeader));
            //cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            cellHeader = new PdfPCell(new Phrase(couponModel.Coupon_Folio, fontHeader));
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            //linea 3
            cellHeader = new PdfPCell(new Phrase("FCCOM G", fontRed));
            cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            cellHeader = new PdfPCell(new Phrase("Concepto del Vale:", fontHeader));
            //cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            cellHeader = new PdfPCell(new Phrase("ABASTO", fontHeader));
            //cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.Border = PdfPCell.NO_BORDER;
            tableHeader.AddCell(cellHeader);

            pdfDoc.Add(tableHeader);

            #endregion

            #region TableLocation
            //*****************************************************************************************
            //tabla location
            PdfPTable tableLoc = new PdfPTable(3);
            tableLoc.WidthPercentage = 97;
            tableLoc.SpacingBefore = 5;
            tableLoc.SetWidths(new[] { 15, 20, 65 });

            BaseFont bfLoc = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontLoc = new iTextSharp.text.Font(bfLoc, 7, iTextSharp.text.Font.NORMAL);


            PdfPCell cellLoc = new PdfPCell(new Phrase("Región:", fontHeader));
            tableLoc.AddCell(cellLoc);

            cellLoc = new PdfPCell(new Phrase(couponModel.Region, fontHeader));
            tableLoc.AddCell(cellLoc);

            cellLoc = new PdfPCell(new Phrase("Domicilio:\n" + couponModel.Address, fontHeader));
            cellLoc.Rowspan = 5;
            tableLoc.AddCell(cellLoc);

            cellLoc = new PdfPCell(new Phrase("Municipio:", fontHeader));
            tableLoc.AddCell(cellLoc);

            cellLoc = new PdfPCell(new Phrase(couponModel.Description, fontHeader));
            tableLoc.AddCell(cellLoc);

            cellLoc = new PdfPCell(new Phrase("No de Beneficiarios:", fontHeader));
            tableLoc.AddCell(cellLoc);

            cellLoc = new PdfPCell(new Phrase(couponModel.RationQuantity.ToString(), fontHeader));
            tableLoc.AddCell(cellLoc);

            cellLoc = new PdfPCell(new Phrase("Fecha de Solicitud:", fontHeader));
            tableLoc.AddCell(cellLoc);

            cellLoc = new PdfPCell(new Phrase(DateTime.Now.ToShortDateString(), fontHeader));
            tableLoc.AddCell(cellLoc);

            pdfDoc.Add(tableLoc);
            #endregion

            #region ProductTable
            //*****************************************************************************************
            //tabla productos
            PdfPTable table = new PdfPTable(13);
            table.WidthPercentage = 97;
            table.SpacingBefore = 5;

            table.SetWidths(new[] { 3, 39, 10, 10, 10, 10, 10, 10, 10, 4, 4, 4, 4 });
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontTP = new iTextSharp.text.Font(bf, 7, iTextSharp.text.Font.NORMAL);


            //linea 1
            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = PdfPCell.NO_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Primera Entrega", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(165, 165, 165);
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Segunda Entrega", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(165, 165, 165);
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Tercera Entrega", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(165, 165, 165);
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Tercera Entrega", fontTP));
            cell.Rotation = -90;
            cell.Rowspan = 6;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Segunda Entrega", fontTP));
            cell.Rotation = -90;
            cell.Rowspan = 6;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Primera Entrega", fontTP));
            cell.Rotation = -90;
            cell.Rowspan = 6;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nombre responsable DICOINSA", fontTP));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
            cell.Rotation = -90;
            cell.Rowspan = 27;
            table.AddCell(cell);

            //linea 2
            cell = new PdfPCell(new Phrase("Descripción del Producto", fontTP));
            cell.Colspan = 2;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Cantidad Solicitada", fontTP));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Cantidad Entregada", fontTP));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Diferencia Pendiente", fontTP));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Cantidad Entregada", fontTP));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Diferencia Pendiente", fontTP));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Cantidad Entregada", fontTP));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Diferencia Pendiente", fontTP));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
            table.AddCell(cell);




            for (int i = 1; i < 54; i++)
            {

                cell = new PdfPCell(new Phrase(i.ToString(), fontTP));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                table.AddCell(cell);
                if (couponModel.ListProduct.Count > i)
                {
                    cell = new PdfPCell(new Phrase(couponModel.ListProduct[i - 1].Description.ToString(), fontTP));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(couponModel.ListProduct[i - 1].QuantityCoupon.ToString(), fontTP));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                }
                else
                {
                    cell = new PdfPCell(new Phrase("", fontTP));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("", fontTP));
                    table.AddCell(cell);
                }
                cell = new PdfPCell(new Phrase("", fontTP));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", fontTP));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", fontTP));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", fontTP));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", fontTP));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("", fontTP));
                table.AddCell(cell);
                if (i == 5)
                {
                    cell = new PdfPCell(new Phrase(" ", fontTP));
                    cell.Rowspan = 21;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" ", fontTP));
                    cell.Rowspan = 21;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" ", fontTP));
                    cell.Rowspan = 21;
                    table.AddCell(cell);

                }
                if (i == 26)
                {
                    cell = new PdfPCell(new Phrase("Tercera Entrega", fontTP));
                    cell.Rotation = -90;
                    cell.Rowspan = 6;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Segunda Entrega", fontTP));
                    cell.Rotation = -90;
                    cell.Rowspan = 6;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Primera Entrega", fontTP));
                    cell.Rotation = -90;
                    cell.Rowspan = 6;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Nombre Vocal de Alimentación", fontTP));
                    cell.Rowspan = 28;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(216, 216, 216);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Rotation = -90;
                    table.AddCell(cell);
                }
                if (i == 32)
                {
                    cell = new PdfPCell(new Phrase(" ", fontTP));
                    cell.Rowspan = 22;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" ", fontTP));
                    cell.Rowspan = 22;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(couponModel.Contact, fontTP));
                    cell.Rowspan = 22;
                    cell.Rotation = -90;
                    table.AddCell(cell);
                }
            }

            //footer
            cell = new PdfPCell(new Phrase("Fecha de la Entrega:", fontTP));
            cell.Colspan = 3;
            cell.Rowspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("DD/MM/AAAA", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("DD/MM/AAAA", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("DD/MM/AAAA", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 4;
            cell.Rowspan = 2;
            table.AddCell(cell);

            //linea 2
            cell = new PdfPCell(new Phrase(" ", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            //footer
            cell = new PdfPCell(new Phrase("Firma Responsable DICOINSA:", fontTP));
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.FixedHeight = 20;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.FixedHeight = 20;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.FixedHeight = 20;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.FixedHeight = 20;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 4;
            cell.FixedHeight = 20;
            table.AddCell(cell);

            //footer
            cell = new PdfPCell(new Phrase("Firma Vocal de Alimentación:", fontTP));
            cell.Colspan = 3;
            cell.Rowspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.FixedHeight = 20;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.FixedHeight = 20;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.FixedHeight = 20;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("", fontTP));
            cell.Colspan = 4;
            cell.FixedHeight = 20;
            table.AddCell(cell);

            pdfDoc.Add(table);

            #endregion

            #region FooterTable
            //*************************** tabla footer
            PdfPTable tablefooter = new PdfPTable(1);
            tablefooter.WidthPercentage = 97;
            tablefooter.SpacingBefore = 7;
            tablefooter.SetWidths(new[] { 100 });

            BaseFont bfFooter = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontFooter = new iTextSharp.text.Font(bfFooter, 7, iTextSharp.text.Font.NORMAL);

            PdfPCell cellFooter = new PdfPCell(new Phrase("Este Programa es público, ajeno a cualquier partido político. Queda prohibido su uso para fines distintos al desarrollo social.", fontFooter));
            cellFooter.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFooter.Border = PdfPCell.NO_BORDER;
            tablefooter.AddCell(cellFooter);

            pdfDoc.Add(tablefooter);

            #endregion

            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(Server.MapPath("../img") + "/logo_logo_color_Sedesol_gobmx.png");
            pdfImage.ScaleToFit(100, 50);
            pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING; pdfImage.SetAbsolutePosition(30, 800);
            pdfDoc.Add(pdfImage);

           //return pdfDoc;

        }
        #region HtmlToPdf
        //[SessionExpireFilterAttribute]
        //public ActionResult GetCouponFile(int pIdRation)
        //{
        //    SedesolServiceClient client = new SedesolServiceClient();
        //    var couponModel = client.GetCouponData(pIdRation);
        //    byte[] bytes;
        //    string finalHtml = string.Empty;

        //    StringBuilder sb1 = new StringBuilder();
        //    //tabla encabezado
        //    sb1.Append("<div style='margin-top:10px;'>");
        //    sb1.Append("<table border=0 width='100%' style='font-size:9pt; width:100%; margin-top:20px;'><tbody>");
        //    sb1.Append("<tr><td style='text-align: center'>Secretaría de Desarrollo Social</td></tr>");
        //    sb1.Append("<tr style='text-align: center'><td>Programa de Comedores Comunitarios</td></tr>");
        //    sb1.Append("</tbody></table>");
        //    finalHtml = sb1.ToString();

        //    StringBuilder sb = GetHtml(couponModel);
        //    finalHtml = finalHtml + sb.ToString();
        //    finalHtml = finalHtml + "</div>";

        //    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 16f, 16f, 10f, 10f);

        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, memoryStream);
        //        pdfDoc.Open();

        //        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(Server.MapPath("../img") + "/logo_logo_color_Sedesol_gobmx.png");
        //        pdfImage.ScaleToFit(100, 50);
        //        pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING; pdfImage.SetAbsolutePosition(30, 790);
        //        pdfDoc.Add(pdfImage);

        //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, new StringReader(finalHtml));
        //        pdfDoc.Close();

        //        bytes = memoryStream.ToArray();
        //        memoryStream.Close();
        //    }

        //    FileResult fileResult = new FileContentResult(bytes, "application/pdf");
        //    fileResult.FileDownloadName = "Vale_" + couponModel.Folio + "_" + couponModel.Month + "" + couponModel.Year + ".pdf";
        //    return fileResult;

        //}

        //[SessionExpireFilterAttribute]
        //public ActionResult GetCouponFileByState(int pIdState = 0, int pYear=0, int pMonth=0)
        //{
        //    SedesolServiceClient client = new SedesolServiceClient();
        //    List<CouponModel> couponModelList = client.GetCouponDataByState(pIdState, pYear, pMonth);
        //    byte[] bytes;
        //    string finalHtml = string.Empty;
        //    int count = couponModelList.Count();

        //    StringBuilder sb1 = new StringBuilder();
        //    //tabla encabezado
        //    sb1.Append("<div style='margin-top:10px;'>");
        //    sb1.Append("<table border=0 width='100%' style='font-size:9pt; width:100%; margin-top:20px;'><tbody>");
        //    sb1.Append("<tr><td style='text-align: center'>Secretaría de Desarrollo Social</td></tr>");
        //    sb1.Append("<tr style='text-align: center'><td>Programa de Comedores Comunitarios</td></tr>");
        //    sb1.Append("</tbody></table>");

        //    finalHtml = sb1.ToString();

        //    foreach (var item in couponModelList)
        //    {

        //        StringBuilder sb = GetHtml(item);
        //        finalHtml = finalHtml + sb.ToString();
        //        count--;
        //        if (count != 0)
        //            finalHtml = finalHtml + "<div style='page-break-before:always'>&nbsp;</div>";
        //    }
        //    finalHtml = finalHtml + "</div>";

        //    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 16f, 16f, 10f, 10f);

        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, memoryStream);
        //        pdfDoc.Open();

        //        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(Server.MapPath("../img") + "/logo_logo_color_Sedesol_gobmx.png");
        //        pdfImage.ScaleToFit(100, 50);
        //        pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING; pdfImage.SetAbsolutePosition(30, 790);
        //        pdfDoc.Add(pdfImage);

        //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, new StringReader(finalHtml));
        //        pdfDoc.Close();

        //        bytes = memoryStream.ToArray();
        //        memoryStream.Close();
        //    }

        //    FileResult fileResult = new FileContentResult(bytes, "application/pdf");
        //    fileResult.FileDownloadName = "Consolidado-" + pIdState.ToString() + "_" + pMonth.ToString() + "" + pYear.ToString() + ".pdf";
        //    return fileResult;

        //}

        //private StringBuilder GetHtml(CouponModel couponModel)
        //{
        //    StringBuilder sb = new StringBuilder();



        //    //folios
        //    sb.Append("<table border=1 width='100%' style='border-collapse: collapse; font-size:8pt; margin-top:10px;'><tbody>");
        //    sb.Append("<tr> style='height: 26px;'<td>Folio Comedor:</td><td style='height: 26px;'>" + couponModel.Folio + "</td><td style='height: 26px;'>Folio Vale:</td><td style='height: 26px;'>" + couponModel.Id_Coupon + "</td>");
        //    sb.Append("<td style='height: 26px;'>Concepto Vale:</td><td>ABASTO</td></tr>");
        //    sb.Append("</tbody></table>");
        //    //datos de comedor
        //    sb.Append("<table style='width: 100%; border-collapse: collapse; font-size:8pt; margin-top:5px;'><tbody>");
        //    sb.Append("<tr style='height: 26px;'>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'>Región:</td>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'>" + couponModel.Region + "</td><td style='width: 60%; height: 26px; border: 1px solid; valign: top;' rowspan='5'>Domicilio:</td></tr>");
        //    sb.Append("<tr style='height: 26px;'>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'>Municipio:</td>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'></td></tr>");
        //    sb.Append("<tr style='height: 26px;'>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'>Localidad:</td>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'></td></tr>");
        //    sb.Append("<tr style='height: 26px;'>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'>No de Beneficiarios:</td>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'>" + couponModel.RationQuantity + "</td></tr>");
        //    sb.Append("<tr style='height: 26px;'>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'>Fecha Solicitud:</td>");
        //    sb.Append("<td style='width: 20%; height: 26px; border: 1px solid;'>" + DateTime.Now.ToShortDateString() + "</td></tr>");
        //    sb.Append("</tbody></table>");

        //    //nueva tabla productos
        //    sb.Append("<table style='width: 100%; border-collapse: collapse; font-size:7pt; margin-top:10px;'><tbody>");
        //    sb.Append("<tr style='height: 25px;'><td style='width: 80%; height: 26px; text-align: center;'></td>");
        //    sb.Append("<td style='width: 20%; height: 25px; text-align: center;'></td>");
        //    sb.Append("<td style='width: 40%; height: 25px; text-align: center; border: 1px solid; bgcolor:#00FF00' colspan='2' bgcolor='#00FF00'><strong>Primera Entrega</strong></td>");
        //    sb.Append("<td style='width: 40%; height: 25px; text-align: center; border: 1px solid;' colspan='2'><strong>Segunda Entrega</strong></td>");
        //    sb.Append("<td style='width: 40%; height: 25px; text-align: center; border: 1px solid;' colspan='2'><strong>Tercera Entrega</strong></td>");
        //    sb.Append("</tr>");
        //    sb.Append("<tr style='height: 25px;'>");
        //    sb.Append("<td style='width: 80%; height: 25px; text-align: center; border: 1px solid;'><strong>Descripci&oacute;n del Producto</strong></td>");
        //    sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'><strong>Cantidad Solicitada</strong></td>");
        //    sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'><strong>Cantidad Entregada</strong></td>");
        //    sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'><strong>Diferencia Pendiente</strong></td>");
        //    sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'><strong>Cantidad Entregada</strong></td>");
        //    sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'><strong>Diferencia Pendiente</strong></td>");
        //    sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'><strong>Cantidad Entregada</strong></td>");
        //    sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'><strong>Diferencia Pendiente</strong></td></tr>");
        //    // registros de productos
        //    foreach (var item in couponModel.ListProduct)
        //    {

        //        sb.Append("<tr style='height: 25px;'>");
        //        sb.Append("<td style='width: 80%; height: 25px; border: 1px solid;'>" + item.Description + "</td>");
        //        sb.Append("<td style='width: 20%; height: 25px; border: 1px solid;'>" + item.QuantityCoupon + "</td>");
        //        sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'></td>");
        //        sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'></td>");
        //        sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'></td>");
        //        sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'></td>");
        //        sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'></td>");
        //        sb.Append("<td style='width: 20%; height: 25px; text-align: center; border: 1px solid;'></td></tr>");
        //    }
        //    sb.Append("</tbody></table>");

        //    //firmas
        //    sb.Append("<table style='width: 100%; border-collapse: collapse; font-size:7pt; margin-top:5px;'><tbody>");
        //    sb.Append("<tr style='height: 30px;'><td style='width: 100%; height: 30px; text-align: right; border: 1px solid;' colspan='2'>Fecha de la Entrega (DD/MM/AAAA)&nbsp;&nbsp;&nbsp;</td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("</tr>");
        //    sb.Append("<tr style='height: 30px;'><td style='width: 80%; height: 30px; text-align: right; border: 1px solid;' colspan='2'>Firma Responsable DICONSA&nbsp;&nbsp;&nbsp;</td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("</tr>");
        //    sb.Append("<tr style='height: 30px;'><td style='width: 80%; height: 30px; text-align: right; border: 1px solid;' colspan='2'>Nombre Responsable DICONSA&nbsp;&nbsp;&nbsp;</td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("</tr>");
        //    sb.Append("<tr style='height: 30px;'><td style='width: 80%; height: 30px; text-align: right; border: 1px solid;' colspan='2'>Firma Vocal de Alimentación&nbsp;&nbsp;&nbsp;</td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("</tr>");
        //    sb.Append("<tr style='height: 30px;'><td style='width: 80%; height: 30px; text-align: right; border: 1px solid;' colspan='2'>Nombre Vocal de Alimentación&nbsp;&nbsp;&nbsp;</td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("<td style='width: 40%; height: 30px; text-align: center; border: 1px solid;' colspan='2'><strong></strong></td>");
        //    sb.Append("</tr>");

        //    sb.Append("</tbody></table>");

        //    //tabla footer
        //    sb.Append("<table border=0 width='100%' style='font-size:6pt; width:100%; margin-top:20px;'><tbody>");
        //    sb.Append("<tr><td style='text-align: center'>Este Programa es público, ajeno a cualquier otro partido político. Queda prohibido su uso para fines distintos al desarrollo social</td></tr>");
        //    sb.Append("</tbody></table>");
        //    //sb.Append("</div>");


        //    return sb;
        //}
        #endregion

        [SessionExpireFilterAttribute]
        public ActionResult ExportExcel(int pIdState = 0, int pMonth = 0, int pYear = 0)
        {
            string data = string.Empty;
            SedesolServiceClient proxy = new SedesolServiceClient();

            DataSet ds = (DataSet)DTOSerializer.Deserialize(proxy.GetDataExcel(pIdState, pMonth, pYear), typeof(DataSet));
            GridView gv = new GridView();
            DataTable dt1 = ds.Tables[0];
            //DataTable dt2 = ds.Tables[1];

            //#region Create Dt
            //DataTable dt = new DataTable();

            //dt.Columns.Add("Folio Comedor", typeof(string));
            //dt.Columns.Add("Entidad", typeof(string));
            //dt.Columns.Add("Folio Vale", typeof(string));
            //dt.Columns.Add("Fecha de Emision", typeof(string));
            //dt.Columns.Add("No.Beneficiarios", typeof(string));

            //foreach (DataRow col in dt1.Rows)
            //{
            //    dt.Columns.Add(col[0].ToString(), typeof(string));
            //}

            //#endregion

            
            //DataRow sampleDataRow;
            //foreach (DataRow col in dt2.Rows)
            //{
            //    sampleDataRow = dt.NewRow();
            //    sampleDataRow["Folio Comedor"] = col[0].ToString();
            //    sampleDataRow["Entidad"] = "";
            //    sampleDataRow["Folio Vale"] = col[1].ToString();
            //    sampleDataRow["Fecha de Emision"] = DateTime.Now.ToShortDateString();
            //    sampleDataRow["No.Beneficiarios"] = col[2].ToString();

            //    dt.Rows.Add(sampleDataRow);
            //}

            gv.DataSource = dt1;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Consolidado.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            string style = @"<style> td { mso-number-format:\@;} </style>";
            Response.Output.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Create");
        }
    }
}