using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
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
        private Commodity commodity = new Commodity();
        private Utilities utilities = new Utilities();
        private HttpClient client = new HttpClient();


        // GET: Commodities
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string ID)
        {
            if (ID != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(api);
                    //HTTP GET
                    var responseTask = client.GetAsync("commodity/getbyid?id=" + ID.ToString() + "&getprices=true");
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

        #region Actions
        [HttpPost]
        public async Task<JsonResult> DeletePrice(string Id)
        {
            //using (DatabaseContext _context = new DatabaseContext())
            //{
            //    var customer = _context.Customers.Find(ID);
            //    if (ID == null)
            //        return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
            //    _context.Customers.Remove(customer);
            //    _context.SaveChanges();

            //    return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            //}
            if (!string.IsNullOrEmpty(Id))
            {
                //string res = await utilities.PostDataAPI(api + "price/delete?id=" + Id, null, client);
                string res = await utilities.PostDelete(api + "price/delete?id=" + Id);
                if (res == "OK")
                    return Json(data: "Delete successfully", behavior: JsonRequestBehavior.AllowGet);
                else
                    return Json(data: "Delete not successfully", behavior: JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            return Json(data: $"Not found Price: {Id}", behavior: JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region MyRegion

        public ActionResult AjaxDataProvider(JQueryDataTableParamModel param)
        {
            var allPrices = commodity.Prices;
            
            IEnumerable<aih.oems.wapi.models.Price> filteredPrices;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);
                var amountFilter = Convert.ToString(Request["sSearch_2"]);
                var dateFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isAmountSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isDateSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                CultureInfo provider = new CultureInfo("en-us");
                filteredPrices = commodity.Prices.Where(c => isNameSearchable && c.Vendor.Name.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isAmountSearchable && c.Amount.ToString().Contains(param.sSearch.ToLower())
                               ||
                               isDateSearchable && c.StartDate.ToString("dd/MM/yyyy").ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredPrices = allPrices;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isAmountSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isDateSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<aih.oems.wapi.models.Price, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.Vendor.Name :
                                                          sortColumnIndex == 2 && isAmountSortable ? c.Amount.ToString() :
                                                          sortColumnIndex == 3 && isDateSortable ? c.StartDate.ToString("dd/MM/yyyy") :
                                                          "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredPrices = filteredPrices.OrderBy(orderingFunction);
            else
                filteredPrices = filteredPrices.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredPrices.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies select new[] { c.Id, c.Vendor.Name, c.Amount.ToString(), c.StartDate.ToString("dd/MM/yyyy") };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allPrices.Count(),
                iTotalDisplayRecords = filteredPrices.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public class JQueryDataTableParamModel
        {
            /// <summary>
            /// Request sequence number sent by DataTable, same value must be returned in response
            /// </summary>       
            public string sEcho { get; set; }

            /// <summary>
            /// Text used for filtering
            /// </summary>
            public string sSearch { get; set; }

            /// <summary>
            /// Number of records that should be shown in table
            /// </summary>
            public int iDisplayLength { get; set; }

            /// <summary>
            /// First record that should be shown(used for paging)
            /// </summary>
            public int iDisplayStart { get; set; }

            /// <summary>
            /// Number of columns in table
            /// </summary>
            public int iColumns { get; set; }

            /// <summary>
            /// Number of columns that are used in sorting
            /// </summary>
            public int iSortingCols { get; set; }

            /// <summary>
            /// Comma separated list of column names
            /// </summary>
            public string sColumns { get; set; }


        }
        #endregion
    }
}