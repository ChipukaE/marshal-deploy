using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace marshal_deploy.Models
{
    public partial class Deploy : DbContext
    {
        public Deploy()
            : base("name=Deploy")
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Cluster> Clusters { get; set; }
        public virtual DbSet<DailyPerform> DailyPerforms { get; set; }
        public virtual DbSet<DailyTarget> DailyTargets { get; set; }
        public virtual DbSet<Deployment> Deployments { get; set; }
        public virtual DbSet<MonthlyPerform> MonthlyPerforms { get; set; }
        public virtual DbSet<MonthlyTarget> MonthlyTargets { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<Precinct> Precincts { get; set; }
        public virtual DbSet<PrecinctPerformance> PrecinctPerformances { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Precinct>()
                .HasMany(e => e.PrecinctPerformances)
                .WithOptional(e => e.Precinct)
                .HasForeignKey(e => e.PrecinctId);

            modelBuilder.Entity<Precinct>()
                .HasMany(e => e.PrecinctPerformances1)
                .WithOptional(e => e.Precinct1)
                .HasForeignKey(e => e.PrecinctId);
        }
    }
}
