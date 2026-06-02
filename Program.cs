
using Microsoft.EntityFrameworkCore;
using TodoApi.DataAccess;

namespace TodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("ApplicationDBContext") ?? throw new InvalidOperationException("Connection string ApplicationDBContext not found!");

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "astroToDo.api", Version = "v1" });
            });
            builder.Services.AddDbContext<TodoDbContext>(o => o.UseSqlServer(connectionString));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(o => { o.SwaggerEndpoint("/swagger/v1/swagger.json", "astroToDo api v1"); });
            }

            app.Map("/hello", () => $"Just testing...");

            app.Run();
        }
    }
}
