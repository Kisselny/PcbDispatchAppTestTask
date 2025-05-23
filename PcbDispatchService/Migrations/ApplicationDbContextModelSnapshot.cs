﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PcbDispatchService.Dal;

#nullable disable

namespace PcbDispatchService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PcbDispatchService.Domain.Models.BoardComponent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ComponentType")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<int?>("PrintedCircuitBoardId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PrintedCircuitBoardId");

                    b.ToTable("BoardComponents");
                });

            modelBuilder.Entity("PcbDispatchService.Domain.Models.ComponentType", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<int>("AvailableSupply")
                        .HasColumnType("integer");

                    b.HasKey("Name");

                    b.ToTable("ComponentTypes");

                    b.HasData(
                        new
                        {
                            Name = "A0-B0",
                            AvailableSupply = 28
                        },
                        new
                        {
                            Name = "A0-B1",
                            AvailableSupply = 66
                        },
                        new
                        {
                            Name = "A1-B0",
                            AvailableSupply = 47
                        },
                        new
                        {
                            Name = "A1-B1",
                            AvailableSupply = 12
                        });
                });

            modelBuilder.Entity("PcbDispatchService.Domain.Models.PrintedCircuitBoard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BusinessProcessStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<int>("QualityControlStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PrintedCircuitBoards");
                });

            modelBuilder.Entity("PcbDispatchService.Domain.Models.BoardComponent", b =>
                {
                    b.HasOne("PcbDispatchService.Domain.Models.PrintedCircuitBoard", null)
                        .WithMany("Components")
                        .HasForeignKey("PrintedCircuitBoardId");
                });

            modelBuilder.Entity("PcbDispatchService.Domain.Models.PrintedCircuitBoard", b =>
                {
                    b.Navigation("Components");
                });
#pragma warning restore 612, 618
        }
    }
}
