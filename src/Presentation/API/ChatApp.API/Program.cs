using ChatApp.Application;
using ChatApp.Persistence;
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
        builder.Services.AddSwaggerGen();

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

        app.UseCors("CorsPolicy");
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
