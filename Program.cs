using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using VoluntariosConectadosRD.Services;
using VoluntariosConectadosRD.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews();

// Agregar soporte de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Agregar cliente HTTP
builder.Services.AddHttpClient();

// Agregar servicios de API
builder.Services.AddScoped<IBaseApiService, BaseApiService>();
builder.Services.AddScoped<IAccountApiService, AccountApiService>();
builder.Services.AddScoped<IVolunteerApiService, VolunteerApiService>();
builder.Services.AddScoped<IDashboardApiService, DashboardApiService>();

// Configuración JWT para autenticación
var jwtSettings = builder.Configuration.GetSection("JWT");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "JW|fK>j1?HAG7&>o{=lA6Sz,X|/Y%tZ),-f&:Vyz+hQfJu'~:A}%nz|KY_s9ymh");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Configuración para el manejo de archivos (imágenes)
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true; // Necesario para algunas operaciones de archivos
});

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor HSTS predeterminado es 30 días. Puede cambiar esto para escenarios de producción, vea https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Configuración para servir archivos estáticos (incluyendo imágenes)
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache de archivos estáticos por 1 semana (604800 segundos)
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=604800");
    }
});

app.UseRouting();

app.UseSession();

// Middleware personalizado para JWT
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();