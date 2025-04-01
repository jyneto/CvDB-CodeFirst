using CvCodeFirst.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.DTOs.EducationDtos
{
    public class CreateEducationDTO
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [NotPlaceholder("string")]
        public required string School { get; set; }

        [Required]
        [MaxLength(100)]
        [NotPlaceholder("string")]
        public required string Degree { get; set; }

        [Required]
        public required DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
