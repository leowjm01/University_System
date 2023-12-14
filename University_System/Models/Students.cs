using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UniSystemTest.Models
{
    [Index(nameof(email), IsUnique = true)] // Declare the unique data
    public class Students
    {
        [Key]
        public int? studentId { get; set; }

        [Required]
        [StringLength(50)]
        public string? studentName { get; set; } = null!;

        [Required]
        public bool gender { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string email { get; set; } = null!;

        public int? examSelected { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
