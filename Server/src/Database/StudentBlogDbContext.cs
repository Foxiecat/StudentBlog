using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using src.Features.Comments;
using src.Features.Posts;
using src.Features.Users;

namespace src.Database;

public class StudentBlogDbContext(DbContextOptions<StudentBlogDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Post> Post { get; set; }
    public DbSet<Comment> Comment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(user => user.Posts)
                .WithOne(post => post.User)
                .HasForeignKey(post => post.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(user => user.UserName).IsUnique();
            entity.HasIndex(user => user.Email).IsUnique();
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasMany(post => post.Comments)
                .WithOne(comment => comment.Post)
                .HasForeignKey(comment => comment.PostId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(comment => comment.Post)
                .WithMany(post => post.Comments)
                .HasForeignKey(comment => comment.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(comment => comment.User)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}