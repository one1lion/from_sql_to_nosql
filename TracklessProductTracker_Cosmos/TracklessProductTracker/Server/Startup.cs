using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TracklessProductTracker.Database;
using System;
using Microsoft.Extensions.Logging;

namespace TracklessProductTracker.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TracklessProductContext>(options
                =>
                {
                    options.UseCosmos(
                        accountEndpoint: Configuration.GetValue<string>("AzureCosmos:AccountEndpoint"),
                        accountKey: Configuration.GetValue<string>("AzureCosmos:AccountKey"),
                        databaseName: Configuration["AzureCosmos:DatabaseName"]);
#if DEBUG
                    options.EnableSensitiveDataLogging();
#endif
                });

            // For use with EF Core Cosmos 5.0
            //   Example from https://docs.microsoft.com/en-us/ef/core/providers/cosmos/?tabs=dotnet-core-cli
            //services.AddDbContext<TracklessProductContext>(options 
            //    => options.UseCosmos(
            //        $"AccountEndpoint={Configuration.GetValue<string>("AzureCosmos:AccountEndpoint")};AccountKey={Configuration.GetValue<string>("AzureCosmos:AccountKey")}",
            //        databaseName: Configuration["AzureCosmos:DatabaseName"]),
            //        options =>
            //        {
            //            options.ConnectionMode(ConnectionMode.Gateway);
            //            options.WebProxy(new WebProxy());
            //            options.LimitToEndpoint();
            //            options.Region(Regions.AustraliaCentral);
            //            options.GatewayModeMaxConnectionLimit(32);
            //            options.MaxRequestsPerTcpConnection(8);
            //            options.MaxTcpConnectionsPerEndpoint(16);
            //            options.IdleTcpConnectionTimeout(TimeSpan.FromMinutes(1));
            //            options.OpenTcpConnectionTimeout(TimeSpan.FromMinutes(1));
            //            options.RequestTimeout(TimeSpan.FromMinutes(1));
            //        });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            TracklessProductContext db,
            ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            EnsureDatabaseCreated(db, env, logger);

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

        void EnsureDatabaseCreated(TracklessProductContext context, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            try
            {
                if (Configuration.GetValue<bool>("RecreateDatabase") && env.IsDevelopment()) { context.Database.EnsureDeleted(); }
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }
}
