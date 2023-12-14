using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniSystemTest.Models
{
    public class ScoreResults
    {
        [Key]
        public int? scoreResultId { get; set; }

        public decimal? mark { get; set; }

        [StringLength(5)]
        public string? grade { get; set; }

        public Courses? Courses { get; set; } = null!;          // Navigation property
        [ForeignKey("Courses")]                                 //Define the foreign key relationship
        public int courseId { get; set; }                       // Foreign key property


        public Students? Students { get; set; } = null!;
        [ForeignKey("Students")] 
        public int studentId { get; set; } 

        [Required]
        public bool IsDeleted { get; set; }
    }

    public class ResultStudentCourse
    {

        //students Object data
        public int studentId { get; set; }
        public string? studentName { get; set; } = null!;
        public bool gender { get; set; }
        public string? email { get; set; }

        //courses Object data
        public int courseId { get; set; }
        public string? courseName { get; set; } = null!;

        //ScoreResults Object data
        public int scoreResultId { get; set; }
        public decimal? mark { get; set; }
        public string? grade { get; set; }
    }
}

