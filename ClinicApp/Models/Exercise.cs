using ClinicApp.Data;

namespace ClinicApp.Models
{
    public class Exercise:ILogsAttribuite
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

        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Url)]
        public string VideoURL { get; set; }

        public int NumberOfSets { get; set; }

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Instructions { get; set; }

        public ExerciseDifficulty Difficulty { get; set; }

        [ForeignKey("ExerciseCategory")]
        public int? CategoryID { get; set; }

        public virtual ExerciseCategory ExerciseCategory { get; set; } = null;

        public virtual HashSet<Equipment>  Equipments { get; set; } = new HashSet<Equipment>();

        public virtual HashSet<Muscle> Muscles { get; set; } = new HashSet<Muscle>();
        public virtual HashSet<TreatmentPlanExercises> TreatmentPlanExercises { get; set; } 
            = new HashSet<TreatmentPlanExercises>();

    }
}
