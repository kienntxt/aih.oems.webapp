using System;
using System.Net.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using OEMS.Web.Models;

namespace OEMS.Web.Controllers
{
    public class BillOfMaterialController : Controller
    {
        // GET: BillOfMaterial - BOM
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string ID)
        {
            BOM bom = new BOM();
            if (ID !=null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://113.160.87.222:9876/api/BOM/");
                    //HTTP GET
                    var responseTask = client.GetAsync("GetById?id=" + ID.ToString());
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        bom = JsonConvert.DeserializeObject<BOM>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(bom);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(BOM bom)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://113.160.87.222:9876/api/BOM/");
                //HTTP POST
                if (bom.Id !=null)
                {
                    var postTask = client.PostAsJsonAsync<BOM>("Update?BOM=object", bom);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    var postTask = client.PostAsJsonAsync<BOM>("Create?BOM=object", bom);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(bom);
        }
        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://113.160.87.222:9876/api/BOM/");
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
        public ActionResult BOMDetail(string Id)
        {
            BOM bom = new BOM();
            if (Id != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://113.160.87.222:9876/api/BOM/");
                    //HTTP GET
                    var responseTask = client.GetAsync("GetById?id=" + Id.ToString());
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        bom = JsonConvert.DeserializeObject<BOM>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(bom);
        }
    }
}