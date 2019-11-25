using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using OEMS.Web.Models;

namespace OEMS.Web.Controllers
{
    public class ContractsController : Controller
    {
        string api = ConfigurationManager.AppSettings["base_api"].ToString();
        // GET: Contract
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string ID)
        {
            Contract Contract = new Contract();
            if (ID !=null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(api);
                    //HTTP GET
                    var responseTask = client.GetAsync("Contract/GetById?id=" + ID.ToString());
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
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Create(Contract Contract)
        {
            using (var client = new HttpClient())
            {
                Utilities utilities = new Utilities();
                string res = string.Empty;
                //HTTP POST
                if (Contract.Id != null)
                {
                    res = await utilities.PostDataAPI(api + "Contract/Update?Contract=object", Contract, client);
                    if (res == "OK")
                        return RedirectToAction("Index");
                }
                else
                {
                    res = await utilities.PostDataAPI(api + "Contract/Create?Contract=object", Contract, client);
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
                var deleteTask = client.DeleteAsync("Contract/Delete?id=" + id.ToString());
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
                    var responseTask = client.GetAsync("Contract/GetById?id=" + Id.ToString());
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