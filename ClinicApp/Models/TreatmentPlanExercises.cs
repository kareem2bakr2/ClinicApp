using ClinicApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Models
{
    public class TreatmentPlanExercises:ILogsAttribuite
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

        [ForeignKey("TreatmentPlan")]
        public int? treatmentPlanID { get; set; }

        [ForeignKey("Exercise")]
        public int? exerciseID { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual TreatmentPlan TreatmentPlan { get; set; } = null;
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual Exercise Exercise { get; set; } = null;

    }
}
