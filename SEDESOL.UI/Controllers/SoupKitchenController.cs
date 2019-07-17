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
    public class SoupKitchenController : Controller
    {
        SedesolServiceClient cliente;
        
        public SoupKitchenController()
        {
            cliente = new SedesolService.SedesolServiceClient();
        }

        [SessionExpireFilterAttribute]
        public ActionResult SoupKitchen()
        {
            List<SoupKitchenDTO> list = new List<SoupKitchenDTO>();
            list = cliente.GetSoupKitchenAll();

            return View(list);
        }

        [SessionExpireFilterAttribute]
        public ActionResult Deactivate(int id)
        {
            cliente.Deactivate(id);
            return RedirectToAction("SoupKitchen");
        }

        [SessionExpireFilterAttribute]
        public ActionResult Activate(int id)
        {
            cliente.Activate(id);
            return RedirectToAction("SoupKitchen");
        }

        [SessionExpireFilterAttribute]
        public ActionResult Edit(int id = 0)
        {
            SoupKitchenModel model = new SoupKitchenModel();
            if (id > 0)
            {
                SoupKitchenDTO sk = new SoupKitchenDTO();
                sk = cliente.GetSoupKitchenById(id);
                model.SoupKitchen = sk;
                var states = cliente.GetStates().Where(i => i.IsActive == true).ToList();
                var regions = cliente.GetActiveRegions();

                var itemToRemove = states.Single(r => r.Id == Convert.ToInt32(ConfigurationManager.AppSettings["nacExtranjero"].ToString()));
                states.Remove(itemToRemove);

                model.States = states;
                model.ListRegion = regions;

                model.ListUserType = cliente.GetUserTypeApproval();
                model.ListUserTypeSKDto = cliente.GetUserTypeBySKId(id).OrderBy(x => x.UserTypeDto.ApprovalOrder).ToList();

            }
            else
            {
                model.States = cliente.GetStates().Where(i => i.IsActive == true).ToList();
                model.ListRegion = cliente.GetActiveRegions();

                model.ListUserType = cliente.GetUserTypeApproval();
                model.ListUserTypeSKDto = cliente.GetUserTypeBySKId(id).OrderBy(x => x.UserTypeDto.ApprovalOrder).ToList();
            }
            return View(model);
        }

        //[SessionExpireFilterAttribute]
        //[HttpPost]
        //public ActionResult Edit(SoupKitchenModel model)
        //{
        //    //SoupKitchenModel model = new SoupKitchenModel();
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    //model.States = cliente.GetStates().Where(i => i.IsActive == true).ToList();
        //    //    return View(model);
        //    //}

        //    cliente.SaveSoupKitchen(model.SoupKitchen);
        //    return RedirectToAction("SoupKitchen");
        //}

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult SaveSoupKitchen(SoupKitchenDTO att)
        {
            att = cliente.SaveSoupKitchen(att);
            List<SkUserTypeDTOcs> list = new List<SkUserTypeDTOcs>();
            if (att.Message == "SUCCESS")
            {
                list = cliente.GetUserTypeBySKId(att.Id).OrderBy(x => x.UserTypeDto.ApprovalOrder).ToList();
            }
            string viewContent = ConvertViewToString("UserTypeList", list);
            return Json(new { message = att.Message, Id = att.Id, PartialView = viewContent });
        }

        [SessionExpireFilterAttribute]
        public ActionResult Create()
        {
            SoupKitchenModel model = new SoupKitchenModel();

            var states = cliente.GetStates().Where(i => i.IsActive == true).ToList();

            var itemToRemove = states.Single(r => r.Id == Convert.ToInt32(ConfigurationManager.AppSettings["nacExtranjero"].ToString()));
            states.Remove(itemToRemove);
            var regions = cliente.GetActiveRegions();

            model.States = states;
            model.ListRegion = regions;

            model.ListUserType = cliente.GetUserTypeApproval();
            model.ListUserTypeSKDto = cliente.GetUserTypeBySKId(0).OrderBy(x => x.UserTypeDto.ApprovalOrder).ToList();
            return View("Edit", model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult SaveUserTypeSk(SkUserTypeDTOcs att)
        {
            try
            {
                att.IsActive = true;
                SedesolServiceClient proxy = new SedesolServiceClient();
                att = proxy.SaveUserTypeSK(att);

                List<SkUserTypeDTOcs> list = new List<SkUserTypeDTOcs>();

                if (att.Message == "SUCCESS")
                {
                    list = proxy.GetUserTypeBySKId(att.Id_SoupKitchen);
                    list = list.OrderBy(x => x.UserTypeDto.ApprovalOrder).ToList();
                }

                string viewContent = ConvertViewToString("UserTypeList", list);
                return Json(new { message = att.Message, PartialView = viewContent });
            }
            catch (Exception ex)
            {
                return Json(new { message = "ERROR" });
            }
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult DeleteLevel(int idUserTypeSk, int idSoupKitchen)
        {
            try
            {
                SedesolServiceClient proxy = new SedesolServiceClient();
                string message = proxy.DeleteUserTypeSk(idUserTypeSk);

                List<SkUserTypeDTOcs> list = new List<SkUserTypeDTOcs>();

                if (message == "SUCCESS")
                {
                    list = proxy.GetUserTypeBySKId(idSoupKitchen);
                    list = list.OrderBy(x => x.UserTypeDto.ApprovalOrder).ToList();
                }

                string viewContent = ConvertViewToString("UserTypeList", list);
                return Json(new { message = message, PartialView = viewContent });
            }
            catch (Exception ex)
            {
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
    }
}