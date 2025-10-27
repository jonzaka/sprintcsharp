using InvestmentApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core InMemory
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("InvestmentsDB"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed InMemory
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

// Swagger SEMPRE habilitado
app.UseSwagger();
app.UseSwaggerUI();

// Controllers
app.MapControllers();

// Redireciona raiz para Swagger (evita 404 em "/")
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
