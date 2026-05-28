
using AppMigration.Msql;
using AppMigration.Postgree;
using AppMigration.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppMigration
{
    class Program
    {

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Start Migration");
                Console.WriteLine("(C) 2026 ANDY THREE S");
                Console.WriteLine($"Process started at {DateTime.Now:dd-MMM-yyyy HH:mm:ss}");
                // Build configuration
                //var configuration = new ConfigurationBuilder()
                //    .SetBasePath(AppContext.BaseDirectory)
                //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //    .Build();
                //var connectionStringSQL = configuration.GetConnectionString("DBSQLConfig");
                //var connectionStringPG = configuration.GetConnectionString("DBPGConfig");

                var host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices((context, services) =>
                    {
                        var configuration = context.Configuration;

                        var sqlConn = configuration.GetConnectionString("DBSQLConfig");
                        var pgConn = configuration.GetConnectionString("DBPGConfig");

                        services.AddDbContextFactory<SqlDbContext>(options => options.UseSqlServer(sqlConn));

                        services.AddDbContextFactory<PgDbContext>(options => options.UseNpgsql(pgConn));
                        
                        services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
                        
                        services.AddScoped<ServiceJob>();
                    })
                    .Build();

                using var scope = host.Services.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ServiceJob>();

                await service.DoWork();

                Console.WriteLine("Migration done successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Migration failed!");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


    }
}