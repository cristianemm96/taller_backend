using ApiStock.Interfaces;
using ApiStock.Models;
using ApiStock.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IStockService, StockService>();  
builder.Services.AddScoped<IRepuestoService, RepuestoService>();
builder.Services.AddScoped<IService<Accion>, AccionService>();
builder.Services.AddScoped<IService<Rol>, RolService>();
builder.Services.AddScoped<IService<Categoria>, CategoriaService>();
builder.Services.AddScoped<IEstanteriaService, EstanteriaService>();
builder.Services.AddScoped<IService<Usuario>, UsuarioService>();
builder.Services.AddScoped<OrdenTrabajoService>();
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StockContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFlutterWeb", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()                     
              .AllowAnyMethod();                    
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirFlutterWeb");
app.MapControllers();

app.Run();

