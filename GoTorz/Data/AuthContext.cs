using System;
using System.Collections.Generic;
using GoTorz.Model;
using Microsoft.EntityFrameworkCore;

namespace GoTorz.Data
{
    public partial class AuthContext : DbContext
    {
        public AuthContext()
        {
        }

        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID); // Set UserID as the Primary Key

                entity.ToTable("User");

                entity.Property(e => e.UserID)
                .ValueGeneratedOnAdd(); // ID is Auto Incremented when making a new User
                entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
