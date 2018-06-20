using Microsoft.EntityFrameworkCore;

namespace EWIP.Notification.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<NotificationDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for MyProjectDbContext */
            dbContextOptions.UseSqlServer(connectionString, opt => opt.UseRowNumberForPaging());
        }
    }
}
