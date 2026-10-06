using Microsoft.EntityFrameworkCore;
using MAN1.Models;

namespace MAN1.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Lesson> Lessons { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
