using System.Collections.Generic;

namespace BBMS_1.Models
{
    public class UserProfileViewModel
    {
        public string patient_name { get; set; }
        public string blood_grp { get; set; }
        public string email { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }

        public List<BloodRequestModel> BloodRequests { get; set; }
    }
}
