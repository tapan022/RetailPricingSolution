using Microsoft.EntityFrameworkCore;
using RetailPricing.API;

var builder = WebApplication.CreateBuilder(args);

// DB Context
builder.Services.AddDbContext<RetailDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// CORS SERVICE
builder.Services.AddCors(options =>
{
    options.AddPolicy("all", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// CORS MIDDLEWARE
app.UseCors("all");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();