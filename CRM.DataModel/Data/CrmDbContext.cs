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

        public DbSet<CrmEmployees> CrmEmployees { get; set; }
        public DbSet<CrmEmployeesWorkPlans> CrmEmployeesWorkPlans { get; set; }
        public DbSet<CrmHolidays> CrmHolidays { get; set; }
        public DbSet<CrmPatients> CrmPatients { get; set; }
        public DbSet<CrmPatientsAppointments> CrmPatientsAppointments { get; set; }
        public DbSet<CrmPatientsAppointmentsFiles> CrmPatientsAppointmentsFiles { get; set; }
        public DbSet<CrmPatientsAppointmentsServices> CrmPatientsAppointmentsServices { get; set; }
        public DbSet<CrmPatientsIntolerances> CrmPatientsIntolerances { get; set; }
        public DbSet<CrmPatientsLoyaltyPrograms> CrmPatientsLoyaltyPrograms { get; set; }
        public DbSet<DictCities> DictCities { get; set; }
        public DbSet<DictCountries> DictCountries { get; set; }
        public DbSet<DictDepartments> DictDepartments { get; set; }
        public DbSet<DictEnterprises> DictEnterprises { get; set; }
        public DbSet<DictGenders> DictGenders { get; set; }
        public DbSet<DictIntolerances> DictIntolerances { get; set; }
        public DbSet<DictLoyaltyPrograms> DictLoyaltyPrograms { get; set; }
        public DbSet<DictPositions> DictPositions { get; set; }
        public DbSet<DictServices> DictServices { get; set; }
        public DbSet<DictStatuses> DictStatuses { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CrmUsers>(entity =>
            {
                entity.HasOne(c => c.CrmEmployee)
                .WithOne(d => d.CrmUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmUsers_CrmEmployees");

                entity.HasOne(c => c.CrmPatient)
                .WithOne(d => d.CrmUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmUsers_CrmPatients");

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

            builder.Entity<CrmEmployees>(entity =>
            {
                entity.HasOne(c => c.DictGender)
                .WithMany(d => d.CrmEmployees)
                .HasForeignKey(e => e.DictGendersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmEmployees_DictGenders");

                entity.HasOne(c => c.DictEnterprise)
                .WithMany(d => d.CrmEmployees)
                .HasForeignKey(e => e.DictEnterprisesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmEmployees_DictEnterprises");

                entity.HasOne(c => c.DictDepartment)
                .WithMany(d => d.CrmEmployees)
                .HasForeignKey(e => e.DictDepartmentsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmEmployees_DictDepartments");

                entity.HasOne(c => c.DictPosition)
                .WithMany(d => d.CrmEmployees)
                .HasForeignKey(e => e.DictPositionsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmEmployees_DictPositions");

                entity.HasOne(c => c.DictCity)
                .WithMany(d => d.CrmEmployees)
                .HasForeignKey(e => e.DictCitiesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmEmployees_DictCities");

                entity.ToTable("CrmEmployees");
            });

            builder.Entity<CrmEmployeesWorkPlans>(entity =>
            {
                entity.HasOne(c => c.Author)
                .WithMany(d => d.CrmSystemSettingsAuthors)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmSystemSettings_Authors_CrmUsers");

                entity.HasOne(c => c.Editor)
                .WithMany(d => d.CrmSystemSettingsEditors)
                .HasForeignKey(e => e.EditorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmSystemSettings_Editor_CrmUsers");

                entity.ToTable("CrmEmployeesWorkPlans");
            });

            builder.Entity<CrmPatients>(entity =>
            {
                entity.HasOne(c => c.DictGender)
                .WithMany(d => d.CrmPatients)
                .HasForeignKey(e => e.DictGendersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatients_DictGenders");

                entity.HasOne(c => c.Author)
                .WithMany(d => d.CrmPatientsAuthors)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatients_Author_CrmUsers");

                entity.HasOne(c => c.Editor)
                .WithMany(d => d.CrmPatientsEditors)
                .HasForeignKey(e => e.EditorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatients_Editor_CrmUsers");

                entity.HasOne(c => c.DictCity)
                .WithMany(d => d.CrmPatients)
                .HasForeignKey(e => e.DictCitiesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatients_DictCities");

                entity.ToTable("CrmPatients");
            });

            builder.Entity<CrmPatientsAppointments>(entity =>
            {
                entity.HasOne(c => c.CrmPatient)
                .WithMany(d => d.CrmPatientsAppointments)
                .HasForeignKey(e => e.CrmPatientsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsAppointments_CrmPatients");

                entity.HasOne(c => c.ToCrmEmployee)
                .WithMany(d => d.CrmPatientsAppointments)
                .HasForeignKey(e => e.ToCrmEmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsAppointments_CrmEmployees");

                entity.HasOne(c => c.DictStatus)
                .WithMany(d => d.CrmPatientsAppointments)
                .HasForeignKey(e => e.DictStatusesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsAppointments_DictStatuses");

                entity.ToTable("CrmPatientsAppointments");
            });

            builder.Entity<CrmPatientsAppointmentsFiles>(entity =>
            {
                entity.HasOne(c => c.CrmPatientsAppointment)
                .WithMany(d => d.CrmPatientsAppointmentsFiles)
                .HasForeignKey(e => e.CrmPatientsAppointmentsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsAppointmentsFiles_CrmPatientsAppointments");

                entity.ToTable("CrmPatientsAppointmentsFiles");
            });

            builder.Entity<CrmPatientsAppointmentsServices>(entity =>
            {
                entity.HasOne(c => c.CrmPatientsAppointment)
                .WithMany(d => d.CrmPatientsAppointmentsServices)
                .HasForeignKey(e => e.CrmPatientsAppointmentsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsAppointmentsServices_CrmPatientsAppointments");

                entity.HasOne(c => c.DictService)
                .WithMany(d => d.CrmPatientsAppointmentsServices)
                .HasForeignKey(e => e.DictServicesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsAppointmentsServices_DictServices");

                entity.ToTable("CrmPatientsAppointmentsServices");
            });

            builder.Entity<CrmPatientsIntolerances>(entity =>
            {
                entity.HasOne(c => c.CrmPatient)
                .WithMany(d => d.CrmPatientsIntolerances)
                .HasForeignKey(e => e.CrmPatientsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsIntolerances_CrmPatients");

                entity.HasOne(c => c.DictIntolerance)
                .WithMany(d => d.CrmPatientsIntolerances)
                .HasForeignKey(e => e.DictIntolerancesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsIntolerances_DictIntolerances");

                entity.ToTable("CrmPatientsIntolerances");
            });

            builder.Entity<CrmPatientsLoyaltyPrograms>(entity =>
            {
                entity.HasOne(c => c.CrmPatient)
                .WithMany(d => d.CrmPatientsLoyaltyPrograms)
                .HasForeignKey(e => e.CrmPatientsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsLoyaltyPrograms_CrmPatients");

                entity.HasOne(c => c.DictLoyaltyProgram)
                .WithMany(d => d.CrmPatientsLoyaltyPrograms)
                .HasForeignKey(e => e.DictLoyaltyProgramsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmPatientsLoyaltyPrograms_DictLoyaltyPrograms");

                entity.ToTable("CrmPatientsLoyaltyPrograms");
            });

            builder.Entity<CrmHolidays>(entity =>
            {
                entity.HasOne(c => c.Author)
                .WithMany(d => d.CrmHolidaysAuthors)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmHolidays_Authors_CrmUsers");

                entity.HasOne(c => c.Editor)
                .WithMany(d => d.CrmHolidaysEditors)
                .HasForeignKey(e => e.EditorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CrmHolidays_Editor_CrmUsers");

                entity.ToTable("CrmHolidays");
            });

            builder.Entity<DictCities>(entity =>
            {
                entity.HasOne(c => c.DictCountry)
                .WithMany(d => d.DictCities)
                .HasForeignKey(e => e.DictCountriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DictCities_DictCountries");

                entity.ToTable("DictCities");
            });

            builder.Entity<DictCountries>(entity =>
            {
                entity.ToTable("DictCountries");
            });

            builder.Entity<DictDepartments>(entity =>
            {
                entity.HasOne(c => c.DictEnterprise)
                .WithMany(d => d.DictDepartments)
                .HasForeignKey(e => e.DictEnterprisesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DictDepartments_DictEnterprises");

                entity.ToTable("DictDepartments");
            });

            builder.Entity<DictEnterprises>(entity =>
            {
                entity.HasOne(c => c.ParentEnterprise)
                .WithMany(d => d.EnterpriseBranches)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DictEnterprises_EnterpriseBranches");

                entity.ToTable("DictEnterprises");
            });

            builder.Entity<DictGenders>(entity =>
            {
                entity.ToTable("DictGenders");
            });

            builder.Entity<DictIntolerances>(entity =>
            {
                entity.ToTable("DictIntolerances");
            });

            builder.Entity<DictLoyaltyPrograms>(entity =>
            {
                entity.HasOne(c => c.DictEnterprise)
                .WithMany(d => d.DictLoyaltyPrograms)
                .HasForeignKey(e => e.DictEnterprisesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DictLoyaltyPrograms_DictEnterprises");

                entity.ToTable("DictLoyaltyPrograms");
            });

            builder.Entity<DictPositions>(entity =>
            {
                entity.HasOne(c => c.DictEnterprise)
                .WithMany(d => d.DictPositions)
                .HasForeignKey(e => e.DictEnterprisesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DictPositions_DictEnterprises");

                entity.ToTable("DictPositions");
            });

            builder.Entity<DictServices>(entity =>
            {
                entity.HasOne(c => c.DictDepartment)
                .WithMany(d => d.DictServices)
                .HasForeignKey(e => e.DictDepartmentsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DictServices_DictDepartments");

                entity.ToTable("DictServices");
            });

            builder.Entity<DictStatuses>(entity =>
            {
                entity.ToTable("DictStatuses");
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
