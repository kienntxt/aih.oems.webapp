using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using OEMS.Web.Models;

namespace OEMS.Web.Controllers
{
    public class BillOfMaterialController : Controller
    {
        string api = ConfigurationManager.AppSettings["base_api"].ToString();
        private Utilities utilities = new Utilities();
        private HttpClient client = new HttpClient();

        // GET: BillOfMaterial - BOM
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Create(string ID)
        {
            BOM bom = new BOM();
            if (ID != null)
            {
                string res = await utilities.GetDataAPI(api, "bom/getbyid?id=" + ID.ToString() + "&getdetails=true");
                if (!string.IsNullOrEmpty(res))
                {
                    bom = JsonConvert.DeserializeObject<BOM>(res);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(bom);
        }
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Create(BOM bom)
        {
            using (var client = new HttpClient())
            {
                Utilities utilities = new Utilities();
                string res = string.Empty;
                //HTTP POST
                if (bom.Id != null)
                {
                    res = await utilities.PostDataAPI(api + "BOM/Update?Bom=object", bom, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
                else
                {
                    res = await utilities.PostDataAPI(api + "BOM/Create?Bom=object", bom, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(bom);
        }
        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api, UriKind.RelativeOrAbsolute);
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("BOM/Delete?id=" + id.ToString());
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
                    client.BaseAddress = new Uri(api, UriKind.RelativeOrAbsolute); ;
                    //HTTP GET
                    var responseTask = client.GetAsync("BOM/GetById?id=" + Id.ToString());
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