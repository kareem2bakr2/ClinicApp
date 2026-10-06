global using System.ComponentModel.DataAnnotations.Schema;
global using System.ComponentModel.DataAnnotations;
global using ClinicApp.Models;
global using ClinicApp.Audit;
global using ClinicApp.Service;
global using ClinicApp.Repository;
global using ClinicApp.Data;
global using ClinicApp.ViewModel;
global using Microsoft.EntityFrameworkCore;
global using ClinicApp.Validation;

public enum ContractStatus 
{
    Draft,
    Active,
    Expired
}

public enum StaticNums { 
    AppManagerID = 24

}

public enum AccountType
{
    Receptionist,
    Patient,
    Therapist, 
    Notfound
}
public enum Gender
{
    Male,
    Female
}
//cancel button has it is own flag to show 
//shedule button appear if the sattus is pending 
//complete button appears if the status is scheduled 
public enum AppointmentStatus
{
    cancelled ,// return money to client delete if there is a payment credited ,
               // add a credit to the medical service with notes canceled with appointment number
               // delete sessions if created
               // if ther is any session completed make cancel button disabled 
    scheduled ,// schedule sessions add number of session based on plan in the relation with appointment 
    pending ,  // has no info
    completed  // make sure no session is pending or show error message indicating this 
}
//you must assign a plan id when create an appointment
public enum SessionStatus
{
    Pending,    // when sceduling a session to another time scedule appointment too id it ends befor the session date 
    Completed, // when complete check if it is the last session complete the appointment automatically
    Cancelled,
    NoShow
}
//[Flags]
public enum TherapistSpecialization
{
    Orthopedic,
    Neurological,
    Pediatric,
    Sports,
    Geriatric,
    Cardiopulmonary,
    WomensHealth,
    Musculoskeletal,
    Rehabilitation
}

public enum SeniorityLevel
{
    Intern,
    Junior,
    MidLevel,
    Senior,
    Lead,
    Principal
}

public enum PaymentMethod
{
    Cash,
    CreditCard,
    DebitCard,
    BankTransfer,
    MobileWallet,
    InstaPay,
   
}
//[Flags]
public enum Paymentfilter { 
    credit,
    debit
}
public enum PaymentType
{
    PatientAppointmentFee=1,
    TherapistSalary=2,
    EmployeeSalary=4,
    Equipment=8,
    Rent=16,
    Utilities=32,
    Supplies=64,
    Other=128,
    MedicalProviderAppointmentFee = 256
}
//[Flags]
public enum TreatmentPlanLevel
{
    Initial=1,
    Intermediate=2,
    Advanced=4
}
//[Flags]
public enum TreatmentPlanCategory
{
    Rehabilitation=1,
    PainManagement=2,
    PostSurgery=4,
    InjuryRecovery=8,
    MobilityImprovement=16,
    Strengthening=32,
    ChronicCondition=64,
    PreventiveCare=128
}
public enum ExerciseDifficulty
{
    Beginner,
    Intermediate,
    Advanced
}