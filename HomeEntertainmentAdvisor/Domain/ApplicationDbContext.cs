using HomeEntertainmentAdvisor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
          //  Database.Migrate();
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<ReviewLike> ReviewLikes { get; set; }
        public DbSet<MediaPiece> MediaPieces { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<ReviewTagRelation> ReviewTagRelations { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}