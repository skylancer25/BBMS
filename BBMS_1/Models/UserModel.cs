using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBMS_1.Models
{

    public class UserModel
    {
        [Key]
        public int patient_id { get; set; }
        public string patient_name { get; set; }
        public int age { get; set; } // "Patient" or "Donor"
        public string patient_address { get; set; }
        public int mobile_number { get; set; }
        public string blood_grp { get; set; }
        public int usr_id { get; set; }
        public string email { get; set; }

        //public string password { get; set; }

            public string Address { get; set; }
            public string Pincode { get; set; } // Pincode instead of Latitude/Longitude
        
    

}
}