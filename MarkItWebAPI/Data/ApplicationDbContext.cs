using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MarkItWebAPI.Data
{
    public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany<Todo>(u => u.Todos)
                .WithOne()
                .HasForeignKey(t => t.UserId);
                
        }
    }
}
