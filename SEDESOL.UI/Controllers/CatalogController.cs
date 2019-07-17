using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEDESOL.UI.SedesolService;
using SEDESOL.DataEntities.DTO;
using System.Configuration;

namespace SEDESOL.UI.Controllers
{
    public class CatalogController : Controller
    {
        SedesolServiceClient cliente = null;
        CatalogDTO catalogos = new CatalogDTO();

        public CatalogController()
        {
            cliente = new SedesolServiceClient();
        }

        [SessionExpireFilterAttribute]
        public ActionResult Catalog()
        {
            GetCatalogs();
            return View(catalogos);
        }

        [SessionExpireFilterAttribute]
        public JsonResult Insertar(string descripcion, string catalogoId)
        {
            string result = "duplicate";
            switch (catalogoId)
            {
                case "m":
                    if (!cliente.GetStates().Any(m => m.Name.ToLower() == descripcion.ToLower()))
                    {
                        StateDTO state = new StateDTO();
                        state.Name = descripcion;
                        cliente.UpdateState(state);
                        result = "success";
                    }
                    break;
                case "p":
                    if (!cliente.GetMonths().Any(m => m.Description.ToLower() == descripcion.ToLower()))
                    {
                        MonthDTO month = new MonthDTO();
                        month.Description = descripcion;
                        cliente.UpdateMonth(month);
                        result = "success";
                    }
                    break;
                case "n":
                    if (!cliente.GetStatus().Any(m => m.Description.ToLower() == descripcion.ToLower()))
                    {
                        StatusDTO status = new StatusDTO();
                        status.Description = descripcion;
                        cliente.UpdateStatus(status);
                        result = "success";
                    }
                    break;
                case "c":
                    if (!cliente.GetYears().Any(m => m.Description.ToLower() == descripcion.ToLower()))
                    {
                        YearDTO year = new YearDTO();
                        year.Description = descripcion;
                        cliente.UpdateYear(year);
                        result = "success";
                    }
                    break;
            }

            return new JsonResult { Data = new { Result = result } };
        }

        [SessionExpireFilterAttribute]
        public JsonResult Editar(string descripcion, string catalogoId, string itemId)
        {
            string result = "duplicate";
            switch (catalogoId)
            {
                case "m":
                    if (!cliente.GetStates().Any(m => m.Name.ToLower() == descripcion.ToLower()))
                    {
                        StateDTO state = new StateDTO();
                        state.Name = descripcion;
                        state.Id = Int32.Parse(itemId);
                        cliente.UpdateState(state);
                        result = "success";
                    }
                    break;
                case "p":
                    if (!cliente.GetMonths().Any(m => m.Description.ToLower() == descripcion.ToLower()))
                    {
                        MonthDTO month = new MonthDTO();
                        month.Description = descripcion;
                        month.Id = Int32.Parse(itemId);
                        cliente.UpdateMonth(month);
                        result = "success";
                    }
                    break;
                case "n":
                    if (!cliente.GetStatus().Any(m => m.Description.ToLower() == descripcion.ToLower()))
                    {
                        StatusDTO status = new StatusDTO();
                        status.Description = descripcion;
                        status.Id = Int32.Parse(itemId);
                        cliente.UpdateStatus(status);
                        result = "success";
                    }
                    break;
                case "c":
                    if (!cliente.GetYears().Any(m => m.Description.ToLower() == descripcion.ToLower()))
                    {
                        YearDTO year = new YearDTO();
                        year.Description = descripcion;
                        year.Id = Int32.Parse(itemId);
                        cliente.UpdateYear(year);
                        result = "success";
                    }
                    break;
            }

            return new JsonResult { Data = new { Result = result } };
        }

        [SessionExpireFilterAttribute]
        public ActionResult Listas()
        {
            GetCatalogs();
            return PartialView(catalogos);
        }

        [SessionExpireFilterAttribute]
        private void GetCatalogs()
        {
            catalogos.Status = cliente.GetStatus().ToList();
            var states = cliente.GetStates().ToList();

            var itemToRemove = states.Single(r => r.Id == Convert.ToInt32(ConfigurationManager.AppSettings["nacExtranjero"].ToString()));
            states.Remove(itemToRemove);

            catalogos.States = states;

            catalogos.Years = cliente.GetYears().ToList();
            catalogos.Months = cliente.GetMonths().ToList();
        }

        [SessionExpireFilterAttribute]
        public JsonResult Activar(string catalogoId, string id)
        {
            string result = "success";

            switch (catalogoId)
            {
                case "m":
                    cliente.ActivateState(Int32.Parse(id));
                    break;
                case "p":
                    cliente.ActivateMonth(Int32.Parse(id));
                    break;
                case "n":
                    cliente.ActivateStatus(Int32.Parse(id));
                    break;
                case "c":
                    cliente.ActivateYear(Int32.Parse(id));
                    break;
            }

            return new JsonResult { Data = new { Result = result } };
        }

        [SessionExpireFilterAttribute]
        public JsonResult Borrar(string catalogoId, string id)
        {
            string result = "success";
            switch (catalogoId)
            {
                case "m":
                    cliente.DeactivateState(Int32.Parse(id));

                    break;
                case "p":
                    cliente.DeactivateMonth(Int32.Parse(id));
                    break;
                case "n":
                    cliente.DeactivateStatus(Int32.Parse(id));
                    break;
                case "c":
                    cliente.DeactivateYear(Int32.Parse(id));
                    break;
            }
            result = "desactivado";

            return new JsonResult { Data = new { Result = result } };
        }
    }
}