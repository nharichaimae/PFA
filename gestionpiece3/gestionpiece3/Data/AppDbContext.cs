using gestionpiece3.Models;

using Microsoft.EntityFrameworkCore;

namespace gestionpiece3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Piece> piece{ get; set; }
        public DbSet<equipementModel> equipement { get; set; }
    }
} 