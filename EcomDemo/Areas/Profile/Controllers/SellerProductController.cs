using EcomDemo.Areas.Profile.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcomDemo.Areas.Profile.Controllers
{
    public class SellerProductController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // GET: Profile/SellerProduct
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllSubCategories()
        {
            List<SubCategoryModel> subCategories = new List<SubCategoryModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_GET_ALL_SUB_CATEGORIES", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        subCategories.Add(new SubCategoryModel
                        {
                            SUB_CATEGORY_ID = Convert.ToInt32(rdr["SUB_CATEGORY_ID"]),
                            SUB_CATEGORY_DESCRIPTION = rdr["SUB_CATEGORY_DESCRIPTION"].ToString(),
                            CATEGORY_DESCRIPTION = rdr["CATEGORY_DESCRIPTION"].ToString(),
                            CATEGORY_ID = Convert.ToInt32(rdr["CATEGORY_ID"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(subCategories, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetCategories()
        {
            List<SelectListItem> categories = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT CATEGORY_ID, CATEGORY_DESCRIPTION FROM TBL_CATEGORY_MASTER", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    categories.Add(new SelectListItem
                    {
                        Value = rdr["CATEGORY_ID"].ToString(),
                        Text = rdr["CATEGORY_DESCRIPTION"].ToString()
                    });
                }
            }
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        // 1. Retrieve All Products
        public JsonResult GetAllProducts()
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

            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        // 2. Add New Product
        [HttpPost]
        public JsonResult AddProduct(Product product)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO TBL_PRODUCT_MASTER (TITLE, CATEGORY_ID, SUB_CATEGORY_ID, BRAND, SIZE_ID, COLOR_ID, MRP, PRICE,GENDER_ID) VALUES (@Title, @CategoryId, @SubCategoryId, @Brand, @SizeId, @ColorId, @MRP, @Price,@Gender_id)", con);
                cmd.Parameters.AddWithValue("@Title", product.Title);
               // cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("@SubCategoryId", product.SubCategoryId);
                cmd.Parameters.AddWithValue("@Brand", product.Brand);
                cmd.Parameters.AddWithValue("@SizeId", product.SizeId);
                cmd.Parameters.AddWithValue("@ColorId", product.ColorId);
                cmd.Parameters.AddWithValue("@MRP", product.MRP);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Gender_id", product.Gender_id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // 3. Edit Product
        [HttpPost]
        public JsonResult EditProduct(Product product)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE TBL_PRODUCT_MASTER SET TITLE = @Title, DESCRIPTION = @Description, CATEGORY_ID = @CategoryId, SUB_CATEGORY_ID = @SubCategoryId, BRAND = @Brand, SIZE_ID = @SizeId, COLOR_ID = @ColorId, MRP = @MRP, PRICE = @Price WHERE PRODUCT_ID = @ProductId", con);
                cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                cmd.Parameters.AddWithValue("@Title", product.Title);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("@SubCategoryId", product.SubCategoryId);
                cmd.Parameters.AddWithValue("@Brand", product.Brand);
                cmd.Parameters.AddWithValue("@SizeId", product.SizeId);
                cmd.Parameters.AddWithValue("@ColorId", product.ColorId);
                cmd.Parameters.AddWithValue("@MRP", product.MRP);
                cmd.Parameters.AddWithValue("@Price", product.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // 4. Delete Product
        [HttpPost]
        public JsonResult DeleteProduct(int productId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM TBL_PRODUCT_MASTER WHERE PRODUCT_ID = @ProductId", con);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProductById(int id)
        {
            Product product = new Product();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM TBL_PRODUCT_MASTER WHERE PRODUCT_ID = @ProductId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ProductId", id);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            product.ProductId = Convert.ToInt32(reader["PRODUCT_ID"]);
                            product.Title = reader["TITLE"].ToString();
                            product.Description = reader["DESCRIPTION"].ToString();
                            product.CategoryId = Convert.ToInt32(reader["CATEGORY_ID"]);
                            product.SubCategoryId = Convert.ToInt32(reader["SUB_CATEGORY_ID"]);
                            product.Brand = reader["BRAND"].ToString();
                            product.SizeId = Convert.ToInt32(reader["SIZE_ID"]);
                            product.ColorId = Convert.ToInt32(reader["COLOR_ID"]);
                            product.MRP = Convert.ToDecimal(reader["MRP"]);
                            product.Price = Convert.ToDecimal(reader["PRICE"]);
                        }
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(product, JsonRequestBehavior.AllowGet);
        }
    }
}