using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcomDemo.Areas.Profile.Models
{
    public class SubCategoryMasterViewModel
    {
        public List<SubCategoryModel> SubCategories { get; set; }
        public List<Category> Categories { get; set; }
    }
}