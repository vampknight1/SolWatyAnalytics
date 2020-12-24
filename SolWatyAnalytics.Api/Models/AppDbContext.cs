using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SolWatyAnalytics.BLL.Models;

namespace SolWatyAnalytics.Api.Models
{
    public partial class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AeWwc> AeWwc { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Klhksents> Klhksents { get; set; }
        public virtual DbSet<Station> Station { get; set; }
        public virtual DbSet<StationX> StationX { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_config.GetConnectionString("localPgSQL"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AeWwc>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ComputerId })
                    .HasName("ae_wwc_pk");

                entity.ToTable("ae_wwc");

                entity.HasIndex(e => e.DeviceId)
                    .HasName("ae_wwc_deviceid");

                entity.HasIndex(e => e.EndTimeStamp)
                    .HasName("ae_wwc_endtimestamp");

                entity.HasIndex(e => new { e.ObjectId, e.StartTimeStamp })
                    .HasName("ae_wwc_almid_starttimestamp");

                entity.HasIndex(e => new { e.StartTimeStamp, e.ObjectId })
                    .HasName("ae_wwc_starttimestamp_almid");

                entity.HasIndex(e => new { e.Id, e.ObjectId, e.StartTimeStamp })
                    .HasName("ae_wwc_id_almid_starttimestamp")
                    .IsUnique();

                entity.Property(e => e.Note).HasMaxLength(4000);

                entity.Property(e => e.Text).HasMaxLength(500);
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("area");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('\"areas_ID_seq\"'::regclass)");

                entity.Property(e => e.CodeArea)
                    .HasColumnName("code_area")
                    .HasMaxLength(16);

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.NameArea)
                    .HasColumnName("name_area")
                    .HasMaxLength(32);

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('\"customers_ID_seq\"'::regclass)");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasColumnType("character varying");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("character varying");

                entity.Property(e => e.CompanyName)
                    .HasColumnName("company_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ContactName)
                    .HasColumnName("contact_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("character varying");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .HasColumnType("character varying");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Klhksents>(entity =>
            {
                entity.ToTable("klhksents");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cod).HasColumnName("cod");

                entity.Property(e => e.DateTime).HasColumnName("date_time");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.Nh3n).HasColumnName("nh3n");

                entity.Property(e => e.Ph).HasColumnName("ph");

                entity.Property(e => e.Tss).HasColumnName("tss");

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .HasMaxLength(13);
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.ToTable("station");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('\"stations_ID_seq\"'::regclass)");

                entity.Property(e => e.AreaCode)
                    .HasColumnName("area_code")
                    .HasMaxLength(16);

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.NameStation)
                    .HasColumnName("name_station")
                    .HasMaxLength(32);

                entity.Property(e => e.StationUid)
                    .HasColumnName("station_uid")
                    .HasMaxLength(13);

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<StationX>(entity =>
            {
                entity.HasKey(e => e.TimeStamp)
                    .HasName("station_x_pk");

                entity.ToTable("station_x");

                entity.HasIndex(e => e.TimeStamp2)
                    .HasName("station_x_timestamp2");

                entity.Property(e => e.TimeStamp).ValueGeneratedNever();

                entity.Property(e => e.Cod).HasColumnName("cod");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.Hum).HasColumnName("hum");

                entity.Property(e => e.Nh3n).HasColumnName("nh3n");

                entity.Property(e => e.Ph).HasColumnName("ph");

                entity.Property(e => e.Temp).HasColumnName("temp");

                entity.Property(e => e.Tss).HasColumnName("tss");

                entity.Property(e => e.Uid)
                    .IsRequired()
                    .HasColumnName("uid")
                    .HasMaxLength(13);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}