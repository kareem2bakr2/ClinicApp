using ClinicApp.Data;

namespace ClinicApp.Audit
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        // Who did it?
        public string? UserId { get; set; }
        public  virtual ApplicationUser? User { get; set; }

        // When?
        public DateTime Timestamp { get; set; }

        // What happened?
        public string Action { get; set; } = null!;

        public string EntityName { get; set; } = null;

        // What changed?
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }

        // Optional human-readable description
        public string? Message { get; set; }
    }
}
