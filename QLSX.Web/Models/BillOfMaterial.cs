using aih.oems.wapi.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OEMS.Web.Models
{
    public class BOM : aih.oems.wapi.models.BOM
    {
        new public Commodity Product { get; set; }
        new public List<BOMDetail> Detail { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        new public DateTime StartDate { get; set; }
        private readonly string param = "Commodity/Filter?type=semi,product";
        public async Task<List<Commodity>> GetAPI()
        {
            Utilities utilities = new Utilities();
            List<Commodity> commodities = new List<Commodity>();
            string strCommodities = await utilities.GetDataAPI(ConfigurationManager.AppSettings["base_api"].ToString(), param).ConfigureAwait(false);
            commodities = JsonConvert.DeserializeObject<List<Commodity>>(Convert.ToString(JsonConvert.DeserializeObject<JObject>(strCommodities)["Items"])).ToList();
            return commodities;
        }

        public List<Commodity> GetAllCommodities()
        {
            try
            {
                HttpClient client = new HttpClient();
                string BASE_URL = ConfigurationManager.AppSettings["base_api"].ToString() + param;
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                List<Commodity> lstCommodities = client.GetAsync(BASE_URL).Result.Content.ReadAsAsync<Pagination<Commodity>>().Result?.Items;
                var lst = lstCommodities.Select(x => new Commodity
                {
                    Id = x.Id,
                    Code = x.Code,
                    CodeXT = x.CodeXT,
                    CommodityGroup = x.CommodityGroup,
                    CommodityType = x.CommodityType,
                    CommodityUnit = x.CommodityUnit,
                    CreatedDate = x.CreatedDate,
                    Description = x.Description
                }).ToList();
                return lst;
            }
            catch (Exception)
            {
                return new List<Commodity>();
            }
        }
    }
}