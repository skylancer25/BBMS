namespace BBMS_1.Models
{
    public class PatientViewModel
    {
        public UserModel Patient { get; set; }
        public List<UserModel> NearbyDonors { get; set; }

    }
}
