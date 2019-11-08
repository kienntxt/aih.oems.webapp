using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Web.Models
{
    public class Partner:BaseInfo
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// VENDOR, CUSTOMER, PARTNER
        /// </summary>
        public string Type { get; set; }
    }
}