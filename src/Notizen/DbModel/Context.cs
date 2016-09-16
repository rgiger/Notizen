using Microsoft.EntityFrameworkCore;

namespace Notizen.DbModel
{
    namespace Notizen
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<NotizDbModel> Notizen { get; set; }
        }
    }
}