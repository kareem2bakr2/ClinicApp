using ClinicApp.Data;

namespace ClinicApp.Models
{
    public class TreatmentPlan : ILogsAttribuite
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
        [MaxLength(100)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        
        public TreatmentPlanCategory Category { get; set; }

        [Required]
        public TreatmentPlanLevel Level { get; set; }
        
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public virtual HashSet<TherapySession> TherapySessions { get; set; } = new HashSet<TherapySession>();

        public virtual HashSet<TreatmentPlanExercises> TreatmentPlanExercises { get; set; } = new HashSet<TreatmentPlanExercises>();
    }
}
