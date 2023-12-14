using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniSystemTest.Models
{
    public class Courses
    {
        [Key]
        public int courseId { get; set; }

        [Required]
        [StringLength(50)]
        public string? courseName { get; set; } = null!;

        public Teachers? Teachers { get; set; }  // Navigation property
        [ForeignKey("Teachers")] // create foreign key
        public int teacherId { get; set; } // Foreign key property

        [Required]
        public bool IsDeleted { get; set; }
    }

    public class CoursesAndTeachers
    {

        //Teachers Object data
        public int teacherId { get; set; }
        public string? teacherName { get; set; } = null!;
        public bool gender { get; set; }
        public string? email { get; set; } = null!;
        public bool IsDeleted { get; set; }

        //Courses Object data
        public int courseId { get; set; }
        public string? courseName { get; set; } = null!;

    }

    public class TeacherCourseViewModel
    {
        public Teachers? Teachers { get; set; }
        public List<Courses>? Courses { get; set; }
    }
}
