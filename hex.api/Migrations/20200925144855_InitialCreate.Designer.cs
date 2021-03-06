﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using hex.api.Repositories;

namespace hex.api.Migrations
{
    [DbContext(typeof(HexDBContext))]
    [Migration("20200925144855_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("hex.api.Models.Beacon", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("BatteryReplaced")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GATTId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Beacons");
                });

            modelBuilder.Entity("hex.api.Models.Container", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("BeaconId")
                        .HasColumnType("bigint");

                    b.Property<int>("Content")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("Kanban")
                        .HasColumnType("boolean");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<decimal>("Weight")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("hex.api.Models.ContainerPlace", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("ContainerId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("DateFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Finish")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("LastDetected")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("Number")
                        .HasColumnType("bigint");

                    b.Property<long>("WarehouseId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ContainerId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("ContainerPlaces");
                });

            modelBuilder.Entity("hex.api.Models.ContainerPlacePlan", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("ContaierId")
                        .HasColumnType("bigint");

                    b.Property<long>("ContaierPlaceId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<long>("WarehouseId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("ContainerPlacePlans");
                });

            modelBuilder.Entity("hex.api.Models.Observer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Hostname")
                        .HasColumnType("text");

                    b.Property<string>("SerialNumer")
                        .HasColumnType("text");

                    b.Property<string>("SoftwareVersion")
                        .HasColumnType("text");

                    b.Property<long>("WarehouseId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Observers");
                });

            modelBuilder.Entity("hex.api.Models.Warehouse", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Lat")
                        .HasColumnType("double precision");

                    b.Property<double>("Lng")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("hex.api.Models.ContainerPlace", b =>
                {
                    b.HasOne("hex.api.Models.Container", "Container")
                        .WithMany()
                        .HasForeignKey("ContainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("hex.api.Models.Container", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
