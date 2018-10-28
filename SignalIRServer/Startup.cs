using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalIRServer.Crone;
using SignalIRServer.Hubs;
using SignalIRServer.Model;
using SignalIRServer.MongoDbContexts;
using SignalIRServer.Repository;
using SignalIRServer.Scheduling;
using SignalIRServer.Services;

namespace SignalIRServer
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });
           
            services.AddSignalR();
           // services.Add(new ServiceDescriptor(typeof(ILightsService), typeof(LightsService), ServiceLifetime.Singleton));
            services.AddSingleton<ILightsService, LightsService>();
            services.AddTransient<ILightBulbContext, LightBulbContext>();
            services.AddTransient<ILightBulbRepository, LightBulbRepository>();
            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IScheduledTask, NewDayTask>();
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.UseCors("AllowAny");
            app.UseSignalR(routes =>
            {
                routes.MapHub<Broadcaster>("/LightApp");
            });
          
            app.UseMvc();
        }
    }
}
