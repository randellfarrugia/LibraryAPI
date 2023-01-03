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
        }
    }
}
