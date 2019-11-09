using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Web
{
    public class Constant
    {
        public class CommodityTypes
        {
            public const string Material = "Material";
            public const string Product = "Product";
            public const string SemiProduct = "Semi";
            //public string Code { get; set; }
            //public string Name { get; set; }
            //public static List<string> lstCode = new List<string> { "Material", "Product", "Semi-Product" };
            public static readonly Dictionary<string, string> dctCommdityTypes = new Dictionary<string, string>()
            {
                {"Material", "Vật tư"},
                {"Product", "Thành phẩm"},
                {"Semi", "Bán thành phẩm"},
            };
        }

        public class PartnerTypes
        {
            public const string Partner = "Partner";
            public const string Vendor = "Vendor";
            public const string Customer = "Customer";
            public static readonly Dictionary<string, string> dctPartnerTypes = new Dictionary<string, string>()
            {
                {"Partner", "Nhà cung cấp - Khách hàng"},
                {"Vendor", "Nhà cung cấp"},
                {"Customer", "Khách hàng"},
            };
        }
    }
}