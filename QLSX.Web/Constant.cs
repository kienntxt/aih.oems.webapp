using System.Collections.Generic;

namespace OEMS.Web
{
    public class Constant
    {
        public class CommodityTypes
        {
            public const string Material = "material";
            public const string Product = "product";
            public const string SemiProduct = "semi";
            //public string Code { get; set; }
            //public string Name { get; set; }
            //public static List<string> lstCode = new List<string> { "Material", "Product", "Semi-Product" };
            public static readonly Dictionary<string, string> dctCommdityTypes = new Dictionary<string, string>()
            {
                {"material", "Vật tư"},
                {"product", "Thành phẩm"},
                {"semi", "Bán thành phẩm"},
            };
        }

        public class PartnerTypes
        {
            public const string Partner = "partner";
            public const string Vendor = "vendor";
            public const string Customer = "customer";
            public static readonly Dictionary<string, string> dctPartnerTypes = new Dictionary<string, string>()
            {
                {"partner", "Nhà cung cấp - Khách hàng"},
                {"vendor", "Nhà cung cấp"},
                {"customer", "Khách hàng"},
            };
        }
    }
}