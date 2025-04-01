using CvCodeFirst.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.DTOs.WorkExperienceDtos
{
    public class CreateWorkExperienceDTO
    {
        [Required]
        [MaxLength(50)]
        [NotPlaceholder("string")]
        public required string JobTitle { get; set; }

        [Required]
        [MaxLength(50)]
        [NotPlaceholder("string")]
        public required string Company { get; set; }

        [Required]
        [MaxLength(500)]
        [NotPlaceholder("string")]
        public required string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }


    }
}
