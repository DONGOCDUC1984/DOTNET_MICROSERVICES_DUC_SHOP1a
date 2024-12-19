﻿// <auto-generated />
using EmployeeService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmployeeService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241125164055_1-work")]
    partial class _1work
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.35")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EmployeeService.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("YOB")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "An",
                            Position = "Seller",
                            YOB = 1998
                        },
                        new
                        {
                            Id = 2,
                            Name = "Binh",
                            Position = "Security guard",
                            YOB = 1999
                        },
                        new
                        {
                            Id = 3,
                            Name = "Cuong",
                            Position = "Security guard",
                            YOB = 2000
                        },
                        new
                        {
                            Id = 4,
                            Name = "Dung",
                            Position = "Accountant",
                            YOB = 1996
                        },
                        new
                        {
                            Id = 5,
                            Name = "Duong",
                            Position = "Cleaner",
                            YOB = 1995
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
