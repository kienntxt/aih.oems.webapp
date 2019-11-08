using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Web.Models
{
    public class BaseInfo
    {
        public string StatusCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UsrId { get; set; }
        public XtraInfo XtraInfo { get; set; }
    }
}