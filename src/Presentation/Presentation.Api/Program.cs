using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Core.Application.Interfaces;
using Infrastructure.Persistence.Repositories;
using MediatR;
using FluentValidation;
using System.Reflection;
using Core.Application.Queries;
using Core.Application.Commands;
using Core.Application.Validators;
using AutoMapper;
using Presentation.Api.Mapping;
namespace Mc2.CrudTest.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
                builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>();

            builder.Services.AddMediatR(typeof(CreateCustomerCommandHandler).Assembly);
            builder.Services.AddMediatR(typeof(GetAllCustomersQueryHandler).Assembly);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:7046") // Blazor client HTTPS URL
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    options.RoutePrefix = "swagger"; 
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("AllowSpecificOrigins");



            app.UseAuthorization();

            app.MapControllers();

   

            app.Run();
        }
    }
}
