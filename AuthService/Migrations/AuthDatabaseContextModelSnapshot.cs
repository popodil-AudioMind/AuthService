﻿// <auto-generated />
using AuthService.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthService.Migrations
{
    [DbContext(typeof(AuthDatabaseContext))]
    partial class AuthDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AuthService.Interfaces.ILoginUser", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("forumIDs")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("hashedPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("LoginUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
