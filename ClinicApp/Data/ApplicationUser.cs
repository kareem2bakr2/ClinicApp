using ClinicApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ClinicApp.Data;

// Add profile data for application users by adding properties to the ApplicationUser class

public class ApplicationUser : IdentityUser 
{
    [Required]
    public AccountType type { get; set; }

    public virtual Receptionist Receptionist { get; set; } = null;

    public virtual Patient Patient { get; set; } = null;
    
    public virtual Therapist Therapist { get; set; } = null;




    #region All app Logs
    public virtual HashSet<Patient> PatientUserCreated { get; set; } = new HashSet<Patient>();
    public virtual HashSet<Patient> PatientUserModified { get; set; } = new HashSet<Patient>();


    public virtual HashSet<Receptionist> ReceptionistUserCreated { get; set; } = new HashSet<Receptionist>();
    public virtual HashSet<Receptionist> ReceptionistUserModified { get; set; } = new HashSet<Receptionist>();


    public virtual HashSet<Therapist> TherapistUserCreated { get; set; } = new HashSet<Therapist>();
    public virtual HashSet<Therapist> TherapistUserModified { get; set; } = new HashSet<Therapist>();



    public virtual HashSet<Appointment> AppointmentsCreated { get; set; } = new HashSet<Appointment>();
    public virtual HashSet<Appointment> AppointmentsModified { get; set; } = new HashSet<Appointment>();

    public virtual HashSet<Equipment> EquipmentCreated { get; set; } = new HashSet<Equipment>();
    public virtual HashSet<Equipment> EquipmentModified { get; set; } = new HashSet<Equipment>();


    public virtual HashSet<Exercise> ExerciseCreated { get; set; } = new HashSet<Exercise>();
    public virtual HashSet<Exercise> ExerciseModified { get; set; } = new HashSet<Exercise>();


    public virtual HashSet<ExerciseCategory> ExerciseCategoriesCreated { get; set; } = new HashSet<ExerciseCategory>();
    public virtual HashSet<ExerciseCategory> ExerciseCategoriesModified { get; set; } = new HashSet<ExerciseCategory>();


    public virtual HashSet<MedicalService> MedicalServiceCreated { get; set; } = new HashSet<MedicalService>();
    public virtual HashSet<MedicalService> MedicalServiceModified { get; set; } = new HashSet<MedicalService>();


    public virtual HashSet<Muscle> MuscleCreated { get; set; } = new HashSet<Muscle>();
    public virtual HashSet<Muscle> MuscleModified { get; set; } = new HashSet<Muscle>();


    public virtual HashSet<Payment> PaymentsCreated { get; set; } = new HashSet<Payment>();
    public virtual HashSet<Payment> PaymentsModified { get; set; } = new HashSet<Payment>();


    public virtual HashSet<Plan> PlanCreated { get; set; } = new HashSet<Plan>();
    public virtual HashSet<Plan> PlanModified { get; set; } = new HashSet<Plan>();


    public virtual HashSet<TherapySession> TherapySessionCreated { get; set; } = new HashSet<TherapySession>();
    public virtual HashSet<TherapySession> TherapySessionModified { get; set; } = new HashSet<TherapySession>();


    public virtual HashSet<TreatmentPlan> TreatmentPlanCreated { get; set; } = new HashSet<TreatmentPlan>();
    public virtual HashSet<TreatmentPlan> TreatmentPlanModified { get; set; } = new HashSet<TreatmentPlan>();


    public virtual HashSet<TreatmentPlanExercises> TreatmentPlanExerciseCreated { get; set; } = new HashSet<TreatmentPlanExercises>();
    public virtual HashSet<TreatmentPlanExercises> TreatmentPlanExerciseModified { get; set; } = new HashSet<TreatmentPlanExercises>();

    #endregion

}
