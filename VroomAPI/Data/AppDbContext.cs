using Microsoft.EntityFrameworkCore;
using VroomAPI.Model;

namespace VroomAPI.Data {
    public class AppDbContext : DbContext{

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            
        }

        public DbSet<Tag> tags { get; set; }
        public DbSet<Moto> motos { get; set; }
    }
}
