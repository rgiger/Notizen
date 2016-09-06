using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notizen.DbModel;

namespace Notizen
{
    using Microsoft.EntityFrameworkCore;

    namespace Notizen
    {
        public class Context : DbContext
        {
            public Context(DbContextOptions<Context> options)
                : base(options)
            {
            }

            public DbSet<Notiz> Notizen { get; set; }
            
        }
    }
}
