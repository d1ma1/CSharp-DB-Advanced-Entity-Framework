using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext: DbContext
    {
        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DIMA\\SQLEXPRESS;Database=Hospital;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Patient>(entity => {

                 entity.HasKey(p => p.PatientId);

                 entity.Property(p => p.FirstName)
                     .HasMaxLength(50)
                     .IsUnicode(true);

                 entity.Property(p => p.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(true);

                 entity.Property(p => p.Address)
                    .HasMaxLength(250)
                    .IsUnicode(true);

                 entity.Property(p => p.Email)
                    .HasMaxLength(80)
                    .IsUnicode(true);

                entity.HasMany(v => v.Visitations)
                    .WithOne(p => p.Patient);

                entity.HasMany(v => v.Diagnoses)
                    .WithOne(p => p.Patient);

                //entity.HasMany(v => v.Prescriptions)
                //    .WithOne(p => p.Patient);
            });

            modelBuilder.Entity<Visitation>(entity=>{

                entity.HasKey(v => v.VisitationId);

                entity.Property(v => v.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.HasKey(d => d.DiagnoseId);

                entity.Property(d => d.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                entity.Property(d => d.Comments)
                    .HasMaxLength(250)
                    .IsUnicode();
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(m => m.MedicamentId);

                entity.Property(m => m.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                //entity.HasMany(pm => pm.Prescriptions)
                //    .WithOne(m => m.Medicament);
            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(s => new { s.PatientId, s.MedicamentId });

                entity.HasOne(p => p.Patient)
                    .WithMany(pm => pm.Prescriptions)
                    .HasForeignKey(p=>p.PatientId);

                entity.HasOne(m => m.Medicament)
                    .WithMany(pm => pm.Prescriptions)
                     .HasForeignKey(m => m.MedicamentId);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.DoctorId);

                entity.Property(d => d.Name)
                    .HasMaxLength(100)
                    .IsUnicode();

                entity.Property(d => d.Specialty)
                    .HasMaxLength(100)
                    .IsUnicode();

                entity.HasMany(v => v.Visitations)
                    .WithOne(d => d.Doctor);
            });
        }
    }
}

