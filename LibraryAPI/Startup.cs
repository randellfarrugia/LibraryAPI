using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using LibraryAPI.Services;
using LibraryAPI.Models;
using LibraryAPI.Interfaces;
using Newtonsoft.Json;
using LibraryAPI.DataHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Context;

namespace LibraryAPI
{
    public class Startup
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add the book service, which will be used to retrieve and manipulate books
            //services.AddScoped<IBookService, BookService>();
            services.AddScoped<ILibrary, Movie>();
            services.AddSingleton(new Queries(Configuration, new SQLConnection(Configuration.GetConnectionString("LibraryDB"))));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton(Log.Logger); //not needed
            //services.AddHttpContextAccessor();

            // Add the API and configure it to use JSON formatting and dependency injection
            services.AddControllers()
         .AddJsonOptions(options =>
         {
             options.JsonSerializerOptions.IgnoreNullValues = true;
             options.JsonSerializerOptions.PropertyNamingPolicy = null;
         });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSerilogRequestLogging();



            string Date = DateTime.Now.ToString("yyyy-MM-dd");

            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Information()
              .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
              .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)
              .Enrich.FromLogContext()
              .WriteTo.File(@"Logs\" + Date + "\\log-.txt", rollingInterval: RollingInterval.Hour)             //LIVE
            //.WriteTo.File(@"../../../logs\" + Date + "\\log-.txt", rollingInterval: RollingInterval.Hour)   //TESTING
              .CreateLogger();
            Log.Information("Application Starting");
            
        }
    }
}
