using Newtonsoft.Json;
using OEMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Web.Controllers
{
    public class PriceController : Controller
    {
        // GET: Price
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string ID)
        {
            Price prices = new Price();
            if (ID != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Price/");
                    //HTTP GET
                    var responseTask = client.GetAsync("GetById?id=" + ID.ToString());
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        prices = JsonConvert.DeserializeObject<Price>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(prices);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Price Price)
        {
            Price prices = new Price();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Price/");
                //HTTP GET
                var responseTask = client.GetAsync("GetById?id=" + Price.Id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    prices = JsonConvert.DeserializeObject<Price>(readTask);
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Price/");
                //HTTP POST
                if (Price.Id != null)
                {
                    prices.Amount = Price.Amount;
                    var postTask = client.PostAsJsonAsync<Price>("Update?Price=object", prices);
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
                        return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(Price);
        }
    }
}