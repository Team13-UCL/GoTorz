using System;
using System.Collections.Generic;
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
                entity.HasKey(e => e.Email);

                entity.ToTable("User");

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
