﻿// <auto-generated />
using System;
using Jaylan.Area.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Jaylan.Area.Data.Migrations
{
    [DbContext(typeof(AreaDbContext))]
    [Migration("20181116053747_Initial_Mi_Area")]
    partial class Initial_Mi_Area
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("Jaylan.Area.Data.Mi_Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreationTime");

                    b.Property<bool>("IsDel");

                    b.Property<int>("Level");

                    b.Property<int>("MiId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(true);

                    b.Property<string>("ParentCode")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<int>("ParentId");

                    b.Property<int>("Status");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Mi_Area");
                });

            modelBuilder.Entity("Jaylan.Area.Data.NBS_Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChildNodeUrl")
                        .HasMaxLength(512)
                        .IsUnicode(false);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreationTime");

                    b.Property<bool>("IsDel");

                    b.Property<bool>("IsGetChild");

                    b.Property<int>("Level");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(true);

                    b.Property<string>("ParentCode")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<int>("Status");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("NBS_Area");
                });

            modelBuilder.Entity("Jaylan.Area.Data.Taobao_Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreationTime");

                    b.Property<bool>("IsDel");

                    b.Property<int>("Level");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(true);

                    b.Property<string>("ParentCode")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false);

                    b.Property<int>("Status");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Taobao_Area");
                });
#pragma warning restore 612, 618
        }
    }
}
