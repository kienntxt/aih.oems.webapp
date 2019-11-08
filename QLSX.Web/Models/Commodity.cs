using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Web.Models
{
    public class Commodity:BaseInfo
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string CodeXT { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// MATERIAL, PRODUCT, SEMI-PRODDUCT
        /// </summary>
        public string CommodityType { get; set; }

        public List<Price> Prices { get; set; }
    }
}