using CvCodeFirst.DTOs.EducationDtos;
using CvCodeFirst.DTOs.WorkExperienceDtos;
using CvCodeFirst.Helpers;
using CvCodeFirst.Models;
using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.DTOs.PersonDTOs
{
    public class CreatePersonWithDetailsDto
    {
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

            public List<CreateEducationDTO> Educations { get; set; } = new();
            public List<CreateWorkExperienceDTO> WorkExperiences { get; set; } = new();
        

        //[Required]
        //[MinLength(5)]
        //[MaxLength(50)]
        //public required string FullName { get; set; }

        //[Required]
        //[EmailAddress]
        //public required string Email { get; set; }

        //[Required]
        //[RegularExpression(@"^\+?[0-9\s\-]+$", ErrorMessage = "Ogiltigt telefonnummer.")]
        //public required string Phone { get; set; }

        //[Required]
        //[MaxLength(500)]
        //public required string Description { get; set; }

        //public List<CreateEducationDTO> Educations { get; set; } = new();
        //public List<CreateWorkExperienceDTO> WorkExperiences { get; set; } = new();
    }
}
