using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using OEMS.Web.Models;

namespace OEMS.Web.Controllers
{
    public class ProductsController : Controller
    {
        string api = ConfigurationManager.AppSettings["base_api"].ToString();
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string ID)
        {
            Product product = new Product();
            if (ID !=null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(api);
                    //HTTP GET
                    var responseTask = client.GetAsync("Commodity/GetById?id=" + ID.ToString());
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        product = JsonConvert.DeserializeObject<Product>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(product);
        }
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Create(Product product)
        {
            using (var client = new HttpClient())
            {
                Utilities utilities = new Utilities();
                string res = string.Empty;
                product.CommodityType = "product";
                //HTTP POST
                if (product.Id != null)
                {
                    res = await utilities.PostDataAPI(api + "Commodity/Update?commodity=object", product, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
                else
                {
                    res = await utilities.PostDataAPI(api + "Commodity/Create?commodity=object", product, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(product);
        }
        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api);
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Commodity/Delete?id=" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult ProductDetail(string Id)
        {
            Product Product = new Product();
            if (Id != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(api, UriKind.RelativeOrAbsolute);
                    //HTTP GET
                    var responseTask = client.GetAsync("Commodity/GetById?id=" + Id.ToString());
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        Product = JsonConvert.DeserializeObject<Product>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(Product);
        }
    }
}