using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Web
{
    public class Constant
    {
        public class TypeCommodity
        {
            public const string Material = "Material";
            public const string Prod = "Prod";
            public const string SemiProd = "SemiProd";
            public static readonly Dictionary<string, string> dctName = new Dictionary<string, string>()
            {
                {Material, "Material"},
                {Prod, "Prod"},
                 {SemiProd, "SemiProd"},
            };
        }
    }
}