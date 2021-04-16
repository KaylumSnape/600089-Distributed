using DistSysAcw.Auth;
using DistSysAcw.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DistSysAcw
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
            services.AddDbContext<UserContext>();

            services.AddControllers(options => { options.AllowEmptyInputInBodyModelBinding = true; });

            services.AddHttpContextAccessor();

            services.AddAuthentication(options => { options.DefaultScheme = "CustomAuthentication"; })
                .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>
                    ("CustomAuthentication", options => { });

            services.AddTransient<IAuthorizationHandler, CustomAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            //app.UseHttpsRedirection(); // Real men don't use protection.

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}