using ClinicApp.Data;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicApp.Models
{
    public class Plan : ILogsAttribuite
    {
        #region log flags
        public bool isDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? Lastmodified { get; set; }

        [ForeignKey("CreatedBY")]
        public string? CreatedById { get; set; }

        [ForeignKey("Modifiedby")]
        public string? modefiedById { get; set; }

        public virtual ApplicationUser Modifiedby { get; set; }

        public virtual ApplicationUser CreatedBY { get; set; }

        #endregion

        [Key]
        public int ID { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name ="Number Of Sessions")]
        public int? numberOfSession { get; set; }

        [DataType(dataType:DataType.Currency)]
        public decimal? Price { get; set; }

        public virtual HashSet<Appointment> Appointments { get; set; } = new HashSet<Appointment>();

    }
}
