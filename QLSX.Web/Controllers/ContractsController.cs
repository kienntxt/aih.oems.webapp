using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OEMS.Web.Models;

namespace OEMS.Web.Controllers
{
    public class ContractsController : Controller
    {
        Utilities utilities = new Utilities();
        HttpClient client = new HttpClient();
        string api = ConfigurationManager.AppSettings["base_api"].ToString();
        // GET: Contract
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Create(string ID)
        {
            Contract Contract = new Contract();
            if (ID != null)
            {
                string res = await utilities.GetDataAPI(api, "contract/getbyid?id=" + ID.ToString() + "&getdetails=true");
                if (!string.IsNullOrEmpty(res))
                {
                    Contract = JsonConvert.DeserializeObject<Contract>(res);
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(Contract);
        }

        public ActionResult LoadContractDetails()
        {
            var contractDetais = new Contract();

            //return Json(new { aaData = contractDetais.Select(x => new[] { x.Number, x.Description })}, JsonRequestBehavior.AllowGet);
            return null;
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Create(Contract Contract)
        {
            using (var client = new HttpClient())
            {

                string res = string.Empty;
                //HTTP POST
                if (Contract.Id != null)
                {
                    res = await utilities.PostDataAPI(api + "contract/update?contract=object", Contract, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
                else
                {
                    res = await utilities.PostDataAPI(api + "contract/create?contract=object", Contract, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(Contract);
        }
        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api);
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("contract/delete?id=" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult ContractDetail(string Id)
        {
            Contract Contract = new Contract();
            if (Id != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(api, UriKind.RelativeOrAbsolute);
                    //HTTP GET
                    var responseTask = client.GetAsync("contract/getbyid?id=" + Id.ToString());
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        Contract = JsonConvert.DeserializeObject<Contract>(readTask);
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            return View(Contract);
        }
    }
}