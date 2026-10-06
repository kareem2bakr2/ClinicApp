using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ClientModel.Primitives;

namespace ClinicApp.Audit
{
    public class AudittingSystem:SaveChangesInterceptor
    {
        ICurrentUserService _currentUserService;
        
        public AudittingSystem(ICurrentUserService currentUserService ) 
        { 
            _currentUserService = currentUserService;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var Context = eventData.Context;
            
            if (Context is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var entries = Context.ChangeTracker.Entries()
                    .Where(e => e.Entity is ILogsAttribuite).ToList();
            //var entiries = Context.ChangeTracker.Entries<ILogsAttribuite>();
            if(entries == null) { return base.SavingChangesAsync(eventData, result, cancellationToken); }
            foreach (var entry in entries)
            {

                var user = _currentUserService.CurrentUserID;
                var time = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property("CreatedById").CurrentValue = user;
                        entry.Property("CreationDate").CurrentValue = time;

                        var _newValues = string.Empty;
                        foreach (var prop in entry.Properties)
                        {
                            _newValues += $"{prop.Metadata.Name} : {prop.CurrentValue} , ";
                        }

                        AuditLog log = new AuditLog()
                        {
                            EntityName = entry.Entity.ToString(),
                            UserId = user,
                            Timestamp = time,
                            Action = "Create",
                            OldValues = "",
                            NewValues = _newValues
                            ,
                            Message = "Created By MVC Request"
                        };
                        Context.Set<AuditLog>().Add(log);
                        break;
                    case EntityState.Modified:
                        entry.Property("modefiedById").CurrentValue = user;
                        entry.Property("Lastmodified").CurrentValue = time;
                        foreach (var prop in entry.Properties)
                        {
                            var oldValues = string.Empty;
                            var newValues = string.Empty;
                            if (prop.IsModified)
                            {
                                var oldValue = prop.OriginalValue;
                                var newValue = prop.CurrentValue;
                                var propName = prop.Metadata.Name;
                                oldValue += $"{propName} : {oldValue} , ";
                                newValues += $"{propName} : {newValue} , ";
                            }

                            AuditLog logModify = new AuditLog()
                            {
                                EntityName = entry.Entity.ToString(),
                                UserId = user,
                                Timestamp = time,
                                Action = "Modify",
                                OldValues = oldValues,
                                NewValues = newValues,
                                Message = "Created By MVC Request"
                            };
                            Context.Set<AuditLog>().Add(logModify);
                        }
                        break;
                    case EntityState.Deleted:

                        break;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

    }
}
