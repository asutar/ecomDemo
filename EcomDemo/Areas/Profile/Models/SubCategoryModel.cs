using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcomDemo.Areas.Profile.Models
{
    public class SubCategoryModel
    {
        public int SUB_CATEGORY_ID { get; set; }
        public string SUB_CATEGORY_DESCRIPTION { get; set; }
        public int CATEGORY_ID { get; set; }
        public string CATEGORY_DESCRIPTION { get; set; } // For display purposes
       // public IEnumerable<SelectListItem> Categories { get; set; } // For Category Dropdown
       // public List<SubCategoryModel> SubCategories { get; set; } // For Category Dropdown
    }
}