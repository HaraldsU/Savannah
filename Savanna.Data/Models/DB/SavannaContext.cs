using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Savanna.Data.Models.DB
{
    public class SavannaContext : DbContext
    {
        public DbSet<GameStateModel> GameState { get; set; }

        public SavannaContext(DbContextOptions options) : base(options)
        {

        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SavannaContext>
    {
        public SavannaContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                         .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Savanna.WebAPI/appsettings.json")
                                                                         .Build();
            var builder = new DbContextOptionsBuilder<SavannaContext>();
            var connectionString = configuration.GetConnectionString("DatabaseConnection");
            builder.UseSqlServer(connectionString);
            return new SavannaContext(builder.Options);
        }
    }
}
