﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuanLySanBong.Data;

#nullable disable

namespace QuanLySanBong.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241105084709_initAccount")]
    partial class initAccount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuanLySanBong.Entities.Account.Model.AccountModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("IdCustomer")
                        .HasColumnType("int");

                    b.Property<int?>("IdStaff")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdCustomer")
                        .IsUnique()
                        .HasFilter("[IdCustomer] IS NOT NULL");

                    b.HasIndex("IdStaff")
                        .IsUnique()
                        .HasFilter("[IdStaff] IS NOT NULL");

                    b.ToTable("Account", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin",
                            Password = "$2a$11$ueyunPjin2aMwMOyv4c5ZOSoCVy2EJDKh4.QHN0mTobCHWCpMWI5K",
                            Role = "Admin"
                        });
                });

            modelBuilder.Entity("QuanLySanBong.Entities.Customer.Model.CustomerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("QuanLySanBong.Entities.Staff.Model.StaffModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Staff", (string)null);
                });

            modelBuilder.Entity("QuanLySanBong.Entities.Account.Model.AccountModel", b =>
                {
                    b.HasOne("QuanLySanBong.Entities.Customer.Model.CustomerModel", "Customer")
                        .WithOne("Account")
                        .HasForeignKey("QuanLySanBong.Entities.Account.Model.AccountModel", "IdCustomer");

                    b.HasOne("QuanLySanBong.Entities.Staff.Model.StaffModel", "Staff")
                        .WithOne("Account")
                        .HasForeignKey("QuanLySanBong.Entities.Account.Model.AccountModel", "IdStaff");

                    b.Navigation("Customer");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("QuanLySanBong.Entities.Customer.Model.CustomerModel", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();
                });

            modelBuilder.Entity("QuanLySanBong.Entities.Staff.Model.StaffModel", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
