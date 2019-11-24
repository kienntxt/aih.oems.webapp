using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace OEMS.Web.Models
{
    public class Price : aih.oems.wapi.models.Price
    {
        new public Commodity Material { get; set; }
        new public Partner Vendor { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        new public DateTime StartDate { get; set; }
        private readonly string commParam = "Commodity/Filter?type=";
        private readonly string partnerParam = "Partner/Filter?type=";

        Utilities utilities = new Utilities();
        public async Task<List<Commodity>> GetCommodyties()
        {
            string strCommodities = await utilities.GetDataAPI(ConfigurationManager.AppSettings["base_api"].ToString(), commParam).ConfigureAwait(false);
            List<Commodity> commodities = JsonConvert.DeserializeObject<List<Commodity>>(Convert.ToString(JsonConvert.DeserializeObject<JObject>(strCommodities)["Items"])).ToList();
            return commodities;
        }
        public async Task<List<Partner>> GetPartners()
        {
            string strpartners = await utilities.GetDataAPI(ConfigurationManager.AppSettings["base_api"].ToString(), partnerParam).ConfigureAwait(false);
            List<Partner>  partners = JsonConvert.DeserializeObject<List<Partner>>(Convert.ToString(JsonConvert.DeserializeObject<JObject>(strpartners)["Items"])).ToList();
            return partners;
        }
    }
}