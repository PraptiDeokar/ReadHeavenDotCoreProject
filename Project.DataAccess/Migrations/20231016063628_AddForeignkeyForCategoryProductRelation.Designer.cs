﻿// <auto-generated />
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
    [Migration("20231016063628_AddForeignkeyForCategoryProductRelation")]
    partial class AddForeignkeyForCategoryProductRelation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.1.23419.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Project.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Display_Order")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

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

            modelBuilder.Entity("Project.Models.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ListPrice")
                        .HasColumnType("float");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("Price100")
                        .HasColumnType("float");

                    b.Property<double>("Price50")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Author = "Charles Duhigg",
                            CategoryId = 1,
                            Description = "This book explores the science behind habit formation and provides valuable insights on how to change and build habits for personal and professional success. ",
                            ISBN = " 978-0812981605",
                            ListPrice = 100.0,
                            Price = 95.0,
                            Price100 = 85.0,
                            Price50 = 90.0,
                            Title = "The Power of Habit: Why We Do What We Do in Life and Business"
                        },
                        new
                        {
                            ID = 2,
                            Author = "James Clear",
                            CategoryId = 1,
                            Description = "James Clear's book focuses on the power of small changes and how they can lead to significant personal development. It offers practical strategies for building positive habits and breaking destructive ones. ",
                            ISBN = "978-0735211292",
                            ListPrice = 99.0,
                            Price = 95.0,
                            Price100 = 85.0,
                            Price50 = 90.0,
                            Title = "Atomic Habits: An Easy & Proven Way to Build Good Habits & Break Bad Ones"
                        },
                        new
                        {
                            ID = 3,
                            Author = "Dale Carnegie",
                            CategoryId = 2,
                            Description = " A timeless classic, this book offers valuable advice on building meaningful relationships, improving communication, and becoming more influential in both personal and professional settings.",
                            ISBN = "978-0671027032",
                            ListPrice = 90.0,
                            Price = 88.0,
                            Price100 = 83.0,
                            Price50 = 85.0,
                            Title = "How to Win Friends and Influence People"
                        });
                });

            modelBuilder.Entity("Project.Models.Models.Product", b =>
                {
                    b.HasOne("Project.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
