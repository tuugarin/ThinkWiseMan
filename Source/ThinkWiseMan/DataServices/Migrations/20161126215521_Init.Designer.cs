﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DataServices;

namespace DataServices.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161126215521_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("DataServices.Models.WiseIdea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Content");

                    b.Property<int>("Day");

                    b.Property<bool>("IsFavorite");

                    b.Property<int>("Month");

                    b.HasKey("Id");

                    b.ToTable("Ideas");
                });
        }
    }
}