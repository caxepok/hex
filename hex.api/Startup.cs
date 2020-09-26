using hex.api.Hubs;
using hex.api.Infrastructure;
using hex.api.Repositories;
using hex.api.Services;
using hex.common.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace hex
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
            services.AddControllersWithViews();
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            }).AddJsonProtocol();

            services.AddDbContext<HexDBContext>();

            services.AddHostedService<IContainerStateService, ContainerStateService>();
            services.AddTransient<IWarehouseService, WarehouseService>();
            services.AddTransient<IChannelWriterService<CTrack, string>, ChannelWriterService<CTrack, string>>();
            services.AddSingleton<IChannelResolver<CTrack, string>, ChannelResolver<CTrack, string>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ObserverHub>("/hub/observer");
            });
        }
    }
}
