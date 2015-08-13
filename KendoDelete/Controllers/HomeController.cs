using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoDelete.Models;

namespace KendoDelete.Controllers
{

    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Kendo UI!";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Slider()
        {
            ViewBag.Message = "Hello From Slider Control";
            return View();
        }

        public ActionResult Editor()
        {
            ViewBag.Message = "Hello From Editor";
            return View();
        }

        public ActionResult BrowserSidePaging()
        {
            ProductContext context = new ProductContext();
            var product = context.Products.ToList();
            return View(product);
        }

        public ActionResult ServerSidePaging()
        {
            return View();
        }


        // If you put data in the browser it is going to slow down when you have thousands of rows of data. 
        // We want the user to see only that data. We want the server to handl the paging , 
        // grouping, soring and filtering. Server are good at this type of things. 
        // we want user to see only they need to see not everything load
        // We would need to parse the incoming httprequest from Kendo UI and aplly all the filtering. 
        // You need to parse the incoming httpb request into Kendo UI DataSourceRequest,. 
        //You can do it in the signature of the controller method. After this you need to  apply dynamic where
        //clause  and filtering and grouping and handling null values. 
        // How to apply this request and parameter to a query ?
        // It is simple, All we have to do it is that we are returning Json result and then call the ToDatasourceResult on the linq query passing the request 
        // object
        // DataSourceRequest as well as a toDataSourceResult does all this work for you. 
        //

        public JsonResult ServerSidePagingGrid([DataSourceRequest]DataSourceRequest request)
        {
            ProductContext context = new ProductContext();
            var product = context.Products.ToList();

            // DataSourceResult result = product.ToDataSourceResult(request);
            // How to apply this request and parameter to a query ?
            // returning Json result and then call the ToDatasourceResult on the linq query passing the request object

            return this.Json(product.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, Models.Product product)
        {
            ProductContext context = new ProductContext();
            if (product != null && ModelState.IsValid)
            {
                var productToUpdate = context.Products.Where(p => p.ID == product.ID).FirstOrDefault();
                productToUpdate.ID = product.ID;
                productToUpdate.Level = product.Level;
                productToUpdate.Name = product.Name;
                productToUpdate.PerUnit = product.PerUnit;
                productToUpdate.Price = product.Price;
                productToUpdate.Status = product.Status;
                productToUpdate.Units = product.Units;
            }
            context.SaveChanges();

            return Json(ModelState.IsValid ? true : ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, Models.Product product)
        {
            ProductContext context = new ProductContext();
            if (product != null && ModelState.IsValid)
            {
                try
                {
                    context.Products.Attach(product);
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
                catch (System.Exception e)
                {
                    this.Session["ErrorMessage"] = e.Message;
                }
            }
            return Json(ModelState.IsValid ? true : ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, Models.Product product)
        {
            ProductContext context = new ProductContext();
            if (product != null && ModelState.IsValid)
            {
                try
                {
                    Product productToUpdate = new Product();
                    productToUpdate.ID = product.ID;
                    productToUpdate.Level = product.Level;
                    productToUpdate.Name = product.Name;
                    productToUpdate.PerUnit = product.PerUnit;
                    productToUpdate.Price = product.Price;
                    productToUpdate.Status = product.Status;
                    productToUpdate.Units = product.Units;
                    context.Products.Add(productToUpdate);
                    context.SaveChanges();
                }
                catch (System.Exception e)
                {
                    this.Session["ErrorMessage"] = e.Message;
                }
            }
            return Json(ModelState.IsValid ? true : ModelState.ToDataSourceResult());
        }

        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

    }
}
