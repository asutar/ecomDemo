using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcomDemo.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string MobileNumber { get; set; }
        public string OTP { get; set; }
        public bool OTPSent { get; set; }
        public string UserType { get; set; }
    }
}