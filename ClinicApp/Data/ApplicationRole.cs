using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Data
{
    public class ApplicationRole : IdentityRole
    {
        public const string Admin = "Admin";
        public const string Therapist = "Therapist";
        public const string Patient = "Patient";
        public const string Receptionist = "Receptionist";

        public const string AdminReceptionist = Admin + "," + Receptionist;
        public const string AdminTherapist = Admin + "," + Therapist;
        public const string AdminReceptionistPatient = Admin + "," + Receptionist + "," + Patient;
        public const string AdminTherapistPatient = Admin + "," + Therapist + "," + Patient;
        public const string ClinicalStaff = Admin + "," + Receptionist + "," + Therapist;
        public const string AllClinicRoles = Admin + "," + Receptionist + "," + Therapist + "," + Patient;

        public ApplicationRole():base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}
