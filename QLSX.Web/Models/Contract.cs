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
    public class Contract:aih.oems.wapi.models.Contract
    {
        [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        new public DateTime StartDate { get; set; }
        new public DateTime EndtDate { get; set; }
        new public Partner Customer { get; set; }
        new public Commodity Material { get; set; }

        private readonly string commParam = "commodity/filter?type=product";
        private readonly string partnerParam = "partner/filter?type=customer";

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
            List<Partner> partners = JsonConvert.DeserializeObject<List<Partner>>(Convert.ToString(JsonConvert.DeserializeObject<JObject>(strpartners)["Items"])).ToList();
            return partners;
        }
    }
}