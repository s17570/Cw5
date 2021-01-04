using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5
{
    public class Program
    {
        private static string dataSource = "db-mssql";
        private static string initialCatalog = "s17570";
        private static bool isIntegratedSecurity = true;

        public static string GetConnectionString()
        {
            var connectionBuilder = new SqlConnectionStringBuilder();
            connectionBuilder.DataSource = dataSource;
            connectionBuilder.InitialCatalog = initialCatalog;
            connectionBuilder.IntegratedSecurity = isIntegratedSecurity;

            return connectionBuilder.ConnectionString;
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
