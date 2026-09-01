using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<LowkeysDrops.API.Data.LowkeysDropsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LowkeysDropsDB")));

builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IProductoRepository, LowkeysDrops.API.Repositories.ProductoRepository>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.ICategoriaRepository, LowkeysDrops.API.Repositories.CategoriaRepository>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IDropRepository, LowkeysDrops.API.Repositories.DropRepository>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IDireccionRepository, LowkeysDrops.API.Repositories.DireccionRepository>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IPedidoRepository, LowkeysDrops.API.Repositories.PedidoRepository>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IResenaRepository, LowkeysDrops.API.Repositories.ResenaRepository>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IConsultasRepository, LowkeysDrops.API.Repositories.ConsultasRepository>();

builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IProductoService, LowkeysDrops.API.Services.ProductoService>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.ICategoriaService, LowkeysDrops.API.Services.CategoriaService>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IDropService, LowkeysDrops.API.Services.DropService>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IDireccionService, LowkeysDrops.API.Services.DireccionService>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IPedidoService, LowkeysDrops.API.Services.PedidoService>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IResenaService, LowkeysDrops.API.Services.ResenaService>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IConsultasService, LowkeysDrops.API.Services.ConsultasService>();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKeyForDevelopmentOnly1234567890!";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IUsuarioRepository, LowkeysDrops.API.Repositories.UsuarioRepository>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IAuthService, LowkeysDrops.API.Services.AuthService>();
builder.Services.AddScoped<LowkeysDrops.API.Interfaces.IUsuarioService, LowkeysDrops.API.Services.UsuarioService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LowkeysDrops.API.Middleware.GlobalExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();







