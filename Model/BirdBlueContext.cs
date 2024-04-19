using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Nova_pasta.Model;

public partial class BirdBlueContext : DbContext
{
    public BirdBlueContext()
    {
    }

    public BirdBlueContext(DbContextOptions<BirdBlueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BirdBlueet> BirdBlueets { get; set; }

    public virtual DbSet<BirdUser> BirdUsers { get; set; }

    public virtual DbSet<Follower> Followers { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SNCCHLAB04F17;Initial Catalog=BirdBlue;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BirdBlueet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BirdBlue__3213E83F712608AD");

            entity.ToTable("BirdBlueet");

            entity.HasIndex(e => new { e.PostDate, e.UserId }, "timeline_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(140)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.Likes).HasColumnName("likes");
            entity.Property(e => e.PostDate)
                .HasColumnType("datetime")
                .HasColumnName("postDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.BirdBlueets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BirdBluee__userI__398D8EEE");
        });

        modelBuilder.Entity<BirdUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BirdUser__3213E83F5C6B83DA");

            entity.ToTable("BirdUser");

            entity.HasIndex(e => new { e.Username, e.Email, e.Pass }, "login_index");

            entity.HasIndex(e => e.Username, "username_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Pass)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("pass");
            entity.Property(e => e.Username)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Follower>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Follower__3213E83FC717E3B1");

            entity.HasIndex(e => new { e.UserIdFollowing, e.UserIdFollowed }, "follower_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserIdFollowed).HasColumnName("userIdFollowed");
            entity.Property(e => e.UserIdFollowing).HasColumnName("userIdFollowing");

            entity.HasOne(d => d.UserIdFollowedNavigation).WithMany(p => p.FollowerUserIdFollowedNavigations)
                .HasForeignKey(d => d.UserIdFollowed)
                .HasConstraintName("FK__Followers__userI__3D5E1FD2");

            entity.HasOne(d => d.UserIdFollowingNavigation).WithMany(p => p.FollowerUserIdFollowingNavigations)
                .HasForeignKey(d => d.UserIdFollowing)
                .HasConstraintName("FK__Followers__userI__3C69FB99");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Likes__3213E83FDAEFF603");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("LikeOnLike");
                    tb.HasTrigger("dislikeOndislike");
                });

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PostId).HasColumnName("postId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Post).WithMany(p => p.LikesNavigation)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Likes__postId__412EB0B6");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Likes__userId__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
