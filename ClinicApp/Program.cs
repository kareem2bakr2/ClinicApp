using Microsoft.AspNetCore.Identity;
using ClinicApp.Comman;
namespace ClinicApp
{
    // add account controller

    public class Program
    {
        //register emailservice and account service user identity 
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddScoped<IGenericRepository, GenericRepository>();

            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            #region register Repositories
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IEquipmentRepository,EquipmentRepository> ();
            builder.Services.AddScoped<IExerciseCategoryRepository, ExerciseCategoryRepository> ();
            builder.Services.AddScoped<IExerciseRepository, ExerciseRepository> ();
            builder.Services.AddScoped<IMedicalServiceRepository, MedicalServiceRepository>();
            builder.Services.AddScoped<IMuscleRepository, MuscleRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<IReceptionistRepository, ReceptionistRepository>();
            builder.Services.AddScoped<ITherapistRepository, TherapistRepository>();
            builder.Services.AddScoped<ITherapySessionRepository, TherapySessionRepository>();
            builder.Services.AddScoped<ITreatmentPlanExercisesRepository, TreatmentPlanExercisesRepository>();
            builder.Services.AddScoped<ITreatmentPlanRepository, TreatmentPlanRepository>();
            #endregion

            #region register Services

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IEquipmentService, EquipmentService>();
            builder.Services.AddScoped<IExerciseCategoryService, ExerciseCategoryService>();
            builder.Services.AddScoped<IExerciseService, ExerciseService>();
            builder.Services.AddScoped<IMedicalServiceService, MedicalServiceService>();
            builder.Services.AddScoped<IMuscleService, MuscleService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IPlanService , PlanService>();
            builder.Services.AddScoped<IReceptionistService, ReceptionistService>();
            builder.Services.AddScoped<ITherapistService, TherapistService>();
            builder.Services.AddScoped<ITherapySessionService , TherapySessionService>();
            builder.Services.AddScoped<ITreatmentPlanExercisesService, TreatmentPlanExercisesService>();
            builder.Services.AddScoped<ITreatmentPlanService, TreatmentPlanService>();

            #endregion



            builder.Services.AddScoped<AudittingSystem>();

            var connectionString = builder.Configuration
                .GetConnectionString("ClinicAppContext");

            builder.Services.AddDbContext<ClinicAppContext>((sp,options )=> {
                var audittingSystemInterceptor = sp.GetService<AudittingSystem>();
                options.UseLazyLoadingProxies()
                        .UseSqlServer(connectionString)
                        .AddInterceptors(audittingSystemInterceptor);
            }

            );
            // FIX 1: Add .AddRoles<IdentityRole>() so RoleManager is registered in DI
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.SignIn.RequireConfirmedAccount = false; // Set to false during development
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ClinicAppContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                    var UserManager = services.GetRequiredService<UserManager<ApplicationUser>>();
   
                    await SeedRolesAsync(roleManager , UserManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding roles.");
                }
            }

            // Local function to handle seeding
            async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager , UserManager<ApplicationUser> userManager)
            {
                string[] roles =
                {
                    ApplicationRole.Admin,
                    ApplicationRole.Therapist,
                    ApplicationRole.Patient,
                    ApplicationRole.Receptionist
                };

                foreach (var roleName in roles)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new ApplicationRole(roleName));
                        var user = new ApplicationUser
                        {
                            UserName = roleName,
                            Email = $"{roleName.ToLower()}@clinic.com"
                        };

                        await userManager.CreateAsync(user , "kareem12345");
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
            }
            app.Run();

        }
    }
}
