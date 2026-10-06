using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Data
{
    public class ClinicAppContext : IdentityDbContext<ApplicationUser, ApplicationRole,string>
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; set; }
        public DbSet<MedicalService> MedicalServices { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<TreatmentPlanExercises> TreatmentPlanExercises { get; set; }
        public DbSet<TreatmentPlan> TreatmentPlans { get; set; }
        public DbSet<TherapySession> TherapySessions { get; set; }
       
        public DbSet<AuditLog> auditLogs { get; set; }
        public ClinicAppContext(DbContextOptions<ClinicAppContext> options): base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            #region isdeleted = flase
            builder.Entity<Appointment>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<Equipment>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<Exercise>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<ExerciseCategory>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<MedicalService>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<Patient>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<Muscle>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<Plan>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<Payment>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<Receptionist>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<Therapist>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<TreatmentPlan>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<TreatmentPlanExercises>().Property(e => e.isDeleted).HasDefaultValue(false);
            builder.Entity<TherapySession>().Property(e => e.isDeleted).HasDefaultValue(false);

            #endregion
            
            #region composite keys

            builder.Entity<TreatmentPlanExercises>().HasKey(t => new { t.treatmentPlanID, t.exerciseID });
            #endregion
        
            #region many to many relationships
            builder.Entity<Exercise>().HasMany(e=>e.Muscles).WithMany(e=>e.Exercises)
                .UsingEntity<Dictionary<string, object>>(
                "ExerciseMuscle",
                j=>j.HasOne<Muscle>().WithMany().HasForeignKey("MuscleID").OnDelete(DeleteBehavior.Restrict),
                j=>j.HasOne<Exercise>().WithMany().HasForeignKey("ExerciseID").OnDelete(DeleteBehavior.Restrict)
            );

            builder.Entity<Exercise>().HasMany(e => e.Equipments).WithMany(e => e.Exercises)
                .UsingEntity<Dictionary<string, object>>(
                "ExerciseEquipment",
                j=>j.HasOne<Equipment>().WithMany().HasForeignKey("EquipmentID").OnDelete(DeleteBehavior.Restrict),
                j=>j.HasOne<Exercise>().WithMany().HasForeignKey("ExerciseID").OnDelete(DeleteBehavior.Restrict)
            );
            #endregion

            #region logging relationships mapping




            builder.Entity<ApplicationUser>().HasMany(u => u.ReceptionistUserCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<ApplicationUser>().HasMany(u => u.TherapistUserCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<ApplicationUser>().HasMany(u=>u.PatientUserCreated)
                .WithOne(u=>u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<ApplicationUser>().HasMany(u => u.AppointmentsCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.EquipmentCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.PlanCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.ExerciseCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.ExerciseCategoriesCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.MedicalServiceCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.MuscleCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.PaymentsCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.TherapySessionCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.TreatmentPlanCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>().HasMany(u => u.TreatmentPlanExerciseCreated)
                .WithOne(u => u.CreatedBY)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationUser>().HasMany(u => u.PatientUserModified)
                .WithOne(u => u.Modifiedby)
                .HasForeignKey(u => u.modefiedById)
                .OnDelete(DeleteBehavior.Restrict);


            #endregion

            #region only avoiding some errors
            
            builder.Entity<ApplicationUser>().HasOne(e => e.Patient)
                .WithOne(e => e.appUser).HasForeignKey<Patient>(u =>u.AppUserId);

            builder.Entity<ApplicationUser>().HasOne(e => e.Receptionist)
                .WithOne(e => e.appUser).HasForeignKey<Receptionist>(u => u.AppUserId);

            builder.Entity<ApplicationUser>().HasOne(e => e.Therapist)
                .WithOne(e => e.appUser).HasForeignKey<Therapist>(u => u.AppUserId);

            #endregion

            #region default Values
            builder.Entity<Appointment>().Property(e => e.Status).HasDefaultValue(AppointmentStatus.pending);

            #endregion

        }

    }
}
