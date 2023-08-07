using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using WebApplication1.Filters;
using WebApplication1.Middlewares;
using WebApplication1.Services;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            }).AddJsonOptions(x=>
            x.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);//E s para no hacer ciclos cuando se relacionan tablas
            services.AddControllers();
            services.AddDbContext<DbConfig>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            services.AddEndpointsApiExplorer();

            services.AddTransient<IService, ServiceA>();

            services.AddTransient<TransientService>();
            services.AddScoped<ScopedService>();
            services.AddSingleton<SingletonService>();
            services.AddTransient<FilterAction>();
            services.AddHostedService<FileWritter>();

            services.AddResponseCaching();


            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            app.UseMiddleware<HttpLogResponse>();

            app.Map("/ended", app => { 

                 app.Run(async context =>
                 {
                     await context.Response.WriteAsync("Ejecución finalizada");
                 });
                });

         

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
