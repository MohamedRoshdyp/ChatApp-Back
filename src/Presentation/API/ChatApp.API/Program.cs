using ChatApp.Application;
using ChatApp.Persistence;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ChatApp.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(s =>
        {
            var securitySchema = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Description = "JWT Auth Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,  
                Scheme = "bearer",
                Reference = new OpenApiReference()
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };
            s.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
            s.AddSecurityRequirement(securityRequirement);

            //configure xml 
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            s.IncludeXmlComments(xmlPath);
        });

        //Configure External Project(dll)
        builder.Services.ConfigureApplicationService();
        builder.Services.ConfigurePersistenceServices(builder.Configuration);


        //Enable Cors
        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", p =>
            {
                p.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
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
        app.UseStaticFiles();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        ChatApp.Persistence.DependancyInjection.ConfigMiddleware(app);
        app.Run();
    }
}
