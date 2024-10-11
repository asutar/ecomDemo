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
    public class SubCategoryMasterController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Profile/SubCategoryMaster
        public ActionResult Index()
        {
            //SubCategoryModel model = new SubCategoryModel
            //{
            //    Categories = GetCategories(),
            //    SubCategories = GetAllSubCategories()
            //};
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(SubCategoryModel model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(model.SUB_CATEGORY_ID == 0 ? "SP_INSERT_SUB_CATEGORY" : "SP_UPDATE_SUB_CATEGORY", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SUB_CATEGORY_DESCRIPTION", model.SUB_CATEGORY_DESCRIPTION);
                cmd.Parameters.AddWithValue("@CATEGORY_ID", model.CATEGORY_ID);

                if (model.SUB_CATEGORY_ID != 0)
                {
                    cmd.Parameters.AddWithValue("@SUB_CATEGORY_ID", model.SUB_CATEGORY_ID);
                }

                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_DELETE_SUB_CATEGORY", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SUB_CATEGORY_ID", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
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
    }
}