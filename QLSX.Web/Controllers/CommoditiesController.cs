using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Web.Controllers
{
    public class CommoditiesController : Controller
    {
        private static string result;
        
        // GET: Commodities
        public ActionResult Index()
        {
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://113.160.87.222:9876/api/Commodity/GetCommodities?type=material");
            //request.Method = "GET";
            //request.ContentType = "application/json";
            //try
            //{
            //    WebResponse webResponse = request.GetResponse();
            //    using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
            //    using (StreamReader responseReader = new StreamReader(webStream))
            //    {
            //        long count = 50;
            //        string response = responseReader.ReadToEnd();
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.Out.WriteLine("-----------------");
            //    Console.Out.WriteLine(e.Message);
            //}
            //PostRequest();
            return View();
        }
        private static string PostRequest()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://113.160.87.222:9876/api/Commodity/Create?Commodity=object");
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "[  { \"Id\": \"material2\",\"Code\": \"material2\",\"CodeXT\": \"material2\",\"Name\": \"material2\", \"CommodityType\": \"material\", \"Prices\": [  {  \"Id\": \"price2\",\"Material\": { \"Id\": \"material2\", \"Code\": null,  \"Name\": null,  \"CommodityType\": null, \"Prices\": null, \"StatusCode\": null,\"CreatedDate\": \"0001/01/01 00:00:00\",   \"UpdatedDate\": \"0001/01/01 00:00:00\", \"UsrId\": null,   \"XtraInfo\": null },\"Amount\": 1000,\"StartDate\": \"2019/10/31 14:04:17\",\"StatusCode\": null,\"CreatedDate\": \"0001/01/01 00:00:00\",\"UpdatedDate\": \"0001/01/01 00:00:00\",  \"UsrId\": null,\"XtraInfo\": null } ],\"StatusCode\": null, \"CreatedDate\": \"0001/01/01 00:00:00\",\"UpdatedDate\": \"0001/01/01 00:00:00\", \"UsrId\": null,\"XtraInfo\": null}";
                Debug.Write(json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpWebRequest.HaveResponse && response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            result = error;
                        }
                    }

                }
            }

            return result;

        }
        public ActionResult Create(string url)
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create()
        {
            return View();
        }
    }
}