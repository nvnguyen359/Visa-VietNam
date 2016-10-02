using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailTemplate
{
    public class OrderViewModel
    {
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string CustomerName { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public string ShipToAddress { get; set; }
        public string SiteUrl { get; set; }
        public string SiteName { get; set; }
    }
    public class ProductViewModel
    {
        public string ProductName { get; set; }
        public string Price { get; set; }
    }
}