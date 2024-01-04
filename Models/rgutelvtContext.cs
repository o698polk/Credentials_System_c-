using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using carnetutelvt.Models;

namespace carnetutelvt.Models
{
    public partial class rgutelvtContext : DbContext
    {
        public rgutelvtContext()
        {
        }

        public rgutelvtContext(DbContextOptions<rgutelvtContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Detallestb> Detallestbs { get; set; } = null!;
        public virtual DbSet<Usertb> Usertbs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {/*
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=POLK\\SQLEXPRESS;database=rgutelvt;integrated security=true;");
           */ }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Cuaderno>(entity =>
			{
				entity.ToTable("cuaderno");

				entity.Property(e => e.Id).HasColumnName("idC");

				entity.Property(e => e.NameC)
					.HasMaxLength(100)
					.IsUnicode(false)
					.HasColumnName("nameC");

				entity.Property(e => e.Datecreate)
					.HasColumnType("date")
					.HasColumnName("datecreate");

				entity.Property(e => e.Dateupdate)
					.HasColumnType("date")
					.HasColumnName("dateupdate");


				entity.Property(e => e.Iduser).HasColumnName("idUser");

				entity.Property(e => e.InfoC)
					.HasColumnType("text")
					.HasColumnName("infoC");

				
			});





			modelBuilder.Entity<Detallestb>(entity =>
            {
                entity.ToTable("detallestb");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ci)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CI");

                entity.Property(e => e.Datecreate)
                    .HasColumnType("date")
                    .HasColumnName("datecreate");

                entity.Property(e => e.Dateupdate)
                    .HasColumnType("date")
                    .HasColumnName("dateupdate");

                entity.Property(e => e.Faculty)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("faculty");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("fullname");

                entity.Property(e => e.Iduser).HasColumnName("iduser");

                entity.Property(e => e.Imgcarnet)
                    .HasColumnType("text")
                    .HasColumnName("imgcarnet");

                entity.Property(e => e.Specialty)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("specialty");

                entity.Property(e => e.Surnames)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("surnames");
                entity.Property(e => e.Bloodtype)
                   .HasMaxLength(10)
                   .IsUnicode(false)
                   .HasColumnName("bloodtype");
                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.Detallestbs)
                    .HasForeignKey(d => d.Iduser)
                    .HasConstraintName("FK__detallest__iduse__398D8EEE");
            });


            modelBuilder.Entity<Usertb>(entity =>
            {
                entity.ToTable("usertb");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Datecreate)
                    .HasColumnType("date")
                    .HasColumnName("datecreate");

                entity.Property(e => e.Dateupdate)
                    .HasColumnType("date")
                    .HasColumnName("dateupdate");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Numberverify)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("numberverify");

                entity.Property(e => e.Passwords)
                     .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("passwords");

				entity.Property(e => e.Rol)
					.HasMaxLength(10)
				   .IsUnicode(false)
				   .HasColumnName("rol");
               
              


                entity.Property(e => e.Verifyuser).HasColumnName("verifyuser");
                entity.Property(e => e.Typess)
                   .HasMaxLength(10)
                  .IsUnicode(false)
                  .HasColumnName("typess");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<carnetutelvt.Models.Cuaderno>? Cuaderno { get; set; }
    }
}
