﻿// <auto-generated />
using System;
using ABAC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ABAC.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("ABAC.Models.LNK_ROLE_RESOURCES", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ResourceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RoleId", "ResourceId");

                    b.HasIndex("ResourceId");

                    b.ToTable("RoleResources");

                    b.HasData(
                        new
                        {
                            RoleId = 4,
                            ResourceId = 101
                        },
                        new
                        {
                            RoleId = 3,
                            ResourceId = 104
                        },
                        new
                        {
                            RoleId = 1,
                            ResourceId = 105
                        });
                });

            modelBuilder.Entity("ABAC.Models.LNK_USER_ROLE", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            RoleId = 4
                        },
                        new
                        {
                            UserId = 2,
                            RoleId = 1
                        },
                        new
                        {
                            UserId = 3,
                            RoleId = 3
                        });
                });

            modelBuilder.Entity("ABAC.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sensitivity")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Resources");

                    b.HasData(
                        new
                        {
                            Id = 101,
                            Content = "The data type for this entity is a Document with High sensitivity department IT",
                            Department = "IT",
                            Owner = "[1]",
                            Sensitivity = "High",
                            Type = "Document"
                        },
                        new
                        {
                            Id = 102,
                            Content = "The data type for this entity is a Server with Medium sensitivity department IT",
                            Department = "IT",
                            Owner = "[1]",
                            Sensitivity = "Medium",
                            Type = "Server"
                        },
                        new
                        {
                            Id = 103,
                            Content = "The data type for this entity is a Report with low sensitivity department Finance",
                            Department = "Finance",
                            Owner = "[3]",
                            Sensitivity = "Low",
                            Type = "Report"
                        },
                        new
                        {
                            Id = 104,
                            Content = "The data type for this entity is a Network with High sensitivity department Finance",
                            Department = "Finance",
                            Owner = "[3,4]",
                            Sensitivity = "High",
                            Type = "Network"
                        },
                        new
                        {
                            Id = 105,
                            Content = "The data type for this entity is a Document with High sensitivity department HR",
                            Department = "HR",
                            Owner = "[2]",
                            Sensitivity = "High",
                            Type = "Document"
                        },
                        new
                        {
                            Id = 106,
                            Content = "The data type for this entity is a Document with Medium sensitivity department HR",
                            Department = "HR",
                            Owner = "[2,4]",
                            Sensitivity = "Medium",
                            Type = "Document"
                        });
                });

            modelBuilder.Entity("ABAC.Models.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "HR_Manager"
                        },
                        new
                        {
                            Id = 2,
                            Name = "IT_Specialist"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Auditor"
                        },
                        new
                        {
                            Id = 4,
                            Name = "IT_Admin"
                        },
                        new
                        {
                            Id = 5,
                            Name = "HR_Assistant"
                        });
                });

            modelBuilder.Entity("ABAC.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("sysAdmin")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "ec1ed180-7fa8-4aee-b312-9b38bd415cb3",
                            Department = "IT",
                            Email = "admin@example.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEC7iZq/G8B9LMdv8BemfGIm/BZAJnWtP9AM1LxZZpgjAYqMWfpQI8AMK3s7sfwVMfg==",
                            PhoneNumber = "1234567890",
                            PhoneNumberConfirmed = true,
                            TwoFactorEnabled = false,
                            UserName = "admin",
                            sysAdmin = false
                        },
                        new
                        {
                            Id = 2,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "63800cee-58d4-4a7a-b8ff-b81d8548dc3d",
                            Department = "HR",
                            Email = "john@example.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEOfY45207AePjKb+ch2mspNAjia055h5hnG23ZCJhrAh6esqwSzqiYN861P48o7g9A==",
                            PhoneNumber = "1234567890",
                            PhoneNumberConfirmed = true,
                            TwoFactorEnabled = false,
                            UserName = "quocanh",
                            sysAdmin = false
                        },
                        new
                        {
                            Id = 4,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "1533333d-bb12-4148-9274-30642997cc4c",
                            Department = "",
                            Email = "hoanganh@example.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAEEQn7oTgdY1nz43k1LIwuP0QFZGcSxdM6uc5byHZmIjiFxOt7fZj4ant7wZLWptI5Q==",
                            PhoneNumber = "1234567890",
                            PhoneNumberConfirmed = true,
                            TwoFactorEnabled = false,
                            UserName = "hoanganh",
                            sysAdmin = true
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ABAC.Models.LNK_ROLE_RESOURCES", b =>
                {
                    b.HasOne("ABAC.Models.Resource", "Resource")
                        .WithMany("RoleResources")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ABAC.Models.Roles", "Role")
                        .WithMany("RoleResources")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resource");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ABAC.Models.LNK_USER_ROLE", b =>
                {
                    b.HasOne("ABAC.Models.Roles", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ABAC.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("ABAC.Models.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("ABAC.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("ABAC.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("ABAC.Models.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ABAC.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("ABAC.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ABAC.Models.Resource", b =>
                {
                    b.Navigation("RoleResources");
                });

            modelBuilder.Entity("ABAC.Models.Roles", b =>
                {
                    b.Navigation("RoleResources");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("ABAC.Models.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}