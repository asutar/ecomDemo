using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcomDemo.Areas.Profile.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CATEGORY_DESCRIPTION { get; set; }
        public int SubCategoryId { get; set; }
        public string SUB_CATEGORY_DESCRIPTION { get; set; }
        public string Brand { get; set; }
        public int SizeId { get; set; }
        public string SIZE_DESCRIPTION { get; set; }
        public int ColorId { get; set; }
        public string COLOR_DESCRIPTION { get; set; }
        public decimal MRP { get; set; }
        public decimal Price { get; set; }
        public int Gender_id { get; set; }
        public string GENDER_DESCRIPTION { get; set; }
    }
}