using AuthService.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialisation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var database = serviceScope.ServiceProvider.GetService<AuthDatabaseContext>().Database;
                var migrations = database.GetMigrations();
                if (migrations == null)
                {
                    database.Migrate();
                }
            }
        }
    }
}
