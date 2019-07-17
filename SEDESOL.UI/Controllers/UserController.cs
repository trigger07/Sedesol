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

    public class UserController : Controller
    {
        SedesolServiceClient cliente;


        public UserController()
        {
            cliente = new SedesolService.SedesolServiceClient();
        }

        [SessionExpireFilterAttribute]
        public ActionResult Index()
        {
            List<UserDTO> list = new List<UserDTO>();
            list = cliente.GetUserAll();

            return View(list);
        }

        [SessionExpireFilterAttribute]
        public ActionResult Deactivate(int id)
        {
            cliente.DeactivateUser(id);
            return RedirectToAction("Index");
        }

        [SessionExpireFilterAttribute]
        public ActionResult Activate(int id)
        {
            cliente.ActivateUser(id);
            return RedirectToAction("Index");
        }

        [SessionExpireFilterAttribute]
        public ActionResult Edit(int id = 0)
        {
            UserModel model = new UserModel();
            UserDTO sk = new UserDTO();

            if (id > 0)
            {
                sk = cliente.GetUserById(id);
                sk.Password = string.Empty;
                model.userDTO = sk;
                model.StateDTO = cliente.GetStates().Where(i => i.IsActive == true).ToList();
                model.UserTypeDTO = cliente.GetUserTypeAll().Where(i => i.IsActive == true).ToList();
                model.ListSKToAssign = cliente.GetSoupKitchenToAssignUser(id);
                model.ListSKAssigned = cliente.GetSKAssignedUser(id);
            }
            else
            {
                model.userDTO = sk;
                model.StateDTO = cliente.GetStates().Where(i => i.IsActive == true).ToList();
                model.UserTypeDTO = cliente.GetUserTypeAll().Where(i => i.IsActive == true).ToList();
                model.ListSKAssigned = new List<SoupKitchenDTO>();
                model.ListSKToAssign = cliente.GetSoupKitchenAll().Where(f => f.IsActive).ToList();
            }
            return View(model);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult Edit(UserDTO att)
        {
            List<SoupKitchenDTO> listToAssign = new List<SoupKitchenDTO>();
            List<SoupKitchenDTO> listAssigned = new List<SoupKitchenDTO>();
            if (att.Password == null)
                att.Password = string.Empty;

            if (att.Password != string.Empty)
            {
                att.Password = Encryption.EncodePassword(att.Password);
            }
            else
            {
                if (att.Id == 0)
                {
                    return Json(new { message = "Debe ingresar una contraseña." });
                }
            }
            
            att = cliente.SaveUser(att);
            if (att.Message == "SUCCESS")
            {
                listToAssign = cliente.GetSoupKitchenToAssignUser(att.Id);
                listAssigned = cliente.GetSKAssignedUser(att.Id);
            }

            string viewContentTo = ConvertViewToString("SoupKitchenList", listToAssign);
            string viewContentAss = ConvertViewToString("SKAssignedList", listAssigned);

            return Json(new { message = att.Message, Id = att.Id, PartialViewTo = viewContentTo, PartialViewAss = viewContentAss });
        }

        [SessionExpireFilterAttribute]
        public ActionResult Create()
        {
            UserModel model = new UserModel();
            UserDTO sk = new UserDTO();
            model.userDTO = sk;
            model.StateDTO = cliente.GetStates().Where(i => i.IsActive == true).ToList();
            model.UserTypeDTO = cliente.GetUserTypeAll().Where(i => i.IsActive == true).ToList();
            model.ListSKToAssign = cliente.GetSoupKitchenAll().Where(i => i.IsActive == true).ToList();
            model.ListSKAssigned = new List<SoupKitchenDTO>();

            return View("Edit", model);
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
        [HttpPost]
        public JsonResult DeleteUserSoupKitchen(int id, int idUser)
        {
            List<SoupKitchenDTO> listToAssign = new List<SoupKitchenDTO>();
            List<SoupKitchenDTO> listAssigned = new List<SoupKitchenDTO>();

            SedesolServiceClient proxy = new SedesolServiceClient();
            string data = proxy.DeleteUserSoupKitchen(id, idUser);

            if (data == "SUCCESS")
            {
                listToAssign = cliente.GetSoupKitchenToAssignUser(idUser);
                listAssigned = cliente.GetSKAssignedUser(idUser);
            }

            string viewContentTo = ConvertViewToString("SoupKitchenList", listToAssign);
            string viewContentAss = ConvertViewToString("SKAssignedList", listAssigned);

            return Json(new { message = data, PartialViewTo = viewContentTo, PartialViewAss = viewContentAss });
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult SaveUserSoupKitchen(UserSoupKitchen dto)
        {
            List<SoupKitchenDTO> listToAssign = new List<SoupKitchenDTO>();
            List<SoupKitchenDTO> listAssigned = new List<SoupKitchenDTO>();

            dto.IsActive = true;

            SedesolServiceClient proxy = new SedesolServiceClient();
            dto = proxy.SaveUserSoupKitchen(dto);

            if (dto.Message == "SUCCESS")
            {
                listToAssign = cliente.GetSoupKitchenToAssignUser(dto.Id_User);
                listAssigned = cliente.GetSKAssignedUser(dto.Id_User);
            }

            string viewContentTo = ConvertViewToString("SoupKitchenList", listToAssign);
            string viewContentAss = ConvertViewToString("SKAssignedList", listAssigned);

            return Json(new { message = dto.Message, PartialViewTo = viewContentTo, PartialViewAss = viewContentAss });
        }
    }
}