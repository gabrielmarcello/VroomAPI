using Microsoft.EntityFrameworkCore;
using VroomAPI.Model;

namespace VroomAPI.Data {
    public class AppDbContext : DbContext{

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            
        }

        public DbSet<Tag> tags { get; set; }
        public DbSet<Moto> motos { get; set; }
        public DbSet<EventoIot> eventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventoIot>()
                .Property(e => e.LedOn)
                .HasConversion<int>();

            // Configure the primary key for EventoIot
            modelBuilder.Entity<EventoIot>()
                .HasKey(e => e.Id);
        }
    }
}
