using DL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SL_CQRS.CQRS.Restaurante.Commands.Add;
using FluentValidation.AspNetCore;
using SL_CQRS.CQRS.Restaurante.Queries.GetAll;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString =
 builder.Configuration.GetConnectionString("RESTAURANTEDBCORE")
     ?? throw new InvalidOperationException("Connection string"
     + "'DefaultConnection' not found.");

builder.Services.AddDbContext<RestaurantedbcoreContext>(options =>
options.UseSqlServer(connectionString));


builder.Services.AddScoped<BL.Restaurante>();

//Mediator handler
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetAllRestauranteQuery).Assembly));

// Registro de validadores automáticamente
builder.Services.AddValidatorsFromAssemblyContaining<AddRestauranteCommand>();

// Activa la validación automática
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
