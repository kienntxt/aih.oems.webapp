using Newtonsoft.Json;
using OEMS.Web.Models;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OEMS.Web.Controllers
{
    public class PartnerController : Controller
    {
        string api = ConfigurationManager.AppSettings["base_api"].ToString();
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
                    client.BaseAddress = new Uri(api, UriKind.RelativeOrAbsolute);
                    //HTTP GET
                    var responseTask = client.GetAsync("Partner/GetById?id=" + ID.ToString());
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
        public async Task<ActionResult> Create(Partner partner)
        {
            using (var client = new HttpClient())
            {
                Utilities utilities = new Utilities();
                string res = string.Empty;
                //HTTP POST
                if (partner.Id != null)
                {
                    res = await utilities.PostDataAPI(api + "Partner/Update?Partner=object", partner, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
                else
                {
                    res = await utilities.PostDataAPI(api + "Partner/Create?Partner=object", partner, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(partner);
        }
        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api, UriKind.RelativeOrAbsolute);
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Partner/Delete?id=" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult PartnerDetail(string Id)
        {
            Partner partner = new Partner();
            if (Id != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(api, UriKind.RelativeOrAbsolute);
                    //HTTP GET
                    var responseTask = client.GetAsync("Partner/GetById?id=" + Id.ToString());
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