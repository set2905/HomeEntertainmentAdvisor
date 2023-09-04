using HomeEntertainmentAdvisor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace HomeEntertainmentAdvisor.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.Migrate();
        }


        public DbSet<Comment> Comments { get; set; }
        public DbSet<ReviewLike> ReviewLikes { get; set; }
        public DbSet<MediaPiece> MediaPieces { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<ReviewTagRelation> ReviewTagRelations { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<MediaPiece>().HasData(
            // new MediaPiece()
            // {
            //     Id=new Guid("706C2E99-6F6C-4472-81A5-43C56E11637C"),
            //     Name="TestMediaPiece"

            // }
            //);

            //builder.Entity<Rating>().HasData(
            // new Rating()
            // {
            //     Id=new Guid("bbd5efd3-bd3b-4bc9-8474-a6820a313483"),
            //     MediaPieceId=new Guid("706C2E99-6F6C-4472-81A5-43C56E11637C")

            // }
            //);
            //builder.Entity<Review>().HasData(
            // new Review()
            // {
            //     Id=new Guid("ddd4edd3-bd3b-4bc9-8474-a6820a313483"),
            //     RatingId=new Guid("bbd5efd3-bd3b-4bc9-8474-a6820a313483"),
            //     Content="test TestSearch 1234",
            //     Name="Test Review"
            // }
            //);
        }
    }
}