using Microsoft.EntityFrameworkCore;

namespace FootBallTournament.Models
{
    public class ApplicationDbContext : DbContext
    {
          public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){
          
  
        }
           protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //  modelBuilder.Entity<ApplicationUser>()
            //    .HasKey(u => u.email);
            modelBuilder.Entity<Admin>()
            .Property(u=>u.Id)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Teams>()
            .Property(u=>u.Id)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Players>()
            .Property(u=>u.Id)
            .ValueGeneratedOnAdd();
               modelBuilder.Entity<Mappings>()
            .Property(u=>u.Id)
            .ValueGeneratedOnAdd();
             modelBuilder.Entity<viewer>()
            .Property(u=>u.Id)
            .ValueGeneratedOnAdd();
             modelBuilder.Entity<Bookings>()
            .Property(u=>u.Id)
            .ValueGeneratedOnAdd();
        }
        public DbSet<Admin> Admin{get;set;}
        public DbSet<Teams> Teams{get;set;}
         public DbSet<viewer> viewers{get;set;}
         public DbSet<Bookings> bookings{get;set;}
        public DbSet<Players> Players{get;set;}
        public DbSet<Mappings> Mappings{get;set;}

    }
}