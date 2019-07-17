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

namespace SEDESOL.UI.Controllers
{
    public class CaptureController : Controller
    {
        [SessionExpireFilterAttribute]
        public ActionResult Index()
        {
            ListCaptureModel model = new ListCaptureModel();
            SedesolServiceClient proxy = new SedesolServiceClient();
            UserDTO userDto = (UserDTO)Session["userData"];

            List<CaptureDTO> capture = proxy.GetCapturesFilter(userDto.Id_User_Type, userDto.Id);
            List<StatusDTO> status = proxy.GetStatus().Where(item => item.IsActive == true).ToList();
            List<StateDTO> states = proxy.GetStatesFilter(userDto.Id_User_Type, userDto.Id);

            var itemToRemove = states.SingleOrDefault(r => r.Id == Convert.ToInt32(ConfigurationManager.AppSettings["nacExtranjero"].ToString()));
            states.Remove(itemToRemove);

            model.ListCapture = capture;
            model.ListState = states;
            model.ListStatus = status;

            return View(model);
        }

        [SessionExpireFilterAttribute]
        public ActionResult Create()
        {
            UserDTO userDto = (UserDTO)Session["userData"];

            SedesolServiceClient proxy = new SedesolServiceClient();
            CaptureParam model = proxy.GetParamCapture(userDto.Id_User_Type, userDto.Id);
            List<YearDTO> years = model.Years;

            years = years.Where(item => Convert.ToInt32(item.Description) <= DateTime.Now.Year).ToList();
            model.Years = years;
            
            return View(model);
        }

        [SessionExpireFilterAttribute]
        public JsonResult SetParamCapture(int idYear, int idMonth, int idSoupK, string soupK, string year, string month, string pFolio, string pCode)
        {
            SedesolServiceClient proxy = new SedesolServiceClient();
            CaptureModel cModel = new CaptureModel();
            CaptureDTO captDto = new CaptureDTO();
            AttendanceDTO attDto = new AttendanceDTO();
            UserDTO userDto = (UserDTO)Session["userData"];
            string message = string.Empty;

            captDto.Id_Month = idMonth;
            captDto.Id_Year = idYear;
            captDto.Id_Soup_Kitchen = idSoupK;
            captDto.CreateDate = DateTime.Now;
            captDto.Description = year + " | " + month + " | " + soupK;
            captDto.IsActive = true;
            captDto.Id_Status = Convert.ToInt32(ConfigurationManager.AppSettings["stIniciada"].ToString());
            captDto.Folio = pFolio;
            captDto.InspectionCode = pCode;
            captDto.Id_LevelApproval = userDto.UserType.Id;
            captDto.Id_User = userDto.Id;
            cModel.Capture = proxy.SaveCapture(captDto);

            //cModel.Capture = captDto;
            cModel.Attendance = attDto;

            Session.Add("CaptureModel", cModel);

            return Json(new { message = cModel.Capture.Message });
        }

        [SessionExpireFilterAttribute]
        public ActionResult CaptureAttendance(int id = 0)
        {
            SedesolServiceClient proxy = new SedesolServiceClient();
            CaptureModel model = new CaptureModel();
            int countImg = 0;
            if (id != null && id > 0)
            {
                
                CaptureDTO capture = proxy.GetCaptureById(id);
                foreach (CaptureImageDTO item in capture.ImageList)
                {
                    item.ImageFileB64 = "data:image/jpeg; base64," + Convert.ToBase64String(item.ImageFile);
                }
                countImg = capture.ImageList.Count();
                model.Capture = capture;
                model.StateDtoList = proxy.GetActiveStates();
                model.ConditionDtoList = proxy.GetActiveCondition();

                //CaptureModel cModel = new CaptureModel();
                //cModel.Capture = capture;
                Session.Add("CountImg", countImg);
                Session.Add("CaptureModel", model);
            }
            else
            {
                model = (CaptureModel)Session["CaptureModel"];
                model.StateDtoList = proxy.GetActiveStates();
                model.ConditionDtoList = proxy.GetActiveCondition();
            }
           
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult CaptureAttendance(AttendanceDTO att)
        {
            try
            {
                CaptureModel captureModel = (CaptureModel)Session["CaptureModel"];

                att.Id_Capture = captureModel.Capture.Id;
                att.IsActive = true;
                att.CreateDate = DateTime.Now;
                var oldId = att.Id;

                SedesolServiceClient proxy = new SedesolServiceClient();

                Session["CaptureModel"] = null;
                Session.Add("CaptureModel", captureModel);

                string curpValidation = string.Empty;

                if (!att.HasCurp || att.IsAnonym)
                {
                    curpValidation = "SUCCESS";
                }
                else
                {
                    //validate curp
                    curpValidation = ValidateCurp(att.Curp);
                }
                

                if (curpValidation == "SUCCESS")
                {
                    att = proxy.SaveAttendance(att);

                    if (att.Message == "SUCCESS")
                    {
                        captureModel.StatusMessage = "SUCCESS";
                        if (oldId > 0)
                        {
                            var ItemRemove = captureModel.Capture.AttendanceList.Single(r => r.Id == oldId);
                            captureModel.Capture.AttendanceList.Remove(ItemRemove);
                        }
                        captureModel.Capture.AttendanceList.Add(att);
                    }
                    else
                    {
                        captureModel.StatusMessage = "ERROR";
                    }
                }
                else
                {
                    captureModel.StatusMessage = "ERRORCURP";
                    //att.Message = "ERRORCURP";
                }

                string viewContent = ConvertViewToString("AttendanceList", captureModel.Capture.AttendanceList);
                return Json(new { message = captureModel.StatusMessage, PartialView = viewContent, MessageError = curpValidation });
            }
            catch(Exception ex)
            {
                return Json(new { message = "ERROR" });
            }
        }

        private string ValidateCurp(string curp)
        {
            string message = "Error en la validación del CURP \n";
            bool error = false;

            SedesolServiceClient proxy = new SedesolServiceClient();

            List<ConfigurationDTO> config = proxy.GetConfiguration("CURP");
            for (int i = 0; i < curp.Length; i++)
            {
                //if(curp[i].ToString().Contains((config.Single(r => r.KeyDetail == i.ToString())).ToString()))
                //{

                //}
                if (!(config.Single(r => r.KeyDetail == (i + 1).ToString())).KeyValue.Contains(curp[i].ToString()))
                {
                    message = message + "Valor no permitido en la posición: " + (i + 1).ToString() + "\n";
                    error = true;
                }
                //else
                //{
                //    message = message + "Valor no permitido en la posición: " + i.ToString() + "\n";
                //    error = true;
                //}
            }

            if (error == false)
            {
                message = "SUCCESS";
            }

            return message;
        }

        [SessionExpireFilterAttribute]
        public JsonResult EditAttendance(int id)
        {
            SedesolServiceClient proxy = new SedesolServiceClient();
            AttendanceDTO data = proxy.GetAttendanceById(id);
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult DeleteAttendance(int idAttendance)
        {
            CaptureModel captureModel = (CaptureModel)Session["CaptureModel"];

            SedesolServiceClient proxy = new SedesolServiceClient();
            string data = proxy.DeleteAttendance(idAttendance);

            var ItemRemove = captureModel.Capture.AttendanceList.Single(r => r.Id == idAttendance);
            captureModel.Capture.AttendanceList.Remove(ItemRemove);

            Session["CaptureModel"] = null;
            Session.Add("CaptureModel", captureModel);

            string viewContent = ConvertViewToString("AttendanceList", captureModel.Capture.AttendanceList);
            return Json(new { message = data, PartialView = viewContent });
        }

        [SessionExpireFilterAttribute]
        public JsonResult GetDataForAc(string term)
        {
            SedesolServiceClient proxy = new SedesolServiceClient();
            List<AttendanceDTO> listDto = proxy.GetDataForAc(term);

            var result = new List<KeyValuePair<string, string>>();
            var namecodes = new List<SelectListItem>();
            //namecodes = (from u in db.w_Items select new SelectListItem { Text = u.ItemCode, Value = u.w_ItemId.ToString() }).ToList();

            namecodes = listDto.Select(x => new SelectListItem() { Text = x.Name + " | " + x.LastName + " | " + x.LastName2 +
                " | " + x.Curp + " | " + Convert.ToDateTime(x.Birthdate).ToShortDateString() + 
                " | " + x.Gender + " | " + x.Id_Condition + " | " + x.Id_State_Birth +
                " | " + x.Address + " | " + x.PhoneNumber, Value = "0" }).ToList();

            foreach (var item in namecodes)
            {
                result.Add(new KeyValuePair<string, string>(item.Value.ToString(), item.Text));
            }

            var namecodes1 = result.Where(s => s.Value.ToLower().Contains(term.ToLower())).Select(w => w).ToList();
            return Json(namecodes1, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilterAttribute]
        public JsonResult EditStatus(int id)
        {
            SedesolServiceClient proxy = new SedesolServiceClient();
            CaptureDTO capture = proxy.GetCaptureById(id);
            string data = string.Empty;

            UserDTO userDto = (UserDTO)Session["userData"];

            if (capture.AttendanceList.Count() == 0)
                data = "Debe registrar asistencia a la captura.";
            else if (capture.ImageList.Count() == 0)
                data = "Debe ingresar imágenes a la captura.";
            else
            {
                data = proxy.EditStatus(Convert.ToInt32(ConfigurationManager.AppSettings["stEnviada"].ToString()), id, userDto.Id_User_Type);
            }
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilterAttribute]
        public ActionResult ExportAttendance(int id)
        {
            string data = string.Empty;
            SedesolServiceClient proxy = new SedesolServiceClient();

            DataSet ds = (DataSet)DTOSerializer.Deserialize(proxy.GetAttendanceReport(id), typeof(DataSet));
            GridView gv = new GridView();

            #region Create Dt
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[11]
            {
                new DataColumn("Curp"),
                new DataColumn("Nombre"),
                new DataColumn("Apellido_Paterno"),
                new DataColumn("Apellido_Materno"),
                new DataColumn("Fecha_Nacimiento"),
                new DataColumn("Cantidad"),
                new DataColumn("Sexo"),
                new DataColumn("Condicion"),
                new DataColumn("Lugar_Nacimiento"),
                new DataColumn("Domicilio"),
                new DataColumn("Telefono")
            });

            #endregion

            if (ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow item in ds.Tables[0].DefaultView.Table.Rows)
                {
                    dt.Rows.Add(item["Curp"],
                                item["Nombre"],
                                item["Apellido_Paterno"],
                                item["Apellido_Materno"],
                                item["Fecha_Nacimiento"],
                                item["Cantidad"],
                                item["Sexo"],
                                item["Condicion"],
                                item["Lugar_Nacimiento"],
                                item["Domicilio"],
                                item["Telefono"]);
                }
            }
            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Asistencia.xls");
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

            return RedirectToAction("CaptureAttendance");
        }

        [SessionExpireFilterAttribute]
        public ActionResult ExportCapture(int idState = 0, int idSoupK = 0, int idStatus = 0)
        {
            List<FilterDTO> listFilter = new List<FilterDTO>();
            UserDTO userDto = (UserDTO)Session["userData"];

            FilterDTO dtoUserType = new FilterDTO { Name = "Id_UserType", Value = userDto.Id_User_Type.ToString() };
            listFilter.Add(dtoUserType);

            FilterDTO dtoUser = new FilterDTO { Name = "Id_User", Value = userDto.Id.ToString() };
            listFilter.Add(dtoUser);

            if (idState > 0)
            {
                FilterDTO dto = new FilterDTO { Name = "Id_State", Value = idState.ToString() };
                listFilter.Add(dto);
            }

            if (idSoupK > 0)
            {
                FilterDTO dto = new FilterDTO { Name = "Id_SoupKitchen", Value = idSoupK.ToString() };
                listFilter.Add(dto);
            }

            if (idStatus > 0)
            {
                FilterDTO dto = new FilterDTO { Name = "Id_Status", Value = idStatus.ToString() };
                listFilter.Add(dto);
            }

            SedesolServiceClient proxy = new SedesolServiceClient();

            DataSet ds = (DataSet)DTOSerializer.Deserialize(proxy.GetCaptureReport(listFilter), typeof(DataSet));
            GridView gv = new GridView();

            #region Create Dt
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("Estado"),
                new DataColumn("Descripción"),
                new DataColumn("Comedor"),
                new DataColumn("Ubicación"),
                new DataColumn("Fecha_Creación"),
                new DataColumn("Beneficiarios"),
                new DataColumn("Asistencia")
            });

            #endregion

            if (ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow item in ds.Tables[0].DefaultView.Table.Rows)
                {
                    dt.Rows.Add(item["Estado"],
                                item["Descripcion"],
                                item["Comedor"],
                                item["Location"],
                                item["CreateDate"],
                                item["Beneficiarios"],
                                item["Asistencia"]);
                }
            }
            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Captura.xls");
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

            return RedirectToAction("Index");
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult FindSoupKitchen(int StateId)
        {
            UserDTO userDto = (UserDTO)Session["userData"];

            SedesolServiceClient proxy = new SedesolServiceClient();
            List<SoupKitchenDTO> kitchenList = new List<SoupKitchenDTO>();
            kitchenList = proxy.GetSoupKitchenByUser(userDto.Id_User_Type, userDto.Id);
            var filteredCollection = kitchenList.Where(item => item.Id_State == StateId).ToList();

            return Json(filteredCollection);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult FindMonth(int year)
        {
            UserDTO userDto = (UserDTO)Session["userData"];

            SedesolServiceClient proxy = new SedesolServiceClient();
            CaptureParam model = proxy.GetParamCapture(userDto.Id_User_Type, userDto.Id);
            List<MonthDTO> months = model.Months;

            if (year == DateTime.Now.Year)
                months = months.Where(item => item.Id <= DateTime.Now.Month).ToList();
            

            return Json(months);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public ActionResult SearchCaptures(int pState=0, int pSoupK=0, int pStatus=0)
        {
            try
            {
                UserDTO userDto = (UserDTO)Session["userData"];

                ListCaptureModel model = new ListCaptureModel();
                SedesolServiceClient proxy = new SedesolServiceClient();
                List<CaptureDTO> capture = proxy.GetCapturesSearch(userDto.Id_User_Type, userDto.Id, pState, pSoupK, pStatus);

                //if (pState > 0)
                //    capture = capture.Where(item => item.SoupKitchen.State.Id == pState).ToList();
                //if (pSoupK > 0)
                //    capture = capture.Where(item => item.SoupKitchen.Id == pSoupK).ToList();

                return PartialView("CaptureList", capture);
            }
            catch (Exception ex)
            {
                return PartialView("CaptureList", null);
            }

        }

        [SessionExpireFilterAttribute]
        public ActionResult GetModalAttendance(int id)
        {
            CaptureModel model = new CaptureModel();

            if (id != null && id > 0)
            {
                SedesolServiceClient proxy = new SedesolServiceClient();
                CaptureDTO capture = proxy.GetCaptureById(id);
                model.Capture = capture;
                CaptureModel cModel = new CaptureModel();

                cModel.Capture = capture;

                //Session.Add("CaptureModel", cModel);
            }
            else
            {
                model = (CaptureModel)Session["CaptureModel"];
            }
            return PartialView("ModalAttendance", model);
        }

        //[SessionExpireFilterAttribute]
        //public ActionResult Approve(int id)
        //{
        //    SedesolServiceClient proxy = new SedesolServiceClient();
        //    string data = proxy.EditStatus(Convert.ToInt32(ConfigurationManager.AppSettings["stAprobada"].ToString()), id);
        //    return RedirectToAction("Index");
        //}

        //[SessionExpireFilterAttribute]
        //public ActionResult Reject(int id)
        //{
        //    SedesolServiceClient proxy = new SedesolServiceClient();
        //    string data = proxy.EditStatus(Convert.ToInt32(ConfigurationManager.AppSettings["stRechazada"].ToString()), id);
        //    return RedirectToAction("Index");
        //}

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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Description,CreateDate,IsActive,Id_Month,Id_Year,Id_Status")] CAPTURE cAPTURE)
        //{

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id = 0)
        //{
        //    return View();
        //}

        [SessionExpireFilterAttribute]
        public ActionResult UploadFiles(string pFileCam, HttpPostedFileBase pFile, string pType)
        {
            string responseS = string.Empty;
            SedesolServiceClient proxy = new SedesolServiceClient();
            string viewContent = string.Empty;
            int countImage = Convert.ToInt32(Session["CountImg"].ToString());
            try
            {
                if (countImage < Convert.ToInt32(ConfigurationManager.AppSettings["maxCountFiles"].ToString()))
                {
                    if (pType == "1")
                    {
                        if (pFileCam != string.Empty)
                        {
                            string[] ar = pFileCam.Split(',');
                            CaptureModel captureModel = (CaptureModel)Session["CaptureModel"];
                            CaptureImageDTO dto = new CaptureImageDTO();

                            dto.Id_Capture = captureModel.Capture.Id;
                            dto.FromCam = false;
                            dto.ImageFile = Convert.FromBase64String(ar[1]);
                            dto.ImageFileB64 = string.Empty;
                            dto.ImagePath = string.Empty;
                            dto.Name = "test";

                            int response = proxy.SaveCaptureImage(dto);
                            if (response > 0)
                                responseS = "SUCCESS";
                        }
                        else
                        {
                            responseS = "Debe activar la cámara y tomar la foto.";
                        }
                    }
                    else
                    {
                        //fileupload
                        if (Request.Files.Count > 0 && pFile != null)
                        {
                            if (pFile.ContentType == "image/jpeg")
                            {
                                if (pFile.ContentLength <= (1048576))
                                {
                                    string fname;
                                    //bool success = false;

                                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                    {
                                        string[] testfiles = pFile.FileName.Split(new char[] { '\\' });
                                        fname = testfiles[testfiles.Length - 1];
                                    }
                                    else
                                    {
                                        fname = pFile.FileName;
                                    }

                                    BinaryReader binaryReader = new BinaryReader(pFile.InputStream);
                                    byte[] binaryData = binaryReader.ReadBytes((int)pFile.InputStream.Length);

                                    CaptureModel captureModel = (CaptureModel)Session["CaptureModel"];
                                    CaptureImageDTO dto = new CaptureImageDTO();

                                    dto.Id_Capture = captureModel.Capture.Id;
                                    dto.FromCam = false;
                                    dto.ImageFile = binaryData;
                                    dto.ImageFileB64 = string.Empty;
                                    dto.ImagePath = string.Empty;
                                    dto.Name = fname;

                                    int response = proxy.SaveCaptureImage(dto);
                                    if (response > 0)
                                        responseS = "SUCCESS";
                                }
                                else
                                {
                                    responseS = "Tamaño máximo permitido: 1MB.";
                                }
                            }
                            else
                            {
                                responseS = "El archivo ingresado debe ser en formato jpg.";
                            }
                        }
                        else
                        {
                            responseS = "Debe seleccionar un archivo.";
                        }
                    }
                }
                else
                {
                    responseS = "Máximo de imágenes por captura: " + ConfigurationManager.AppSettings["maxCountFiles"].ToString();
                }
            }
            catch(Exception ex)
            {
                responseS = "Error al guardar archivo. Contacte al Administrador.";
            }
            finally
            {
                CaptureModel captureModel = (CaptureModel)Session["CaptureModel"];
                List<CaptureImageDTO> listImage = new List<CaptureImageDTO>();

                listImage = proxy.GetImageByCaptureId(captureModel.Capture.Id);
                Session["CountImg"] = null;
                Session.Add("CountImg", listImage.Count());

                foreach (CaptureImageDTO item in listImage)
                {
                    item.ImageFileB64 = "data:image/jpeg; base64," + Convert.ToBase64String(item.ImageFile);
                }
                proxy.Close();
                viewContent = ConvertViewToString("ImageList", listImage);
            }
            var sjonResult = Json(new { message = responseS, PartialView = viewContent });
            sjonResult.MaxJsonLength = int.MaxValue;

            return sjonResult;
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult DeleteFile(int idFile)
        {
            var client = new SedesolService.SedesolServiceClient();
            string data = client.DeleteCaptureImage(idFile);

            CaptureModel captureModel = (CaptureModel)Session["CaptureModel"];
            List<CaptureImageDTO> listImage = new List<CaptureImageDTO>();

            listImage = client.GetImageByCaptureId(captureModel.Capture.Id);
            foreach (CaptureImageDTO item in listImage)
            {
                item.ImageFileB64 = "data:image/jpeg; base64," + Convert.ToBase64String(item.ImageFile);
            }

            Session["CountImg"] = null;
            Session.Add("CountImg", listImage.Count());

            client.Close();

            string viewContent = ConvertViewToString("ImageList", listImage);
            var sjonResult = Json(new { message = data, PartialView = viewContent });

            sjonResult.MaxJsonLength = int.MaxValue;

            return sjonResult;
        }

        [SessionExpireFilter]
        public PartialViewResult Pictures(int hq)
        {
            var client = new SedesolService.SedesolServiceClient();
            List<CaptureImageDTO> pictures = new List<CaptureImageDTO>();
            pictures = client.GetImageByCaptureId(hq);

            foreach (CaptureImageDTO item in pictures)
            {
                item.ImageFileB64 = Convert.ToBase64String(item.ImageFile);
            }

            return PartialView(pictures);
        }

        [SessionExpireFilterAttribute]
        public ActionResult GetModalApproval(int id)
        {
            List<CaptureApprovalDTO> listApproval = new List<CaptureApprovalDTO>();

            if (id > 0)
            {
                SedesolServiceClient proxy = new SedesolServiceClient();
                listApproval = proxy.GetApprovalByCaptureId(id);

            }
            return PartialView("ModalHistoricApproval", listApproval);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult SendToApproval2(int idCapture, int approved, int pState, int pSoupK, int pStatus)
        {
            UserDTO userDto = (UserDTO)Session["userData"];

            SedesolServiceClient proxy = new SedesolServiceClient();
            CaptureApprovalDTO dto = new CaptureApprovalDTO();
            List<CaptureDTO> captureList = new List<CaptureDTO>();

            dto.Id_Capture = idCapture;
            dto.UserDto = userDto;
            if (approved == 1)
                dto.Id_Status = Convert.ToInt32(ConfigurationManager.AppSettings["stAprobada"].ToString());
            else
                dto.Id_Status = Convert.ToInt32(ConfigurationManager.AppSettings["stRechazada"].ToString());
            dto.Id_User = userDto.Id;

            string resp = proxy.SendToApproval(dto);
            if (resp == "SUCCESS")
            {
                captureList = proxy.GetCapturesSearch(userDto.Id_User_Type, userDto.Id, pState, pSoupK, pStatus);
            }
            string viewContent = ConvertViewToString("CaptureList", captureList);
            return Json(new { message = resp, PartialView = viewContent });
        }
    }
}
