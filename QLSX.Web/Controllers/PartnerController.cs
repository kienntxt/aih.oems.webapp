using Newtonsoft.Json;
using OEMS.Web.Models;
using System;
using System.Net.Http;
using System.Web.Mvc;

namespace OEMS.Web.Controllers
{
    public class PartnerController : Controller
    {
        // GET: Partner
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string ID)
        {
            Partner partner = new Partner();
            if (ID != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Partner/");
                    //HTTP GET
                    var responseTask = client.GetAsync("GetById?id=" + ID.ToString());
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        partner = JsonConvert.DeserializeObject<Partner>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(partner);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Partner partner)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Partner/");
                //HTTP POST
                if (partner.Id != null)
                {
                    var postTask = client.PostAsJsonAsync("Update?Partner=object", partner);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    var postTask = client.PostAsJsonAsync("Create?Partner=object", partner);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(partner);
        }
        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Partner/");
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
        public ActionResult PartnerDetial(string Id)
        {
            Partner partner = new Partner();
            if (Id != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://113.160.87.222:9876/api/Partner/");
                    //HTTP GET
                    var responseTask = client.GetAsync("GetById?id=" + Id.ToString());
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        partner = JsonConvert.DeserializeObject<Partner>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(partner);
        }
    }
}