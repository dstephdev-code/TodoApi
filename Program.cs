using Microsoft.EntityFrameworkCore;
using TodoApi.DataAccess;
using TodoApi.DataAccess.Repositories;
using TodoApi.Middleware;
using TodoApi.Services;
using Serilog;

namespace TodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .CreateLogger();

            try
            {
                Log.Information("Application is starting...");

                var builder = WebApplication.CreateBuilder(args);
                var connectionString = builder.Configuration.GetConnectionString("ApplicationDBContext") ?? throw new InvalidOperationException("Connection string ApplicationDBContext not found!");

                builder.Host.UseSerilog();
                builder.Services.AddControllers();
                builder.Services.AddSwaggerGen(o =>
                {
                    o.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "astroToDo.api", Version = "v1" });
                });
                builder.Services.AddDbContext<TodoDbContext>(o => o.UseSqlServer(connectionString));
                builder.Services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
                builder.Services.AddScoped<ITodoTasksService, TodoTasksService>();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(o => { o.SwaggerEndpoint("/swagger/v1/swagger.json", "astroToDo api v1"); });
                }

                app.UseMiddleware<RequestLoggingMiddleware>();
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application was shut down with fatal error on start!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
