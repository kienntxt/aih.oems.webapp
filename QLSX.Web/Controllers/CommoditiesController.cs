using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using OEMS.Web.Models;

namespace OEMS.Web.Controllers
{
    public class CommoditiesController : Controller
    {
        string api = ConfigurationManager.AppSettings["base_api"].ToString();
        // GET: Commodities
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string ID)
        {
            Commodity commodity = new Commodity();
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
                        commodity = JsonConvert.DeserializeObject<Commodity>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(commodity);
        }
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Create(Commodity commodity)
        {
            using (var client = new HttpClient())
            {
                Utilities utilities = new Utilities();
                string res = string.Empty;
                //HTTP POST
                if (commodity.Id != null)
                {
                    res = await utilities.PostDataAPI(api + "Commodity/Update?Commodity=object", commodity, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
                else
                {
                    res = await utilities.PostDataAPI(api + "Commodity/Create?Commodity=object", commodity, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(commodity);
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
        public ActionResult CommodityDetail(string Id)
        {
            Commodity commodity = new Commodity();
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
                        commodity = JsonConvert.DeserializeObject<Commodity>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(commodity);
        }
    }
}