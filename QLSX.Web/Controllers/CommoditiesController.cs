using System;
using System.Net.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using OEMS.Web.Models;

namespace OEMS.Web.Controllers
{
    public class CommoditiesController : Controller
    {
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
                    client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Commodity/");
                    //HTTP GET
                    var responseTask = client.GetAsync("GetById?id=" + ID.ToString());
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
        public ActionResult Create(Commodity commodity)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Commodity/");
                //HTTP POST
                if (commodity.Id !=null)
                {
                    var postTask = client.PostAsJsonAsync<Commodity>("Update?Commodity=object", commodity);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //var readTask = result.Content.ReadAsStringAsync().Result;
                        //commodity = JsonConvert.DeserializeObject<Commodity>(readTask);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    var postTask = client.PostAsJsonAsync("Create?Commodity=object", commodity);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(commodity);
        }
        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Commodity/");
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Delete?id=" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult CommodityDetial(string Id)
        {
            Commodity commodity = new Commodity();
            if (Id != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Commodity/");
                    //HTTP GET
                    var responseTask = client.GetAsync("GetById?id=" + Id.ToString());
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