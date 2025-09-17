using Microsoft.EntityFrameworkCore;

namespace VroomAPI.Data {
    public class AppDbContext : DbContext{

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            
        }
    }
}
