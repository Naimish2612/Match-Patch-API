using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Home> Homes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .HasKey(key => new {key.liker_id,key.likee_id });
            modelBuilder.Entity<Like>()
                .HasOne(u => u.likee)
                .WithMany(u => u.likers)
                .HasForeignKey(u => u.likee_id)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Like>()
                .HasOne(u => u.liker)
                .WithMany(u => u.likees)
                .HasForeignKey(u => u.liker_id)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Message>()
                .HasOne(u=>u.sender)
                .WithMany(u => u.messages_send)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.recipient)
                .WithMany(u => u.messages_received)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}