using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.DTOs.WorkExperienceDtos
{
    public class UpdateWorkExperienceDTO
    {
        [Required]
        [MaxLength(50)]
        public required string JobTitle { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Company { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
