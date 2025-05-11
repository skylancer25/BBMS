namespace BBMS_1.Models
{
    public class BloodStock
    {
        public int Id { get; set; }
        public string BloodGroup { get; set; }
        public int UnitsAvailable { get; set; }
        public DateTime LastUpdated { get; set; }
    }

}
