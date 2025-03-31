using CvCodeFirst.DTOs.EducatioDto;
using CvCodeFirst.DTOs.WorkExperienceDtos;
using CvCodeFirst.Models;
using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.DTOs.PersonDTOs
{
    public class CreatePersonWithDetailsDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(@"^\+?[0-9\s\-]+$", ErrorMessage = "Ogiltigt telefonnummer.")]
        public required string Phone { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        public List<EducationDTO> Educations { get; set; }
        public List<WorkExperienceDTO> WorkExperiences { get; set; }
    }
}
