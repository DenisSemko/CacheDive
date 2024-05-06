﻿// <auto-generated />
using System;
using ExperimentAgent.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExperimentAgent.API.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240428135747_Add ExperimentType")]
    partial class AddExperimentType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ExperimentAgent.API.Entities.ExperimentOutcome", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("CacheHitRate")
                        .HasColumnType("double precision");

                    b.Property<double>("CacheMissRate")
                        .HasColumnType("double precision");

                    b.Property<double>("CacheSize")
                        .HasColumnType("double precision");

                    b.Property<int>("DatabaseType")
                        .HasColumnType("integer");

                    b.Property<string>("ExperimentExecutionTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ExperimentType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsExecutedFromCache")
                        .HasColumnType("boolean");

                    b.Property<string>("Query")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("QueryExecutionNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Resources")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ExperimentOutcomes");
                });
#pragma warning restore 612, 618
        }
    }
}
