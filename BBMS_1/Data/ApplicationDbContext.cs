using Microsoft.EntityFrameworkCore;
using BBMS_1.Models;

namespace BBMS_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }  // Normal EF table mapping
        public DbSet<BloodRequestModel> BloodRequests { get; set; }
        public DbSet<BloodStock> BloodStocks { get; set; }
        public DbSet<Donor> Donors { get; set; }
    }

}
