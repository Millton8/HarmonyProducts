using HarmonyAPI.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HarmonyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddControllers();
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite("Data Source=harmony.db"));

            var app = builder.Build();

            
            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
