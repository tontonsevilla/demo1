using AspDotNetCoreDemo.Infrastrcuture.Data;
using AspDotNetCoreDemo.Infrastructure.Services;
using AspDotNetCoreDemo.WebInfrastructure.Filters;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AspDotNetCoreDemo
{
    public class Startup
    {
        private static string databaseConnectionString = string.Empty;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            databaseConnectionString = Configuration.GetConnectionString("AspDotNetCoreDemoSQLiteConnection");

            ConfigureIdentity(services);

            services.AddHangfire(config => config.UseSQLiteStorage(databaseConnectionString));

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            ConfigureCustomServices(services);
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<AspDotNetCoreDemoDatabaseContext>(efOptions => {
                efOptions.UseSqlite(databaseConnectionString, sqliteOptions => {
                    sqliteOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                });
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AspDotNetCoreDemoDatabaseContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                if (File.Exists("AspDotNetCoreDemo.db"))
                {
                    File.Delete("AspDotNetCoreDemo.db");
                }

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }


            ConfigureDatabase();

            //app.UsePathBase("/");

            //app.UseHttpsRedirection();
            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            ConfigureHangfire(app, env);

            app.UseMvc(routes =>
            {
                routes
                    .MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureHangfire(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var options = new SQLiteStorageOptions();
            GlobalConfiguration.Configuration.UseSQLiteStorage(databaseConnectionString, options);

            var backgroundJobServerOptions = new BackgroundJobServerOptions
            {
                WorkerCount = Environment.ProcessorCount * 4

            };

            app.UseHangfireServer(backgroundJobServerOptions);
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireDashboardAuthorizationFilter(env) }
            });

            // RecurringJob.AddOrUpdate(() => Debug.WriteLine($"Done ! {DateTime.Now}"), Cron.Minutely);
        }

        private void ConfigureDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AspDotNetCoreDemoDatabaseContext>();
            optionsBuilder.UseSqlite(databaseConnectionString);

            using (var dbContext = new AspDotNetCoreDemoDatabaseContext(optionsBuilder.Options))
            {
                dbContext.Database.EnsureCreated();

                if (!dbContext.Blogs.Any())
                {
                    dbContext.Blogs.AddRange(new Blog[]
                        {
                             new Blog{ BlogId=1, Title="Blog 1", SubTitle="Blog 1 subtitle" },
                             new Blog{ BlogId=2, Title="Blog 2", SubTitle="Blog 2 subtitle" },
                             new Blog{ BlogId=3, Title="Blog 3", SubTitle="Blog 3 subtitle" }
                        });
                    dbContext.SaveChanges();
                }
            }
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddTransient<SigninManagerService>()
                    .AddTransient<UserManagerService>();
        }
    }
}
