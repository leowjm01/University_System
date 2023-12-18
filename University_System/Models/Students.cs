using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UniSystemTest.Models
{
    [Index(nameof(email), IsUnique = true)] // Declare the unique data
    public class Students
    {
        [Key]
        public int? studentId { get; set; }

        [Required(ErrorMessage = "The student name field is required.")]
        [StringLength(50)]
        public string? studentName { get; set; } = null!;

        [Required]
        public bool gender { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid email!! Example: ali@gmail.com")]
        [RegularExpression(@"^\w+@\w+\.com$", ErrorMessage = "Invalid email!! Example: ali@gmail.com")]
        public string email { get; set; } = null!;

        public int? examSelected { get; set; }

        public bool IsDeleted { get; set; }
    }
}
