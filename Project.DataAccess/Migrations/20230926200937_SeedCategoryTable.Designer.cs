﻿// <auto-generated />
using Project.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.DataAccess.Data;

#nullable disable

namespace Project.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230926200937_SeedCategoryTable")]
    partial class SeedCategoryTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.1.23419.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CoreAppWithEntityfw1.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Display_Order")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("categories");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Display_Order = 1,
                            Name = "Maths"
                        },
                        new
                        {
                            ID = 2,
                            Display_Order = 2,
                            Name = "Science"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
