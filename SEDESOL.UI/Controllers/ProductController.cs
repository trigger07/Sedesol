using SEDESOL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEDESOL.UI.Controllers
{
    public class ProductController : Controller
    {
        [SessionExpireFilterAttribute]
        // GET: Product
        public ActionResult Index()
        {
            SedesolService.SedesolServiceClient client = new SedesolService.SedesolServiceClient();
            List<ProductDTO> model = new List<ProductDTO>();

            model = client.GetProductAll();
            client.Close();
            return View(model);
        }

        [SessionExpireFilterAttribute]
        public JsonResult LoadForm(int idProduct)
        {
            SedesolService.SedesolServiceClient client = new SedesolService.SedesolServiceClient();
            var prod = client.GetProductById(idProduct);
            client.Close();

            prod.MomioString = prod.Momio.ToString();
            prod.MeasureString = prod.Measure.ToString();
            return Json(prod, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilterAttribute]
        public JsonResult GetActiveRegions()
        {
            SedesolService.SedesolServiceClient client = new SedesolService.SedesolServiceClient();
            var regions = client.GetActiveRegions();
            client.Close();

            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilterAttribute]
        [HttpPost]
        public JsonResult SaveProduct(ProductDTO productDTO)
        {
            productDTO.Price = 0;
            productDTO.CreateDate = DateTime.Now;
            productDTO.EditDate = DateTime.Now;
            productDTO.CreateUser = ((UserDTO)Session["userData"]).Username;
            productDTO.EditUser = ((UserDTO)Session["userData"]).Username;

            SedesolService.SedesolServiceClient client = new SedesolService.SedesolServiceClient();

            productDTO = client.SaveProduct(productDTO);
            List<ProductDTO> listProduct = new List<ProductDTO>();

            if (productDTO.Message == "SUCCESS")
            {
                listProduct = client.GetProductAll();
            }
            client.Close();

            string viewContent = ConvertViewToString("ProductList", listProduct);
            return Json(new { message = productDTO.Message, PartialView = viewContent });
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