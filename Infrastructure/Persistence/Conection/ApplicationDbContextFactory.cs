using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IO;

namespace Infrastructure.Persistence.Conection
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "API");
            Console.WriteLine($"Using base path: {basePath}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
                        sqlServerOptions.EnableRetryOnFailure());

            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
