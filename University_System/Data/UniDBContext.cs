using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniSystemTest.Models;

namespace University_System.Data
{
    public class UniDBContext : DbContext
    {
        public UniDBContext (DbContextOptions<UniDBContext> options)
            : base(options)
        {
        }

        public DbSet<Students> Students { get; set; } = default!;
        public DbSet<Teachers> Teachers { get; set; } = default!;
        public DbSet<Courses> Courses { get; set; } = default!;
        public DbSet<ScoreResults> ScoreResults { get; set; } = default!;
        public DbSet<CoursesAndTeachers> CoursesTeachers { get; set; } = default!;
        public DbSet<ResultStudentCourse> ResultStudentCourse { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Declare keyless entity type
            modelBuilder.Entity<CoursesAndTeachers>().HasNoKey(); 
            modelBuilder.Entity<ResultStudentCourse>().HasNoKey();
        }
    }
}
