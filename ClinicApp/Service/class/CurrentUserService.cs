using System.Security.Claims;

namespace ClinicApp.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string? CurrentUserID =>
                        httpContextAccessor?
                        .HttpContext?
                        .User
                        .FindFirstValue(ClaimTypes.NameIdentifier);
       
    }
}
