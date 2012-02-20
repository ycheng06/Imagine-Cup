using System.Data.Entity;

namespace TBTracker.Models
{
    public class TrackerEntities : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<DrugInfo> DrugInfos { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestInfo> TestInfos { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}