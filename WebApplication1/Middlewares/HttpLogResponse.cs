using Microsoft.Extensions.Logging;

namespace WebApplication1.Middlewares
{
    public class HttpLogResponse
    {
        private readonly RequestDelegate next;
        private readonly ILogger<HttpLogResponse> logger;

        public HttpLogResponse(RequestDelegate next, ILogger<HttpLogResponse>logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
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
            await next(context);

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
        }
    }
}
