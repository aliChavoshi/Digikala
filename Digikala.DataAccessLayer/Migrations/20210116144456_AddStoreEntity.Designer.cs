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
    [Migration("20210116144456_AddStoreEntity")]
    partial class AddStoreEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
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
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Permission");
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
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Role");
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
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActiveCode")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("ConfirmActiveCode")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorUser")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Fullname")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(11)")
                        .HasMaxLength(11);

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ModifierUser")
                        .HasColumnType("int");

                    b.Property<string>("NationalCode")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Store", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Mail")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Tel")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("Version")
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
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Identity.User", b =>
                {
                    b.HasOne("Digikala.DataAccessLayer.Entities.Identity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Digikala.DataAccessLayer.Entities.Store", b =>
                {
                    b.HasOne("Digikala.DataAccessLayer.Entities.Identity.User", "User")
                        .WithOne("Store")
                        .HasForeignKey("Digikala.DataAccessLayer.Entities.Store", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
