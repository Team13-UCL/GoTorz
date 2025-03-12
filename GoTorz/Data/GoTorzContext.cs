using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoTorz.Model;

namespace GoTorz.Data
{
    public class GoTorzContext : DbContext
    {
        public GoTorzContext (DbContextOptions<GoTorzContext> options)
            : base(options)
        {
        }

        public DbSet<Package> Package { get; set; } = default!;
    }
}
