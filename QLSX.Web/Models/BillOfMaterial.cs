using aih.oems.wapi.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OEMS.Web.Models
{
    public class BOM : aih.oems.wapi.models.BOM
    {
        new public Commodity Product { get; set; }
        private readonly string BASE_URL = "http://113.160.87.222:9876/api/Commodity/Filter?type=semi,product";
        public List<Commodity> GetAllCommodities()
        {
            try
            {
                HttpClient client = new HttpClient();
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