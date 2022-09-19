using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.Data
{
    public class ClientDbContext : DbContext
    {

        public DbSet<ClientData> Data { get; set; }

        public ClientDbContext( DbContextOptions<ClientDbContext> contextOptions ) : base(contextOptions) {   }
        public ClientDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
