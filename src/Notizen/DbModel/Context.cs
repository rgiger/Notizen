using Microsoft.EntityFrameworkCore;

namespace Notizen.DbModel
{
    namespace Notizen
    {
        public class Context : DbContext
        {
            public Context(DbContextOptions<Context> options)
                : base(options)
            {
            }

            public DbSet<NotizDbModel> Notizen { get; set; }
            
        }
    }
}
