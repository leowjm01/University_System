﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using University_System.Data;

#nullable disable

namespace University_System.Migrations
{
    [DbContext(typeof(UniDBContext))]
    partial class UniDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UniSystemTest.Models.Courses", b =>
                {
                    b.Property<int>("courseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("courseId"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("courseName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("teacherId")
                        .HasColumnType("int");

                    b.HasKey("courseId");

                    b.HasIndex("teacherId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("UniSystemTest.Models.CoursesAndTeachers", b =>
                {
                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("courseId")
                        .HasColumnType("int");

                    b.Property<string>("courseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("gender")
                        .HasColumnType("bit");

                    b.Property<int>("teacherId")
                        .HasColumnType("int");

                    b.Property<string>("teacherName")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("CoursesTeachers");
                });

            modelBuilder.Entity("UniSystemTest.Models.ResultStudentCourse", b =>
                {
                    b.Property<int>("courseId")
                        .HasColumnType("int");

                    b.Property<string>("courseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("gender")
                        .HasColumnType("bit");

                    b.Property<string>("grade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal?>("mark")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("scoreResultId")
                        .HasColumnType("int");

                    b.Property<int>("studentId")
                        .HasColumnType("int");

                    b.Property<string>("studentName")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("ResultStudentCourse");
                });

            modelBuilder.Entity("UniSystemTest.Models.ScoreResults", b =>
                {
                    b.Property<int?>("scoreResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("scoreResultId"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("courseId")
                        .HasColumnType("int");

                    b.Property<string>("grade")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<decimal?>("mark")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("studentId")
                        .HasColumnType("int");

                    b.HasKey("scoreResultId");

                    b.HasIndex("courseId");

                    b.HasIndex("studentId");

                    b.ToTable("ScoreResults");
                });

            modelBuilder.Entity("UniSystemTest.Models.Students", b =>
                {
                    b.Property<int?>("studentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("studentId"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("examSelected")
                        .HasColumnType("int");

                    b.Property<bool>("gender")
                        .HasColumnType("bit");

                    b.Property<string>("studentName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("studentId");

                    b.HasIndex("email")
                        .IsUnique();

                    b.ToTable("Students");
                });

            modelBuilder.Entity("UniSystemTest.Models.Teachers", b =>
                {
                    b.Property<int>("teacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("teacherId"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("gender")
                        .HasColumnType("bit");

                    b.Property<string>("teacherName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("teacherId");

                    b.HasIndex("email")
                        .IsUnique();

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("UniSystemTest.Models.Courses", b =>
                {
                    b.HasOne("UniSystemTest.Models.Teachers", "Teachers")
                        .WithMany()
                        .HasForeignKey("teacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("UniSystemTest.Models.ScoreResults", b =>
                {
                    b.HasOne("UniSystemTest.Models.Courses", "Courses")
                        .WithMany()
                        .HasForeignKey("courseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniSystemTest.Models.Students", "Students")
                        .WithMany()
                        .HasForeignKey("studentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Courses");

                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
