using EcomDemo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcomDemo.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            var model = new UserLoginViewModel();
            model.UserType = "C"; 
            return View(new UserLoginViewModel());
        }

        // POST: Login
        [HttpPost]
        public ActionResult Index(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool ss = model.OTPSent;
                // if (TempData.Peek("OTPSent") != null && (bool)TempData.Peek("OTPSent") == true)
                if (model.OTPSent == true)
                {
                    // Validate the OTP
                    if (ValidateOTP(model.MobileNumber, model.OTP))
                    {
                        Session["UserType"] = "C";
                        Session["UserId"] = model.MobileNumber;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Invalid OTP. Please try again.";
                    }
                }
                else
                {
                    if (ValidateMobileIsExists(model.MobileNumber,"C"))
                    {
                        TempData["OTPSent"] = true;
                        UpdateOTP(model.MobileNumber);
                        model.OTPSent = true;
                        Session["UserType"] = "C";
                    }
                    else
                    {
                        // Send OTP to mobile number (not implemented here)
                        TempData["OTPSent"] = true;
                        // Call your stored procedure to insert OTP into the database
                        SaveUserAndOTP(model.MobileNumber,"C");
                        model.OTPSent = true;
                        Session["UserType"] = "C";
                    }
                   
                }
            }

            return View(model);
        }

        private bool ValidateMobileIsExists(string mobileNumber, string UserType)
        {
            bool isValid = false;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM TBL_USER_LOGIN_MASTER WHERE MOBILE_NO = @MobileNumber AND USER_TYPE=@UserType";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                    cmd.Parameters.AddWithValue("@UserType", UserType);
                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    isValid = count > 0;
                }
            }

            return isValid;
        }
        private void UpdateOTP(string mobileNumber)
        {
            string otp2 = GenerateOTP(); // Generate OTP (not implemented here)

            bool RowsAffected = false;
            object objReturn = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PROC_UPDATE_OTP";

                cmd.Parameters.Add("@MOBILE_NO", SqlDbType.VarChar).Value = mobileNumber;
                cmd.Parameters.Add("@OTP", SqlDbType.VarChar).Value = otp2;
                cmd.Parameters.Add("@USER_TYPE", SqlDbType.VarChar).Value = "C";

                cmd.Parameters.Add("@RowCount", SqlDbType.Int);
                cmd.Parameters["@RowCount"].Direction = ParameterDirection.Output;

                con.Open();
                objReturn = cmd.ExecuteScalar();
                RowsAffected = Convert.ToBoolean(cmd.Parameters["@RowCount"].Value);
                con.Close();
            }
        }
        private bool ValidateOTP(string mobileNumber, string otp)
        {
            bool isValid = false;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM TBL_USER_LOGIN_MASTER WHERE MOBILE_NO = @MobileNumber AND OTP = @OTP";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                    cmd.Parameters.AddWithValue("@OTP", otp);

                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    isValid = count > 0;
                }
            }

            return isValid;
        }

        private void SaveUserAndOTP(string mobileNumber,string User_type)
        {
            string otp = GenerateOTP(); // Generate OTP (not implemented here)
            
            bool RowsAffected = false;
            object objReturn = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PROC_ADD_USER_LOGIN";

                cmd.Parameters.Add("@MOBILE_NO", SqlDbType.VarChar).Value = mobileNumber;
                cmd.Parameters.Add("@OTP", SqlDbType.VarChar).Value = otp;
                cmd.Parameters.Add("@USER_TYPE", SqlDbType.VarChar).Value = User_type;

                cmd.Parameters.Add("@RowCount", SqlDbType.Int);
                cmd.Parameters["@RowCount"].Direction = ParameterDirection.Output;

                con.Open();
                objReturn = cmd.ExecuteScalar();
                RowsAffected = Convert.ToBoolean(cmd.Parameters["@RowCount"].Value);
                con.Close();
            }
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString();
        }
    }
}