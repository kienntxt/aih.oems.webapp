using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Web.Models
{
    public class Price:BaseInfo
    {
        public string Id { get; set; }
        public Commodity Material { get; set; }
        public Partner Vendor { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
    }
}