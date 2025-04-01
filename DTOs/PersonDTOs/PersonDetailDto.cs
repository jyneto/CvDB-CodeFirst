using CvCodeFirst.DTOs.EducatioDto;
using CvCodeFirst.DTOs.WorkExperienceDtos;
using CvCodeFirst.Helpers;
using CvCodeFirst.Models;
using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.DTOs.PersonDTO
{
    public class PersonDetailDto
    {
        //To Get and show a persons info with education and workexperience
        [Required]
        [MinLength(2)]
        [NotPlaceholder("string")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [NotPlaceholder("user@example.com")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\+?[0-9\s\-]+$", ErrorMessage = "Ogiltigt telefonnummer.")]
        [NotPlaceholder("492255--3-202745  2684607895353 4614 76348918335")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(500)]
        [NotPlaceholder("string")]
        public string Description { get; set; }

        public List<EducationDTO> Educations { get; set; } = new();
        public List<WorkExperienceDTO> WorkExperiences { get; set; } = new();
    }
}
