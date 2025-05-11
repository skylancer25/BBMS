using Microsoft.AspNetCore.Mvc.Rendering;

namespace BBMS_1.Models
{
    public class NewUser
    {

        public string user_name{ get; set; }
        public string email { get; set; }

        public string UserType { get; set; }

        public string password { get; set; }

        // Optional Donor Fields
        public string? bloodGroup { get; set; }
        public int? unitsAvailable { get; set; }
    }
}
