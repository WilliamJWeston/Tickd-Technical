using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace API
{
    public class TickdDbContext : DbContext
    {
        public TickdDbContext(DbContextOptions<TickdDbContext> options) : base(options) { }

        // Users
        public DbSet<MeterReading> MeterReadings { get; set; }

        // Meter Readings
        public DbSet<Accounts> Accounts { get; set; }
    }
}
