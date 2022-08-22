using CoreLayout.Repositories.Common;
using CoreLayout.Repositories.Masters.City;
using CoreLayout.Repositories.Masters.Country;
using CoreLayout.Repositories.Masters.Dashboard;
using CoreLayout.Repositories.Masters.Deparment;
using CoreLayout.Repositories.Masters.Pratice;
using CoreLayout.Repositories.Masters.Role;
using CoreLayout.Repositories.Masters.State;
using CoreLayout.Repositories.UserManagement.AssignMenuByRole;
using CoreLayout.Repositories.UserManagement.AssignMenuByUser;
using CoreLayout.Repositories.UserManagement.AssignRole;
using CoreLayout.Repositories.UserManagement.ButtonPermission;
using CoreLayout.Repositories.UserManagement.Login;
using CoreLayout.Repositories.UserManagement.Menu;
using CoreLayout.Repositories.UserManagement.ParentMenu;
using CoreLayout.Repositories.UserManagement.Registration;
using CoreLayout.Repositories.UserManagement.SubMenu;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.City;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.Masters.Department;
using CoreLayout.Services.Masters.Pratice;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.AssignMenuByRole;
using CoreLayout.Services.UserManagement.AssignMenuByUser;
using CoreLayout.Services.UserManagement.AssignRole;
using CoreLayout.Services.UserManagement.ButtonPermission;
using CoreLayout.Services.UserManagement.Login;
using CoreLayout.Services.UserManagement.Menu;
using CoreLayout.Services.UserManagement.ParentMenu;
using CoreLayout.Services.UserManagement.SubMenu;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CoreLayout
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //authorization
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddControllersWithViews();
            services.AddMvcCore().AddDataAnnotations();

            //session
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time   
            });
          
            //login
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ILoginService, LoginService>();

            //registration
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<IRegistrationService, RegistrationService>();

            //Dashboard
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IDashboardService, DashboardService>();

            //country
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryService, CountryService>();

            //state
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IStateService, StateService>();

            //city
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICityService, CityService>();

            //menu
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuRepository, MenuRepository>();

            //parent menu
            services.AddScoped<IParentMenuService, ParentMenuService>();
            services.AddScoped<IParentMenuRepository, ParentMenuRepository>();

            //sub menu
            services.AddScoped<ISubMenuService, SubMenuService>();
            services.AddScoped<ISubMenuRepository, SubMenuRepository>();

            //role
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository,RoleRepository>();

            //assign role
            services.AddScoped<IAssignRoleService, AssignRoleService>();
            services.AddScoped<IAssignRoleRepository, AssignRoleRepository>();

            //assign menu by role
            services.AddScoped<IAssignMenuByRoleService, AssignMenuByRoleService>();
            services.AddScoped<IAssignMenuByRoleRepository, AssignMenuByRoleRepository>();

            //assign  menu by user
            services.AddScoped<IAssignMenuByUserService, AssignMenuByUserService>();
            services.AddScoped<IAssignMenuByUserRepository, AssignMenuByUserRepository>();

            //pratice
            services.AddScoped<IPraticeService, PraticeService>();
            services.AddScoped<IPraticeRepository, PraticeRepository>();

            //Button
            services.AddScoped<IButtonService, ButtonService>();
            services.AddScoped<IButtonRepository, ButtonRepository>();

            //ButtonPermission
            services.AddScoped<IButtonPermissionService, ButtonPermissionService>();
            services.AddScoped<IButtonPermissionRepository, ButtonPermissionRepository>();

            //department
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            //common
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICommonRepository, CommonRepository>();

            //session
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //add for encrypt value
            services.AddDataProtection();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
           
            //new
            app.UseSession();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}");
            });

        }
    }
}
