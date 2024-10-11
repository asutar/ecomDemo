using EcomDemo.Areas.Profile.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcomDemo.Areas.Profile.Controllers
{
    public class CategoryMasterController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // READ: Get all categories
        public ActionResult Index()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM TBL_CATEGORY_MASTER";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CATEGORY_ID = (int)reader["CATEGORY_ID"],
                        CATEGORY_DESCRIPTION = reader["CATEGORY_DESCRIPTION"].ToString()
                    });
                }
            }
            return View(categories);
        }

        // CREATE: Add new category
        [HttpPost]
        public ActionResult Create(Category category)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO TBL_CATEGORY_MASTER (CATEGORY_DESCRIPTION) VALUES (@Description)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Description", category.CATEGORY_DESCRIPTION);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // UPDATE: Edit existing category
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE TBL_CATEGORY_MASTER SET CATEGORY_DESCRIPTION = @Description WHERE CATEGORY_ID = @CategoryId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryId", category.CATEGORY_ID);
                cmd.Parameters.AddWithValue("@Description", category.CATEGORY_DESCRIPTION);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // DELETE: Delete category
        public ActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM TBL_CATEGORY_MASTER WHERE CATEGORY_ID = @CategoryId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryId", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}