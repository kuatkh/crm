using CRM.DataModel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DataModel.Data
{
    public class CrmDbContext : IdentityDbContext<CrmUsers, CrmRoles, long, CrmUserClaims, CrmUserRoles,
        CrmUserLogins, CrmRoleClaims, CrmUserTokens>
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options)
            : base(options)
        {
        }

        public DbSet<DictDepartments> DictDepartments { get; set; }
        public DbSet<DictEnterpirses> DictEnterpirses { get; set; }
        public DbSet<DictPositions> DictPositions { get; set; }
        public DbSet<DictGenders> DictGenders { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CrmUsers>(entity =>
            {
                entity.HasOne(c => c.DictGender)
                .WithMany(d => d.Users)
                .HasForeignKey(e => e.DictGendersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmUsers_DictGenders");

                entity.HasOne(c => c.DictEnterpirse)
                .WithMany(d => d.Users)
                .HasForeignKey(e => e.DictEnterprisesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmUsers_DictEnterpirses");

                entity.HasOne(c => c.DictDepartment)
                .WithMany(d => d.Users)
                .HasForeignKey(e => e.DictDepartmentsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmUsers_DictDepartments");

                entity.HasOne(c => c.DictPosition)
                .WithMany(d => d.Users)
                .HasForeignKey(e => e.DictPositionsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmUsers_DictPositions");

                entity.ToTable("CrmUsers");
            });

            builder.Entity<CrmUserClaims>(entity =>
            {
                entity.ToTable("CrmUserClaims");
            });

            builder.Entity<CrmUserLogins>(entity =>
            {
                entity.ToTable("CrmUserLogins");
            });

            builder.Entity<CrmUserTokens>(entity =>
            {
                entity.ToTable("CrmUserTokens");
            });

            builder.Entity<CrmRoles>(entity =>
            {
                entity.ToTable("CrmRoles");
            });

            builder.Entity<CrmRoleClaims>(entity =>
            {
                entity.ToTable("CrmRoleClaims");
            });

            builder.Entity<CrmUserRoles>(entity =>
            {
                entity.ToTable("CrmUserRoles");
            });

            builder.Entity<DictGenders>(entity =>
            {
                entity.ToTable("DictGenders");
            });

            builder.Entity<DictEnterpirses>(entity =>
            {
                entity.ToTable("DictEnterpirses");
            });

            builder.Entity<DictDepartments>(entity =>
            {
                entity.HasOne(c => c.DictEnterpirse)
                .WithMany(d => d.DictDepartments)
                .HasForeignKey(e => e.DictEnterprisesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DictDepartments_DictEnterpirses");

                entity.ToTable("DictDepartments");
            });

            builder.Entity<DictPositions>(entity =>
            {
                entity.HasOne(c => c.DictEnterpirse)
                .WithMany(d => d.DictPositions)
                .HasForeignKey(e => e.DictEnterprisesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DictPositions_DictEnterpirses");

                entity.ToTable("DictPositions");
            });

            builder.Entity<Notifications>(entity =>
            {
                entity.HasOne(c => c.NotificationReceiver)
                .WithMany(d => d.Notifications)
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_NotificationReceiver_CrmUsers");

                entity.ToTable("Notifications");
            });

        }
    }
}
