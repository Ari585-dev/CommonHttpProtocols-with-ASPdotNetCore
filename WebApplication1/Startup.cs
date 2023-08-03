using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
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

            services.AddControllers().AddJsonOptions(x=>
            x.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);//E s para no hacer ciclos cuando se relacionan tablas
            services.AddControllers();
            services.AddDbContext<DbConfig>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            services.AddEndpointsApiExplorer();

            services.AddTransient<IService, ServiceA>();

            services.AddTransient<TransientService>();
            services.AddScoped<ScopedService>();
            services.AddSingleton<SingletonService>();


            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

           app.Use(async (context, next) =>
            {
                // Entrada del middleware

                // 1. Creo un MemoryStream para poder manipular
                // y copiarme el cuerpo de la responsePetition.
                // Esto se hace porque el stream del cuerpo de la
                // responsePetition no tiene permisos de lectura.
                using var ms = new MemoryStream();

                // 2. Guardo la referencia del Stream donde se
                // escribe el cuerpo de la responsePetition
                var originalBody = context.Response.Body;

                // 3. Cambio el stream por defecto del cuerpo
                // de la responsePetition por el MemoryStream creado
                // para poder manipularlo
                context.Response.Body = ms;

                // 4. Esperamos a que el next middleware
                // devuelva la responsePetition.
                await next.Invoke();

                // Salida del middleware

                // 5. Nos movemos al principio del MemoryStream
                // Para copiar el cuerpo de la responsePetition
                ms.Seek(0, SeekOrigin.Begin);

                // 6. Leemos stream hasta el final y almacenamos
                // el cuerpo de la responsePetition obtenida
                var responsePetition = new StreamReader(ms).ReadToEnd();

                // 5. Nos volvemos a posicionar al principio
                // del MemoryStream para poder copiarlo al 
                // cuerpo original de la responsePetition
                ms.Seek(0, SeekOrigin.Begin);

                // 7. Copiamos el contenido del MemoryStream al
                // stream original del cuerpo de la responsePetition
                await ms.CopyToAsync(originalBody);

                // 8.Volvemos asignar el stream original al el cuerpo
                // de la responsePetition para que siga el flujo normal.
                context.Response.Body = originalBody;

                // 9. Escribimos en el log la responsePetition obtenida
                logger.LogInformation(responsePetition);

            });


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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
