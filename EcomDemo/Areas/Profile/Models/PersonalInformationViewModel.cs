using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcomDemo.Areas.Profile.Models
{
    public class PersonalInformationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string UserType { get; set; }
    }
}