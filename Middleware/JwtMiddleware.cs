using VoluntariosConectadosRD.Services;

namespace VoluntariosConectadosRD.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IBaseApiService baseApiService)
        {
            // Obtener token de la sesión
            var token = context.Session.GetString("JWTToken");
            
            if (!string.IsNullOrEmpty(token))
            {
                // Configurar el token en el servicio base de la API
                baseApiService.SetAuthToken(token);
            }

            await _next(context);
        }
    }
}