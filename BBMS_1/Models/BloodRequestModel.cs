namespace BBMS_1.Models
{
    public class BloodRequestModel
    {
        public int Id { get; set; }
        public string RequiredBloodGroup { get; set; }
        public string AdditionalDetails { get; set; }
        public int UnitsRequired { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }  // Pending, Approved, Fulfilled
    }

}
