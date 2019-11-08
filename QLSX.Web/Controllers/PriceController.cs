using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
        }
    }
}