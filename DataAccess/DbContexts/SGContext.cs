using Core.DataObjects.EFObjects;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DataAccess.DbContexts;

namespace DataAccess.DbContexts
{
    public partial class SGContext : DbContext
    {
        public SGContext()
        {
        }

        public SGContext(DbContextOptions<SGContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Measurement> Measurements { get; set; }
        public virtual DbSet<MeasurementIdealAmount> MeasurementIdealAmounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=SmartGarden;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Devices", "Data");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.HowMuchHumidityId).HasColumnName("HowMuchHumidityID");

                entity.Property(e => e.HowMuchLightId).HasColumnName("HowMuchLightID");

                entity.Property(e => e.HowMuchWaterId).HasColumnName("HowMuchWaterID");

                entity.Property(e => e.IdealTemperatureId).HasColumnName("IdealTemperatureID");

                entity.Property(e => e.PlantName).HasMaxLength(50);

                entity.Property(e => e.PlantSpecies).HasMaxLength(100);

                entity.Property(e => e.SerialNumber).HasMaxLength(50);

                entity.HasOne(d => d.HowMuchHumidity)
                    .WithMany(p => p.DeviceHowMuchHumidities)
                    .HasForeignKey(d => d.HowMuchHumidityId)
                    .HasConstraintName("FK_Devices_MeasurementIdealAmounts2");

                entity.HasOne(d => d.HowMuchLight)
                    .WithMany(p => p.DeviceHowMuchLights)
                    .HasForeignKey(d => d.HowMuchLightId)
                    .HasConstraintName("FK_Devices_MeasurementIdealAmounts1");

                entity.HasOne(d => d.HowMuchWater)
                    .WithMany(p => p.DeviceHowMuchWaters)
                    .HasForeignKey(d => d.HowMuchWaterId)
                    .HasConstraintName("FK_Devices_MeasurementIdealAmounts");

                entity.HasOne(d => d.IdealTemperature)
                    .WithMany(p => p.DeviceIdealTemperatures)
                    .HasForeignKey(d => d.IdealTemperatureId)
                    .HasConstraintName("FK_Devices_MeasurementIdealAmounts3");
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.ToTable("Measurements", "Data");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.Humidity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LightPercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SoilMoisturePercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Temperature).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Measurements)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Measurements_Devices");
            });

            modelBuilder.Entity<MeasurementIdealAmount>(entity =>
            {
                entity.ToTable("MeasurementIdealAmounts", "Catalogs");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
