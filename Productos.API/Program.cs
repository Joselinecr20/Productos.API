using Productos.API.Data;
using Productos.API.Data.Seed;
using Productos.API.Interfaces;
using Productos.API.Models;
using Productos.API.Repositorios;
using Productos.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configurar la cadena de conexión a la base de datos

var sqlServerConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(sqlServerConnection)
);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    //Reglas de contraseña: 8 caracteres, al menos un carácter especial y una letra mayúscula
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;

    //Bloquear usuarios después de 5 intentos fallidos
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// inyeccion de dependencias
builder.Services.AddScoped<TokenServices>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddControllers()
//evitando referencia circular en las respuestas json
.AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
 });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configurar la autenticación JWT: es decir las características del token que se va a generar o permitir
var jwrSection = builder.Configuration.GetSection("Jwt");
var signingKey = jwrSection.GetValue<string>("Key");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwrSection.GetValue<string>("Issuer"),
        ValidAudience = jwrSection.GetValue<string>("Audience"),
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey)),
        ClockSkew = TimeSpan.Zero // Eliminar el tiempo de tolerancia para la expiración del token
    };
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    await DataSeeder.SeedAsync(scope.ServiceProvider);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // quien es el usuario.
app.UseAuthorization();  // que puede hacer el usuario.

app.MapControllers();

app.Run();
