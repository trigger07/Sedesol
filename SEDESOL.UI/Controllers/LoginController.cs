using SEDESOL.DataEntities.DTO;
using SEDESOL.DataEntities.IntegrationObjects;
using SEDESOL.UI.SedesolService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEDESOL.UI.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserValidationRequest userValRequest)
        {
            if (userValRequest.Username == null || userValRequest.Password == null)
            {
                ModelState.AddModelError("", "El Usuario y Contraseña son campos requeridos");
                return View(userValRequest);
            }

            string passwordEncoded = Encryption.EncodePassword(userValRequest.Password);
            userValRequest.Password = passwordEncoded;

            SedesolServiceClient proxy = new SedesolServiceClient();
            UserDTO uDto = new UserDTO();
            uDto = proxy.ValidateUser(userValRequest);
            

            if (uDto != null && uDto.Id > 0)
            {
                Session.Add("userData", uDto);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "El usuario y la contraseña no coinciden");
                return View(userValRequest);
            }
        }
    }
}