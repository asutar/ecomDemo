using EcomDemo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcomDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        [HttpGet]
        public ActionResult Index()
        {
                List<Product> productList = new List<Product>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT A.PRODUCT_ID, A.TITLE,A.DESCRIPTION, A.CATEGORY_ID, B.CATEGORY_DESCRIPTION, C.SUB_CATEGORY_ID, C.SUB_CATEGORY_DESCRIPTION, A.BRAND, A.SIZE_ID,CASE WHEN A.SIZE_ID = 1 THEN 'S' WHEN A.SIZE_ID = 2 THEN 'M' WHEN A.SIZE_ID = 3 THEN 'L' WHEN A.SIZE_ID = 4 THEN 'XL' END AS SIZE_DESCRIPTION,A.COLOR_ID,CASE WHEN A.COLOR_ID = 1 THEN 'Red' WHEN A.COLOR_ID = 2 THEN 'Orange' WHEN A.COLOR_ID = 3 THEN 'Black' WHEN A.COLOR_ID = 4 THEN 'Pink' END AS COLOR_DESCRIPTION,A.MRP,A.PRICE, A.GENDER_ID,CASE WHEN A.GENDER_ID = 1 THEN 'Male' WHEN A.GENDER_ID = 2 THEN 'Female'  END AS GENDER_DESCRIPTION FROM TBL_PRODUCT_MASTER A WITH(NOLOCK) INNER JOIN TBL_CATEGORY_MASTER B WITH(NOLOCK) ON A.CATEGORY_ID = B.CATEGORY_ID INNER JOIN TBL_SUB_CATEGORY_MASTER C WITH(NOLOCK) ON A.SUB_CATEGORY_ID = C.SUB_CATEGORY_ID", con);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = Convert.ToInt32(rdr["PRODUCT_ID"]),
                            Title = rdr["TITLE"].ToString(),
                            Description = rdr["DESCRIPTION"].ToString(),
                            CategoryId = Convert.ToInt32(rdr["CATEGORY_ID"]),
                            CATEGORY_DESCRIPTION = Convert.ToString(rdr["CATEGORY_DESCRIPTION"]),
                            SubCategoryId = Convert.ToInt32(rdr["SUB_CATEGORY_ID"]),
                            SUB_CATEGORY_DESCRIPTION = Convert.ToString(rdr["SUB_CATEGORY_DESCRIPTION"]),
                            Brand = rdr["BRAND"].ToString(),
                            SizeId = Convert.ToInt32(rdr["SIZE_ID"]),
                            SIZE_DESCRIPTION = Convert.ToString(rdr["SIZE_DESCRIPTION"]),
                            ColorId = Convert.ToInt32(rdr["COLOR_ID"]),
                            COLOR_DESCRIPTION = Convert.ToString(rdr["COLOR_DESCRIPTION"]),
                            MRP = Convert.ToDecimal(rdr["MRP"]),
                            Price = Convert.ToDecimal(rdr["PRICE"]),
                            Gender_id = Convert.ToInt32(rdr["GENDER_ID"]),
                            GENDER_DESCRIPTION = Convert.ToString(rdr["GENDER_DESCRIPTION"]),
                        };
                        productList.Add(product);
                    }
                }
            return View(productList);
        }
       
        public ActionResult ProductDetails(int PRODUCT_ID)
        {
            Product model = new Product();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT A.PRODUCT_ID, A.TITLE,A.DESCRIPTION, A.CATEGORY_ID, B.CATEGORY_DESCRIPTION, C.SUB_CATEGORY_ID, C.SUB_CATEGORY_DESCRIPTION, A.BRAND, A.SIZE_ID,CASE WHEN A.SIZE_ID = 1 THEN 'S' WHEN A.SIZE_ID = 2 THEN 'M' WHEN A.SIZE_ID = 3 THEN 'L' WHEN A.SIZE_ID = 4 THEN 'XL' END AS SIZE_DESCRIPTION,A.COLOR_ID,CASE WHEN A.COLOR_ID = 1 THEN 'Red' WHEN A.COLOR_ID = 2 THEN 'Orange' WHEN A.COLOR_ID = 3 THEN 'Black' WHEN A.COLOR_ID = 4 THEN 'Pink' END AS COLOR_DESCRIPTION,A.MRP,A.PRICE, A.GENDER_ID,CASE WHEN A.GENDER_ID = 1 THEN 'Male' WHEN A.GENDER_ID = 2 THEN 'Female'  END AS GENDER_DESCRIPTION FROM TBL_PRODUCT_MASTER A WITH(NOLOCK) INNER JOIN TBL_CATEGORY_MASTER B WITH(NOLOCK) ON A.CATEGORY_ID = B.CATEGORY_ID INNER JOIN TBL_SUB_CATEGORY_MASTER C WITH(NOLOCK) ON A.SUB_CATEGORY_ID = C.SUB_CATEGORY_ID WHERE  A.PRODUCT_ID='"+ PRODUCT_ID + "'", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    model.ProductId = Convert.ToInt32(rdr["PRODUCT_ID"]);
                    model.Title = rdr["TITLE"].ToString();
                    model.Description = rdr["DESCRIPTION"].ToString();
                    model.CategoryId = Convert.ToInt32(rdr["CATEGORY_ID"]);
                    model.CATEGORY_DESCRIPTION = Convert.ToString(rdr["CATEGORY_DESCRIPTION"]);
                    model.SubCategoryId = Convert.ToInt32(rdr["SUB_CATEGORY_ID"]);
                    model.SUB_CATEGORY_DESCRIPTION = Convert.ToString(rdr["SUB_CATEGORY_DESCRIPTION"]);
                    model.Brand = rdr["BRAND"].ToString();
                    model.SizeId = Convert.ToInt32(rdr["SIZE_ID"]);
                    model.SIZE_DESCRIPTION = Convert.ToString(rdr["SIZE_DESCRIPTION"]);
                    model.ColorId = Convert.ToInt32(rdr["COLOR_ID"]);
                    model.COLOR_DESCRIPTION = Convert.ToString(rdr["COLOR_DESCRIPTION"]);
                    model.MRP = Convert.ToDecimal(rdr["MRP"]);
                    model.Price = Convert.ToDecimal(rdr["PRICE"]);
                    model.Gender_id = Convert.ToInt32(rdr["GENDER_ID"]);
                    model.GENDER_DESCRIPTION = Convert.ToString(rdr["GENDER_DESCRIPTION"]);
                }
            }
            //  return View(model);
            return PartialView("_ProductDetails", model);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}