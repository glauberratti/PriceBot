﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PriceBot.Infra.Data.Context;

#nullable disable

namespace PriceBot.Infra.Data.Migrations
{
    [DbContext(typeof(DbPriceBotContext))]
    [Migration("20230305020145_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("PriceBot.Domain.Product.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BRLValue")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(28, 3)
                        .HasColumnType("TEXT")
                        .HasDefaultValue(0m);

                    b.Property<decimal>("EURValue")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(28, 3)
                        .HasColumnType("TEXT")
                        .HasDefaultValue(0m);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("USDValue")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(28, 3)
                        .HasColumnType("TEXT")
                        .HasDefaultValue(0m);

                    b.HasKey("Id");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}