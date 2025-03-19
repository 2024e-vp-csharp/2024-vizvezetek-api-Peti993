using Microsoft.EntityFrameworkCore;
using Vizvezetek.API.Models;

namespace Vizvezetek.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("VizvezetekDb");

            // Add services to the container.

            builder.Services.AddDbContext<vizvezetekContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            builder.Services.AddControllers();
            builder.Services.AddLogging();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var context = scope.ServiceProvider.GetRequiredService<vizvezetekContext>();

                try
                {
                    // Próbáljuk elérni az adatbázist
                    if (context.Database.CanConnect())
                    {
                        logger.LogInformation("✅ Sikeres adatbázis kapcsolat!");
                    }
                    else
                    {
                        logger.LogWarning("⚠️ Nem sikerült kapcsolódni az adatbázishoz!");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "❌ Hiba történt az adatbázis kapcsolat közben!");
                }
            }

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}