using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Add DbSet properties for your entities here
        // public DbSet<YourEntity> YourEntities { get; set; }
    }
    {
    }
}
