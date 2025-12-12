using DiaMate.Data.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;

namespace DiaMate.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        
        public DbSet<Person> Persons { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<BloodGlucoseReading> BloodGlucoseReadings { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<FootUlcerImage> FootUlcerImages { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
     


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Person>()
                .HasIndex(p => p.Email)
                .IsUnique();

            builder.Entity<Patient>()
                .HasIndex(p=>p.PersonId)
                .IsUnique();

            builder.Entity<AppUser>()
                .HasIndex(a=>a.PatientId)
                .IsUnique();
        }

    }
    
}

