﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServerLibrary;

#nullable disable

namespace ServerLibrary.Context.Migrations
{
    [DbContext(typeof(Glo2GoDbContext))]
    [Migration("20240426081430_addAddressWithForeignkey")]
    partial class addAddressWithForeignkey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BaseLibrary.Models.Address", b =>
                {
                    b.Property<int?>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("AddressId"));

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("TravelAddress")
                        .HasColumnType("text");

                    b.Property<string>("TravelerEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AddressId");

                    b.HasIndex("TravelerEmail")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("BaseLibrary.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminEmail")
                        .HasColumnType("text");

                    b.Property<string>("AdminPass")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("BaseLibrary.Models.RefreshTokenInfo", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<int?>("userId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokenInfos");
                });

            modelBuilder.Entity("BaseLibrary.Models.Review", b =>
                {
                    b.Property<string>("ReviewID")
                        .HasColumnType("text");

                    b.Property<int>("ReviewRating")
                        .HasColumnType("integer");

                    b.Property<string>("ReviewTraveler")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ReviewID");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("BaseLibrary.Models.Site", b =>
                {
                    b.Property<string>("SiteID")
                        .HasColumnType("text");

                    b.Property<string>("SiteAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SiteCountry")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SiteDesc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SiteName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SiteOperatingHour")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("SitePics")
                        .HasColumnType("text[]");

                    b.Property<int>("SiteRating")
                        .HasColumnType("integer");

                    b.HasKey("SiteID");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("BaseLibrary.Models.SystemRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SystemRoles");
                });

            modelBuilder.Entity("BaseLibrary.Models.Traveler", b =>
                {
                    b.Property<string>("TravelerEmail")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Gender")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ProfilePic")
                        .HasColumnType("text");

                    b.Property<string>("TravelerPass")
                        .HasColumnType("text");

                    b.HasKey("TravelerEmail");

                    b.ToTable("Travelers");
                });

            modelBuilder.Entity("BaseLibrary.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("BaseLibrary.Models.Address", b =>
                {
                    b.HasOne("BaseLibrary.Models.Traveler", "Traveler")
                        .WithOne("Address")
                        .HasForeignKey("BaseLibrary.Models.Address", "TravelerEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Traveler");
                });

            modelBuilder.Entity("BaseLibrary.Models.Traveler", b =>
                {
                    b.Navigation("Address")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
