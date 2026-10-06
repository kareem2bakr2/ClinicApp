using ClinicApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Models
{

    public class Receptionist : ILogsAttribuite
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
        
        [DataType(dataType:DataType.ImageUrl)]
        public string PhotoURL { get; set; }

        [RegularExpression(@"^[^ ]+( +[^ ]+)+$", ErrorMessage = "Please Enter Your FullName.")]
        public string Name { get; set; }

        [Range(2,90, ErrorMessage ="Age Must Be Between 2 and 90")]
        public int Age { get; set; }


        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public Gender Gender { get; set; }
        
        public DateTime WorktimeFrom { get; set; }
        public DateTime WorkTimeTO { get; set; }

        
        public DayOfWeek Dayoff { get; set; }


        [Required]
        [ForeignKey("appUser")]
        public string AppUserId { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual ApplicationUser appUser { get; set; } = null;

        public virtual HashSet<Payment> Payments { get; set; } = new HashSet<Payment>();
        public virtual HashSet<Appointment> Appointments { get; set; } = new HashSet<Appointment>();


    }
}
