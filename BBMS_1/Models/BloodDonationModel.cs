using System;
using System.ComponentModel.DataAnnotations;

namespace BBMS_1.Models
{
    public class BloodDonationModel
    {
        public int DonationId { get; set; }

        public int UserId { get; set; }

        [Required]
        public string BloodGroup { get; set; }

        [Required]
        public int Units { get; set; }

        public DateTime DonationDate { get; set; } = DateTime.Now;
    }
}
