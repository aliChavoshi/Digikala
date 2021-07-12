﻿// <auto-generated />
using System;
using Digikala.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Digikala.DataAccessLayer.Migrations
{
    [DbContext(typeof(DigikalaContext))]
    [Migration("20210712095245_UpdateStoreEntityVote")]
    partial class UpdateStoreEntityVote
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Permission");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            Name = "صفحه اصلی فروشگاه"
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            Name = "صفحه اصلی ادمین"
                        });
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            Title = "کاربر عادی"
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            Title = "فروشندگان"
                        },
                        new
                        {
                            Id = 3,
                            IsDeleted = false,
                            Title = "ادمین"
                        });
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.RolePermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("ExpireRolePermission")
                        .HasColumnType("datetime2");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermission");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PermissionId = 1,
                            RoleId = 2
                        },
                        new
                        {
                            Id = 2,
                            PermissionId = 1,
                            RoleId = 3
                        },
                        new
                        {
                            Id = 3,
                            PermissionId = 2,
                            RoleId = 3
                        });
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActiveCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ActiveCodeEmail")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("ConfirmEmail")
                        .HasColumnType("bit");

                    b.Property<bool>("ConfirmMobile")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Email")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Fullname")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NationalCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConfirmEmail = true,
                            ConfirmMobile = true,
                            CreateDate = new DateTime(2021, 7, 12, 14, 22, 44, 524, DateTimeKind.Local).AddTicks(7183),
                            Email = "Admin@gmail.com",
                            Fullname = "Admin",
                            IsActive = true,
                            IsDeleted = false,
                            Mobile = "09130242780",
                            Password = "nRy/sK5Z7ENvGsSwfcmLzw==",
                            RoleId = 3,
                            Version = 0
                        });
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Store.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorUser")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Icon")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ModifierUser")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorUser");

                    b.HasIndex("ModifierUser");

                    b.HasIndex("ParentId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Store.Store", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ReturnNumber")
                        .HasColumnType("int");

                    b.Property<string>("Tel")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.Property<int>("VoteNegative")
                        .HasColumnType("int");

                    b.Property<int>("VotePositive")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.Permission", b =>
                {
                    b.HasOne("Digikala.DataAccessLayer.Entities.Identity.Permission", null)
                        .WithMany("Permissions")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.RolePermission", b =>
                {
                    b.HasOne("Digikala.DataAccessLayer.Entities.Identity.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Digikala.DataAccessLayer.Entities.Identity.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.User", b =>
                {
                    b.HasOne("Digikala.DataAccessLayer.Entities.Identity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Store.Category", b =>
                {
                    b.HasOne("Digikala.DataAccessLayer.Entities.Identity.User", "UserCreator")
                        .WithMany("CreateCategory")
                        .HasForeignKey("CreatorUser")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Digikala.DataAccessLayer.Entities.Identity.User", "UserModifier")
                        .WithMany("ModifyCategory")
                        .HasForeignKey("ModifierUser");

                    b.HasOne("Digikala.DataAccessLayer.Entities.Store.Category", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");

                    b.Navigation("UserCreator");

                    b.Navigation("UserModifier");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Store.Store", b =>
                {
                    b.HasOne("Digikala.DataAccessLayer.Entities.Identity.User", "User")
                        .WithOne("Store")
                        .HasForeignKey("Digikala.DataAccessLayer.Entities.Store.Store", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.Permission", b =>
                {
                    b.Navigation("Permissions");

                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.User", b =>
                {
                    b.Navigation("CreateCategory");

                    b.Navigation("ModifyCategory");

                    b.Navigation("Store");
                });
#pragma warning restore 612, 618
        }
    }
}
