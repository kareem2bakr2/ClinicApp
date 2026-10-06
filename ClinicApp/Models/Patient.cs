using ClinicApp.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Net;

namespace ClinicApp.Models
{
    public class Patient : ILogsAttribuite
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
        
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string EmergencyPhone { get; set; }
        
        [Required]
        [MinLength(3)]
        public string EmergencyContact { get; set; }
        
        [Required]
        public string EmergencyRelationShip { get; set; }
        
        [Required]
        [RegularExpression(@"^[^ ]+( +[^ ]+)+$", ErrorMessage = "Please Enter Your FullName.")]
        [Length(3,100 ,ErrorMessage ="Name must be between 3 and 100 characters.")]
        public string Name { get; set; }
        
        public Gender Gender { get; set; }
        
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        [ForeignKey("appUser")]
        public string AppUserId { get; set; }
        
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual ApplicationUser appUser { get; set; } = null;

        public virtual HashSet<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public virtual HashSet<Payment> payments { get; set; }= new HashSet<Payment>();
    }
}
