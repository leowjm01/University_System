using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UniSystemTest.Models
{
    [Index(nameof(email), IsUnique = true)] // Declare the unique data
    public class Teachers
    {
        [Key]
        public int teacherId { get; set; }

        [Required]
        [StringLength(50)]
        public string? teacherName { get; set; } = null!;

        [Required]
        public bool gender { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string? email { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }
    }


}
