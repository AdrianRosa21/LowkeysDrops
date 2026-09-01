using Microsoft.EntityFrameworkCore;
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LowkeysDrops.API.Middleware.GlobalExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
