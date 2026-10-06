using ClinicApp.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ClinicApp.Models
{
    public class Therapist :ILogsAttribuite
    {

        #region log flags
        public bool RequirePasswordChange { get; set; }
        public DateTime? LockDate { get; set; }
        public int LockCount { get; set; }
        public bool isLocked { get; set; }
        [ForeignKey("CreatedBY")]
        public string? CreatedById { get; set; }
        [ForeignKey("modefiedBy")]
        public string? modefiedById { get; set; }

        public bool isDeleted { get; set; }
        public virtual ApplicationUser CreatedBY { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ApplicationUser Modifiedby { get; set; }
        public DateTime? Lastmodified { get; set; }
        #endregion

        [Key]
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[^ ]+( +[^ ]+)+$", ErrorMessage = "Please Enter Your FullName.")]
        [Length(3,100 ,ErrorMessage ="Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [DataType(dataType:DataType.ImageUrl)]
        public string PhotoURL { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Length(10,13)]
        public string Phone { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }


        public Gender Gender { get; set; }

        [Required]
        public TherapistSpecialization Specialization { get; set; }

        [Display(Name = "Seniority Level")]
        public SeniorityLevel SeniorityLevel { get; set; }

        [Required]
        [Display(Name= "Hour Rate")]
        public decimal hourRate { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerID { get; set; }

        [Required]
        [ForeignKey("appUser")]
        public string AppUserId { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual ApplicationUser appUser { get; set; } = null;


        public virtual Therapist Manager { get; set; } = null;

        public virtual HashSet<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public virtual HashSet<Payment> Payments { get; set; } = new HashSet<Payment>();

        public virtual HashSet<TherapySession> TherapySessions { get; set; } = new HashSet<TherapySession>();
        
    }
}
