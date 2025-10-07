using Guardia.Aplicacion.Servicios;
using Guardia.Dominio.Repositorios;
using Guardia.Infraestructura.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories
builder.Services.AddScoped<IRepositorioPaciente, RepositorioPacienteEnMemoria>();
builder.Services.AddScoped<IRepositorioIngreso, RepositorioIngresoEnMemoria>();
builder.Services.AddScoped<IRepositorioEnfermero, RepositorioEnfermeroEnMemoria>();

// Register services
builder.Services.AddScoped<IngresoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
