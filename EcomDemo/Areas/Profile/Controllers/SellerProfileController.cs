using EcomDemo.Areas.Profile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcomDemo.Areas.Profile.Controllers
{
    public class SellerProfileController : Controller
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // GET: Profile/SellerProfile
        public ActionResult Index()
        {
            var model = new PersonalInformationViewModel();

            // Get the Mobile Number from the Session
            string mobileNumber = Session["UserId"]?.ToString();

            if (!string.IsNullOrEmpty(mobileNumber))
            {
                // Fetch user data from the database
                model = GetUserInfo(mobileNumber,"V");
            }

            return View(model);
        }
        private PersonalInformationViewModel GetUserInfo(string mobileNumber,string UserType)
        {
            var model = new PersonalInformationViewModel();
            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PROC_GET_USER_INFO", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MOBILE_NO", mobileNumber);
                    cmd.Parameters.AddWithValue("@USER_TYPE", UserType);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        model.FirstName = reader["FIRST_NAME"].ToString();
                        model.LastName = reader["LAST_NAME"].ToString();
                        model.Gender = reader["GENDER"].ToString();
                    }
                }
            }

            return model;
        }
        [HttpPost]
        public ActionResult Index(PersonalInformationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Call the method to save data using ADO.NET and stored procedure
                bool isUpdated = UpdatePersonalInfoInDatabase(model);

                if (isUpdated)
                {
                    ViewBag.SuccessMessage = "Your personal information has been updated successfully!";
                }
            }

            return View(model);
        }
        private bool UpdatePersonalInfoInDatabase(PersonalInformationViewModel model)
        {
            bool RowsAffected = false;
            object objReturn = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PROC_UPDATE_PERSONAL_INFO";

                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = model.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = model.LastName;
                cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = model.Gender;
                cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = Convert.ToString(Session["UserId"]);
                cmd.Parameters.Add("@UserType", SqlDbType.VarChar).Value = "V";

                cmd.Parameters.Add("@RowCount", SqlDbType.Int);
                cmd.Parameters["@RowCount"].Direction = ParameterDirection.Output;

                con.Open();
                objReturn = cmd.ExecuteScalar();
                RowsAffected = Convert.ToBoolean(cmd.Parameters["@RowCount"].Value);
                con.Close();

            }

            return RowsAffected;
        }
    }
}