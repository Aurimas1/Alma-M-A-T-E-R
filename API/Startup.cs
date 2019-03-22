using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;

namespace API
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
            var connection = @"Server=(localdb)\mssqllocaldb;Database=BigDb;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<IdentityDbContext>
                (options => options.UseSqlServer(connection));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(o =>
                    {
                        o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                        o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                    })
                .AddGoogle(o =>
                    {
                        o.ClientId = "536082131203-00r94akb0omi6qdsuvs2u7rcfenhmi47.apps.googleusercontent.com";
                        o.ClientSecret = "xfIcqv3AItFZjWMvZa4qZttS";
                    })
                .AddCookie();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
