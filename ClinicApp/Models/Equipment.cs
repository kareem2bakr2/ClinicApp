using ClinicApp.Data;

namespace ClinicApp.Models
{
    public class Equipment : ILogsAttribuite
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
        [StringLength(100)]
        public string Name { get; set; }
        public decimal price { get; set; }
        public string MerchantName { get; set; }

        public virtual HashSet<Exercise> Exercises { get; set; } = new HashSet<Exercise>();
    }

}
