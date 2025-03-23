using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.DTOs
{
    public class PersonDto
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(@"^\+?[0-9\s\-]+$", ErrorMessage = "Ogiltigt telefonnummer.")]
        public required string Phone { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

    }
}
