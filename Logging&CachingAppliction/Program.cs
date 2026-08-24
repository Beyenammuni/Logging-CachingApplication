using FluentValidation;
using Logging_CachingApplication.Behaviors;
using Logging_CachingApplication.Common.Interfaces;
using Logging_CachingDomain.Models;
using Logging_CachingInfrastructure.Data;
using Logging_CachingInfrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using static System.Net.Mime.MediaTypeNames;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


var redisConnection =
    builder.Configuration.GetConnectionString("Redis");

var options = ConfigurationOptions.Parse(redisConnection!);

options.AbortOnConnectFail = false;

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(options)
);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAppDbContext, AppDbContext>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IProductRepository, productRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IAssamplyMarker).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(IAssamplyMarker).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
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
