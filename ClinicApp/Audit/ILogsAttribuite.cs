using ClinicApp.Data;
using System.ComponentModel;

namespace ClinicApp.Audit
{
    public interface ILogsAttribuite
    {

   
        public bool isDeleted { get; set; }
        
        public string? CreatedById { get; set; }
        
        public string? modefiedById { get; set; }
        
        public ApplicationUser CreatedBY { get; set; }
        public DateTime CreationDate { get; set; }
        public ApplicationUser Modifiedby { get; set; }
        public DateTime? Lastmodified { get; set; }
        
    }
}
