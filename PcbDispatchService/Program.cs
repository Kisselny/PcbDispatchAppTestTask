using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using PcbDispatchService.Controllers;
using PcbDispatchService.Dal;
using PcbDispatchService.Domain.Logic;
using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Services;

namespace PcbDispatchService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(c =>
        {
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, 
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        });

        builder.Services.AddControllers();

        builder.Services.AddScoped<IMyCustomLoggerService, MyCustomLoggerService>();
        builder.Services.AddScoped<IQualityControlService, QualityControlService>();
        builder.Services.AddScoped<IPcbFactory, PcbFactory>();
        builder.Services.AddScoped<IPcbService, PcbService>();
        builder.Services.AddScoped<IBusinessRules, BusinessRules>();
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        builder.Services.AddScoped<IComponentTypesRepository, ComponentTypesRepository>();
        builder.Services.AddScoped<IPcbRepository, PcbRepository>();


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
    }
}